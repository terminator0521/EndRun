using EndRun.Entities;
using EndRun.weapons.Guns;
using Raylib_cs;
using System.Numerics;
using System.Windows;

namespace EndRun.Weapons.Guns
{
    public class BallShot : Gun
    {
        override public Rectangle Laser { get; set; }
        override public float AimLaserWidth { get; set; }
        override public Texture2D Texture { get; set; }

        public float radius;
        public Vector2 ballPos;
        public Vector2 dis;

        private bool fired;
        private Rectangle bounds;
        private float vel;

        public BallShot(ref Rectangle bounds)
        {
            AimLaserWidth = 20f;
            energyUsage = 20;
            this.bounds = bounds;
            vel = 5f;
            radius = 20f;
        }

        override public void Update(Vector2 playerOriginPos, ref int score)
        {
            //Console.WriteLine(ballPos);
            Console.WriteLine(fired);
            base.Update(playerOriginPos, ref score);

            if (!Raylib.CheckCollisionCircleRec(ballPos, radius, bounds))
            {
                fired = false;
                dis = new Vector2(0);
            }

            if (fired)
            {
                ballPos += dis;
            }
            else
            {
                ballPos = new Vector2(-radius * 2);
            }
        }
        override public void Draw()
        {
            base.Draw();

            if (fired)
            {
                Raylib.DrawCircleV(ballPos, radius, Color.Green);
            }
        }

        public override void Shoot(ref List<Entity> entities, ref int energy)
        {

            if (energy >= energyUsage && !fired)
            {
                base.Shoot(ref entities, ref energy);
                fired = true;
                //ballpos change
                ballPos = playerOriginPos;
            }
            if (Raylib.CheckCollisionCircleRec(ballPos, radius, bounds) && dis == new Vector2(0))
            {
                //get displacement vector
                float distance = Vector2.Distance(playerOriginPos, Raylib.GetMousePosition());
                float cosRatio = (Raylib.GetMouseX() - playerOriginPos.X) / distance;
                float angle = MathF.Acos(cosRatio);
                dis.X = vel * MathF.Cos(angle);
                dis.Y = vel * MathF.Sin(angle) * (playerOriginPos.Y < Raylib.GetMouseY() ? 1 : -1);

            }

        }
    }
}
