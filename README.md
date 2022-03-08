![modula-banner5](https://user-images.githubusercontent.com/26601205/157169291-1604022b-12e5-4708-b990-b2951ab3245a.png)
## About
This asset brings the concept of Modular Entities into Unity Engine.

When needed, inherit from `ModularBehaviour` instead of Unity's default `MonoBehaviour` class.
This lets you **split** all the logic inside your behaviour and **move** it to independent modules, which could be later connected to/removed from each instance of that behaviour, using the inspector GUI:

![modula2](https://user-images.githubusercontent.com/26601205/157162945-b4b174e2-c7ce-4d8c-af3d-d07a0f27f20b.gif)

That way by splitting the code into small self-containing modules you could make the codebase cleaner and easier to understand.

# Setup
`work in progress`

## Creating Modules
To create a basic module, simply derive from `Module` class. As we imagine, a module should represent a relatively small self-contained part of your bigger entity/behaviour, such as:
- Engine/Transmission of a car
- Camera setup for the player
- Shooting logic for guns in your awesome FPS ;)
### Usage
_Imagine,_ you have the same setup for human characters that could run around a level in your game. The only difference is - most of them are controlled by AI and the last one is the player. By properly issuing the approach Modula offers you, changing that little part of how each 'human' is controlled should be straightforward :).

### Notice from the creator
> I hope Modula helps you to shift away from non-indicative naming e.g. using Controllers, Managers, Getters and Setters for everything. Of course I didn't bring anything exactly new to this world by publishing this package, but I hope that the vision it spreads will actually help you handle the frustration of managing the architecture of each system in your game. As any engineering pattern Modula has its shortcomings, but I believe it should be useful for SOME cases :)

The main purpose of _Modula_ is to nudge **you** to make better systems that consist of self-contained and re-usable parts.


