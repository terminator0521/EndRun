using Raylib_cs;
using raygui_cs;

using EndRun.Entities;
using EndRun.User;
using System.Diagnostics.Eventing.Reader;
using System.Windows.Input;

namespace EndRun
{
    internal static class Game
    {
        static int currentState; //current state
        static Player player; //player object
        static Rectangle gameBounds; //game bounds
        static Rectangle continueBounds; //rectangle that allows player to proceed
        static int distance; //travelled distance on screen
        static int realDistance; //total travelled distance (screen + interval)
        static int currentLevel; //current level
        static bool atCheckpoint = false;
        static int[] levelDistances =
        {
            50, 1000, 2000, 4000, 7000
        };
        //timer
        static int interval; //interval of time
        static int maxInterval; //max interval of time

        static List<Entity> entityList = new List<Entity>();

        //difficulties
        static Difficulties difficulty; //current difficulty
        static int currentDifficulty = 0;
        readonly static Difficulties[] difficulties =
        {

            new Difficulties(5f, 4, 3f, 1, 4f, 1),
            new Difficulties(3f, 6, 3f, 2, 3f, 1),
            new Difficulties(1f, 8, 2f, 4, 1f, 2),
        };

        //entity counts values
        static int zombieCount;
        static int batCount;
        static int bugCount;

        static User.User user;

        //state enum
        enum States
        {
            menu,
            play,
            gameover,
            end
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
            continueBounds = new Rectangle(1200, 80, 80, 500);
            distance = 0; //set default distance travelled to 0;
            player = new Player("Assets/Player.png", 1, gameBounds); //add player 
            user = new User.User(ref player);
            currentLevel = 0;

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
                    if (Raygui.GuiButton(new Rectangle(400, 400, 230, 80), "Start") == 1)
                    {
                        Reset(); //reset game states
                        currentState = (int)States.play; //change state to play
                    }
                    else if (Raygui.GuiButton(new Rectangle(650, 400, 230, 80), "Quit") == 1)
                    {
                        Environment.Exit(0); //exit application
                    }
                    break;
                case States.play:
                    //update distance
                    if (realDistance < levelDistances[currentLevel])
                    {
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
                    }
                    else if (!atCheckpoint)
                    {
                        atCheckpoint = true;
                    }

                    //gui updates and inputs
                    user.Update(distance);
                    user.Input();

                    //player updates
                    player.Update();



                    //collisions
                    CollisionChecks();

                    if (atCheckpoint) //at checkpoint
                    {
                        for (int i = 0; i < entityList.Count; i++)
                        {
                            entityList[i].Kill();
                        }

                        entityList.Clear(); //clear entities

                        if (Raylib.CheckCollisionRecs(player.Dest, continueBounds)) //if at continue area
                        {
                            atCheckpoint = false;
                            if (currentDifficulty < difficulties.Length)
                            {
                                Console.WriteLine(currentDifficulty);
                                currentDifficulty++;
                                SetDifficulty(difficulties[currentDifficulty]);
                                currentLevel++;
                                distance -= realDistance;
                                player.ResetState();
                            }
                        }
                    }
                    else //still heading towards checkpoint
                    {
                        //entity updates
                        for (int i = 0; i < entityList.Count; i++)
                        {
                            entityList[i].Update(player.pos + player.origin);
                        }
                    }
                    break;
                case States.gameover:
                    if (Raygui.GuiButton(new Rectangle(400, 300, 230, 80), "Retry") == 1)
                    {
                        Reset(); //reset game states
                        currentState = (int)States.play; //change state to play
                    }
                    else if (Raygui.GuiButton(new Rectangle(650, 300, 230, 80), "Exit") == 1)
                    {
                        currentState = (int)States.menu; //change state to play
                    }
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
                    Raylib.DrawText("End Run", 525, 200, 56, Color.Black);
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

                    if (atCheckpoint) //if check point reached
                    {
                        Raylib.DrawText("You have reached a checkpoint", 60, 400, 48, Color.Black);
                        Raylib.DrawText("Proceed to right side of screen to continue", 60, 500, 48, Color.Black);
                        Raylib.DrawRectangleRec(continueBounds, Color.Red);
                    }
                    break;
                case States.gameover:
                    Raylib.DrawText("Game Over", 425, 105, 90, Color.Black);
                    break;
            }
        }

        public static void CollisionChecks()
        {

            List<Entity> collidedEntities = new List<Entity>();

            for (int i = 0; i < entityList.Count; i++)
            {
                if (Raylib.CheckCollisionRecs(gameBounds, entityList[i].Dest)) //zombies out of bounds cannot be attacked
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
                
                if (Raylib.CheckCollisionRecs(player.Dest, entityList[i].Dest)) //game over if player collides with entity
                {
                    currentState = (int)States.gameover;
                }
            }

            player.UpdateCollidedObjects(collidedEntities);
        }

        public static void SetDifficulty(Difficulties level)
        {
            //set locals 
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
                    bat.WaitTime = level.batWaitTime;
                }
                else if (entityList[i] is Bug bug)
                {
                    bug.DownTime = level.spawnTime;
                    bug.WaitTime = level.bugWaitTime;
                }
            }

            //reset list
            entityList.Clear();

            for (int i = 0; i < zombieCount; i++)
            {
                entityList.Add(new Zombie(40, 40));
            }
            for (int i = 0; i < batCount; i++)
            {
                entityList.Add(new Bat(80, 30));
            }
            for (int i = 0; i < bugCount; i++)
            {
                entityList.Add(new Bug(20, 20, ref gameBounds));
            }

        }

        public static void Reset()
        {
            distance = 0; //reset distance to 0
            realDistance = 0; //reset real distance to 0
            interval = 0; //reset timer for distance tracking to 0
            SetDifficulty(difficulties[0]);
            player.ResetState();
        }

    }
}