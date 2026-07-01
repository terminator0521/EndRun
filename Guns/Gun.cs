using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Windows.Documents;
using System.Xml;

namespace EndRun.Guns
{
    internal class Gun : IDisposable
    {
        public virtual Rectangle laser {get; set;}
        public virtual float AimLaserWidth { get; set; }
        public bool Aiming;
        public float distance;
        public float angle;
        public Vector2 playerOriginPos;

        public virtual void Update(Vector2 playerOriginPos)
        {

            this.playerOriginPos = playerOriginPos;

            if (Raylib.IsMouseButtonDown(MouseButton.Right))
            {
                Aiming = true;
                SetupLaser();
            }
            else
            {
                Aiming = false;
            }
        }
        public virtual void Draw()
        {
            if (Aiming)
            {
                Raylib.DrawRectanglePro(laser, new Vector2(0), -1 * (angle * 180 / MathF.PI) + (angle < 0 ? 180 : 0) + (playerOriginPos.Y - Raylib.GetMouseY() <= 0 ? 180 : 0) + (angle == 0 ? 180 : 0), Color.Red);
            }
        }

        public virtual void SetupLaser()
        {
            distance = Vector2.Distance(playerOriginPos, Raylib.GetMousePosition());
            angle = MathF.Atan((playerOriginPos.Y - Raylib.GetMouseY()) / (Raylib.GetMouseX() - playerOriginPos.X));
            laser = new Rectangle(playerOriginPos, distance, AimLaserWidth);
        }

        public virtual void Dispose()
        {
            Console.WriteLine("freed texture");
        }
    }
}
