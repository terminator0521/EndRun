using EndRun.Entities;
using EndRun.weapons.Guns;
using EndRun.weapons.Melees;
using EndRun.Weapons.Guns;
using Raylib_cs;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace EndRun.User
{
    public class Player
    {
        public Texture2D texture; //player sprite
        public Vector2 pos = new Vector2(0); //player position
        public Vector2 lastPos = new Vector2(0); //player's last position
        public Vector2 dis = new Vector2(0); //player displacement vector
        public Rectangle Dest = new Rectangle(); //sprite dest rect
        public Vector2 origin = new Vector2(0);
        public int gadget;
        public Melee? melee; //player melee
        public Gun? gun; //players gun
        public int selectedSlot; //selected slot
        public int score;
        public int energy;
        public int energyCap; //default energy cap

        //powerbank related
        readonly private int rechargeRate = 60 * 2;
        readonly private int rechargeWaitTime = 60 * 3;
        private int rechargeTimeInterval = 0;
        bool waited = false;

        //shocker related
        private int shockedWaitTime = 60 * 4;
        public int shockedTimeInterval = 0;
        public bool invincible = false;
        public bool shocked = false; //shocked or nah

        //collided object lists
        public List<Entity> collidedObjects;

        enum SlotGadget
        {
            powerBank,
            shocker,
            antiLeak
        }

        enum SlotMelee
        {
            knife,
            katana,
            blaster,
        }
        enum SlotGun
        {
            handGun,
            ballShot,
            shotgun,
        }

        private float vel = 5f; //speed of player
        private Rectangle src; //sprite source rect
        private Rectangle bounds; //game bounds

        public bool up = false;
        public bool down = false;
        public bool left = false;
        public bool right = false;

        public Player(string spriteSheetLocation, int health, in Rectangle bounds)
        {
            SetSlot(0, gadget);
            energy = 60;
            texture = Raylib.LoadTexture(spriteSheetLocation); //load player texture
            origin = new Vector2(texture.Width / 2, texture.Height / 2);
            src = new Rectangle(0, 0, texture.Width, texture.Height); //set up texture src rect
            Dest = new Rectangle(pos, texture.Width, texture.Height); //set up texture dest rect
            this.bounds = bounds; //set game bounds
            selectedSlot = 1;
            collidedObjects = new List<Entity>();
            ResetState();
        }

        public void Update()
        {
            if (invincible)
            {
                if(shockedTimeInterval < shockedWaitTime)
                {
                    shockedTimeInterval++;
                }
                else
                {
                    invincible = false;
                    shockedTimeInterval = 0;
                }
            }

            if (gadget == (int)SlotGadget.antiLeak)
            {
                energyRecharge();
            }

            //cap energy level
            if (energy > energyCap)
            {
                energy = energyCap;
            }

            //actions
            gun?.Update(pos + origin + dis, ref score);
            melee?.Update(pos + origin + dis, ref score);
            Move();

            //reset movement bools
            up = false;
            down = false;
            left = false;
            right = false;
        }

        public void Draw()
        {
            //draw weapons
            melee?.Draw(ref selectedSlot);
            gun?.Draw(ref selectedSlot);

            //draw player sprite
            if (!invincible)
            {
                Raylib.DrawTexturePro(texture, src, Dest, new Vector2(0), 0, Color.White);
            }
            else
            {
                Raylib.DrawTexturePro(texture, src, Dest, new Vector2(0), 0, Raylib.Fade(Color.White, 0.5f));
            }
        }

        public void Move()
        {
            lastPos = pos;
            dis = new Vector2(0);

            //initial vector2 change
            dis.X = ((right ? 1 : 0) + (left ? -1 : 0)) * vel;
            dis.Y = ((down ? 1 : 0) + (up ? -1 : 0)) * vel;

            //vector2 normalization = divide by tan 45 deg if the other axis isn't 0
            dis.X /= dis.Y == 0 ? 1 : (float)MathF.Sqrt(2);
            dis.Y /= dis.X == 0 ? 1 : (float)MathF.Sqrt(2);

            //add displacement to y-pos
            pos.Y += dis.Y;
            Dest.Y = pos.Y;

            if (Functions.CheckCollisionEdges(Dest, bounds))
            {
                pos.Y = lastPos.Y;
                Dest.Y = pos.Y;
            }

            //add displacement to x-pos
            pos.X += dis.X;
            Dest.X = pos.X;

            if (Functions.CheckCollisionEdges(Dest, bounds))
            {
                pos.X = lastPos.X;
                Dest.X = pos.X;
            }

        }

        public void UpdateCollidedObjects(List<Entity> collided)
        {
            collidedObjects = collided;
        }

        public void ResetState()
        {
            energyCap = 100 + (gadget == (int)SlotGadget.powerBank ? 20 : 0); //set energy cap

            selectedSlot = 0;
            score = 0;
            energy = 60;
            ResetPos();
            gun?.Reset();
        }

        public void ResetPos()
        {
            pos = new Vector2(50, (bounds.Height / 2) - (Dest.Height / 2));
            shocked = false;
            shockedTimeInterval = 0;
            invincible = false;
        }

        public void Recharge()
        {
            if (score >= 50 & energy < energyCap)
            {
                score -= 50;
                energy++;
            }
        }

        public void SetSlot(int slot, int weapon)
        {

            switch (slot)
            {
                case 0:
                    switch (weapon)
                    {
                        case (int)SlotGadget.powerBank:
                            gadget = (int)SlotGadget.powerBank;
                            break;
                        case (int)SlotGadget.shocker:
                            gadget = (int)SlotGadget.shocker;
                            break;
                    }
                    break;
                case 1:
                    switch (weapon)
                    {
                        case (int)SlotMelee.knife:
                            melee = new Knife();
                            break;
                        case (int)SlotMelee.katana:
                            melee = new Katana();
                            break;
                        case (int)SlotMelee.blaster:
                            melee = new Blaster();
                            break;
                    }
                    break;
                case 2:
                    switch (weapon)
                    {
                        case (int)SlotGun.handGun:
                            gun = new HandGun();
                            break;
                        case (int)SlotGun.ballShot:
                            gun = new BallShot(ref bounds);
                            break;
                        case (int)SlotGun.shotgun:
                            gun = new Shotgun(ref bounds);
                            break;
                    }
                    break;
                case 3:
                    break;
                default:
                    Console.WriteLine("nothing set");
                    break;
            }
        }

        private void energyRecharge()
        {
            if (up || down || left || right || Raylib.IsMouseButtonPressed(MouseButton.Left))
            {
                if (waited)
                {
                    waited = false;
                }
                if (rechargeTimeInterval != 0)
                {
                    rechargeTimeInterval = 0;
                }
            }
            else
            {
                if (!waited)
                {
                    if (rechargeTimeInterval < rechargeWaitTime)
                    {
                        rechargeTimeInterval++;
                    }
                    else
                    {
                        rechargeTimeInterval = 0;
                        waited = true;
                    }
                }
                else
                {
                    if (rechargeTimeInterval < rechargeRate)
                    {
                        rechargeTimeInterval++;
                    }
                    else
                    {
                        rechargeTimeInterval = 0;

                        //recharge logic
                        energy++;
                    }
                }
            }
        }
        public void Shock()
        {
            energy = 0;
            shocked = true;
            invincible = true;
        }
    }
}
