# Learn 2D Game Development with CSharp

Please visit: http://www.monogame.net/2017/03/01/monogame-3-6/

Download and install MonoGame for Visual Studio.

Download the complete project with solution and placement art, build and enjoy!

To try the game, please find the executable file in Game1/bin/DesktopGL/AnyCPU/Debug/

NOTES:

- In this second project I explored some new field while still improving some older features.

- The Sprite class now features not only basic movement (WASD keys), but also rotation (J and I keys).

- Camera can be controlled independently now (ARROW keys), and zoommed in/out (K and L keys).

- Learned the relationship between game world coordinate space and pixel coordinate space.

- A collision system was implemented. Sprites collide in two ways: AABB (axis-alligned bounding box) and pixel perfect (use carefully!). The collision system can act in all entities as needed, player-object or object-object.

- For the first time an Audio engine interface was created. The aim was for me to understand how to play/stop/resume sounds in a game.

- I implemented FontSupport and learned how to deal with printable characters (bitmap and truetype fonts).

- A new particle system (simpler one this time, could have use the previous particle system as well) was created to show the point of impact when sprites collided.

- Created the GameState and GameObject classes in order to practice good Object Oriented code, use inheritance and be able to reuse code more.

- Created a more robust input handling class (inputMapper) with more button options, and learned how to code deadzone and the pressure sensitive buttons on gamepads.

- Created a very simple AI-controlled enemies like the classes Chaser, Patrol, SpinningArrow. Some of those AI classes had auto-steering behaviors. This allowed me to learn more about vector math (dot, cross, angle conversions, normalize, etc).
