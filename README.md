# Idle-Wave-Defense
Idle Tower Defense to learn Unity DOTS

I think the biggest issues with the project are having a MainReferences that I use for tying everything together, esentially turning all of those classes into singletons. However, I'm not sure of a much better system for a Unity project. I can find the instance through a string lookup, but I would still run into issues if I accidently added multiple to the project. I still could easily improve the current implementation by adding null checks to provide warnings/errors for multiple/null instances.

Another issue is how I handle dealing damage from enemies being hit by projectiles. I'm currently using an int counter and a while loop because I didn't know the Unity job system well enough. I now know there is a type of component that I can add multiple of to an entity and have multiple damage hits that way, that conforms to the entity component system better.

I'm unhappy with the lightning chainer system. I spent a week trying to think how to implement it, trying to come up with something besides the current implementation. The current implementation creates an array that is 250 (max numebr of enemies spawned) * 250 (max number of enemies that could be hit) = 62500 every frame. It then takes that array and does calculations from each enemy into a chain of every other enemy hitting each enemy at most 1 time resulting in up to 5.6 million calculations per frame, using a huge ammount of my available CPU power.
