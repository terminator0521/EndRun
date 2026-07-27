using EndRun.User;
using Raylib_cs;
using System.Net;
using System.Numerics;

namespace EndRun.Weapons
{
    public class Projectile
    {
        public bool active;
        public Vector2 ProjectilePos;
        public float radius;

        private Vector2 dis;
        private float angle;
        private float vel = 5;
        private Rectangle bounds;

        public Projectile(float angleOffset, float radius, ref Rectangle bounds)
        {
            angle = angleOffset;
            this.radius = radius;
            this.bounds = bounds;
        }

        public void Update()
        {
            if (active)
            {
                ProjectilePos += dis; //update pos
            }
        }

        public void Draw()
        {
            if (active)
            { 
                Raylib.DrawCircleV(ProjectilePos, radius, Color.DarkGreen); //draw if active
            }
        }

        public void Fire(ref Vector2 originalPos, ref float gunAngle)
        {
            if(!active) //initiallize members if unactive
            {
                active = true;
                ProjectilePos = originalPos;
                angle = gunAngle + angle;
                dis.X = vel * MathF.Cos(angle);
                dis.Y = vel * MathF.Sin(angle) * -1;
            }
        }

        public void Terminate()
        {
            //move offscreen
            active = false;
            ProjectilePos = new Vector2(-radius * 2);
        }
    }
}
