using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Text;

namespace EndRun.Guns
{
    public class HandGun : Gun
    {
        public int ammo;
        public override float AimLaserWidth { get; set; }
        public override Rectangle laser { get ; set; }


        public HandGun(int ammo)
        {
            this.ammo = ammo;
            this.AimLaserWidth = 10f;
        }
        
    }
}
