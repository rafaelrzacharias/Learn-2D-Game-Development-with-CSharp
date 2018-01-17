using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using System;

namespace Game1
{
    public class Game1 : Game
    {
        static public SpriteBatch sSpriteBatch;  // Drawing support
        static public ContentManager sContent;   // Loading textures
        static public GraphicsDeviceManager sGraphics; // Current display size
        static public Random sRan; // for generating random numbers

        static public float RandomNumber(float n)
        {
            return (float)(sRan.NextDouble() * n);
        }
        static public float RandomNumber(float min, float max)
        {
            return min + ((max - min) * (float)(sRan.NextDouble()));
        }

        const int kWindowWidth = 1000;
        const int kWindowHeight = 700;

        GameState mMyGame;

        public Game1()
        {
            Content.RootDirectory = "Content";
            sContent = Content;

            // Create graphics device to access window size
            sGraphics = new GraphicsDeviceManager(this)
            {
                // Set prefer window size
                PreferredBackBufferWidth = kWindowWidth,
                PreferredBackBufferHeight = kWindowHeight
            };

            sRan = new Random();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            sSpriteBatch = new SpriteBatch(GraphicsDevice);

            // Define Camera Window Bounds
            Camera.SetCameraWindow(new Vector2(0f, 0f), 100f);

            mMyGame = new GameState();
        }

        protected override void Update(GameTime gameTime)
        {
            // Allow the game to exit
            if (InputWrapper.Buttons.Back == ButtonState.Pressed)
                Exit();

            mMyGame.UpdateGame();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            // Clear to background color
            GraphicsDevice.Clear(Color.CornflowerBlue);

            sSpriteBatch.Begin(); // Initialize drawing support

            mMyGame.DrawGame();

            sSpriteBatch.End();

            base.Draw(gameTime);
        }
    }
}