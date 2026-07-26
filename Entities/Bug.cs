using Microsoft.Win32.SafeHandles;
using Raylib_cs;
using System.IO;
using System.Numerics;
using System.Windows.Input;
using System.Windows.Media.TextFormatting;

namespace EndRun.Entities
{
    public class Bug : Entity
    {
        override public int Score { get; set; } = 40;
        override protected Texture2D Texture { get; set; }
        override protected Rectangle Src { get; set; } //texture source rectangle
        override public Rectangle Dest { get; set; } //dest rect
        override protected int Height { get; set; }
        override protected int Width { get; set; }
        override protected float Vel { get; set; } = 5f; //entity 

        private bool settingUp = true; //is entity still moving into bounds

        //wait timer
        private float waitTime = 120f;
        private float waitTimeInterval = 0;
        private bool waiting = false;
        public float WaitTime
        {
            set { waitTime = value; }
        }

        //next move directions
        private enum Direction
        {
            right, down, left, up
        }
        private int currentDir; //direction entity is going to travel in
        private bool attacking = false;

        Rectangle bounds; //entity's working area
        private float firstInOffset = 100; //first in offset

        private float trackAccuracy = 10f;
        public Bug(int width, int height, ref Rectangle bounds)
        {
            Height = height;
            Width = width;
            this.bounds = bounds;
            Respawn();

            Dest = new Rectangle(pos.X, pos.Y, Width, Height);
        }

        override public void Update(Vector2 playerPos)
        {
            //if killed
            if (killed)
            {
                attacking = false;
                waiting = false;
            }

            //first spawn in movement
            if (settingUp)
            {
                if (Dest.X < bounds.X + firstInOffset) //to the right of bounds, so go right
                {
                    dis = new Vector2(Vel, 0);
                }
                else if (Dest.X + Width >= bounds.X + bounds.Width - firstInOffset) //to the left of bounds, so go left
                {
                    dis = new Vector2(-Vel, 0);
                }
                else
                {
                    dis = new Vector2(0);
                    settingUp = false;
                    waiting = true;
                    attacking = false;
                }
            }
            else //regular update logic
            {
                //attacking
                if (currentDir == (int)Direction.right || currentDir == (int)Direction.left)
                {
                    if (MathF.Abs(pos.X + (Width / 2) - playerPos.X + (Width / 2)) <= trackAccuracy)
                    {
                        waiting = true;
                        attacking = false;

                        //change dir
                        if (pos.Y < playerPos.Y)
                        {
                            currentDir = (int)Direction.down;
                        }
                        else
                        {
                            currentDir = (int)Direction.up;
                        }
                    }
                }
                else
                {
                    if (MathF.Abs(pos.Y + (Height / 2) - playerPos.Y + (Height / 2)) <= trackAccuracy)
                    {
                        waiting = true;
                        attacking = false;

                        //change dir
                        if (pos.X < playerPos.X)
                        {
                            currentDir = (int)Direction.right;
                        }
                        else
                        {
                            currentDir = (int)Direction.left;
                        }
                    }
                }
            }

            //movement logic
            switch (currentDir)
            {
                case (int)Direction.right:
                    dis = new Vector2(Vel, 0);
                    break;
                case (int)Direction.down:
                    dis = new Vector2(0, Vel);
                    break;
                case (int)Direction.left:
                    dis = new Vector2(-Vel, 0);
                    break;
                case (int)Direction.up:
                    dis = new Vector2(0, -Vel);
                    break;
            }


            if (!waiting)
            {
                base.Update(playerPos); //update pos
            }
            else
            {
                Waiting();
            }
        }

        protected override void Respawn()
        {
            pos = new Vector2(-200);

            if (WaitForRespawn())
            {

                switch (Random.Shared.Next(4)) //random pos
                {
                    case 0:
                        pos = new Vector2(-100, 100);
                        currentDir = (int)Direction.right;
                        break;
                    case 1:
                        pos = new Vector2(-100, 530);
                        currentDir = (int)Direction.right;
                        break;
                    case 2:
                        pos = new Vector2(1280 - Width, 100);
                        currentDir = (int)Direction.left;
                        break;
                    case 3:
                        pos = new Vector2(1280 - Width, 530);
                        currentDir = (int)Direction.left;
                        break;
                    default:
                        throw new NotImplementedException();
                }

                attacking = false;
                waiting = false;
                waitTimeInterval = 0;
                dis = new Vector2(0);
                settingUp = true;
            }
        }

        private void Waiting()
        {
            if (waiting)
            {
                if (waitTimeInterval < waitTime)
                {
                    waitTimeInterval++;
                }
                else
                {
                    waitTimeInterval = 0;
                    waiting = false;
                    attacking = true;
                }
            }

        }
    }
}
