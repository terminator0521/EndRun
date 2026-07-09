using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace EndRun.Entities
{
    public class Zombie
    {
        public Texture2D texture;
        public Rectangle dest; //dest rect
        public Vector2 pos; //current pos
        public Vector2 dis; //displacement
        public float distance; //distance between player and entity
        public float angle; //angle between player and entity 
        public float ratio; //cos ratio between player and entity
        public float vel = 1f; //entity velocity

        private Rectangle src;

        public Zombie(int spawnPoint)
        {
            pos = spawnPoint switch
            {
                0 => new Vector2(-60, 0),
                1 => new Vector2(500, 200),
                _ => new Vector2(700, 1200)
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

            dis.Y = vel * MathF.Sin(angle) * (pos.Y < playerPos.Y ? 1 : -1);
            dis.X = vel * MathF.Cos(angle);

            //pos += dis;
            //dest.X = pos.X;
            //dest.Y = pos.Y;
        }

        public void Draw()
        {
            Raylib.DrawRectanglePro(dest, new Vector2(0, 0), 0, Color.Blue);
        }
    }
}
