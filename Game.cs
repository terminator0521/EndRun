using Raylib_cs;
using raygui_cs;
using System.Windows;

namespace EndRun
{
    public static class Game
    {
        static int currentState = (int)States.menu; //start in menu
        static Player player; //player object
        static Rectangle gameBounds; //game bounds

        //state enum
        enum States
        {
            menu = 0,
            play = 1,
            gameover = 2,
        }
        public static void Main()
        {
            Raylib.SetTargetFPS(60); //set fps
            Raylib.InitWindow(1280, 720, "EndRun"); //init window
            Raygui.GuiSetStyle(0, Raygui.TEXT_SIZE, 32); //set text size
            gameBounds = new Rectangle(0, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight()); //set game bounds


            while (!Raylib.WindowShouldClose())
            {
                Update(); //call updates

                Raylib.BeginDrawing(); //start sprite batch
                Raylib.ClearBackground(Color.LightGray); //clear background
                Draw(); //call draw
                Raylib.EndDrawing(); //end sprite batch
            }
        }

        public static void Update()
        {
            switch ((States)currentState)
            {
                case States.menu:
                    if (Raygui.GuiButton(new Rectangle(400, 300, 230, 80), "Start") == 1)
                    {
                        player = new Player("Assets/Player.png", 1, gameBounds); //initialize player object
                        currentState = (int)States.play; //change state to play
                    }
                    else if (Raygui.GuiButton(new Rectangle(650, 300, 230, 80), "Quit") == 1)
                    {
                        Environment.Exit(0); //exit application
                    }
                    break;
                case States.play:
                    player.Update();
                    break;
            }
        }

        public static void Draw()
        {
            //draw cursor position for debug purposes
            Raylib.DrawText(Raylib.GetMouseX() + ", " + Raylib.GetMouseY(), 10, 10, 18, Color.Black);

            switch ((States)currentState)
            {
                case States.menu:
                    //no logic
                    break;
                case States.play:
                    player.Draw();
                    break;
            }
        }
    }
}