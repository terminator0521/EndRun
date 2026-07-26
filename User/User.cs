
using Raylib_cs;

namespace EndRun.User
{
    public class User
    {
        Player player;
        private int selectedSlot;
        private Rectangle bounds;
        public int distance;
        public int score;
        public User(ref Player player, ref Rectangle bounds)
        {
            this.player = player;
            this.bounds = bounds;
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

            for (int i = 0; i < 3; i++)
            {
                Raylib.DrawRectangle(20 + (170 * i), 680, 150, 200, Color.White);
                Raylib.DrawText((1 + i).ToString(), 25 + (170 * i), 850, 30, Color.Black);
            }

            Raylib.DrawRectangleLinesEx(new Rectangle(20 + (170 * selectedSlot), 680, 150, 200), 4, Color.Black);

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
                case (int)KeyboardKey.C:
                    player.Recharge();
                    break;
            }

        }
    }
}
