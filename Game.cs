using Raylib_cs;
using raygui_cs;
using System.Windows;
using System.Numerics;
using System.Timers;

using Timer = System.Timers.Timer;
using EndRun.Entities;

namespace EndRun
{
    internal static class Game
    {
        static int currentState; //current state
        static Player player; //player object
        static Zombie zombie; //zombie object
        static Rectangle gameBounds; //game bounds
        static int distance; //travelled distance on screen
        static int interval; //interval of time
        static int maxInterval; //max interval of time
        static int realDistance; //total travelled distance (screen + interval)
        static Vector2 idk = new Vector2();

        //max amount of entities
        static int zombieCount;
        static int batCount;
        static int bugCount;

        //state enum
        enum States
        {
            menu,
            play,
            gameover,
        }
        public static void Main()
        {
            Raylib.SetConfigFlags(ConfigFlags.HighDpiWindow);
            Raylib.SetTargetFPS(60); //set fps
            Raylib.InitWindow(1280, 720, "EndRun"); //init window
            Raygui.GuiSetStyle(0, Raygui.TEXT_SIZE, 32); //set text size
            currentState = (int)States.menu; //start in menu
            maxInterval = 60;
            gameBounds = new Rectangle(0, 80, Raylib.GetScreenWidth(), Raylib.GetScreenHeight() - 160); //set game bounds
            distance = 0; //set default distance travelled to 0;
            player = new Player("Assets/Player.png", 1, gameBounds); //initialize player object
            zombie = new Zombie(1);

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
                        currentState = (int)States.play; //change state to play
                        distance = 0; //reset distance to 0
                    }
                    else if (Raygui.GuiButton(new Rectangle(650, 300, 230, 80), "Quit") == 1)
                    {
                        Environment.Exit(0); //exit application
                    }
                    break;
                case States.play:
                    //update distance
                    realDistance = 0;
                    if (interval < maxInterval)
                    {
                        interval++;
                    }
                    else
                    {
                        interval = 0;
                        distance++;
                    }
                    realDistance = distance + ((int)player.pos.X / 100);

                    //player updates
                    player.Update();

                    //entity updates
                    zombie.Update(player.pos);

                    //collisions

                    break;
            }
        }

        public static void Draw()
        {
            //draw cursor position for debug purposes
            Raylib.DrawText(Raylib.GetMouseX() + ", " + Raylib.GetMouseY(), 10, 10, 18, Color.Black);
            Raylib.DrawText(Raylib.GetFPS().ToString(), 120, 10, 18, Color.Black);


            switch ((States)currentState)
            {
                case States.menu:
                    //no logic
                    break;
                case States.play:            
                    //draw distance travelled
                    Raylib.DrawText(realDistance.ToString(), 10, 30, 18, Color.Black);

                    //draw bounds
                    Raylib.DrawRectangleRec(gameBounds, Color.Beige);

                    //draw entity
                    zombie.Draw();

                    //draw player
                    player.Draw();
                    break;
            }
        }
    }
}