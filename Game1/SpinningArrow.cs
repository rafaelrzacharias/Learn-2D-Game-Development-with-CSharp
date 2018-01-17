using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    public class SpinningArrow : GameObject
    {
        public enum SpinningArrowState
        {
            ARROW_TRANSITION, ARROW_SPINNING, ARROW_POINT_TO_HERO
        }

        private SpinningArrowState mArrowState = SpinningArrowState.ARROW_TRANSITION;
        private float mSpinRate = 0f;

        private const float HERO_TRIGGER_DISTANCE = 15f;
        private const float MAX_SPIN_RATE = MathHelper.Pi / 10f;
        private const float DELTA_SPIN = MAX_SPIN_RATE / 200f;

        public SpinningArrow(Vector2 position) :
            base("TransientArrow", position, new Vector2(10, 4))
        {
            InitialFrontDirection = Vector2.UnitX;
        }

        private void SpinTheArrow()
        {
            RotateAngleInRadian += mSpinRate;
            if (RotateAngleInRadian > (2 * MathHelper.Pi))
                RotateAngleInRadian -= (2 * MathHelper.Pi);
        }

        public void UpdateSpinningArrow(TexturedPrimitive hero)
        {
            switch(mArrowState)
            {
                case SpinningArrowState.ARROW_TRANSITION:
                    UpdateTransitionState();
                    break;
                case SpinningArrowState.ARROW_SPINNING:
                    UpdateSpinningState(hero);
                    break;
                case SpinningArrowState.ARROW_POINT_TO_HERO:
                    UpdatePointToHero(hero.Position - Position);
                    break;
            }
        }

        private void UpdateTransitionState()
        {
            SpinTheArrow();

            if (mSpinRate < MAX_SPIN_RATE)
                mSpinRate += DELTA_SPIN;
            else
            {
                // Transition to spinning state
                mArrowState = SpinningArrowState.ARROW_SPINNING;
                mImage = Game1.sContent.Load<Texture2D>("RightArrow");
            }
        }

        private void UpdateSpinningState(TexturedPrimitive hero)
        {
            SpinTheArrow();

            Vector2 toHero = hero.Position - Position;
            if (toHero.Length() < HERO_TRIGGER_DISTANCE)
            {
                mSpinRate = 0f;
                mArrowState = SpinningArrowState.ARROW_POINT_TO_HERO;
                mImage = Game1.sContent.Load<Texture2D>("PointingArrow");
                UpdatePointToHero(toHero);
            }
        }

        private void UpdatePointToHero(Vector2 toHero)
        {
            float dist = toHero.Length();
            if (dist < HERO_TRIGGER_DISTANCE)
                FrontDirection = toHero;
            else
            {
                mArrowState = SpinningArrowState.ARROW_TRANSITION;
                mImage = Game1.sContent.Load<Texture2D>("TransientArrow");
            }
        }

        public SpinningArrowState ArrowState
        {
            get { return mArrowState; }
        }
    }
}
