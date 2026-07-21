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
        public readonly float batWaitTime;
        public readonly int batCount;
        public readonly float bugWaitTime;
        public readonly int bugCount;
        public Difficulties(float spawnTime, int zombieCount, float batWaitTime, int batCount, float bugWaitTime, int bugCount)
        {
            this.spawnTime = spawnTime;
            this.zombieCount = zombieCount;
            this.batWaitTime = batWaitTime;
            this.batCount = batCount;
            this.bugWaitTime = bugWaitTime;
            this.bugCount = bugCount;
        }
    }
}
