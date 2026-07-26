using EndRun.Entities;
using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Data;
using System.Numerics;
using System.Text;

namespace EndRun.weapons.Melees
{
    public class Blaster : Melee
    {
        public float thinkness = 4;
        public override float Radius { get; set; } = 60;
        public override Color Highlight { get; set; } = Color.White;
        public override float Damage { get; set; } = 20;
        public override Texture2D Texture { get; set; }

        public float radius = 60;

        public Blaster()
        {
            energyUsage = 20;
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
        public override void Draw()
        {
            Raylib.DrawRing(center, radius - thinkness, radius, 0, 360, 16, Highlight);
        }
    }
}
