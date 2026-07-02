using Raylib_cs;
using raygui_cs;
using System.Windows;
using System.Numerics;

namespace EndRun
{
    internal static class Game
    {
        static int currentState = (int)States.menu; //start in menu
        static Player player; //player object
        static Rectangle gameBounds; //game bounds
        static Rectangle zombieBounds; //zombie area

        //state enum
        enum States
        {
            menu,
            a,
            b,
            c,
            d,
            gameover,
        }
        public static void Main()
        {
            Raylib.SetConfigFlags(ConfigFlags.HighDpiWindow);
            Raylib.SetTargetFPS(60); //set fps
            Raylib.InitWindow(1280, 720, "EndRun"); //init window
            Raygui.GuiSetStyle(0, Raygui.TEXT_SIZE, 32); //set text size
            gameBounds = new Rectangle(0, 0, Raylib.GetScreenWidth(), Raylib.GetScreenHeight()); //set game bounds
            zombieBounds = new Rectangle(200, 200, 600, 600); //set zombie bounds


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
                        player = new Player("Assets/Player.png", 1, zombieBounds); //initialize player object
                        currentState = (int)States.a; //change state to play
                    }
                    else if (Raygui.GuiButton(new Rectangle(650, 300, 230, 80), "Quit") == 1)
                    {
                        Environment.Exit(0); //exit application
                    }
                    break;
                case States.a:
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
                case States.a:
                    Raylib.DrawRectangleRec(zombieBounds, Color.Beige);
                    player.Draw();
                    break;
            }
        }
    }
}