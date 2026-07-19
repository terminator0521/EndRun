using EndRun.Entities;
using EndRun.Guns;
using EndRun.Melees;
using Raylib_cs;
using System.Windows.Media.Media3D;

namespace EndRun
{
    public class User
    {
        Player player;
        private int selectedSlot;
        private int health;
        private int distance;
        public User(ref Player player)
        {
            this.player = player;
        }

        public void Update(int distance)
        {
            this.distance = distance;
            selectedSlot = player.selectedSlot;
            health = player.health;
        }

        public void Draw()
        {
            Raylib.DrawRectangle(0, 610, 1280, 290, Color.Gray);

            for (int i = 0; i < 3; i++)
            {
                Raylib.DrawRectangle(60 + (170 * i), 650, 150, 200, Color.White);
                Raylib.DrawText((1 + i).ToString(), 70 + (170 * i), 815, 30, Color.Black);
            }

            Raylib.DrawRectangleLinesEx(new Rectangle(60 + (170 * selectedSlot), 650, 150, 200), 4, Color.Black);
        }

        public void Input()
        {
            //movement
            if (Raylib.IsKeyDown(KeyboardKey.W))
            {
                player.up = true;
            }
            if (Raylib.IsKeyDown(KeyboardKey.S))
            {
                player.down = true;
            }
            if (Raylib.IsKeyDown(KeyboardKey.D))
            {
                player.right = true;
            }
            if (Raylib.IsKeyDown(KeyboardKey.A))
            {
                player.left = true;
            }

            //use
            if (Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                Console.WriteLine(player.collidedObjects.Count);
                switch (selectedSlot)
                {
                    case 0:
                        player.melee.Use(player.collidedObjects);
                        break;
                    case 1:
                        player.gun.Shoot(player.collidedObjects);
                        break;
                }
            }

            switch (Raylib.GetKeyPressed())
            {
                case (int)KeyboardKey.One:
                    player.selectedSlot = 0;
                    break;
                case (int)KeyboardKey.Two:
                    player.selectedSlot = 1;
                    break;
                case (int)KeyboardKey.Three:
                    player.selectedSlot = 2;
                    break;
            }

        }
    }
}
