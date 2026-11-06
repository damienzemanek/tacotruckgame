# GDIM32-Final
<hr>

## Check-In Milestone 1 - Nov 5
### Group Report
Put your group check-in Devlog here.
- Breakpoints Usage: When I (Damien) was refactoring the `Interactor` to work with the `Pickup` script I was running into a `NullReferenceException,` Originally I had though the NRE was caused by something I did to the `InteractEventCtx`. I used Breakpoints to debug specifically the `InteractEventCtx`'s member variables right before the `OnGrab` event invoke. (This was before Pub/Sub implementation). I was able to use Breakpoints to identify that all 3 member variables were `null` at this state. This informed me that the error was further up the stack trace and I had to go further toward the source. I stack traced the error all the way back into Unity and found that it was an unassigned refernce in the `Pickup` that I needed to assign which was causing the error. I chose this tool because I needed to identify the state of the member variables to determine how the `null` was propagating through the stack trace. The breakpoints helped me acheive this. After identifying what was `null`, I could narrow my search up the stack trace and eliminate more potential candidates for debugging purposes.

### Damien Zemanek
10/5/2026
What I contributed to the project at this stage: 
- `Interactable`: this script is a context based subscription holder that is lightly coupled to `Interactor`. Meaning its only logical use case is tied to `Interactor`. I designed `Interactable` so that changes to `Interactor` do not propagate tech-debt into `Interactable` due to `Interactable` being transiently event based and holding no `Interactor` logic. I accomplished this by using a form of Inversion Of Control called Dependency Injection. Normal dependency Injection injects the dependency to be stored on the dependent, however I inject the dependency into the context layer to be used by subscriber event definitions. This is where my Pub/Sub custom system comes in which supports transient context injection into subscribers. Which is specifically made for use cases like this. `Interactable` is tightly coupled to `Grabber` because grabbing only happens due to logic executed in `Interactor` There is no other logic that is executed using only a Rigidbody parameter outside of `Grabber` meaning I do not need to design unique outputs for Interaction besides the InteractionEventCtx
- `Grabber`: Grabber is a `FixedJoint` mutator that is injected with a Rigidbody from a reference holder that it 'grabs' onto by changing a `FixedJoint`s connectedBody variable. Every time the public business logic `StopGrab` is executed the `FixedJoint` null's its connected body. And when the Monobehaviour physics override `OnJointBreak()` occurs, the `Grabber` re-adds the `FixedJoint` because `FixedJoint`s remove themselves when they break.
- `PlayerController`: this script adds the functionality of WASD movement and Mouse Look w/ 90 -90 Y pitch and 360 degree seamless yaw. The MouseLook uses my own premade MouseLook from my `EMILTools` library in my `EMILTools.Extension` namespace called `MouseLookEX`. Where the body rotates laterally only and the head rotates vertically only. The WASD movement utilizes the `CharacterController` to quickly and simply move.
- `PickupSO`: this script is a ScriptableObject contains configuration data for Pickups. Most pertinent of the `PickupSO`'s member's are `pickupPrompt` as that is used by the `Interactor` and `InteractorEventCtx` The reason why I designed it this way is that `Interactor` doesn't have to rely on a specific `InteractableSO` and rather can use a string to get the prompt. And however the `Interactable` wants to give that string to the `Interactor` is up to it. The effect of this is that this decouples sending SO data to the `InteractorEventCtx`, giving it only what it needs, which is the prompt text.
- `CookwareSO`: currently only contains the `cookwarePrompt` string, continues de-coupled data to pass onto the `Interactor`
- `IngredientCombinationDb`: this is a ScriptableObject that contains a nested class serving as a storage data-type called `IngredientCombinationResult`. The databse contains a List of these combinations that when accessed through `TryGetResult()` outputs using the `out` keyword the outcome `PickupSO`
- `Ingredient`: this script is a collision detector for cross-ingredient collision detections. When an Ingredient collides with another it will check it against the `IngredientCombinationDb` for a potential result. If one is found, it optionally consumes both `Ingredients`. I designed it to allow for some ingredients to not be consumed when used. Like say a knife is used with a Tomato to make Pico, the knife obviously wouldn't be obliterated into the void. It would be optionally, not consumed. The outcome is Instantiated at the midpoint between the two ingredients that collided. There was an issue with having both ingredients call Destroy on eachother and themselves, which would be 4 Destroy calls on 2 objects. So I created a flag bool to mark if the outcome has been determined yet called `outcomeDetermined`, if true on both objects they have already had Destroy() called on them, so it exits early.
- `OrderTracker`: this script is a psuedo game manager that I made before having the customers because I can forsee how their state overall will be tracked to complete the level. This also gave me an oppertunity to give Logan some work. His initial iteration on the foreach he did not implement the if check, after a round of crit he implemented it properly.
- `Cookware` this script is an `abstract` stateful `Interactable` child class that contains direct coupling with its own progress bar. It also contains an outcome position to create the outcome each cookware will have. And of course its own configuration using `CookwareSO`. This script mainly serves as a base for children of itself to override `ProduceOutcome()`
- `MeatGrinder`: overrides `Cookware` to be a held down mouse event. Configured in the Inspector
- `Stove` overrides `Cookware` to be a start and stop mouse event with potential for burning an item. This introduces some time-management gameplay

My proposal has been utilized so far to help me reference what I need to do so I dont have to memorize too many things and I can focus on programming the systems.

Reflection Q Responses:
- Our project proposal was very detailed and served as a good baseline, but pretty soon after I made significant changes that made it out of date. I ended of modyfing the structure of the abstracts to remove uneccessary stuff and centralize more. For example `IInteractable` was changed to `abstract Interactable` which inherits from Monobehaviour. I needed to store the subscriber members directly on the interactable so I opted to not use the `interface` and start using a class. The reason I did this is because it started to hold "state." in the `subscribers`. And the "state" of the subscribers is transient in this case due to being able to unsubscribe and subscribe. Therefore when I saw transient state potentially living on an `interface` I knew this would violate the purpose of `interfaces` in general and make it hard to work with it. I know from experience that alot can be done with the tools I have in scripting, but trying to make certain tools do a job not meant for them will complicate the implementation to a high degree.
- Our tool-load is basically the same, but I have started to use my custom Pub/Sub system in my custom C# Library called `EMILTools`. It allows me to get around delegates being immutable and acting like valuetypes when I change them. That way I can unsubscribe to an eventhandler inside subscriber's event call which will modify the original location. This would be impossible with regular delegate types. I needed to do this for part of the `CookwareIngredientCombinationLoc` implementation's `OnHasAllIngredients` in order to alert the `Cookware` that it can be used.

### Logan Kane

As of 10/30/2026 I have contributed to the project in several ways, including but not limited to: Editing the Trello, Forming half-to-most-of the documentation necessary to start the project, Making a basic UI,  Programming the base of the interaction system (with Damien’s help) and contributed to the overall vision of the game. I feel the Proposal documentation was extremely helpful in getting my ideas down for the project. Without it, I would have needed a lot more time to think and iterate, this same principle goes for Trello and Github, due to their inherent utility for project management. As for how the game is built, the overall structure of the game has changed more in line with scalability, rather than the overall scope or mechanics of the game. The scalability is certainly a problem due to my limited knowledge and experience with coding, however, I am taking steps to rectify this issue with the help of my partner Damien. We have not imported any assets into our projects at this time, due to us wanting to work on the overall scope of the project and the code/mechanics first, before we move to the detailed work of art and making the game look pretty. This is not to say that we intend to leave the game a blank slate in the future, coding is simply a main priority at this time over art, given the subject of the class. We used scriptable objects for the different types of interactables, including the pickup items, which are polymorphed into their respective food items in the game. These food items will be combined with each other to make a new item, in which case we (assuming Damien and I are on the same page here) would simply just make a new item ID for that combination. As of now, we only have one-to-a-handful of items, however, we are continuing to expand and fulfill our scope from our proposal documentation.


<hr>

## Check-In Milestone 2 - Nov 26
### Group Report
Put your group check-in Devlog here.

### Damien Zemanek
Put your individual check-in Devlog here.

### Logan Kane
Put your individual check-in Devlog here.

<hr>

## Check-In Milestone 3 - Dec 17
### Group Report
Put your group check-in Devlog here.

### Damien Zemanek
Put your individual check-in Devlog here.

### Logan Kane
Put your individual check-in Devlog here.

<hr>

# Final Submission
Group Devlog
Put your group Devlog here.

### Damien Zemanek
Put your individual final Devlog here.

### Logan Kane
Put your individual final Devlog here.


# Open-Source Assets
Cite any open-source assets here. Put them in a LIST, and use correctly formatted LINKS.

# GDIM32-Final
<hr>

## Check-In Milestone 1 - Nov 5
### Group Report
Put your group check-in Devlog here.

### Damien Zemanek
10/5/2026
What I contributed to the project at this stage: 
- `Interactable`: this script is a context based subscription holder that is lightly coupled to `Interactor`. Meaning its only logical use case is tied to `Interactor`. I designed `Interactable` so that changes to `Interactor` do not propagate tech-debt into `Interactable` due to `Interactable` being transiently event based and holding no `Interactor` logic. I accomplished this by using a form of Inversion Of Control called Dependency Injection. Normal dependency Injection injects the dependency to be stored on the dependent, however I inject the dependency into the context layer to be used by subscriber event definitions. This is where my Pub/Sub custom system comes in which supports transient context injection into subscribers. Which is specifically made for use cases like this. `Interactable` is tightly coupled to `Grabber` because grabbing only happens due to logic executed in `Interactor` There is no other logic that is executed using only a Rigidbody parameter outside of `Grabber` meaning I do not need to design unique outputs for Interaction besides the InteractionEventCtx
- `Grabber`: Grabber is a `FixedJoint` mutator that is injected with a Rigidbody from a reference holder that it 'grabs' onto by changing a `FixedJoint`s connectedBody variable. Every time the public business logic `StopGrab` is executed the `FixedJoint` null's its connected body. And when the Monobehaviour physics override `OnJointBreak()` occurs, the `Grabber` re-adds the `FixedJoint` because `FixedJoint`s remove themselves when they break.
- `PlayerController`: this script adds the functionality of WASD movement and Mouse Look w/ 90 -90 Y pitch and 360 degree seamless yaw. The MouseLook uses my own premade MouseLook from my `EMILTools` library in my `EMILTools.Extension` namespace called `MouseLookEX`. Where the body rotates laterally only and the head rotates vertically only. The WASD movement utilizes the `CharacterController` to quickly and simply move.
- `PickupSO`: this script is a ScriptableObject contains configuration data for Pickups. Most pertinent of the `PickupSO`'s member's are `pickupPrompt` as that is used by the `Interactor` and `InteractorEventCtx` The reason why I designed it this way is that `Interactor` doesn't have to rely on a specific `InteractableSO` and rather can use a string to get the prompt. And however the `Interactable` wants to give that string to the `Interactor` is up to it. The effect of this is that this decouples sending SO data to the `InteractorEventCtx`, giving it only what it needs, which is the prompt text.
- `CookwareSO`: currently only contains the `cookwarePrompt` string, continues de-coupled data to pass onto the `Interactor`
- `IngredientCombinationDb`: this is a ScriptableObject that contains a nested class serving as a storage data-type called `IngredientCombinationResult`. The databse contains a List of these combinations that when accessed through `TryGetResult()` outputs using the `out` keyword the outcome `PickupSO`
- `Ingredient`: this script is a collision detector for cross-ingredient collision detections. When an Ingredient collides with another it will check it against the `IngredientCombinationDb` for a potential result. If one is found, it optionally consumes both `Ingredients`. I designed it to allow for some ingredients to not be consumed when used. Like say a knife is used with a Tomato to make Pico, the knife obviously wouldn't be obliterated into the void. It would be optionally, not consumed. The outcome is Instantiated at the midpoint between the two ingredients that collided. There was an issue with having both ingredients call Destroy on eachother and themselves, which would be 4 Destroy calls on 2 objects. So I created a flag bool to mark if the outcome has been determined yet called `outcomeDetermined`, if true on both objects they have already had Destroy() called on them, so it exits early.
- `OrderTracker`: this script is a psuedo game manager that I made before having the customers because I can forsee how their state overall will be tracked to complete the level. This also gave me an oppertunity to give Logan some work. His initial iteration on the foreach he did not implement the if check, after a round of crit he implemented it properly.
- `Cookware` this script is an `abstract` stateful `Interactable` child class that contains direct coupling with its own progress bar. It also contains an outcome position to create the outcome each cookware will have. And of course its own configuration using `CookwareSO`. This script mainly serves as a base for children of itself to override `ProduceOutcome()`
- `MeatGrinder`: overrides `Cookware` to be a held down mouse event. Configured in the Inspector
- `Stove` overrides `Cookware` to be a start and stop mouse event with potential for burning an item. This introduces some time-management gameplay

My proposal has been utilized so far to help me reference what I need to do so I dont have to memorize too many things and I can focus on programming the systems.

Reflection Q Responses:
- Our project proposal was very detailed and served as a good baseline, but pretty soon after I made significant changes that made it out of date. I ended of modyfing the structure of the abstracts to remove uneccessary stuff and centralize more. For example `IInteractable` was changed to `abstract Interactable` which inherits from Monobehaviour. I needed to store the subscriber members directly on the interactable so I opted to not use the `interface` and start using a class. The reason I did this is because it started to hold "state." in the `subscribers`. And the "state" of the subscribers is transient in this case due to being able to unsubscribe and subscribe. Therefore when I saw transient state potentially living on an `interface` I knew this would violate the purpose of `interfaces` in general and make it hard to work with it. I know from experience that alot can be done with the tools I have in scripting, but trying to make certain tools do a job not meant for them will complicate the implementation to a high degree.
- Our tool-load is basically the same, but I have started to use my custom Pub/Sub system in my custom C# Library called `EMILTools`. It allows me to get around delegates being immutable and acting like valuetypes when I change them. That way I can unsubscribe to an eventhandler inside subscriber's event call which will modify the original location. This would be impossible with regular delegate types. I needed to do this for part of the `CookwareIngredientCombinationLoc` implementation's `OnHasAllIngredients` in order to alert the `Cookware` that it can be used.

### Logan Kane

As of 10/30/2026 I have contributed to the project in several ways, including but not limited to: Editing the Trello, Forming half-to-most-of the documentation necessary to start the project, Making a basic UI,  Programming the base of the interaction system (with Damien’s help) and contributed to the overall vision of the game. I feel the Proposal documentation was extremely helpful in getting my ideas down for the project. Without it, I would have needed a lot more time to think and iterate, this same principle goes for Trello and Github, due to their inherent utility for project management. As for how the game is built, the overall structure of the game has changed more in line with scalability, rather than the overall scope or mechanics of the game. The scalability is certainly a problem due to my limited knowledge and experience with coding, however, I am taking steps to rectify this issue with the help of my partner Damien. We have not imported any assets into our projects at this time, due to us wanting to work on the overall scope of the project and the code/mechanics first, before we move to the detailed work of art and making the game look pretty. This is not to say that we intend to leave the game a blank slate in the future, coding is simply a main priority at this time over art, given the subject of the class. We used scriptable objects for the different types of interactables, including the pickup items, which are polymorphed into their respective food items in the game. These food items will be combined with each other to make a new item, in which case we (assuming Damien and I are on the same page here) would simply just make a new item ID for that combination. As of now, we only have one-to-a-handful of items, however, we are continuing to expand and fulfill our scope from our proposal documentation.


<hr>

## Check-In Milestone 2 - Nov 26
### Group Report
Put your group check-in Devlog here.

### Damien Zemanek
Put your individual check-in Devlog here.

### Logan Kane
Put your individual check-in Devlog here.

<hr>

## Check-In Milestone 3 - Dec 17
### Group Report
Put your group check-in Devlog here.

### Damien Zemanek
Put your individual check-in Devlog here.

### Logan Kane
Put your individual check-in Devlog here.

<hr>

# Final Submission
Group Devlog
Put your group Devlog here.

### Damien Zemanek
Put your individual final Devlog here.

### Logan Kane
Put your individual final Devlog here.


# Open-Source Assets
Cite any open-source assets here. Put them in a LIST, and use correctly formatted LINKS.
- [Pico Image](https://stock.adobe.com/ch_fr/images/bowl-of-tasty-pico-de-gallo-salsa-on-white-background/439779240)
- [Tomato Image](https://www.istockphoto.com/photos/tomato)
- [Knife Img](https://www.istockphoto.com/photos/knife)
- [Ground Beef Cooked Img](https://www.recipevibes.com/how-to-cook-ground-beef/)Ground Beef Img
- [Beef Img](https://hare-today.com/product/raw_ground/boneless_ground_beef_)
- [Burnt Food](https://www.linkedin.com/pulse/frequently-burned-food-restaurant-kitchens-michael-niksic/)
