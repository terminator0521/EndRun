using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using Raylib_cs;

namespace EndRun
{
    public class Bullet : IDisposable
    {
        private Vector2 pos = new Vector2(0); //bullet position
        private Vector2 displacement = new Vector2(0); //bullet displacement vector
        private Rectangle src; //sprite source rect
        private Texture2D texture; //bullet texture

        public Rectangle dest; //sprite dest rect
        public bool isActive = true; //is bullet active

        public Bullet(Vector2 playerPos, Vector2 mousePos)
        {
            this.pos = playerPos; //set bullet position
            texture = Raylib.LoadTexture("Assets/Player.png"); //load bullet texture
            src = new Rectangle(0, 0, 10, 10); //set up texture src rect
            dest = new Rectangle(pos, 10, 10); //set up texture dest rect
        }

        public void Draw()
        {
            //draw bullet
            if (isActive)
            {
                Raylib.DrawTexturePro(texture, src, dest, pos, 0, Color.White);
            }
        }
        public void Update()
        {

        }

        public void Dispose()
        {
            Raylib.UnloadTexture(texture); //unload bullet texture
        }
    }
}
