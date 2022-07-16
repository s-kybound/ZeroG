# ZERO G - A ZERO GRAVITY FIRST PERSON SHOOTER
### Video Demo:  <URL https://youtu.be/0DFu8p_4HAs>
## Description:
ZERO G is a project that, at it's heart, was made to investigate how our descendants, as part of a fully-fledged space-faring civilization in the far future, would move about in space. That question molded the game mechanics that comprise the backbone of this game. This idea has been floating around in my head for a very long time. However, I realised that such a game could be easily turned into a (if not fun, at least interesting) FPS game. It may not be the first of its kind (I'm quite excited for *Boundary* when it comes out) but I sought to create a simple game that could help solidify that idea of movement in space. CS%) has given me the inspiration to finally create that game.

### Movement in space - Means of travel
To clarify, this game does not focus on the large space vehicles that will eventually carry us around space, but instead focuses on *extravehicular* movement, on individual people navigating across a large corridor, or an open construction site at an orbital shipyard, for example. Excluding the usage of ladders or handlebars, how would we move?

![alt text](https://upload.wikimedia.org/wikipedia/commons/thumb/9/91/Bruce_McCandless_II_during_EVA_in_1984.jpg/1024px-Bruce_McCandless_II_during_EVA_in_1984.jpg)

The MMU (Manned Maneuvering Unit) is a propulsion unit that allows astronauts to perform *untethered* navigations, away from a main vehicle. It is essentially a jetpack that allows you to thrust around space. However, one problem with it is that it has very little fuel capacity. The MMU has ~25 m/s of *delta-v* (cited from Wikipedia). In layman terms, it means that if you were to use the MMU to thrust in a single direction from a stationary start, your final speed will only be 25 m/s. In space, such a small delta-v capacity means that you have to compromise - you could travel far, but at *abysmal* speed, or you could travel quickly, but only in close proximity. Unfortunately, even with future advancements in fuel efficiency and energy storage, *powered* extravehicular navigation will be rather limited. Even at close distances, the use of powered propulsioun could be wasteful.

Hence, non-powered navigation is also necessary. Aside from catwalks and ladders and the like, a tether - a grappling hook - is, at least in my eyes, a possible complement to aid in extravehicular movement. Ignoring the risk of tension breaking the rope, firing a grappling hook and tugging at it to pull an object towards you (or vice versa) was a possible solution.

Hence, ZERO G aimed to implement, at the very least, a jetpack with *very* limited fuel as well as a reusable grapple hook that will serve as complements to you in moving about the levels.

### Movement in space - Misconceptions
One difference between space navigation as compared to navigation on old Earth is the lack of a resisting force - for example, there is no friction, or fluid resistance in the form of air resistance and the like. Without any such external force to act on it, an object that is moving at a certain velocity will continue moving in that velocity forever, as per Newton's First Law.

Another issue is that of relative velocity. It is difficult to interact with an object that is moving at a high relative velocity compared to you. For example, try getting one paper aeroplane to hit another in mid-air. In space, as well, with so many possible moving objects at different trajectories and velocities, you may find yourself forced to think differently in order to successfully interact with such objects. For example, how do you successfully grapple onto an object that is moving at a high relative velocity to you? You could match velocities, cancelling out the relative velocity and then treat the object as if it were stationary, aiming directly at it. Or, you could *lead* your shot, firing your grapple at a point where the object *will* be, intercepting it that way. In Zero G, for most cases, the object isn't moving, *you* are. There is no difference, as the relative velocity is the same regardless of who is moving.

ZERO G implements relative velocity on all of its objects. Your bullets and grapples will keep your initial velocity *as well as* their own forward velocities, which will force you to frame movement in different ways. This idea is better demonstrated in game rather than explained, however. Please enjoy using these two concepts to your own advantage in game.

## Implementation:
ZERO G is written in C# in Unity game engine. This is the first time I have used either C# or the Unity engine. I've learned a great lot from this experience.

### Grapple
I have to thank *DanisTutorials* for his implementation of the grappling hook, which I have heavily modified and turned into my own. His original script is in the /Assets/Scripts/Unused folder.

The GrapplingTool.cs program manages the creation of grapple projectiles. It checks for user input and behaves accordingly - it destroys any previous grapple and fires out a new grapple projectile. If a grapple projectile signals that it has hit an object that is grappleable (meaning we can grapple and swing on it), this file will generate a joint that acts as a spring/rope that allows us to swing or zip towards the object, hence successfully implementing a grapple.

The mos significant difference between my own script compared to the original is that the original used no projectiles, instead instantaneously connecting to the point where we aimed towards. To implement the difficulty of relative velocity, I instead opted for the usage of a slower projectile.

The GrappleProjectile.cs program manages the behaviour of grapple projectiles. It signals when it has collided with an object. As previously mentioned, if the object is grappleable, a spring/rope is formed. However, if the object is instead an item or another player, which are not defined in the game as grappleable, the projectile gives the object substantial velocity towards the player who fired it, implementing the tugging of objects towards you. For players who are hit, it will damage them as well.

The GrappleRotator.cs program is purely aesthetic, rotating the grapple tool in the current direction of the grapple if there is one present.

### Propulsion
The JetpackTool.cs program records a fuel capacity and current fuel. While there is still fuel, this program will, on user input, apply constant force on the player in the intended direction, accelerating the player. Fuel capacity is low, with 60 seconds on low thrust and a mere 20 on full thrust. While the delta-V of my implementation is somewhat higher, I believe it should serve well in the context of this game. The jetpack is intended for short bursts.

### Looking around
The PlayerCamera.cs program controls the orientation of the player. It has 6 DOF(degrees of freedom), allowing the player to orientate themselves any way they want, even upside down. Up or down is meaningless in space, and this program demonstrates that.

### FPS Elements
The rest of the programs serve to turn these basic functions into a full FPS game.

#### Pickups
The JetpackFuel.cs program manages jetpack fuel pickups around the map that refuel your jetpack. If you want to feel cool, instead of running into them, grapple them towards you.

#### Weapons
The grappling hook can serve as a weapon, but I have also implemented a dedicated weapon, the machine pistol. The PistolTool.cs fires bullet projectiles and gives them the velovity of the player as well as their own forward velocity.
Similar to the grapple projectile, the BulletProjectile.cs program manages bullets, ensuring they damage players on contact with them.

#### Health
Humanoid.cs is a generic health program that manages health for all players and AIs. PlayerHealth.cs manages what *happens* to a player based on his current health, such as death and respawn functions.

#### AI
EnemyGrapple.cs is an AI variant of the grapple projectile that was created due to some incompatible functions. It is reduced in function, being unable to grapple on objects or pull on items, but will still tug on and damage a player.

Thanks to "Dave / GameDevelopment" for his ideas in how to structure a capable AI program.
EnemyScript.cs combines some of the above functions, namely the grapple, pistol, health mannagement and propulsion functions, and implements them along with rudimentary AI to serve as an opponent for you, *in lieu* of multiplayer that hasn't been created yet.

There are four states that the AI can be in, based on the distance the player is to the AI. I will describe them in order of decreasing distance.
*Patrol* - The AI picks a random point, and accelerates towards it for half the journey. On the latter half of the journey, it will decelerate so that it ends up somewhere near the point at zero velocity. If the AI exceeds a set distance away from its original spawn point, it will select a point within that distance so that it does not deviate too much
*Intercept* - Written after *Grapple*, this state enjoys its benefits. The AI will calculate a lateral velocity, a component of the relative velocity between you and the AI which describes your movement perpendicular to the AI, and thrust to cancel that velocity, so that you are only moving purely towards or away from the AI. It then aims directly at you and thrusts forward to intercept you.
*Grapple* - I'm very proud of this one. Same as above, this calculates your lateral velocity. Using the lateral velocity, the AI perfectly calculates a direction in which it can fire its grapple such that it will hit your character even with high relative velocity. This implements the concept of "leading your shots" that I spoke of earlier, only that the AI can do this perfectly. The source code has more documentation and an in-depth explanation. This required me to relearn Junior College-level vector maths. A really enjoyable implementation!
*Attack* - Cancels out all relative velocity, aims directly at the player and fires bullets.

The AI, when killed, is programmed to respawn at a random vector away from a central spawn point to add more chaos.

I hope that you will find the AI enjoyable/challenging as well!

### Others
The rest of the programs handle framework such as GUI and scene loading.
SlayerLeaderboard.cs manages Slayer/Deathmatch displayed data as well as the gamemode itself. Slayer is currently the only gamemode in ZERO G. Kill the AI thrice to win. If it kills you thice, you lose.

## Difficulties / Lessons Learned
One current limitation of the game is its simplistic models, and lack of sound. I do not have prior experience with 3D modelling, so I was unable to provide anything of the sort in the time I challenged myself to complete this.
I'd like to say that the lack of sound is intentional, since there is no sound in space, however it was due to a lack of time and knowledge on where to find goodd resources as well. Future releases of ZERO G will have these implemented.
C# is also my first taste of object-oriented-programming. While I learned a lot from it, much of the code is inefficent because of a lack of experience with it, especially with access modifiers.
As I learned more effecient methods to solve problems with different methods provided by the Unity Engine, I tried to rewrite previous code to match. However, large parts of the code are written with different mindsets and ways to solve the same issues, as I was unable to catch some of these before they became *essential* to not breaking other parts of the code. At the very least, I believe I have rewritten it enough to implement multiplayer slightly easier now.

## What's in the future for ZERO G?
- Sound (not too unrealistic sounds though)
- Better models
- Multiplayer
- More efficient and clean code

## Conclusion
I've had a great deal of fun doing this as well as the rest of CS50. I'm thinking of starting on CS50G and CS50AI after this.
I've learned much from CS50, and am now fully committed on taking up a computer science course because of CS50. Thank you!

This was CS50!