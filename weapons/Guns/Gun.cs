using EndRun.Entities;
using Raylib_cs;
using System.Numerics;

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
                Laser = new Rectangle(0, 0, 0, 0);
            }

        }
        public void Draw()
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
            Laser = new Rectangle(playerOriginPos, distance, AimLaserWidth);
        }

        public virtual void Dispose()
        {
            Console.WriteLine("freed texture");
        }

        public virtual void Shoot(List<Entity> entities)
        {
            Entity? zombie = null;

            if (entities.Count == 1)
            {
                zombie = entities[0];
            }
            else
            {
                for (int i = 1; i < entities.Count; i++)
                {
                    if (entities[i].Distance < entities[i - 1].Distance)
                    {
                        zombie = entities[i];
                    }
                    else
                    {
                        zombie = entities[i - 1];
                    }
                }
            }

            zombie?.Kill();

        }
    }
}
