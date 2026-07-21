using Raylib_cs;
using raygui_cs;

using EndRun.Entities;
using System.Windows.Controls;
using Microsoft.Win32.SafeHandles;

namespace EndRun
{
    internal static class Game
    {
        static int currentState; //current state
        static Player player; //player object
        //static Zombie[] zombie = new Zombie[8]; //zombie objects
        static Rectangle gameBounds; //game bounds
        static int distance; //travelled distance on screen
        static int realDistance; //total travelled distance (screen + interval)

        //timer
        static int interval; //interval of time
        static int maxInterval; //max interval of time

        static List<Entity> entityList = new List<Entity>();

        //difficulties
        static Difficulties difficulty; //current difficulty

        readonly static Difficulties a = new Difficulties(5f, 4, 3f, 1, 0, 0);
        readonly static Difficulties b = new Difficulties(3f, 6, 3f, 2, 0, 0);
        readonly static Difficulties c = new Difficulties(1f, 8, 2f, 4, 0, 0);

        //entity counts values
        static int zombieCount;
        static int batCount;
        static int bugCount;

        static User user;

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
            Raylib.InitWindow(1280, 900, "EndRun"); //init window
            Raygui.GuiSetStyle(0, Raygui.TEXT_SIZE, 32); //set text size
            currentState = (int)States.menu; //start in menu
            maxInterval = 60;
            gameBounds = new Rectangle(0, 80, Raylib.GetScreenWidth(), 500); //set game bounds
            distance = 0; //set default distance travelled to 0;
            player = new Player("Assets/Player.png", 1, gameBounds); //add player 
            SetDifficulty(a); //pre-set all difficulty settings to default
            user = new User(ref player);

            for (int i = 0; i < zombieCount; i++)
            {
                entityList.Add(new Zombie(40, 40));
            }
            for (int i = 0; i < zombieCount; i++)
            {
                entityList.Add(new Bat(80, 30));
            }

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
                    //check distance to change difficulty
                    if (distance == 0 && !difficulty.Equals(a))
                    {
                        difficulty = a;
                        SetDifficulty(difficulty);
                    }
                    else if (distance == 200 && !difficulty.Equals(b))
                    {
                        difficulty = b;
                        SetDifficulty(difficulty);
                    }
                    else if (distance == 500 && !difficulty.Equals(c))
                    {
                        difficulty = c;
                        SetDifficulty(difficulty);
                    }

                    //update distance
                    realDistance = 0;
                    if (interval < maxInterval)
                    {
                        interval++;
                    }
                    else
                    {
                        interval = 0;
                        distance += 10;
                    }
                    realDistance = distance + ((int)player.pos.X / 100 * 10);


                    //gui updates and inputs
                    user.Update(distance);
                    user.Input();

                    //player updates
                    player.Update();

                    //entity updates
                    for (int i = 0; i < entityList.Count; i++)
                    {
                        entityList[i].Update(player.pos);
                    }

                    //collisions
                    CollisionChecks();
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
                    for (int i = 0; i < entityList.Count; i++)
                    {
                        entityList[i].Draw();
                    }

                    //draw player
                    player.Draw();

                    //draw gui
                    user.Draw();
                    break;

            }
        }

        public static void CollisionChecks()
        {

            List<Entity> collidedEntities = new List<Entity>();

            for (int i = 0; i < entityList.Count; i++)
            {
                if (Raylib.CheckCollisionRecs(gameBounds, entityList[i].Dest)) //zombies out of bounds cannot be shot
                {
                    if (Raylib.CheckCollisionCircleRec(player.melee.Center, player.melee.Radius, entityList[i].Dest) && player.selectedSlot == 0)
                    {
                        collidedEntities.Add(entityList[i]);
                    }
                    else if (Functions.CheckCollisionsQuad(player.gun.Laser, player.gun.angle * Raylib.DEG2RAD, entityList[i].Dest, 0) && player.selectedSlot == 1)
                    {
                        collidedEntities.Add(entityList[i]);
                    }
                }
            }

            player.UpdateCollidedObjects(collidedEntities);
        }

        public static void SetDifficulty(Difficulties level)
        {
            zombieCount = level.zombieCount;
            batCount = level.batCount;
            bugCount = level.bugCount;


            for (int i = 0; i < entityList.Count; i++)
            {
                
                if (entityList[i] is Zombie zombie)
                {
                    zombie.DownTime = level.spawnTime;
                }
                else if (entityList[i] is Bat bat)
                {
                    bat.DownTime = level.spawnTime;
                    bat.WaitTime = level.spawnTime;
                }
            }
        }

    }
}