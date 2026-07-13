using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Xaml;

namespace EndRun.Entities
{
    public class Zombie
    {
        public Texture2D texture;
        private Rectangle src; //texture source rectangle
        public Rectangle dest; //dest rect
        public Vector2 pos; //current pos
        public Vector2 dis; //displacement
        public float distance; //distance between player and entity
        public float angle; //angle between player and entity 
        public float ratio; //cos ratio between player and entity
        public float vel = 1f; //entity velocity
        bool killed = false;

       
        public Zombie(int spawnPoint)
        {
            pos = spawnPoint switch
            {
                0 => new Vector2(460, 100),
                1 => new Vector2(920, 100),
                2 => new Vector2(460, 600),
                3 => new Vector2(920, 600),
                4 => new Vector2(60, 510),
                5 => new Vector2(1200, 510),
                6 => new Vector2(60, 260),
                7 => new Vector2(1200, 260),
            };

            dest = new Rectangle(pos, 40, 40);
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
                pos = new Vector2(-200);
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
            killed = true;
        }

    }
}
