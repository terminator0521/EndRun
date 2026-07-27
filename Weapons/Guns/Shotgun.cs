using EndRun.Entities;
using EndRun.weapons.Guns;
using Raylib_cs;
using System.Numerics;
using System.Windows;

namespace EndRun.Weapons.Guns
{
    public class Shotgun : Gun
    {
        override public Texture2D Texture { get; set; }
        public Projectile[] projectiles = new Projectile[5];

        private float radius = 60f;
        private float thickness = 50f;


        public Shotgun(ref Rectangle bounds)
        {
            for (int i = 0; i < 5; i++)
            {
                projectiles[i] = new Projectile(-30 + (i * 15), 20, ref bounds);
            }
        }
        override public void Update(Vector2 playerOriginPos, ref int score)
        {
            this.playerOriginPos = playerOriginPos;
        }

        override public void Draw(ref int selectedSlot)
        {
            if (Aiming && selectedSlot == 1)
            {
                Raylib.DrawRing(playerOriginPos, radius, radius + thickness, angle + 30, angle - 30, 5, Raylib.Fade(Color.White, 0.5f));
            }
        }

        override public void Shoot(ref List<Entity> entities, ref int energy)
        {
            if (Aiming )
            {

            }
        }

        override public void Aim()
        {
            //get angle 
            angle = MathF.Atan2(playerOriginPos.Y - Raylib.GetMouseY(), Raylib.GetMouseX() - playerOriginPos.X);
            angle *= -Raylib.RAD2DEG;
        }

        override public void Reset()
        {
            
        }
    }
}
