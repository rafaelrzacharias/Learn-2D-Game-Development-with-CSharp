using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    public class PatrolEnemy : GameObject
    {
        private Vector2 mTargetPosition;
        private PatrolState mCurrentState;
        private int mStateTimer;

        private const float PATROL_SPEED = 0.3f;
        private const float CLOSE_ENOUGH = 5f;
        private const float BORDER_RANGE = 0.45f;
        private const int STATE_TIMER = 60 * 5; // Around 5 sec, if FPS is 60
        private const float DIST_TO_BEGIN_CHASE = 15f;

        protected enum PatrolState
        {
            PATROL, CHASE
        }

        public PatrolEnemy() :
            base("PatrolEnemy", Vector2.Zero, new Vector2(5f, 10f))
        {
            InitialFrontDirection = Vector2.UnitY;
            mTargetPosition = Position = Vector2.Zero;

            RandomNextTarget();
            Position = mTargetPosition;
        }

        private void RandomNextTarget()
        {
            mStateTimer = STATE_TIMER;
            mCurrentState = PatrolState.PATROL;

            double initState = Game1.sRan.NextDouble();
            if (initState < 0.25)
                mTargetPosition = RandomBottomRightPosition();
            else if (initState < 0.5)
                mTargetPosition = RandomTopRightPosition();
            else if (initState < 0.75)
                mTargetPosition = RandomTopLeftPosition();
            else
                mTargetPosition = RandomBottomLeftPosition();

            ComputeNewSpeedAndResetTimer();
        }

        public bool UpdatePatrol(GameObject hero)
        {
            bool caught = false;

            base.Update();
            mStateTimer--;

            Vector2 toTarget = mTargetPosition - Position;
            float distToTarget = toTarget.Length();
            toTarget /= distToTarget;
            ComputeNewDirection(toTarget);

            switch (mCurrentState)
            {
                case PatrolState.PATROL:
                    UpdatePatrolState(hero, distToTarget);
                    break;

                case PatrolState.CHASE:
                    caught = UpdateChaseState(hero, distToTarget);
                    break;
            }
            return caught;
        }

        private void DetectHero(GameObject hero)
        {
            Vector2 toHero = hero.Position - Position;
            if (toHero.Length() < DIST_TO_BEGIN_CHASE)
            {
                mStateTimer = (int)(STATE_TIMER * 1.2f); // 1.2x as much time for chasing
                Speed *= 2.5f; // 2.5x the current speed!
                mCurrentState = PatrolState.CHASE;
                mTargetPosition = hero.Position;
                mImage = Game1.sContent.Load<Texture2D>("AlertEnemy");
            }
        }

        private void UpdatePatrolState(GameObject hero, float distToTarget)
        {
            if ((mStateTimer < 0) || (distToTarget < CLOSE_ENOUGH))
            {
                RandomNextTarget();
                ComputeNewSpeedAndResetTimer();
            }
            DetectHero(hero);
        }

        private bool UpdateChaseState(GameObject hero, float distToTarget)
        {
            bool caught = false;
            caught = PixelTouches(hero, out Vector2 pos);
            mTargetPosition = hero.Position;

            if (caught || (mStateTimer < 0))
            {
                RandomNextTarget();
                mImage = Game1.sContent.Load<Texture2D>("PatrolEnemy");
            }
            return caught;
        }

        private Vector2 ComputePoint(double xOffset, double yOffset)
        {
            Vector2 min = Camera.CameraWindowLowerLeftPosition;
            Vector2 max = Camera.CameraWindowUpperRightPosition;
            Vector2 size = max - min;
            float x = min.X + size.X * (float)(xOffset + (BORDER_RANGE * Game1.sRan.NextDouble()));
            float y = min.Y + size.Y * (float)(yOffset + (BORDER_RANGE * Game1.sRan.NextDouble()));
            return new Vector2(x, y);
        }

        private Vector2 RandomBottomRightPosition() { return ComputePoint(0.5, 0.0); }

        private Vector2 RandomBottomLeftPosition() { return ComputePoint(0.0, 0.0); }

        private Vector2 RandomTopRightPosition() { return ComputePoint(0.5, 0.5); }

        private Vector2 RandomTopLeftPosition() { return ComputePoint(0.0, 0.5); }

        private void ComputeNewDirection(Vector2 toTarget)
        {
            double cosTheta = Vector2.Dot(toTarget, FrontDirection);
            float theta = (float)Math.Acos(cosTheta);
            if (theta > float.Epsilon)
            {
                Vector3 frontDir3 = new Vector3(FrontDirection, 0f);
                Vector3 toTarget3 = new Vector3(toTarget, 0f);
                Vector3 zDir = Vector3.Cross(frontDir3, toTarget3);
                RotateAngleInRadian -= Math.Sign(zDir.Z) * 0.03f * theta; // Rotate by 3% each frame
                VelocityDirection = FrontDirection;
            }
        }

        private void ComputeNewSpeedAndResetTimer()
        {
            Speed = PATROL_SPEED * (0.8f + (float)(0.4f * Game1.sRan.NextDouble())); // Speed ranges from 80% to 120%
            mStateTimer = (int)(STATE_TIMER * (0.8f + (float)(0.6f * Game1.sRan.NextDouble())));
        }
    }
}