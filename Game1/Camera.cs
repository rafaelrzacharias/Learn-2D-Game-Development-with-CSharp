using Microsoft.Xna.Framework;

namespace Game1
{
    static public class Camera
    {
        static private Vector2 sOrigin = Vector2.Zero;      // Origin of the world
        static private float sWidth = 100f;                 // Width of the world
        static private float sRatio = -1f;                  // Ratio between camera window and pixel 
        static private float sHeight = -1f;

        static private float CameraWindowToPixelRatio()
        {
            if (sRatio < 0f)
            {
                sRatio = (float)Game1.sGraphics.PreferredBackBufferWidth / sWidth;
                sHeight = sWidth * (float)Game1.sGraphics.PreferredBackBufferHeight / (float)Game1.sGraphics.PreferredBackBufferWidth;
            }
            return sRatio;
        }

        static public void SetCameraWindow(Vector2 origin, float width)
        {
            sOrigin = origin;
            sWidth = width;
            CameraWindowToPixelRatio();
        }

        static public void ComputePixelPosition(Vector2 cameraPosition, out int x, out int y)
        {
            float ratio = CameraWindowToPixelRatio();

            // Convert the position to pixel space
            x = (int)(((cameraPosition.X - sOrigin.X) * ratio) + 0.5f);
            y = (int)(((cameraPosition.Y - sOrigin.Y) * ratio) + 0.5f);
            y = Game1.sGraphics.PreferredBackBufferHeight - y;
        }

        static public Rectangle ComputePixelRectangle(Vector2 position, Vector2 size)
        {
            float ratio = CameraWindowToPixelRatio();

            // Convert size from Camera Window Space to pixel space
            int width = (int)((size.X * ratio) + 0.5f);
            int height = (int)((size.Y * ratio) + 0.5f);

            // Convert the position to pixel space
            int x, y;
            ComputePixelPosition(position, out x, out y);

            return new Rectangle(x, y, width, height);
        }

        // Accesssors to the camera window bounds
        static public Vector2 CameraWindowLowerLeftPosition { get { return sOrigin; } }
        static public Vector2 CameraWindowUpperRightPosition { get { return sOrigin + new Vector2(sWidth, sHeight); } }

        // Support Collision with the camera bounds
        public enum CameraWindowCollisionStatus
        {
            CollideTop = 0,
            CollideBottom = 1,
            CollideLeft = 2,
            CollideRight = 3,
            InsideWindow = 4
        };

        static public CameraWindowCollisionStatus CollidedWithCameraWindow(TexturedPrimitive prim)
        {
            Vector2 min = CameraWindowLowerLeftPosition;
            Vector2 max = CameraWindowUpperRightPosition;

            if (prim.MaxBound.Y > max.Y)
                return CameraWindowCollisionStatus.CollideTop;
            if (prim.MinBound.X < min.X)
                return CameraWindowCollisionStatus.CollideLeft;
            if (prim.MaxBound.X > max.X)
                return CameraWindowCollisionStatus.CollideRight;
            if (prim.MinBound.Y < min.Y)
                return CameraWindowCollisionStatus.CollideBottom;

            return CameraWindowCollisionStatus.InsideWindow;
        }

        static public Vector2 RandomPosition()
        {
            float rangeX = 0.8f * sWidth;
            float offsetX = 0.1f * sWidth;
            float rangeY = 0.8f * sHeight;
            float offsetY = 0.1f * sHeight;

            float x = (float)(Game1.sRan.NextDouble()) * rangeX + offsetX + sOrigin.X;
            float y = (float)(Game1.sRan.NextDouble()) * rangeY + offsetY + sOrigin.Y;

            return new Vector2(x, y);
        }

        static public void MoveCameraBy(Vector2 delta)
        {
            sOrigin += delta;
        }

        static public void ZoomCameraBy(float deltaX)
        {
            float oldW = sWidth;
            float oldH = sHeight;

            sWidth = sWidth + deltaX;
            sRatio = -1f;
            CameraWindowToPixelRatio();

            float dx = 0.5f * (sWidth - oldW);
            float dy = 0.5f * (sHeight - oldH);
            sOrigin -= new Vector2(dx, dy);            
        }
    }
}
