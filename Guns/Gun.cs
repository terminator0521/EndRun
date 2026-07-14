using EndRun.Entities;
using Raylib_cs;
using System.Collections;
using System.IO.Packaging;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace EndRun.Guns
{
    public class Gun : IDisposable
    {
        public virtual Rectangle laser { get; set; }
        public virtual float AimLaserWidth { get; set; }
        public bool Aiming;
        public float distance;
        public float angle;
        public Vector2 playerOriginPos;

        public void Update(Vector2 playerOriginPos)
        {

            this.playerOriginPos = playerOriginPos;

            if (Raylib.IsMouseButtonDown(MouseButton.Right))
            {
                Aiming = true;
                Aim();
            }
            else
            {
                Aiming = false;
                laser = new Rectangle(0, 0, 0, 0);
            }

        }
        public void Draw()
        {
            if (Aiming)
            {
                Raylib.DrawRectanglePro(laser, new Vector2(0), -1 * angle, Color.Red);
            }
        }

        public void Aim()
        {

            distance = Vector2.Distance(playerOriginPos, Raylib.GetMousePosition());
            angle = MathF.Atan((playerOriginPos.Y - Raylib.GetMouseY()) / (Raylib.GetMouseX() - playerOriginPos.X));
            angle = (angle * 180 / MathF.PI) + (angle < 0 ? 180 : 0) + (playerOriginPos.Y - Raylib.GetMouseY() <= 0 ? 180 : 0) + (angle == 0 ? 180 : 0);
            laser = new Rectangle(playerOriginPos, distance, AimLaserWidth);
        }

        public virtual void Dispose()
        {
            Console.WriteLine("freed texture");
        }

        public virtual void Shoot(List<Zombie> zombies)
        {
            Zombie? zombie = null;

            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                Console.WriteLine(zombies.Count);
                if (zombies.Count == 1)
                { 
                    zombie = zombies[0]; 
                }
                else if (zombies.Count != 0)
                {
                    for (int i = 1; i < zombies.Count; i++)
                    {
                        if (zombies[i].distance < zombies[i - 1].distance)
                        {
                            zombie = zombies[i];
                        }
                        else
                        {
                            zombie = zombies[i - 1];
                        }
                    }
                }

                zombie?.Kill();
            }
        }
    }
}
