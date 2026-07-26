using EndRun.Entities;
using Raylib_cs;
using System.IO.Pipelines;
using System.Numerics;

namespace EndRun.weapons.Melees
{
    
    public class Melee
    {
        public float thinkness = 4;
        public virtual float Radius { get; set; }
        public virtual Color Highlight { get; set; }
        public virtual float Damage { get; set; }
        public virtual Texture2D Texture { get; set; }
        public virtual Vector2 Center { get; set; }
        protected Entity? entity;
        public int scoreGain;
        public virtual void Use(List<Entity> entity)
        {
            
        }

        public virtual void Draw()
        {
            Raylib.DrawRing(Center, Radius - thinkness, Radius, 0, 360, 16, Highlight);

        }

        public virtual void Update(Vector2 pos, ref int score)
        {
            Center = pos;
            if (entity is not null) //score gain
            {
                score += entity.Score;
                entity = null;
            }
        }


    }
}
