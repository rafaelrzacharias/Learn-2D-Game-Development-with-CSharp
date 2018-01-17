using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    public class PlayerControlHero : GameObject
    {
        public PlayerControlHero(Vector2 position) :
            base("KidLeft", position, new Microsoft.Xna.Framework.Vector2(7f, 8f))
        {
        }

        public void UpdateHero()
        {
            Vector2 delta = InputWrapper.ThumbSticks.Left;
            Position += delta;

            if (delta.X > 0)
                mImage = Game1.sContent.Load<Texture2D>("KidRight");
            else
                mImage = Game1.sContent.Load<Texture2D>("KidLeft");
        }
    }
}