using EndRun.User;
using Raylib_cs;
using System.Net;
using System.Numerics;

namespace EndRun.Weapons
{
    public class Projectile
    {
        public bool active = false;
        public Vector2 projectilePos;
        public float radius;

        private Vector2 dis;
        private float angle;
        private float angleOffset;
        private float vel = 5;
        private Rectangle bounds;

        public Projectile(float angleOffset, float radius, ref Rectangle bounds)
        {
            angle = angleOffset * Raylib.DEG2RAD;
            this.angleOffset = angleOffset * Raylib.DEG2RAD;
            this.radius = radius;
            this.bounds = bounds;
        }

        public void Update()
        {
            if (active)
            {
                projectilePos += dis; //update pos
            }

            if (!Raylib.CheckCollisionCircleRec(projectilePos, radius, bounds))
            {
                Terminate();
            }
        }

        public void Draw()
        {
            if (active)
            {
                Raylib.DrawCircleV(projectilePos, radius, Color.DarkGreen); //draw if active
            }
        }

        public void Fire(ref Vector2 originalPos, ref float gunAngle)
        {
            if (!active) //initiallize members if unactive
            {
                active = true;
                projectilePos = originalPos;
                angle = (gunAngle * Raylib.DEG2RAD) + angle;
                dis.X = vel * MathF.Cos(angle);
                dis.Y = vel * MathF.Sin(angle) * (projectilePos.Y < originalPos.Y ? -1 : 1);
            }
        }

        public void Terminate()
        {
            //move offscreen
            angle = angleOffset;
            active = false;
            projectilePos = new Vector2(-radius * 2);
        }
    }
}
