using Accessibility;
using EndRun.Entities;
using EndRun.User;
using EndRun.weapons.Guns;
using EndRun.weapons.Melees;
using EndRun.Weapons.Guns;
using raygui_cs;
using Raylib_cs;
using System.Windows.Controls.Primitives;

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
            100, 200, 300, 400, 500
        };
        //timer
        static int interval; //interval of time
        static int maxInterval; //max interval of time

        //entity list
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

        //user interacting object
        static User.User user;

        //state enum
        enum States
        {
            menu,
            setup,
            play,
            gameover,
            end
        }

        //player setup members
        enum Gadget
        {
            powerBank,
            shocker,
            antiLeak
        }

        enum Gun
        {
            handGun,
            ballShot,
            shotgun,
        }

        static int selectedGadget;
        static int selectedStartingWeapon;
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
            user = new User.User(ref player, ref gameBounds);
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
                    if (Raygui.GuiButton(new Rectangle(400, 480, 230, 80), "Start") == 1)
                    {
                        Reset(); //reset game states
                        currentState = (int)States.setup; //change state to play
                    }
                    else if (Raygui.GuiButton(new Rectangle(650, 480, 230, 80), "Quit") == 1)
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

                    //player updates
                    player.Update();

                    //gui updates and inputs
                    user.Update(ref realDistance);
                    user.Input();

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
                                currentDifficulty++;
                                SetDifficulty(difficulties[currentDifficulty - 1]);
                                currentLevel++;
                            }
                            distance = 0;
                            distance = realDistance;
                            player.ResetPos();
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
                case States.setup:
                    Raylib.DrawText("Setup", 550, 100, 56, Color.Black);

                    Raylib.DrawRectangleLinesEx(new Rectangle(200, 390, 330, 100), 2, Color.Gray);
                    Raylib.DrawRectangleLinesEx(new Rectangle(730, 390, 330, 100), 2, Color.Gray);
                    Raygui.GuiSetStyle(0, Raygui.TEXT_SIZE, 48);
                    Raylib.DrawText("Gadget: ", 250, 310, 60, Color.Black);
                    Raylib.DrawText("Starting Gun: ", 700, 310, 60, Color.Black);
                    //switch logics
                    //gadget
                    if (Raygui.GuiButton(new Rectangle(130, 390, 60, 100), "<-") == 1)
                    {
                        if (selectedGadget != 0)
                        {
                            selectedGadget--;
                        }
                    }
                    if (Raygui.GuiButton(new Rectangle(540, 390, 60, 100), "->") == 1)
                    {
                        if (selectedGadget != 2)
                        {
                            selectedGadget++;
                        }
                    }

                    //selected Weapon
                    if (Raygui.GuiButton(new Rectangle(660, 390, 60, 100), "<-") == 1)
                    {
                        if (selectedStartingWeapon != 0)
                        {
                            selectedStartingWeapon--;
                        }
                    }
                    if (Raygui.GuiButton(new Rectangle(1070, 390, 60, 100), "->") == 1)
                    {
                        if (selectedStartingWeapon != 2)
                        {
                            selectedStartingWeapon++;
                        }
                    }

                    //selected gadget
                    switch ((Gadget)selectedGadget)
                    {
                        case Gadget.powerBank:
                            Raylib.DrawText("Power Bank", 240, 420, 40, Color.Black);
                            break;
                        case Gadget.shocker:
                            Raylib.DrawText("Shocker", 285, 420, 40, Color.Black);
                            break;
                        case Gadget.antiLeak:
                            Raylib.DrawText("Anti-Leak", 265, 420, 40, Color.Black);
                            break;
                    }

                    //selected starting weapon
                    switch ((Gun)selectedStartingWeapon)
                    {
                        case Gun.handGun:
                            Raylib.DrawText("Handgun", 810, 420, 40, Color.Black);
                            break;
                        case Gun.ballShot:
                            Raylib.DrawText("Ballshot", 810, 420, 40, Color.Black);
                            break;
                        case Gun.shotgun:
                            Raylib.DrawText("Shotgun", 810, 420, 40, Color.Black);
                            break;
                    }

                    Raygui.GuiSetStyle(0, Raygui.TEXT_SIZE, 32); //set text size

                    //options

                    if (Raygui.GuiButton(new Rectangle(200, 650, 320, 80), "Back") == 1)
                    {
                        currentState = (int)States.menu;
                    }
                    if (Raygui.GuiButton(new Rectangle(740, 650, 320, 80), "Begin") == 1)
                    {
                        currentState = (int)States.play;
                        player.gadget = selectedGadget;
                        player.SetSlot(1, 0);
                        player.SetSlot(2, selectedStartingWeapon);
                        Reset();
                    }

                    ///include
                    ///selecting staring items
                    ///selecting gadgets???
                    ///
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
                    Raylib.DrawText("Final Distance: \n \t" + user.distance.ToString() + " Studs", 250, 550, 48, Color.Black);
                    Raylib.DrawText("Final Score: \n \t" + user.score.ToString(), 690, 550, 48, Color.Black);
                    break;
            }
        }

        public static void CollisionChecks()
        {

            List<Entity> collidedEntities = new List<Entity>();

            for (int i = 0; i < entityList.Count; i++)
            {
                if (Raylib.CheckCollisionRecs(gameBounds, entityList[i].Dest) || true) //zombies out of bounds cannot be attacked
                {
                    if (player.selectedSlot == 0)
                    {
                        if (player.melee is Blaster b && Raylib.CheckCollisionCircleRec(player.melee.center, player.melee.Radius, entityList[i].Dest))
                        {
                            collidedEntities.Add(entityList[i]);
                        }
                        else
                        {
                            if (player.melee is Katana k)
                            {
                                if (Functions.CheckCollisionsQuad(k.guide, -k.angle * Raylib.DEG2RAD, entityList[i].Dest, 0))
                                {
                                    collidedEntities.Add(entityList[i]);
                                }
                            }
                            if (player.melee is Knife n)
                            {
                                if (Functions.CheckCollisionsQuad(n.guide, -n.angle * Raylib.DEG2RAD, entityList[i].Dest, 0))
                                {
                                    collidedEntities.Add(entityList[i]);
                                }
                            }
                        }
                    }
                    else if (player.selectedSlot == 1)
                    {
                        if (player.gun is HandGun handGun)
                        {
                            if (Functions.CheckCollisionsQuad(handGun.Laser, handGun.angle * Raylib.DEG2RAD, entityList[i].Dest, 0))
                            {
                                collidedEntities.Add(entityList[i]);
                            }
                        }
                        else if (player.gun is BallShot ballShot)
                        {
                            if (Raylib.CheckCollisionCircleRec(ballShot.ballPos, ballShot.radius, entityList[i].Dest))
                            {
                                entityList[i].Kill();
                                if (!ballShot.collided)
                                {
                                    player.score += entityList[0].Score;
                                    ballShot.collided = true;
                                }
                            }
                        }
                        else if (player.gun is Shotgun shotgun)
                        {
                            for (int j = 0; j < shotgun.projectiles.Length; j++)
                            {
                                if (Raylib.CheckCollisionCircleRec(shotgun.projectiles[j].projectilePos, shotgun.projectiles[j].radius, entityList[i].Dest))
                                {
                                    shotgun.projectiles[j].Terminate();
                                    entityList[i].Kill();
                                    if(!shotgun.scored)
                                    {
                                        player.score += entityList[0].Score;
                                        shotgun.scored = true;
                                    }
                                }
                            }
                        }
                    }
                }

                if (Raylib.CheckCollisionRecs(player.Dest, entityList[i].Dest) && !player.invincible) //game over if player collides with entity
                {
                    if (player.gadget == (int)Gadget.shocker)
                    {
                        if (!player.shocked && player.energy > 0)
                        {
                            player.shocked = true;
                            player.Shock();
                        }
                        else
                        {
                            currentState = (int)States.gameover;
                        }
                    }
                    else
                    {
                        currentState = (int)States.gameover;
                    }
                    
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
                entityList.Add(new Zombie(40, 60));
            }
            for (int i = 0; i < batCount; i++)
            {
                entityList.Add(new Bat(80, 30));
            }
            for (int i = 0; i < bugCount; i++)
            {
                entityList.Add(new Bug(40, 40, ref gameBounds));
            }

        }

        public static void Reset()
        {
            distance = 0; //reset distance to 0
            realDistance = 0; //reset real distance to 0
            interval = 0; //reset timer for distance tracking to 0
            SetDifficulty(difficulties[0]);
            player.ResetState();
            currentLevel = 0;
            currentDifficulty = 0;
        }

    }
}