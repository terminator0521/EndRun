using EndRun.Entities;
using Raylib_cs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace EndRun.weapons.Guns
{
    public class HandGun : Gun
    {
        public override float AimLaserWidth { get; set; }
        public override Rectangle Laser { get; set; }


        public HandGun()
        {
            AimLaserWidth = 4f;
            energyUsage = 2;
        }

        override public void Shoot(ref List<Entity> entities, ref int energy)
        {
            if (energy >= energyUsage && Aiming)
            {
                if (entities.Count == 1)
                {
                    entity = entities[0];
                }
                else
                {
                    for (int i = 1; i < entities.Count; i++)
                    {
                        if (entities[i].Distance < entities[i - 1].Distance)
                        {
                            entity = entities[i];
                        }
                        else
                        {
                            entity = entities[i - 1];
                        }
                    }
                }

                base.Shoot(ref entities, ref energy);
            }
        }
    }
}
