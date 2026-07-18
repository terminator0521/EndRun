using EndRun.Entities;
using Raylib_cs;
using System.IO.Pipelines;
using System.Numerics;

namespace EndRun.Melees
{
    
    public class Melee
    {
        public virtual float Radius { get; set; }
        public virtual Color Highlight { get; set; }
        public virtual float Damage { get; set; }
        public virtual Texture2D Texture { get; set; }
        public virtual Vector2 Center { get; set; }

        public virtual void Use(List<Zombie> zombies)
        {
            
        }

        public virtual void Draw()
        {
            Raylib.DrawCircleV(Center, Radius, Highlight);

        }

        public virtual void Update(Vector2 pos)
        {
            Center = pos;
        }


    }
}
