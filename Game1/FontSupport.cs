using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Game1
{
    static public class FontSupport
    {
        static private SpriteFont sTheFont = null;
        static private Color sDefaultDrawColor = Color.Black;
        static private Vector2 sStatusLocation = new Vector2(5, 5);

        static private void LoadFont()
        {
            // For demo purposes, loads Arial.spritefont
            if (null == sTheFont)
                sTheFont = Game1.sContent.Load<SpriteFont>("Arial");
        }

        static private Color ColorToUse(Nullable<Color> c)
        {
            return (null == c) ? sDefaultDrawColor : (Color)c;
        }

        static public void PrintStatusAt(Vector2 pos, String msg, Nullable<Color> drawColor)
        {
            LoadFont();

            Color useColor = ColorToUse(drawColor);

            Camera.ComputePixelPosition(pos, out int pixelX, out int pixelY);
            Game1.sSpriteBatch.DrawString(sTheFont, msg, new Vector2(pixelX, pixelY), useColor);
        }

        static public void PrintStatus(String msg, Nullable<Color> drawColor)
        {
            LoadFont();
            Color useColor = ColorToUse(drawColor);

            // Compute top left corner as the reference for output status
            Game1.sSpriteBatch.DrawString(sTheFont, msg, sStatusLocation, useColor);
        }
    }
}