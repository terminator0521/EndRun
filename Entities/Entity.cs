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
        protected Random random;
        virtual public Rectangle Dest { get; set; } //dest rect
        virtual protected int Height { get; set; }
        virtual protected int Width { get; set; }
        
        public Vector2 pos; //current pos
        public Vector2 dis; //displacement
        public float distance; //distance between player and entity
        public float angle; //angle between player and entity 
        public float ratio; //cos ratio between player and entity
        public float vel = 1f; //entity velocity
        protected bool killed = false; //killed or not?
        
        protected float downTime = 3f * 60f; //default value incase not set

        protected float interval;

        public float DownTime //set downtime value in seconds
        {
            set { downTime = value * 60f; }
        }
       
        public Entity(ref Random spawn)
        {
            random = spawn;
            Dest = new Rectangle(pos, 40, 40); //create rectangle
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


            Dest = new Rectangle(pos.X, pos.Y, Width, Height);
        }

        public void Draw()
        {
            Raylib.DrawRectanglePro(Dest, new Vector2(0, 0), 0, Color.Blue);
        }

        public void Kill()
        {
            killed = true; //set killed to true
            pos = new Vector2(-200); //move entity offscreen
            Console.WriteLine("killed");
        }

        virtual public bool WaitForRespawn()
        {
            if(interval < downTime)
            {
                interval++; //increment counter
                return false;
            }
            else
            {
                interval = 0; //reset counter
                killed = false; //set killed to false
                
                return true;
            }
        }

        public virtual void Respawn()
        {
            //meant to be overriden
        }
    }
}
