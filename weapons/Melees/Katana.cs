using EndRun.Entities;
using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Numerics;
using System.Text;

namespace EndRun.weapons.Melees
{
    public class Katana : Melee
    {
        public override float Radius { get; set; } = 60;
        public override Color Highlight { get; set; } = Color.White;
        public override float Damage { get; set; } = 20;
        public override Texture2D Texture { get; set; }
        public override Vector2 Center { get; set; }
        public override void Use(List<Entity> entity)
        {
            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                for (int i = 0; i < entity.Count; i++)
                {
                    if (Raylib.CheckCollisionCircleRec(Center, Radius, entity[i].Dest))
                    {
                        entity[i]?.Kill();
                        this.entity = entity[i];
                    }
                }
            }

        }
    }
}
