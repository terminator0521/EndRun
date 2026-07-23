using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows.Documents;
using System.Xaml;

namespace EndRun.Entities
{
    public class Entity
    {
        virtual protected Texture2D Texture { get; set; }
        virtual protected Rectangle Src { get; set; } //texture source rectangle
        virtual public Rectangle Dest { get; set; } //dest rect
        virtual protected int Height { get; set; }
        virtual protected int Width { get; set; }
        virtual protected float Vel { get; set; } = 1f; //entity velocity

        public Vector2 pos; //current pos
        public Vector2 dis; //displacement
        public float Distance { get; set; } //distance between player and entity
        public float angle; //angle between player and entity 
        public float ratio; //cos ratio between player and entity
        protected bool killed = true; //killed or not?

        protected float downTime = 3 * 60f; //default value incase not set
        protected float downTimeInterval;

        public Entity()
        {
            downTimeInterval = downTime;
        }

        public float DownTime //set downtime value in seconds
        {
            set { downTime = value * 60f; }
        }

        virtual public void Update(Vector2 playerPos)
        {

            //pos updates
            if (!killed)
            {
                pos += dis;
            }
            else
            {
                Respawn();
            }


            Dest = new Rectangle(pos.X + (Width / 2), pos.Y + (Height / 2), Width, Height);
        }

        public void Draw()
        {
            Raylib.DrawRectangleLinesEx(Dest, 4, Color.Blue);
        }

        public void Kill()
        {
            killed = true; //set killed to true
            pos = new Vector2(-200); //move entity offscreen
        }

        virtual protected bool WaitForRespawn()
        {
            if (downTimeInterval < downTime)
            {
                downTimeInterval++; //increment counter
                return false;
            }
            else
            {
                downTimeInterval = 0; //reset counter
                killed = false; //set killed to false

                return true;
            }
        }

        protected virtual void Respawn()
        {
            //meant to be overriden
        }
    }
}
