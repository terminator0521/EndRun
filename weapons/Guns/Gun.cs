using EndRun.Entities;
using Raylib_cs;
using System.Numerics;
using System.Threading.Tasks.Sources;
using System.Windows;

namespace EndRun.weapons.Guns
{
    public class Gun : IDisposable
    {
        public virtual Rectangle Laser { get; set; }
        public virtual float AimLaserWidth { get; set; }
        public virtual Texture2D Texture { get; set; }
        public bool Aiming;
        public float distance;
        public float angle;
        public Vector2 playerOriginPos;
        public Vector2 center;

        protected int energyUsage;
        protected Entity? entity;

        virtual public void Update(Vector2 playerOriginPos, ref int score)
        {
            Laser = new Rectangle(0, 0, 0, 0);
            this.playerOriginPos = playerOriginPos;

            if (entity is not null) //score gain
            {
                score += entity.Score;
                entity = null;
            }
        }
        public virtual void Draw()
        {
            if (Aiming)
            {
                Raylib.DrawRectanglePro(Laser, new Vector2(0), -1 * angle, Color.Red);
            }
        }

        public void Aim()
        {
            distance = Vector2.Distance(playerOriginPos, Raylib.GetMousePosition());
            angle = MathF.Atan((playerOriginPos.Y - Raylib.GetMouseY()) / (Raylib.GetMouseX() - playerOriginPos.X));
            angle = (angle * 180 / MathF.PI) + (angle < 0 ? 180 : 0) + (playerOriginPos.Y - Raylib.GetMouseY() <= 0 ? 180 : 0) + (angle == 0 ? 180 : 0);
            center = playerOriginPos; //initialize center with player pos
            center.Y -= (AimLaserWidth / 2) * MathF.Cos(angle * Raylib.DEG2RAD);
            center.X -= (AimLaserWidth / 2) * MathF.Sin(angle * Raylib.DEG2RAD);
            Laser = new Rectangle(center, distance, AimLaserWidth);
        }

        public virtual void Dispose()
        {
            Console.WriteLine("freed texture");
        }

        public virtual void Shoot(ref List<Entity> entities, ref int energy)
        {
            entity?.Kill();
            energy -= energyUsage;
        }

    }
}

