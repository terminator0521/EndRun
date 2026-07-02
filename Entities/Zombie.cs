using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace EndRun.Entities
{
    public class Zombie
    {
        public Texture2D texture;
        public Rectangle dest;
        public Vector2 pos;

        private Rectangle src;

        public Zombie(int pos)
        {
            this.pos = pos switch
            {
                0 => new Vector2(0, 0),
                1 => new Vector2(500, 200),
                _ => new Vector2(0, 0)
            };
        }
    }
}
