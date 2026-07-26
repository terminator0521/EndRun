using EndRun.Entities;
using EndRun.weapons.Guns;
using EndRun.weapons.Melees;
using Raylib_cs;
using System.Numerics;

namespace EndRun.User
{
    public class Player
    {
        public Texture2D texture; //player sprite
        public Vector2 pos = new Vector2(0); //player position
        public Vector2 lastPos = new Vector2(0); //player's last position
        public Vector2 dis = new Vector2(0); //player displacement vector
        public Rectangle Dest = new Rectangle(); //sprite dest rect
        public Vector2 origin = new Vector2(0);
        public Gun gun; //players gun
        public Melee melee; //player melee
        public int selectedSlot; //selected slot
        public int score;
        public int energy;
        private bool energyExtended = false;
        public int energyCap = 100; //default energy cap
        //collided object lists
        public List<Entity> collidedObjects;


        private float vel = 5f; //speed of player
        private Rectangle src; //sprite source rect
        private Rectangle bounds; //game bounds

        public bool up = false;
        public bool down = false;
        public bool left = false;
        public bool right = false;

        public Player(string spriteSheetLocation, int health, in Rectangle bounds) : base()
        {
            texture = Raylib.LoadTexture(spriteSheetLocation); //load player texture
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
            src = new Rectangle(0, 0, texture.Width, texture.Height); //set up texture src rect
            Dest = new Rectangle(pos, texture.Width, texture.Height); //set up texture dest rect
            this.bounds = bounds; //set game bounds
            selectedSlot = 1;
            gun = new HandGun();
            melee = new Katana();
            collidedObjects = new List<Entity>();
            energyCap = 100 + (energyExtended ? 20 : 0); //set energy cap
            ResetState();
        }

        public void Update()
        {
            //cap energy level
            if (energy > energyCap)
            {
                energy = energyCap;
            }

            //actions
            gun.Update(pos + origin + dis, ref score);
            melee.Update(pos + origin + dis, ref score);
            Move();
            
            //reset movement bools
            up = false;
            down = false;
            left = false;
            right = false;
        }

        public void Draw()
        {
            //draw weapons
            switch (selectedSlot)
            {
                case 0:
                    melee.Draw();
                    break;
                case 1:
                    gun.Draw();
                    break;
            }

            //draw player sprite
            Raylib.DrawTexturePro(texture, src, Dest, new Vector2(0), 0, Color.White);
        }

        public void Move()
        {
            lastPos = pos;
            dis = new Vector2(0);

            //initial vector2 change
            dis.X = ((right ? 1 : 0) + (left ? -1 : 0)) * vel;
            dis.Y = ((down ? 1 : 0) + (up ? -1 : 0)) * vel;

            //vector2 normalization = divide by tan 45 deg if the other axis isn't 0
            dis.X /= dis.Y == 0 ? 1 : (float)MathF.Sqrt(2);
            dis.Y /= dis.X == 0 ? 1 : (float)MathF.Sqrt(2);

            //add displacement to y-pos
            pos.Y += dis.Y;
            Dest.Y = pos.Y;

            if (Functions.CheckCollisionEdges(Dest, bounds))
            {
                pos.Y = lastPos.Y;
                Dest.Y = pos.Y;
            }

            //add displacement to x-pos
            pos.X += dis.X;
            Dest.X = pos.X;

            if (Functions.CheckCollisionEdges(Dest, bounds))
            {
                pos.X = lastPos.X;
                Dest.X = pos.X;
            }

        }

        public void UpdateCollidedObjects(List<Entity> collided)
        {
            collidedObjects = collided;
        }

        public void ResetState()
        {
            selectedSlot = 0;
            score = 0;
            ResetPos();
        }

        public void ResetPos()
        {
            pos = new Vector2(50, (bounds.Height / 2) - (Dest.Height / 2));
        }

        public void Recharge()
        {
            if (score >= 50 & energy < energyCap)
            {
                score -= 50;
                energy++;
            }
        }
    }
}
