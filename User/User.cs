using System.Numerics;
using Raylib_cs;

namespace EndRun.User
{
    public class User
    {
        //displayed textures
        Texture2D gun;
        Texture2D knife;
        Texture2D device;
        Texture2D lightning;
        Texture2D battery;

        //sourcing dest
        Rectangle src = new Rectangle(0, 0, 64, 64);

        Player player;
        private int selectedSlot;
        private Rectangle bounds;
        public int distance;
        public int score;
        public User(ref Player player, ref Rectangle bounds)
        {
            this.player = player;
            this.bounds = bounds;
            gun = Raylib.LoadTexture("Assets/Gun.png");
            knife = Raylib.LoadTexture("Assets/Melee.png");
            device = Raylib.LoadTexture("Assets/Device.png");
            lightning = Raylib.LoadTexture("Assets/Shock.png");
            battery = Raylib.LoadTexture("Assets/Battery.png");
        }

        public void Update(ref int distance)
        {
            this.distance = distance;
            score = player.score;
            selectedSlot = player.selectedSlot;
        }

        public void Draw()
        {
            Raylib.DrawRectangle(0, 660, 1280, 240, Color.Gray);
            Raylib.DrawRectangle(540, 680, 270, 200, Color.White);
            Raylib.DrawRectangle(820, 680, 170, 200, Color.White);
            Raylib.DrawRectangle(1000, 680, 260, 200, Color.White);

            Raylib.DrawText("Gadget", 360, 840, 40, Color.Black);

            Raylib.DrawRectangle(360, 680, 150, 150, Color.White);
            for (int i = 0; i < 2; i++)
            {
                Raylib.DrawRectangle(20 + (170 * i), 680, 150, 200, Color.White);
                Raylib.DrawText((1 + i).ToString(), 30 + (170 * i), 845, 30, Color.Black);
            }
            Raylib.DrawRectangleLinesEx(new Rectangle(20 + (170 * selectedSlot), 680, 150, 200), 4, Color.Black);

            //slot 1
            Raylib.DrawTexturePro(knife, src, new Rectangle(20, 700, 180, 180), new Vector2(0), 0, Color.White);

            //slot 2
            Raylib.DrawTexturePro(gun, src, new Rectangle(180, 690, 180, 180), new Vector2(0), 0, Color.White);

            //gadget slot
            switch (player.gadget)
            {
                case 0:
                    Raylib.DrawTexturePro(battery, src, new Rectangle(350, 665, 180, 180), new Vector2(0), 0, Color.White);
                    break;
                case 1:
                    Raylib.DrawTexturePro(lightning, src, new Rectangle(340, 665, 180, 180), new Vector2(0), 0, Color.White);
                    break;
                case 2:
                    Raylib.DrawTexturePro(device, src, new Rectangle(345, 665, 180, 180), new Vector2(0), 0, Color.White);
                    break;

            }

            Raylib.DrawText("Distance: ", 580, 700, 40, Color.Black);
            Raylib.DrawText("Score: ", 840, 700, 40, Color.Black);
            Raylib.DrawText(distance.ToString() + " Studs", 580, 780, 40, Color.Black);
            Raylib.DrawText(score.ToString(), 840, 780, 40, Color.Black);
            Raylib.DrawText("Press C to charge", 1015, 830, 24, Color.Black);
            Raylib.DrawText(player.energy.ToString(), 1035, 720, 50, Color.Black);
            Raylib.DrawText("/", 1090, 725, 80, Color.Black);
            Raylib.DrawText(player.energyCap.ToString(), 1135, 755, 50, Color.Black);
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
                switch (selectedSlot)
                {
                    case 0:
                        player.melee.Use(ref player.collidedObjects, ref player.energy);
                        break;
                    case 1:
                        player.gun.Shoot(ref player.collidedObjects, ref player.energy);
                        break;
                }
            }

            //aim
            if (Raylib.IsMouseButtonDown(MouseButton.Right))
            {
                player.gun.Aiming = true;
                player.gun.Aim();
            }
            if (Raylib.IsMouseButtonUp(MouseButton.Right))
            {
                player.gun.Aiming = false;
            }

            switch (Raylib.GetKeyPressed())
            {
                case (int)KeyboardKey.One:
                    player.selectedSlot = 0;
                    break;
                case (int)KeyboardKey.Two:
                    player.selectedSlot = 1;
                    break;
                case (int)KeyboardKey.C:
                    player.Recharge();
                    break;
            }

        }
    }
}
