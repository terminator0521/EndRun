using EndRun.Entities;
using EndRun.User;
using EndRun.weapons.Guns;
using EndRun.weapons.Melees;
using EndRun.Weapons.Guns;
using raygui_cs;
using Raylib_cs;
using RayGUI_cs;
using System.Windows.Controls;

/*  TODO
 * 
 * just need a help page
 *  - readme
 */

namespace EndRun
{
    internal static class Game
    {
        static Shop shop; //shop
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
            4000, 8000, 13000, 20000, 30000
        };

        //help/info
        static int page = 0;
        static int width = 220;
        static int height = 100;

        static string[,] topics =
        {
            {"Zombie", "Bat", "Bug" },
            {"Power Bank", "Shocker", "Anti-Leak" },
            {"Knife", "Katana", "Blaster" },
            {"Hand Gun", "Ball Shot", "Shotgun" }
        };

        static Rectangle[,] boxes = new Rectangle[4, 3];

        static public string[,] descriptions =
        {
            {
                "A glitched robot who blindly follows you",
                "A robotic bird who tracks your position before quickly\nmaking a small move closer to you.",
                "A bug with a damaged tracking system that quickly gets\nto you through moving either horizontally\nor vertically."
            },
            {
                "A portable battery pack that holds condenced\nplasmic energy.",
                "A generator that generates a one time shock\nwhich prevent electronics from powering down due\nto insufficent power output.",
                "A device that modifies energyflow to prevent\nextra power from flowing towards unused\ncomponents and parts."
            },
            {
                "Short-ranged and zero energy knife that is\nconvenient to bring." ,
                "6 feel long katana that slices atoms apart.",
                "Looks like the ground pounders you see\nconstruction workers use to flaten the ground.\nShocks anything aroundyou and is very light"
            },
            {
                "A self-defense pistol issued to families with who\nlived near warzones.",
                "A light-weight cannon that fires energy spheres\nthat disintegrates eveything it touches.",
                "A shotgun prototype created by a government\nfunded weapon company that fires energy\nspheres that spread out."
            }
        };
        //timer
        static int interval; //interval of time
        static int maxInterval; //max interval of time

        //entity list
        static List<Entity> entityList = new List<Entity>();

        //difficulties
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
            end,
            info
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
            Raygui.GuiEnableTooltip();
            Raygui.GuiSetStyle(0, Raygui.TEXT_SIZE, 32); //set text size
            currentState = (int)States.menu; //start in menu
            maxInterval = 60;
            gameBounds = new Rectangle(0, 80, Raylib.GetScreenWidth(), 500); //set game bounds
            continueBounds = new Rectangle(1200, 80, 80, 500);
            distance = 0; //set default distance travelled to 0;
            player = new Player("Assets/Player.png", 1, gameBounds); //add player 
            user = new User.User(ref player, ref gameBounds);
            currentLevel = 0;
            shop = new Shop(ref player);

            //topic rectangles 
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    boxes[i, j] = new Rectangle(75 + (305 * i), 490 + (j * 130), width, height);
                }
            }

            Console.WriteLine(currentState);
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
                    else if (Raygui.GuiButton(new Rectangle(400, 570, 480, 80), "Info/Help") == 1)
                    {
                        currentState = (int)States.info;
                    }
                    break;
                case States.setup:
                    break;
                case States.play:
                    //update distance
                    if (realDistance < levelDistances[currentLevel])
                    {
                        if (!atCheckpoint)
                        {
                            realDistance = 0;
                            if (interval < maxInterval)
                            {
                                interval++;
                            }
                            else
                            {
                                interval = 0;
                                distance += 50;
                            }
                            realDistance = distance + ((int)player.pos.X / 100 * 10);
                        }
                        else
                        {
                            interval = 0;
                        }
                    }
                    else if (!atCheckpoint)
                    {
                        atCheckpoint = true;
                    }

                    //player updates
                    player.Update();

                    //gui updates and inputs
                    user.Update(ref realDistance);
                    user.Input(ref shop.open, ref atCheckpoint);

                    //collisions
                    CollisionChecks();

                    if (atCheckpoint) //at checkpoint
                    {
                        {
                            for (int i = 0; i < entityList.Count; i++)
                            {
                                entityList[i].Kill();
                            }

                            entityList.Clear(); //clear entities

                            if (Raylib.CheckCollisionRecs(player.Dest, continueBounds)) //if at continue area
                            {
                                if (currentLevel == levelDistances.Length - 1)
                                {
                                    currentState = (int)States.end;
                                }
                                else
                                {
                                    currentLevel++;
                                    atCheckpoint = false;
                                    if (currentDifficulty < difficulties.Length - 1)
                                    {
                                        currentDifficulty++;
                                    }
                                    SetDifficulty(difficulties[currentDifficulty]);
                                    distance = 0;
                                    distance = realDistance;
                                    player.ResetPos();
                                }
                            }
                        }
                    }
                    else //still heading towards checkpoint
                    {
                        //entity updates
                        for (int i = 0; i < entityList.Count; i++)
                        {
                            entityList[i].Update(player.pos);

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
                case States.end:
                    if (Raygui.GuiButton(new Rectangle(470, 300, 300, 80), "Retry") == 1)
                    {
                        currentState = (int)States.menu; //change state to menu
                    }
                    break;
                case States.info:
                    for (int i = 0; i < 4; i++)
                    {
                        for (int j = 0; j < 3; j++)
                        {

                        }
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
                    //Raylib.DrawText("Starting Melee:", 710, 280, 60, Color.Black);
                    //Raylib.DrawTexturePro(user.knife, user.src, new Rectangle(860, 380, 128, 128), new Vector2(0), 0, Color.White);
                    //Raylib.DrawRectangleLinesEx(new Rectangle(860, 380, 128, 128), 2, Color.Gray);
                    Raylib.DrawRectangleLinesEx(new Rectangle(200, 340, 330, 100), 2, Color.Gray);
                    Raylib.DrawRectangleLinesEx(new Rectangle(200, 600, 330, 100), 2, Color.Gray);
                    Raygui.GuiSetStyle(0, Raygui.TEXT_SIZE, 48);
                    Raylib.DrawText("Gadget: ", 250, 260, 60, Color.Black);
                    Raylib.DrawText("Starting Gun: ", 170, 520, 60, Color.Black);
                    //switch logics
                    //gadget
                    if (Raygui.GuiButton(new Rectangle(130, 340, 60, 100), "<-") == 1)
                    {
                        if (selectedGadget != 0)
                        {
                            selectedGadget--;
                        }
                    }
                    if (Raygui.GuiButton(new Rectangle(540, 340, 60, 100), "->") == 1)
                    {
                        if (selectedGadget != 2)
                        {
                            selectedGadget++;
                        }
                    }

                    //selected Weapon
                    if (Raygui.GuiButton(new Rectangle(130, 600, 60, 100), "<-") == 1)
                    {
                        if (selectedStartingWeapon != 0)
                        {
                            selectedStartingWeapon--;
                        }
                    }
                    if (Raygui.GuiButton(new Rectangle(540, 600, 60, 100), "->") == 1)
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
                            Raylib.DrawText("Power Bank", 240, 370, 40, Color.Black);
                            break;
                        case Gadget.shocker:
                            Raylib.DrawText("Shocker", 285, 370, 40, Color.Black);
                            break;
                        case Gadget.antiLeak:
                            Raylib.DrawText("Anti-Leak", 265, 370, 40, Color.Black);
                            break;
                    }

                    //selected starting weapon
                    switch ((Gun)selectedStartingWeapon)
                    {
                        case Gun.handGun:
                            Raylib.DrawText("Handgun", 280, 630, 40, Color.Black);
                            break;
                        case Gun.ballShot:
                            Raylib.DrawText("Ballshot", 280, 630, 40, Color.Black);
                            break;
                        case Gun.shotgun:
                            Raylib.DrawText("Shotgun", 280, 630, 40, Color.Black);
                            break;
                    }

                    Raygui.GuiSetStyle(0, Raygui.TEXT_SIZE, 32); //set text size

                    //options

                    if (Raygui.GuiButton(new Rectangle(770, 540, 320, 80), "Back") == 1)
                    {
                        currentState = (int)States.menu;
                    }
                    if (Raygui.GuiButton(new Rectangle(770, 380, 320, 80), "Begin") == 1)
                    {
                        currentState = (int)States.play;
                        player.gadget = selectedGadget;
                        player.SetSlot(1, 0);
                        player.SetSlot(2, selectedStartingWeapon);
                        Reset();
                    }
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
                        Console.WriteLine(currentLevel);
                        if (currentLevel == levelDistances.Length - 1)
                        {
                            Raylib.DrawText("You have reached the end", 60, 400, 48, Color.Black);
                        }
                        else
                        {
                            Raylib.DrawText("You have reached a checkpoint", 60, 400, 48, Color.Black);
                        }

                        Raylib.DrawText("Proceed to right side of screen to continue", 60, 500, 48, Color.Black);
                        Raylib.DrawRectangleRec(continueBounds, Color.Red);
                    }
                    break;
                case States.gameover:
                    Raylib.DrawText("Game Over", 425, 105, 90, Color.Black);
                    Raylib.DrawText("Final Distance: \n \t" + user.distance.ToString() + " Studs", 250, 550, 48, Color.Black);
                    Raylib.DrawText("Final Score: \n \t" + user.score.ToString(), 690, 550, 48, Color.Black);
                    break;
                case States.end:
                    Raylib.DrawText("Finish!", 490, 105, 90, Color.Black);
                    Raylib.DrawText("Final Score: \n \t" + user.score.ToString(), 480, 550, 48, Color.Black);

                    //currentState = (int)States.gameover;
                    break;
                case States.info:
                    Raygui.GuiSetStyle(0, Raygui.TEXT_SIZE, 24);

                    if (page == 1) //first page
                    {
                        //title
                        Raylib.DrawText("INFO", 560, 70, 56, Color.Black);

                        //page button
                        if (Raygui.GuiButton(new Rectangle(1020, 70, 200, 80), "Prev Page") == 1)
                        {
                            page--;
                        }

                        //info
                        Raygui.GuiGroupBox(new Rectangle(60, 220, 1160, 200), "Info");
                        Raylib.DrawText("Hover over a topic for more info", 350, 150, 36, Color.Black);

                        //Entites
                        Raygui.GuiGroupBox(new Rectangle(60, 470, 250, 400), "Entites");

                        //Gadgets
                        Raygui.GuiGroupBox(new Rectangle(365, 470, 250, 400), "Gadgets");

                        //Melees
                        Raygui.GuiGroupBox(new Rectangle(665, 470, 250, 400), "Melees");

                        //Guns
                        Raygui.GuiGroupBox(new Rectangle(970, 470, 250, 400), "Guns");

                        //draw boxes
                        for (int i = 0; i < 4; i++)
                        {
                            for (int j = 0; j < 3; j++)
                            {
                                //draw buttons
                                Raygui.GuiButton(boxes[i, j], topics[i, j]);

                                //draw tooltips
                                if (Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), boxes[i, j]))
                                {
                                    ToolTip(descriptions[i, j], new Rectangle(60, 220, 1160, 200));
                                }
                            }
                        }
                    }
                    else if (page == 0) //second page
                    {
                        //upper
                        Raygui.GuiGroupBox(new Rectangle(40, 180, 580, 310), "Controls");
                        Raygui.GuiGroupBox(new Rectangle(660, 180, 580, 310), "How to Play");

                        Raygui.GuiDrawText("WASD -> Move around\nC -> Charge Energy\nE -> Shop\nRMB -> Aim\nLMB -> Shoot", new Rectangle(40, 180, 580, 310), 1, Color.DarkGray);
                        Raygui.GuiDrawText("Move through the caves,\nkill any entity that's a threat to you,\nand survive till the end.", new Rectangle(660, 180, 580, 310), 1, Color.DarkGray);

                        //lower
                        Raygui.GuiGroupBox(new Rectangle(40, 540, 380, 310), "Score");
                        Raygui.GuiGroupBox(new Rectangle(450, 540, 380, 310), "Energy");
                        Raygui.GuiGroupBox(new Rectangle(860, 540, 380, 310), "Checkpoints");

                        Raygui.GuiDrawText("Every entity has different\nbehaviours (see next page),\nhealth and scoring. Killing them with\nthe same entity with a different\nweapon does not change how\nmuch score you gain. Save it up,\nyou'll need it!", new Rectangle(40, 540, 380, 310), 1, Color.DarkGray);
                        Raygui.GuiDrawText("You have both active and passive\nweapons. Every single weapon has\na different energy consumption\nper use! But you can recharge 1\nenergy by trading in 50 scores.\nSo don't be too crazy with\nattacking, and make every\nattack count!", new Rectangle(350, 540, 580, 310), 1, Color.DarkGray);
                        Raygui.GuiDrawText("Every once a while, you'll reach a\ncheckpoint. That would be the time\nto use your score to trade for\nsome weapons and gadgets.\nNo entities will spawn so take\nyour time!", new Rectangle(760, 540, 580, 310), 1, Color.DarkGray);

                        //page button
                        if (Raygui.GuiButton(new Rectangle(1020, 70, 200, 80), "Next Page") == 1)
                        {
                            page++;
                        }
                        
                        //title
                        Raylib.DrawText("How to Play", 460, 70, 56, Color.Black);
                    }

                    //back button
                    if (Raygui.GuiButton(new Rectangle(60, 70, 200, 80), "Back") == 1)
                    {
                        currentState = (int)States.menu;
                    }
                    break;
            }
            //global shop
            shop.Draw(ref currentState, ref atCheckpoint);
        }

        public static void CollisionChecks()
        {

            List<Entity> collidedEntities = new List<Entity>();

            for (int i = 0; i < entityList.Count; i++)
            {
                if (Raylib.CheckCollisionRecs(gameBounds, entityList[i].Dest)) //zombies out of bounds cannot be attacked
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
                                    if (!shotgun.scored)
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

        public static void ToolTip(string text, Rectangle box)
        {
            Raygui.GuiSetStyle(0, Raygui.TEXT_SIZE, 36);
            Raygui.GuiDrawText(text, box, 1, Color.DarkGray);
            Raygui.GuiSetStyle(0, Raygui.TEXT_SIZE, 24);
        }
    }
}