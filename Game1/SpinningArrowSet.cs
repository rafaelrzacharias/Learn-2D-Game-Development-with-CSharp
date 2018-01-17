using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Game1
{
    public class SpinningArrowSet
    {
        private const int NUM_ROWS = 4;
        private const int NUM_COLUMNS = 5;
        private List<SpinningArrow> mTheSet = new List<SpinningArrow>();

        public SpinningArrowSet()
        {
            Vector2 min = Camera.CameraWindowLowerLeftPosition;
            Vector2 max = Camera.CameraWindowUpperRightPosition;
            Vector2 size = max - min;
            float deltaX = size.X / (NUM_COLUMNS + 1);
            float deltaY = size.Y / (NUM_ROWS + 1);

            for (int r = 0; r < NUM_ROWS; r++)
            {
                min.Y += deltaY;
                float useDeltaX = deltaX;

                for (int c = 0; c < NUM_COLUMNS; c++)
                {
                    Vector2 pos = new Vector2(min.X + useDeltaX, min.Y);
                    SpinningArrow arrow = new SpinningArrow(pos);
                    mTheSet.Add(arrow);
                    useDeltaX += deltaX;
                }
            }
        }

        public void UpdateSpinningSet(TexturedPrimitive hero)
        {
            foreach (var arrow in mTheSet)
                arrow.UpdateSpinningArrow(hero);
        }

        public void DrawSet()
        {
            foreach (var arrow in mTheSet)
                arrow.Draw();
        }
    }
}