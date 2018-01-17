using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    public partial class TexturedPrimitive
    {
        // Support for drawing the image
        protected Texture2D mImage;     // The UWB-JPG.jpg image to be loaded
        protected String mImageName;
        protected Vector2 mPosition;    // Center position of image
        protected Vector2 mSize;        // Size of the image to be drawn
        protected float mRotateAngle;   // In Radians, clockwise rotation
        protected String mLabelString;  // String to draw
        protected Color mLabelColor = Color.Black;
        protected Color mTintColor;

        protected void InitPrimitive(String imageName, Vector2 position, Vector2 size, String label = null)
        {
            mImage = Game1.sContent.Load<Texture2D>(imageName);
            mImageName = imageName;
            mPosition = position;
            mSize = size;
            mRotateAngle = 0f;
            mLabelString = label;
            mTintColor = Color.White;
            ReadColorData();    // For Pixel-level collision support
        }

        public TexturedPrimitive(String imageName, Vector2 position, Vector2 size, String label = null)
        {
            InitPrimitive(imageName, position, size, label);
        }

        public TexturedPrimitive(String imageName)
        {
            InitPrimitive(imageName, Vector2.Zero, new Vector2(1f, 1f));
        }

        // Accessors
        public Vector2 Position { get { return mPosition; } set { mPosition = value; } }
        public float PositionX { get { return mPosition.X; } set { mPosition.X = value; } }
        public float PositionY { get { return mPosition.Y; } set { mPosition.Y = value; } }
        public Vector2 Size { get { return mSize; } set { mSize = value; } }
        public float Width { get { return mSize.X; } set { mSize.X = value; } }
        public float Height { get { return mSize.Y; } set { mSize.Y = value; } }
        public Vector2 MinBound { get { return mPosition - (0.5f * mSize); } }
        public Vector2 MaxBound { get { return mPosition + (0.5f * mSize); } }
        public float RotateAngleInRadian { get { return mRotateAngle; } set { mRotateAngle = value; } }
        public String Label { get { return mLabelString; } set { mLabelString = value; } }
        public Color LabelColor { get { return mLabelColor; } set { mLabelColor = value; } }

        protected virtual int SpriteTopPixel { get { return 0; } }
        protected virtual int SpriteLeftPixel { get { return 0; } }
        protected virtual int SpriteImageWidth { get { return mImage.Width; } }
        protected virtual int SpriteImageHeight { get { return mImage.Height; } }

        public void Update(Vector2 deltaTranslate, Vector2 deltaScale, float deltaAngleInRadian)
        {
            mPosition += deltaTranslate;
            mSize += deltaScale;
            mRotateAngle += deltaAngleInRadian;
        }

        public bool PrimitivesTouches(TexturedPrimitive otherPrim)
        {
            if ((Math.Abs(RotateAngleInRadian) < float.Epsilon) &&
                (Math.Abs(otherPrim.RotateAngleInRadian) < float.Epsilon))
            {
                // no rotations involved ...: check for bound overlaps
                Vector2 myMin = MinBound;
                Vector2 otherMin = otherPrim.MinBound;

                Vector2 myMax = MaxBound;
                Vector2 otherMax = otherPrim.MaxBound;

                return
                    ((myMin.X < otherMax.X) && (myMax.X > otherMin.X) &&
                     (myMin.Y < otherMax.Y) && (myMax.Y > otherMin.Y));
            }
            else
            {
                // One of both are rotated ... use radius ... be conservative
                // Use the larger of the Width/Height and approx radius
                //   Sqrt(1/2)*x Approx = 0.71f * x;
                float r1 = 0.71f * MathHelper.Max(Size.X, Size.Y);
                float r2 = 0.71f * MathHelper.Max(otherPrim.Size.X, otherPrim.Size.Y);
                return ((otherPrim.Position - Position).Length() < (r1 + r2));
            }
        }

        virtual public void Draw()
        {
            // Define location and size of the texture
            Rectangle destRect = Camera.ComputePixelRectangle(Position, Size);

            // Define the rotation origin
            Vector2 org = new Vector2(mImage.Width / 2, mImage.Height / 2);

            // Draw the texture
            Game1.sSpriteBatch.Draw(mImage,
                            destRect,           // Area to be drawn in pixel space
                            null,               //
                            mTintColor,         // 
                            mRotateAngle,       // Angle to rotate (clockwise)
                            org,                // Image reference position
                            SpriteEffects.None, 0f);

            if (null != Label)
                FontSupport.PrintStatusAt(Position, Label, LabelColor);
        }
    }
}
