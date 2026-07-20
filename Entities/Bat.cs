using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Numerics;
using System.Text;
using System.Windows.Media;

namespace EndRun.Entities
{
    public class Bat : Entity
    {
        override protected Texture2D Texture { get; set; }
        override protected Rectangle Src { get; set; } //texture source rectangle
        override public Rectangle Dest { get; set; } //dest rect
        override protected int Height { get; set; }
        override protected int Width { get; set; }
        override protected float Vel { get; set; } = 1f; //entity velocity


        private Rectangle gameBounds;
        private bool attacking = true; //is attacking player 
        private Vector2 finalPos;

        //waiting timer members
        private int waitTimeInterval;
        private int waitTime;
        public int WaitTime //set external modifier for waitTime based on difficulty
        {
            set { waitTime = value; }
        }
        public Bat(ref Random spawn, int width, int height, ref Rectangle gameBounds) : base(ref spawn)
        {
            Height = height;
            Width = width;
            random = spawn;
            this.gameBounds = gameBounds;
            Respawn();

            Dest = new Rectangle(0, 0, Width, Height);
        }

        public override void Update(Vector2 playerPos)
        {
            Console.WriteLine(attacking);
            //attacking phase
            if (Vector2.Distance(pos, finalPos) <= 10f) //if not at destination (final pos never set)
            {
                attacking = false;
            }
            else if (attacking) //if currently attacking
            {
                pos += dis; //change position by displacement vector2
            }

            //wait phase
            if (!attacking) //wait phase starts when attack is finished
            {
                if (waitTimeInterval == waitTime) //if wait time expires
                {
                    //set displacement vector
                    dis = CreatePath(playerPos);

                    //start attacking phase
                    attacking = true;
                    waitTimeInterval = 0;

                    //respawn entity
                    Respawn();
                }
                else
                {
                    waitTimeInterval++;
                }
            }
            

        }

        private Vector2 CreatePath(Vector2 playerPos)
        {
            Vector2 displacement;

            //distance between player and entity
            Distance = Vector2.Distance(pos, playerPos);

            //find trig ratio between player & entity
            ratio = (playerPos.X - pos.X) / Distance;
            angle = MathF.Acos(ratio);

            //find displacement needed & return it as a vector2
            displacement.Y = Vel * MathF.Sin(angle) * (pos.Y < playerPos.Y ? 1 : -1);
            displacement.X = Vel * MathF.Cos(angle);
            return displacement; 
        }

        protected override void Respawn()
        {
            if (WaitForRespawn())
            {
                pos = random.Next(1) switch //random pos
                {
                    0 => new Vector2(0, 0),
                    //1 => new Vector2(, ),
                    //2 => new Vector2(, ),
                    //3 => new Vector2(, ),
                    _ => throw new NotImplementedException(),
                };
            }
        }

    }
}
