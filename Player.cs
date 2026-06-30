using Raylib_cs;
using System.Numerics;

namespace EndRun
{
    public class Player
    {
        public Texture2D texture;
        public Vector2 pos = new Vector2(0); //player position
        public Rectangle dest; //sprite dest rect

        private float vel = 5f; //speed of player
        private Rectangle src; //sprite source rect
        private int health = 0;

        private bool up = false;
        private bool down = false;
        private bool left = false;
        private bool right = false;

        public Player(string spriteSheetLocation, int health)
        {
            this.pos = new Vector2(100, 100);
            this.texture = Raylib.LoadTexture(spriteSheetLocation); //load player texture
            this.src = new Rectangle (0, 0, texture.Width, texture.Height); //set up texture src rect
            this.dest = new Rectangle(pos, texture.Width, texture.Height); //set up texture dest rect
            this.health = health; //set health
        }

        public void Update()
        {
            this.Input();
            this.Move();

            //set dest rect pos to pos vector2
            this.dest.X = this.pos.X;
            this.dest.Y= this.pos.Y;
        }

        public void Draw()
        {
            //draw player sprite
            Raylib.DrawTexturePro(texture, src, dest, new Vector2(0), 0, Color.White);
        }

        //decrement health by 1
        public void RemoveHealth()
        {
            this.health--;
        }

        public void Input()
        {
            //movement
            if (Raylib.IsKeyDown(KeyboardKey.W))
            {
                this.up = true;
            }
            else
            {
                this.up = false;
            }
            if (Raylib.IsKeyDown(KeyboardKey.S))
            {
                this.down = true;
            }
            else
            {
                this.down = false;
            }
            if (Raylib.IsKeyDown(KeyboardKey.D))
            {
                this.right = true;
            }
            else
            {
                this.right = false;
            }
            if (Raylib.IsKeyDown(KeyboardKey.A))
            {
                this.left = true;
            }
            else
            {
                this.left= false;
            }
        }

        void Move()
        {
            Vector2 pos = new Vector2(0, 0);

            //initial vector2 change
            pos.X = ((this.right ? 1 : 0) + (this.left ? -1 : 0)) * this.vel;
            pos.Y = ((this.down ? 1 : 0) + (this.up ? -1 : 0)) * this.vel;

            //vector2 normalization = divide by tan 45 deg if the other axis isn't 0
            pos.X /= pos.Y == 0 ? 1 : (float)Math.Sqrt(2);
            pos.Y /= pos.X == 0 ? 1 : (float)Math.Sqrt(2);

            //add local pos to player pos 
            this.pos += pos;
        }
    }
}
