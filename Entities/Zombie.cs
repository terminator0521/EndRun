using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Numerics;
using System.Text;
using System.Windows.Documents;

namespace EndRun.Entities
{
    public class Zombie : Entity
    {
        override protected Texture2D Texture { get; set; }
        override protected Rectangle Src { get; set; } //texture source rectangle

        override public Rectangle Dest { get; set; } //dest rect
        override protected int Height { get; set; }
        override protected int Width { get; set; }
        override protected float Vel { get; set; } = 1f; //entity velocity


        public Zombie(int width, int height)
        {
            Height = height;
            Width = width;
            Respawn();

            Dest = new Rectangle(0, 0, Width, Height);
        }

        override public void Update(Vector2 playerPos)
        {
            //distance between player and entity
            Distance = Vector2.Distance(pos, playerPos);

            //find trig ratio between player & entity
            ratio = (playerPos.X - pos.X) / Distance;
            angle = MathF.Acos(ratio);

            //find displacement needed
            dis.Y = Vel * MathF.Sin(angle) * (pos.Y < playerPos.Y ? 1 : -1);
            dis.X = Vel * MathF.Cos(angle);

            //update positions and run killed? logic
            base.Update(playerPos);
        }

        protected override void Respawn()
        {
            if (WaitForRespawn())
            {
                Console.WriteLine("ran");
                pos = Random.Shared.Next(8) switch //random pos
                {
                    4 => new Vector2(220, -40),
                    0 => new Vector2(520, -40),
                    1 => new Vector2(820, -40),
                    7 => new Vector2(1120, -40),
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
