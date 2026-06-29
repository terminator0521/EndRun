using Raylib_cs;
using System.Numerics;

namespace EndRun
{
    public class Player
    {
        public Texture2D texture;
        public Vector2 pos = new Vector2(0); //player position
        public Rectangle dest; //sprite dest rect

        private Rectangle src; //sprite source rect
        private int health = 0;
        
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

        }

        public void Draw()
        {
            //draw player sprite
            Raylib.DrawTexturePro(texture, src, dest, new Vector2(0), 0, Color.White);
        }

        public void RemoveHealth()
        {
            this.health--;
        }
    }
}
