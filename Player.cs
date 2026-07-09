using EndRun.Guns;
using Raylib_cs;
using System.Numerics;

namespace EndRun
{
    public class Player
    {
        public Texture2D texture;
        public Vector2 pos = new Vector2(0); //player position
        public Rectangle dest; //sprite dest rect
        public Vector2 origin = new Vector2(0);

        private float vel = 5f; //speed of player
        private Rectangle src; //sprite source rect
        private Rectangle bounds; //game bounds
        private int health = 0;

        private Gun gun = new HandGun(10);

        private bool up = false;
        private bool down = false;
        private bool left = false;
        private bool right = false;

        public Player(string spriteSheetLocation, int health, in Rectangle bounds)
        {
            texture = Raylib.LoadTexture(spriteSheetLocation); //load player texture
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
            pos = new Vector2(100, 100);
            src = new Rectangle(0, 0, texture.Width, texture.Height); //set up texture src rect
            dest = new Rectangle(pos, texture.Width, texture.Height); //set up texture dest rect
            this.bounds = bounds; //set game bounds
            this.health = health; //set health
        }

        public void Update()
        {
            //reset movement bools
            up = false;
            down = false;
            left = false;
            right = false;

            //take inputs
            Input();

            //actions
            gun.Update(pos + origin);
            Move();
            Shoot();

            //set position of dest rect
            dest.X = pos.X;
            dest.Y = pos.Y;
        }

        public void Draw()
        {
            //draw player sprite
            Raylib.DrawTexturePro(texture, src, dest, new Vector2(0), 0, Color.White);

            gun.Draw();
        }

        //decrement health by 1
        public void RemoveHealth()
        {
            health--;
        }

        public void Input()
        {
            //movement
            if (Raylib.IsKeyDown(KeyboardKey.W) && pos.Y > bounds.Y)
            {
                up = true;
            }
            if (Raylib.IsKeyDown(KeyboardKey.S) && pos.Y + texture.Height < bounds.Y + bounds.Height)
            {
                down = true;
            }
            if (Raylib.IsKeyDown(KeyboardKey.D) && pos.X + texture.Width < bounds.X + bounds.Width)
            {
                right = true;
            }
            if (Raylib.IsKeyDown(KeyboardKey.A) && pos.X > bounds.X)
            {
                left = true;
            }
        }

        void Shoot()
        {

        }
        void Move()
        {
            Vector2 dis = new Vector2(0);
            Vector2 newPos = pos;

            //initial vector2 change
            dis.X = ((right ? 1 : 0) + (left ? -1 : 0)) * vel;
            dis.Y = ((down ? 1 : 0) + (up ? -1 : 0)) * vel;

            //vector2 normalization = divide by tan 45 deg if the other axis isn't 0
            dis.X /= dis.Y == 0 ? 1 : (float)Math.Sqrt(2);
            dis.Y /= dis.X == 0 ? 1 : (float)Math.Sqrt(2);


            //add displacement pos to new pos
            newPos += dis;

            //set current pos to new pos
            pos = newPos;
        }
    }
}
