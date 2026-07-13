using EndRun.Guns;
using Raylib_cs;
using System.Configuration;
using System.Numerics;

namespace EndRun.Entities
{
    public class Player
    {
        public Texture2D texture;
        public Vector2 pos = new Vector2(0); //player position
        public Vector2 lastPos = new Vector2(0); //player's last position
        public Rectangle dest; //sprite dest rect
        public Vector2 origin = new Vector2(0);
        public Gun gun = new HandGun(10);

        //overriding dest
        

        
        private float vel = 5f; //speed of player
        private Rectangle src; //sprite source rect
        private Rectangle bounds; //game bounds
        private int health = 0;

        private bool up = false;
        private bool down = false;
        private bool left = false;
        private bool right = false;

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

            //set position of dest rect
            
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
            Console.WriteLine("damage");
        }

        public void Input()
        {
            //movement
            if (Raylib.IsKeyDown(KeyboardKey.W))
            {
                up = true;
            }
            if (Raylib.IsKeyDown(KeyboardKey.S))
            {
                down = true;
            }
            if (Raylib.IsKeyDown(KeyboardKey.D))
            {
                right = true;
            }
            if (Raylib.IsKeyDown(KeyboardKey.A))
            {
                left = true;
            }
        }

        public void Shoot()
        {

        }
        public void Move()
        {
            lastPos = pos;
            Vector2 dis = new Vector2(0);

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
    }
}
