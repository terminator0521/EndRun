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
    public class Zombie
    {
        private Texture2D texture;
        private Rectangle src; //texture source rectangle
        private Random random;
        public Rectangle dest; //dest rect
        public Vector2 pos; //current pos
        public Vector2 dis; //displacement
        public float distance; //distance between player and entity
        public float angle; //angle between player and entity 
        public float ratio; //cos ratio between player and entity
        public float vel = 1f; //entity velocity
        bool killed = false; //killed or not?
        
        private float downTime = 3f * 60f; //default value incase not set

        private float interval;

        public float DownTime //set downtime value in seconds
        {
            set { downTime = value * 60f; }
        }
       
        public Zombie(ref Random spawn)
        {
            random = spawn;

            dest = new Rectangle(pos, 40, 40); //create rectangle
        }

        public void Update(Vector2 playerPos)
        {
            //distance between player and entity
            distance = Vector2.Distance(pos, playerPos);

            //find trig ratio between player & entity
            ratio = (playerPos.X - pos.X) / distance;
            angle = MathF.Acos(ratio);

            //find displacement needed
            dis.Y = vel * MathF.Sin(angle) * (pos.Y < playerPos.Y ? 1 : -1);
            dis.X = vel * MathF.Cos(angle);

            //pos updates
            if (!killed)
            {
                pos += dis;
            }
            else
            {
                WaitForRespawn();
            }


            dest.X = pos.X;
            dest.Y = pos.Y;
        }

        public void Draw()
        {
            Raylib.DrawRectanglePro(dest, new Vector2(0, 0), 0, Color.Blue);
        }

        public void Kill()
        {
            killed = true; //set killed to true
            pos = new Vector2(-200); //move zombie offscreen
        }

        public void WaitForRespawn()
        {
            if(interval < downTime)
            {
                interval++; //increment counter
            }
            else
            {
                interval = 0; //reset counter
                killed = false; //set killed to false
                pos = random.Next(8) switch //random pos
                {
                    4 => new Vector2(220, -20),
                    0 => new Vector2(520, -20),
                    1 => new Vector2(820, -20),
                    7 => new Vector2(1120, -20),
                    5 => new Vector2(220, 650),
                    2 => new Vector2(520, 650),
                    3 => new Vector2(820, 650),
                    6 => new Vector2(1120, 650),
                    _ => throw new NotImplementedException(),
                };
            }
        }
    }
}
