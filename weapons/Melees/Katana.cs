using EndRun.Entities;
using raygui_cs;
using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media.Media3D;

namespace EndRun.weapons.Melees
{
    public class Katana : Melee
    {
        public override Color Highlight { get; set; }
        public override float Damage { get; set; }
        public override Texture2D Texture { get; set; }

        public float angle;
        public Rectangle guide;

        private float range;
        private float width;
        private float offset;
        public float angleD;

        public Katana()
        {
            energyUsage = 15;
            range = 50;
            width = 80;
            guide.Height = width;
            guide.Width = range;
            offset = 30;
        }

        public override void Use(ref List<Entity> entity, ref int energy)
        {
            if (energy >= energyUsage)
            {
                base.Use(ref entity, ref energy);
                for (int i = 0; i < entity.Count; i++)
                {
                    entity[i]?.Kill();
                    this.entity = entity[i];
                }
            }
        }

        public override void Draw(ref int selectedSlot)
        {
            if (selectedSlot == 0)
            {
                Raylib.DrawRectanglePro(guide, new Vector2(0), angle, Raylib.Fade(Color.Blue, 0.3f));
            }
        }

        public override void Update(Vector2 pos, ref int score)
        {
            base.Update(pos, ref score);
            angle = MathF.Atan2(pos.Y - Raylib.GetMouseY(), Raylib.GetMouseX() - pos.X);
            center = pos; //initialize center with player pos
            center.Y -= ((Radius + range + offset) * MathF.Sin(angle)) + (width / 2 * MathF.Cos(angle));
            center.X += ((Radius + range + offset) * MathF.Cos(angle)) - (width / 2 * MathF.Sin(angle));
            angle *= Raylib.RAD2DEG;
            angle = -angle + 90;
            guide = new Rectangle(center, width, range);
        }
    }
}
