using EndRun.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media.Media3D;

namespace EndRun
{
    public struct Difficulties
    {
        public readonly float spawnTime;
        public readonly int zombieCount;
        public readonly int batCount;
        public readonly int bugCount;
        public Difficulties(float spawnTime, int zombieCount, int batCount, int bugCount)
        {
            this.spawnTime = spawnTime;
            this.zombieCount = zombieCount;
            this.batCount = batCount;
            this.bugCount = bugCount;
        }
    }
}
