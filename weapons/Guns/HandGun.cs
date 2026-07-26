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
        public override Rectangle Laser { get ; set; }


        public HandGun()
        {
            AimLaserWidth = 4f;
            energyUsage = 2;
        }

    }
}
