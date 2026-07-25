using Raylib_cs;
using System.Numerics;

namespace EndRun.Entities
{
    public class Bat : Entity
    {
        override public int Score { get; set; } = 10;
        override protected Texture2D Texture { get; set; }
        override protected Rectangle Src { get; set; } //texture source rectangle
        override public Rectangle Dest { get; set; } //dest rect
        override protected int Height { get; set; }
        override protected int Width { get; set; }
        override protected float Vel { get; set; } = 1f; //entity velocity


        private bool attacking = true; //is attacking player 
        private Vector2 finalPos;

        //waiting timer members
        private float waitTimeInterval = 0;
        private float waitTime = 60f * 2f; //default value
        public float WaitTime //set external modifier for waitTime based on difficulty
        {
            set 
            { 
                waitTime = value * 60f; 
                waitTimeInterval = 0;
            }
        }

        //max distance can be travelled
        private float maxDistance = 300f;
        public float MaxDistance
        {
            set { MaxDistance = value; }
        }

        Vector2 startPos; //start position before attacking
        public Bat(int width, int height)
        {
            Height = height;
            Width = width;
            Respawn();

            Dest = new Rectangle(-200, 0, Width, Height);
        }

        override public void Update(Vector2 playerPos)
        {
            //wait phase
            if (!attacking) //wait phase starts when attack is finished
            {
                if (waitTimeInterval == waitTime || finalPos == new Vector2(0, 0)) //if wait time expires
                {
                    //set displacement vector
                    dis = CreatePath(playerPos);

                    //start attacking phase
                    attacking = true;
                    waitTimeInterval = 0;

                    //set start pos
                    startPos = pos - dis;
                }
                else
                {
                    waitTimeInterval++;
                }
            }


            //attacking phase
            if (Vector2.Distance(pos, finalPos) <= 10f || Vector2.Distance(pos, startPos) >= maxDistance) //if not at destination or travelled max distance
            {
                attacking = false;
            }
            else if (attacking) //if currently attacking
            {
                if (dis == new Vector2(0))
                {
                    attacking = false; //fix 0 displacement on run
                }

                base.Update(playerPos); //change position by displacement vector2
            }

            //change dest rect pos
            Dest = new Rectangle(pos, Width, Height);
        }

        private Vector2 CreatePath(Vector2 playerPos)
        {
            finalPos = playerPos;

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
                attacking = true;
                waitTimeInterval = waitTime;

                pos = Random.Shared.Next(6) switch //random pos
                {
                    0 => new Vector2(100, 40),
                    1 => new Vector2(630, 40),
                    2 => new Vector2(1180, 40),
                    3 => new Vector2(100, 600),
                    4 => new Vector2(630, 600),
                    5 => new Vector2(1180, 600),
                    _ => throw new NotImplementedException(),
                };
            }
        }

    }
}
