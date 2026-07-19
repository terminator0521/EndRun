using EndRun.Guns;
using EndRun.Melees;
using Raylib_cs;
using System.Configuration;
using System.Numerics;

namespace EndRun.Entities
{
    public class Player
    {
        public Texture2D texture; //player sprite
        public Vector2 pos = new Vector2(0); //player position
        public Vector2 lastPos = new Vector2(0); //player's last position
        public Vector2 dis = new Vector2(0); //player displacement vector
        public Rectangle dest; //sprite dest rect
        public Vector2 origin = new Vector2(0);
        public Gun gun; //players gun
        public Melee melee; //player melee
        public int selectedSlot; //selected slot

        //collided object lists
        public List<Entity> collidedObjects;


        private float vel = 5f; //speed of player
        private Rectangle src; //sprite source rect
        private Rectangle bounds; //game bounds
        public int health = 0;

        public bool up = false;
        public bool down = false;
        public bool left = false;
        public bool right = false;

        public Player(string spriteSheetLocation, int health, in Rectangle bounds) : base()
        {
            texture = Raylib.LoadTexture(spriteSheetLocation); //load player texture
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
            pos = new Vector2(200, 200);
            src = new Rectangle(0, 0, texture.Width, texture.Height); //set up texture src rect
            dest = new Rectangle(pos, texture.Width, texture.Height); //set up texture dest rect
            this.bounds = bounds; //set game bounds
            this.health = health; //set health
            health = 3;
            selectedSlot = 1;
            gun = new HandGun(5);
            melee = new Katana();
        }

        public void Update()
        {
            //actions
            gun.Update(pos + origin + dis);
            melee.Update(pos + origin + dis);
            Move();


            health++;
            
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
            Raylib.DrawTexturePro(texture, src, dest, new Vector2(0), 0, Color.White);
        }

        //decrement health by 1
        public void RemoveHealth()
        {
            health--;
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
            dest.Y = pos.Y;

            if (Functions.CheckCollisionEdges(dest, bounds))
            {
                pos.Y = lastPos.Y;
                dest.Y = pos.Y;
            }

            //add displacement to x-pos
            pos.X += dis.X;
            dest.X = pos.X;

            if (Functions.CheckCollisionEdges(dest, bounds))
            {
                pos.X = lastPos.X;
                dest.X = pos.X;
            }

        }

        public void UpdateCollidedObjects(List<Entity> collided)
        {
            collidedObjects = collided;
        }
    }
}
