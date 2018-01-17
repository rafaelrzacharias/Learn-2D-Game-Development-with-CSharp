using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    public class GameState
    {
        public const float sGravity = 0.1f;
        const int kSpriteSpeedFactor = 10;    // Value of 1 maps to updates of 10 ticks

        ParticleSystem mParticleSystem;
        SpritePrimitive mHero;  // Hero sprite

        const int kNumPlanes = 4;
        TexturedPrimitive[] mPlane; // The Planes
        TexturedPrimitive mFlower;  // The large background

        // Support for displaying of collision
        TexturedPrimitive mHeroTarget; // Where latest hero pixel collision happened
        bool mHeroPixelCollision; // If there is a pixel collision for the hero
        bool mHeroBoundCollision; // If there is a image bound collision for the hero

        public GameState()
        {
            mParticleSystem = new ParticleSystem();

            AudioSupport.PlayBackgroundAudio("Mind_Meld", 0.4f);

            // Set up the flower ...
            mFlower = new TexturedPrimitive("Flower", new Vector2(50, 35), new Vector2(350, 350));

            // Planes
            mPlane = new TexturedPrimitive[kNumPlanes];
            mPlane[0] = new TexturedPrimitive("PatrolEnemy", new Vector2(20, -80), new Vector2(20, 40));
            mPlane[1] = new TexturedPrimitive("PatrolEnemy", new Vector2(150, -100), new Vector2(20, 40));
            mPlane[2] = new TexturedPrimitive("PatrolEnemy", new Vector2(150, 120), new Vector2(20, 40));
            mPlane[3] = new TexturedPrimitive("PatrolEnemy", new Vector2(20, 170), new Vector2(20, 40));

            mHeroTarget = new TexturedPrimitive("Target", new Vector2(0, 0), new Vector2(3, 3));
            mHeroBoundCollision = false;
            mHeroPixelCollision = false;

            mHero = new SpritePrimitive("SimpleSpriteSheet", new Vector2(10, 10), new Vector2(10, 10),
                            4,  // Number of rows
                            2,  // Number of columns
                            0); // Padding between images


            // Start Hero by walking left and AnotherHero by walking towards right
            mHero.SetSpriteAnimation(0, 0, 0, 3, 10); // Slowly
        }

        public void UpdateGame()
        {
            Vector2 heroMoveDelta = InputWrapper.ThumbSticks.Left;
            mHero.Position += heroMoveDelta;
            mHero.Update();

            CollisionUpdate();

            if (mHeroPixelCollision)
                mHero.Position -= heroMoveDelta;

            HeroMovingCameraWindow();
            UserControlUpdate();

            mParticleSystem.UpdateParticles();
        }

        private void HeroMovingCameraWindow()
        {
            Camera.CameraWindowCollisionStatus status = Camera.CollidedWithCameraWindow(mHero);
            Vector2 delta = Vector2.Zero;
            Vector2 cameraLL = Camera.CameraWindowLowerLeftPosition;
            Vector2 cameraUR = Camera.CameraWindowUpperRightPosition;
            const float kChaseRate = 0.05f;
            float kBuffer = mHero.Width * 2f;
            switch (status)
            {
                case Camera.CameraWindowCollisionStatus.CollideBottom:
                    delta.Y = (mHero.Position.Y - kBuffer - cameraLL.Y) * kChaseRate;
                    break;
                case Camera.CameraWindowCollisionStatus.CollideTop:
                    delta.Y = (mHero.Position.Y + kBuffer - cameraUR.Y) * kChaseRate;
                    break;
                case Camera.CameraWindowCollisionStatus.CollideLeft:
                    delta.X = (mHero.Position.X - kBuffer - cameraLL.X) * kChaseRate;
                    break;
                case Camera.CameraWindowCollisionStatus.CollideRight:
                    delta.X = (mHero.Position.X + kBuffer - cameraUR.X) * kChaseRate;
                    break;
            }
            Camera.MoveCameraBy(delta);
        }

        private void CollisionUpdate()
        {
            Vector2 pixelCollisionPosition = Vector2.Zero;

            #region Collide the hero with the flower
            mHeroBoundCollision = mHero.PrimitivesTouches(mFlower);
            mHeroPixelCollision = mHeroBoundCollision;
            if (mHeroBoundCollision)
            {
                mHeroPixelCollision = mHero.PixelTouches(mFlower, out pixelCollisionPosition);
                if (mHeroPixelCollision)
                {
                    mHeroTarget.Position = pixelCollisionPosition;
                    AudioSupport.PlayCue("Bounce");
                    mParticleSystem.AddEmitterAt(pixelCollisionPosition);
                }
            }
            #endregion

            #region Collide the hero with planes
            int i = 0;
            while ((!mHeroPixelCollision) && (i < kNumPlanes))
            {
                mHeroBoundCollision = mPlane[i].PrimitivesTouches(mHero);
                mHeroPixelCollision = mHeroBoundCollision;
                if (mHeroBoundCollision)
                {
                    mHeroPixelCollision = mPlane[i].PixelTouches(mHero, out pixelCollisionPosition);
                    if (mHeroPixelCollision)
                    {
                        mHeroTarget.Position = pixelCollisionPosition;
                        AudioSupport.PlayCue("Wall");
                        mParticleSystem.AddEmitterAt(pixelCollisionPosition);
                    }
                }
                i++;
            }
            #endregion
        }

        private void UserControlUpdate()
        {
            #region Specifying hero rotation
            if (InputWrapper.Buttons.X == ButtonState.Pressed)
                mHero.RotateAngleInRadian += MathHelper.ToRadians(1);
            if (InputWrapper.Buttons.Y == ButtonState.Pressed)
                mHero.RotateAngleInRadian += MathHelper.ToRadians(-1);
            #endregion

            #region Sprite Sheet Update
            if (InputWrapper.ThumbSticks.Left.X == 0)
            {
                mHero.SpriteEndColumn = 0;  // stops the animation
            }
            else
            {
                float useX = InputWrapper.ThumbSticks.Left.X;
                mHero.SpriteEndColumn = 3;
                if (useX < 0)
                {
                    mHero.SpriteBeginRow = 1;
                    mHero.SpriteEndRow = 1;
                    useX *= -1f;
                }
                else
                {
                    mHero.SpriteBeginRow = 0;
                    mHero.SpriteEndRow = 0;
                }
                mHero.SpriteAnimationTicks = (int)((1f - useX) * kSpriteSpeedFactor);
            }
            #endregion

            #region Camera Control
            if (InputWrapper.Buttons.A == ButtonState.Pressed)
                Camera.ZoomCameraBy(5);
            if (InputWrapper.Buttons.B == ButtonState.Pressed)
                Camera.ZoomCameraBy(-5);

            Camera.MoveCameraBy(InputWrapper.ThumbSticks.Right);
            #endregion
        }

        public void DrawGame()
        {
            mFlower.Draw();
            foreach (var p in mPlane)
                p.Draw();
            mHero.Draw();

            if (mHeroPixelCollision)
                mHeroTarget.Draw();

            mParticleSystem.DrawParticleSystem();

            FontSupport.PrintStatus("Collisions Bound(" +
                                mHeroBoundCollision + ") Pixel(" +
                                mHeroPixelCollision + ")", null);
        }
    }
}