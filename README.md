# GDIM32-Final
<hr>

## Check-In Milestone 1 - Nov 5
### Group Report
Put your group check-in Devlog here.

### Damien Zemanek
10/5/2026
What I contributed to the project at this stage: 


My proposal has been utilized so far to help me reference what I need to do so I dont have to memorize too many things and I can focus on programming the systems.

Reflection Q Responses:
- Our project proposal was very detailed and served as a good baseline, but pretty soon after I made significant changes that made it out of date. I ended of modyfing the structure of the abstracts to remove uneccessary stuff and centralize more. For example `IInteractable` was changed to `abstract Interactable` which inherits from Monobehaviour. There was too much template behaviour I was seeing on the `interface` so I refactored it as a class.
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
