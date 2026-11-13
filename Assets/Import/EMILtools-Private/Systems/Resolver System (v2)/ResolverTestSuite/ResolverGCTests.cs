using System;
using System.Threading.Tasks;
using EMILtools.Systems;
using NUnit.Framework;
using Unity.Profiling;
using UnityEngine;

public class ResolverGCTests
{
    const int WarmupIterations = 1000;
    const int TestIterations = 10000;

    [Test]
    public void Resolver_Construction_GC_Test()
    {
        const int iterations = 10000;

        // Warmup
        var warmup = new Callback(() => { });

        // Prepare storage before measuring
        IResolvable[] results = new IResolvable[iterations];

        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        using var recorder = ProfilerRecorder.StartNew(
            ProfilerCategory.Internal,
            "GC.Alloc",
            100000
        );

        // Construction
        for (int i = 0; i < iterations; i++)
            results[i] = new Callback(() => { });

        recorder.Stop();

        long totalAllocated = 0;

        for (int i = 0; i < recorder.Count; i++)
        {
            var sample = recorder.GetSample(i);
            totalAllocated += sample.Value;
        }

        Debug.Log(
            $"Resolver_Construction_GC_Test ({iterations} iterations)\n" +
            $"---\n" +
            $"GC.Alloc samples: {recorder.Count}\n" +
            $"Total allocated: {totalAllocated} bytes\n" +
            $"Per construction: {(double)totalAllocated / iterations} bytes"
        );

        Assert.IsNotNull(results[0]);
        Assert.IsNotNull(results[iterations - 1]);
        Assert.Greater(totalAllocated, 0);
    }
    
    async Task<long> Measure(Func<Task<bool>> execute)
    {
        // Warmup
        for (int i = 0; i < WarmupIterations; i++)
            await execute();

        GC.Collect();
        GC.WaitForPendingFinalizers();
        GC.Collect();

        using var recorder = ProfilerRecorder.StartNew(
            ProfilerCategory.Internal,
            "GC.Alloc",
            100000
        );

        for (int i = 0; i < TestIterations; i++)
            await execute();

        recorder.Stop();

        long totalAllocated = 0;

        for (int i = 0; i < recorder.Count; i++)
            totalAllocated += recorder.GetSample(i).Value;

        return totalAllocated;
    }

    async Task RunTest(string testName, Func<Task<bool>> execute)
    {
        long allocated = await Measure(execute);

        Debug.Log(
            $"{testName}\n" +
            $"Total allocated: {allocated} bytes\n" +
            $"Per execution: {(double)allocated / TestIterations} bytes"
        );
    }

    [Test]
    public async Task Resolver_GC_Benchmark()
    {
        object ctx = new object();
        var main = new Callback(() => { });

        // 1. Empty configuration
        var empty = new Resolves(true);

        // 2. Before execution
        var before = new Resolves(true,
            beforeExe: new IResolvable[]
            {
                new Callback(() => {})
            });

        // 3. After execution
        var after = new Resolves(true,
            afterExe: new IResolvable[]
            {
                new Callback(() => {})
            });

        // 4. Before + After
        var beforeAfter = new Resolves(true,
            beforeExe: new IResolvable[]
            {
                new Callback(() => {}),
                new Callback(() => {})
            },
            afterExe: new IResolvable[]
            {
                new Callback(() => {}),
                new Callback(() => {})
            });

        // 5. All three arrays populated
        var allStages = new Resolves(true,
            beforeExe: new IResolvable[]
            {
                new Callback(() => {})
            },
            afterExe: new IResolvable[]
            {
                new Callback(() => {})
            },
            failExe: new IResolvable[]
            {
                new Callback(() => {})
            });

        // 6. Context injection
        var context = new Resolves(true,
            beforeExe: new IResolvable[]
            {
                new Callback<object>(_ => {})
            },
            afterExe: new IResolvable[]
            {
                new Callback<object>(_ => {})
            });

        // 7. Multiple context injections
        var multipleContexts = new Resolves(true,
            beforeExe: new IResolvable[]
            {
                new Callback<object>(_ => {}),
                new Callback<object>(_ => {}),
                new Callback<object>(_ => {})
            },
            afterExe: new IResolvable[]
            {
                new Callback<object>(_ => {}),
                new Callback<object>(_ => {}),
                new Callback<object>(_ => {})
            });

        // 8. Execute-once resolvers
        var executeOnce = new Resolves(true,
            beforeExe: new IResolvable[]
            {
                new Callback(() => {}, executeOnce: true)
            });

        await RunTest("01 - Empty", () =>
            Resolver<object>.ResolveContainer(empty, main, true, ctx));

        await RunTest("02 - Before", () =>
            Resolver<object>.ResolveContainer(before, main, true, ctx));

        await RunTest("03 - After", () =>
            Resolver<object>.ResolveContainer(after, main, true, ctx));

        await RunTest("04 - Before + After", () =>
            Resolver<object>.ResolveContainer(beforeAfter, main, true, ctx));

        await RunTest("05 - All Stages", () =>
            Resolver<object>.ResolveContainer(allStages, main, true, ctx));

        await RunTest("06 - Context Injection", () =>
            Resolver<object>.ResolveContainer(context, main, true, ctx));

        await RunTest("07 - Multiple Context Injections", () =>
            Resolver<object>.ResolveContainer(multipleContexts, main, true, ctx));

        await RunTest("08 - Execute Once", () =>
            Resolver<object>.ResolveContainer(executeOnce, main, true, ctx));
    }
}