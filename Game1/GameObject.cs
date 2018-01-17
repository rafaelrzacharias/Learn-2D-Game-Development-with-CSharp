﻿using System;
using Microsoft.Xna.Framework;

namespace Game1
{
    public class GameObject : TexturedPrimitive
    {
        // Initial front direction (when RotateAngle is 0)
        protected Vector2 mInitFrontDir = Vector2.UnitY;

        // GameObject behavior: velocity
        protected Vector2 mVelocityDir; // If not zero, always normalized
        protected float mSpeed;

        protected void InitGameObject()
        {
            mVelocityDir = Vector2.Zero;
            mSpeed = 0f;
        }

        public GameObject(String imageName, Vector2 position, Vector2 size, String label = null)
                : base(imageName, position, size, label)
        {
            InitGameObject();
        }

        virtual public void Update()
        {
            mPosition += (mVelocityDir * mSpeed);
        }

        public Vector2 InitialFrontDirection
        {
            get { return mInitFrontDir; }
            set
            {
                float len = value.Length();
                if (len > float.Epsilon) // If the input vector is well defined
                    mInitFrontDir = value / len;
                else
                    mInitFrontDir = Vector2.UnitY;
            }
        }

        public Vector2 FrontDirection
        {
            get
            {
                return ShowVector.RotateVectorByAngle(mInitFrontDir, RotateAngleInRadian);
            }
            set
            {
                float len = value.Length();
                if (len > float.Epsilon)
                {
                    value *= (1f / len);
                    double theta = Math.Atan2(value.Y, value.X);
                    mRotateAngle = -(float)(theta - Math.Atan2(mInitFrontDir.Y, mInitFrontDir.X));
                }
            }
        }

        public Vector2 Velocity
        {
            get { return mVelocityDir * Speed; }
            set
            {
                mSpeed = value.Length();
                if (mSpeed > float.Epsilon)
                    mVelocityDir = value / mSpeed;
                else
                    mVelocityDir = Vector2.Zero;
            }
        }

        public float Speed
        {
            get { return mSpeed; }
            set { mSpeed = value; }
        }

        public Vector2 VelocityDirection
        {
            get { return mVelocityDir; }
            set
            {
                float s = value.Length();
                if (s > float.Epsilon)
                {
                    mVelocityDir = value / s;
                }
                else
                    mVelocityDir = Vector2.Zero;
            }
        }

        public bool ObjectVisibleInCameraWindow()
        {
            Camera.CameraWindowCollisionStatus status = Camera.CollidedWithCameraWindow(this);
            return (status == Camera.CameraWindowCollisionStatus.InsideWindow);
        }
    }
}
