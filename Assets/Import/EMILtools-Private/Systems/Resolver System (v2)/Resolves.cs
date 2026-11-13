using System;

namespace EMILtools.Systems
{
    /// <summary>
    /// Resolver Execution Configuration Container.
    /// Defines the resolver execution stages and lifecycle behavior for Resolver<TContext>.
    ///
    /// Features/Configuration:
    /// - Three independently configurable execution stages: Before, After, and Failed
    /// - Supports context injection through IContextInjectible<TContext>
    /// - Supports asynchronous execution through IResolveWaitable
    /// - Supports short-circuiting during resolver execution
    /// - Configurable resolver reset behavior after successful completion
    /// - Empty resolver arrays are automatically initialized when constructed
    ///
    /// Usage:
    /// - Initialize with Resolves(bool resetWhenAllResolved)
    /// - Assign beforeExe for pre-execution logic
    /// - Assign afterExe for post-execution logic
    /// - Assign failExe for failed/short-circuited execution logic
    /// - Pass to Resolver<TContext>.ResolveContainer() for execution
    /// - Call ResetAllOnceResolves() to reset all contained resolvers
    /// - Use allNull to detect default-initialized instances
    ///
    /// Execution Order:
    /// - Resolve BeforeExecution sequentially
    /// - Inject context into the main resolver, if supported
    /// - Execute the main resolver
    /// - Resolve FailedExecution if the main resolver short-circuits
    /// - Otherwise, resolve AfterExecution sequentially
    /// - Reset all resolvers if resetWhenAllResolved is enabled
    ///
    /// Validation / Exception Handling:
    /// - Null resolver arrays are replaced with Array.Empty<IResolvable>()
    /// - Default initialization leaves all resolver arrays null
    /// - Resolver exceptions propagate to the caller
    /// </summary>
    public readonly struct Resolves
    {
        public readonly IResolvable[] beforeExecution;
        public readonly IResolvable[] afterExecution;
        public readonly IResolvable[] failedExecution;
        public readonly bool resetWhenAllResolved;
        
        /// <summary>
        /// Returns true if all arrays are null, used when defaulted
        /// </summary>
        public bool allNull => beforeExecution == null && afterExecution == null && failedExecution == null;

        public Resolves(bool resetWhenAllResolved)
        {
            this.resetWhenAllResolved = resetWhenAllResolved;
            beforeExecution = Array.Empty<IResolvable>();
            afterExecution = Array.Empty<IResolvable>();
            failedExecution = Array.Empty<IResolvable>();
        }

        public Resolves(bool resetWhenAllResolved, IResolvable[] beforeExe = null,
            IResolvable[] afterExe = null, IResolvable[] failExe = null)
        {
            this.resetWhenAllResolved = resetWhenAllResolved;
            this.beforeExecution = beforeExe ?? Array.Empty<IResolvable>();
            this.afterExecution = afterExe ?? Array.Empty<IResolvable>();
            this.failedExecution = failExe ?? Array.Empty<IResolvable>();
        }

        public void ResetAllOnceResolves()
        {
            for (int i = 0; i < beforeExecution.Length; i++) beforeExecution[i].ResetWait();
            for (int i = 0; i < afterExecution.Length; i++) afterExecution[i].ResetWait();
            for (int i = 0; i < failedExecution.Length; i++) failedExecution[i].ResetWait();
        }
    }
}