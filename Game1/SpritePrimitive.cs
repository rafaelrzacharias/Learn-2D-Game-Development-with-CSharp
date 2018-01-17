using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game1
{
    class SpritePrimitive : GameObject
    {
        private int mNumRow, mNumColumn, mPaddings;
        private int mSpriteImageWidth, mSpriteImageHeight;

        private int mUserSpecifiedTicks; // number of game loops before changing to next frame
        private int mCurrentTick;
        private int mCurrentRow;
        private int mCurrentColumn;
        private int mBeginRow, mEndRow;
        private int mBeginCol, mEndCol;

        public SpritePrimitive(String imageName, Vector2 position, Vector2 size, int rowCount, int columnCount, int padding) :
            base(imageName, position, size)
        {
            mNumRow = rowCount;
            mNumColumn = columnCount;
            mPaddings = padding;
            mSpriteImageWidth = mImage.Width / mNumRow;
            mSpriteImageHeight = mImage.Height / mNumColumn;

            mUserSpecifiedTicks = 1;
            mCurrentTick = 0;
            mCurrentRow = 0;
            mCurrentColumn = 0;
            mBeginRow = mBeginCol = mEndRow = mEndCol = 0;
        }

        public int SpriteBeginRow
        {
            get { return mBeginRow; }
            set { mBeginRow = value; mCurrentRow = value; }
        }

        public int SpriteEndRow
        {
            get { return mEndRow; }
            set { mEndRow = value; }
        }

        public int SpriteBeginColumn
        {
            get { return mBeginCol; }
            set { mBeginCol = value; mCurrentColumn = value; } 
        }

        public int SpriteEndColumn
        {
            get { return mEndCol; }
            set { mEndCol = value; }
        }

        public int SpriteAnimationTicks
        {
            get { return mUserSpecifiedTicks; }
            set { mUserSpecifiedTicks = value; }
        }

        protected override int SpriteTopPixel
        {
            get { return mCurrentRow * mSpriteImageHeight; }
        }

        protected override int SpriteLeftPixel
        {
            get { return mCurrentColumn * mSpriteImageWidth; }
        }

        protected override int SpriteImageWidth
        {
            get { return mSpriteImageWidth; }
        }

        protected override int SpriteImageHeight
        {
            get { return mSpriteImageHeight; }
        }

        public void SetSpriteAnimation(int beginRow, int beginCol, int endRow, int endCol, int tickInterval)
        {
            mUserSpecifiedTicks = tickInterval;
            mBeginRow = beginRow;
            mBeginCol = beginCol;
            mEndRow = endRow;
            mEndCol = endCol;

            mCurrentRow = mBeginRow;
            mCurrentColumn = mBeginCol;
            mCurrentTick = 0;
        }

        public override void Update()
        {
            base.Update();

            mCurrentTick++;
            if (mCurrentTick > mUserSpecifiedTicks)
            {
                mCurrentTick = 0;
                mCurrentColumn++;
                if (mCurrentColumn > mEndCol)
                {
                    mCurrentColumn = mBeginCol;
                    mCurrentRow++;

                    if (mCurrentRow > mEndRow)
                        mCurrentRow = mBeginRow;
                }
            }
        }

        public override void Draw()
        {
            Rectangle destRect = Camera.ComputePixelRectangle(Position, Size);

            int imageTop = mCurrentRow * mSpriteImageWidth;
            int imageLeft = mCurrentColumn * mSpriteImageHeight;
            Rectangle srcRect = new Rectangle(imageLeft + mPaddings, imageTop + mPaddings, mSpriteImageWidth, mSpriteImageHeight);

            Vector2 origin = new Vector2(mSpriteImageWidth / 2, mSpriteImageHeight / 2);

            Game1.sSpriteBatch.Draw(mImage, destRect, srcRect, Color.White, mRotateAngle, origin, SpriteEffects.None, 0f);

            if (Label != null)
                FontSupport.PrintStatusAt(Position, Label, LabelColor);
        }
    }
}
