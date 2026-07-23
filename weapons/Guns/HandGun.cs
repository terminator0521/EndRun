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
        public int ammo;
        public override float AimLaserWidth { get; set; }
        public override Rectangle Laser { get ; set; }


        public HandGun(int ammo)
        {
            this.ammo = ammo;
            AimLaserWidth = 4f;
        }

    }
}
