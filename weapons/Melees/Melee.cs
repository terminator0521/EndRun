using EndRun.Entities;
using Raylib_cs;
using System.IO.Pipelines;
using System.Numerics;

namespace EndRun.weapons.Melees
{
    
    public class Melee
    {
        public virtual float Radius { get; set; }
        public virtual Color Highlight { get; set; }
        public virtual float Damage { get; set; }
        public virtual Texture2D Texture { get; set; }
        public virtual Vector2 Center { get; set; }
        protected Entity? entity;
        public int scoreGain;
        public int energyUsage;
        public virtual void Use(ref List<Entity> entity, ref int energy)
        {
            energy -= energyUsage;
        }

        public virtual void Draw()
        {

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
