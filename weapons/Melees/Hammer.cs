using EndRun.Entities;
using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace EndRun.weapons.Melees
{
    public class Hammer : Melee
    {
        public override Color Highlight { get; set; }
        public override float Damage { get; set; }
        public override Texture2D Texture { get; set; }
        public override Vector2 Center { get; set; }

        public Hammer()
        {
            energyUsage = 20;
        }

        public override void Use(ref List<Entity> entity, ref int energy)
        {
            if (Raylib.IsMouseButtonPressed(MouseButton.Left) && energy >= energyUsage)
            {
                base.Use(ref entity, ref energy);
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

        public override void Draw()
        {

        }
    }
}
