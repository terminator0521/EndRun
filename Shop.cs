using EndRun.User;
using EndRun.Weapons;
using raygui_cs;
using Raylib_cs;
using System.Diagnostics.Eventing.Reader;
using System.Numerics;
using System.Printing;
using System.Runtime.CompilerServices;

namespace EndRun
{
    public class Shop
    {
        public bool open = true;
        private int selectedIndex;
        private int selectedClass;
        public int scroll;
        Rectangle window = new Rectangle(200, 100, 880, 700);
        Rectangle list = new Rectangle(210, 240, 430, 315);
        Rectangle prev = new Rectangle(210, 170, 50, 50);
        Rectangle next = new Rectangle(570, 170, 50, 50);
        Rectangle r_Class = new Rectangle(270, 170, 290, 50);
        Rectangle stats = new Rectangle(650, 140, 415, 650);
        Rectangle buy = new Rectangle(230, 600, 390, 100);

        Player player;

        private string[,] weapon =
        {
            {"Power Bank", "Shocker", "Anti-Leak" },
            {"Knife", "Katana", "Blaster" },
            {"Hand Gun", "Ballshot", "Shotun" },
        };

        private string[] weaponList =
        {
            "Power Bank;Shocker;Anti-Leak",
            "Knife;Katana;Blaster",
            "Hand Gun;Ballshot;Shotgun",
        };

        private string[,] weaponEnergyUsage =
        {
            { "0", "0", "All" },
            { "0", "15", "20" },
            { "2", "20", "20" },
        };

        private int[,] weaponCost =
        {
            {20000, 22000, 20000 },
            {0, 19000, 22000 },
            {25000, 30000, 27000 }
        };

        private string[,] weaponDescription =
        {
            {
                "A portable\nbattery pack that\nholds condenced\nplasmic energy.",
                "A generator\nthat generates a\none time shock\nwhich prevent\nelectronics from\npowering down\ndue to insufficent\npower output.",
                "A device that\nmodifies energyflow\nto prevent extra\npower from flowing\ntowards unused\ncomponents and parts."
            },
            {
                "Short-ranged and\nzero energy knife\nthat is convenient\nto bring." ,
                "6 feel long\nkatana that slices\natoms apart.",
                "Looks like the\nground pounders\nyou see construction\nworkers use to\nflaten the ground.\nShocks anything\naround you and is\nvery light"
            },
            {
                "A self-defense\npistol issued to\nfamilies with who\nlive near warzones.",
                "A light-weight\ncannon that fires\nenergy spheres\nthat disintegrates\neveything it\ntouches.",
                "A shotgun\nprototype created\nby a government\nfunded weapon\ncompany that\nfires energy spheres\nthat spread out."
            },
        };
        public Shop(ref Player player)
        {
            Raygui.GuiSetStyle(Raygui.LISTVIEW, Raygui.TEXT_SIZE, 96); //set text size
            this.player = player;
        }

        public void Draw(ref int state, ref bool checkpoint)
        {

            if (open && checkpoint)
            {
                int temp = 0;
                Raygui.GuiSetStyle(0, Raygui.TEXT_SIZE, 24); //set text size

                if (Raygui.GuiWindowBox(window, "\tMarket") == 1)
                {
                    open = false;
                    if (state == 1)
                    {
                        state = 0;
                    }
                }
                else
                {
                    //set text size
                    Raygui.GuiSetStyle(Raygui.DEFAULT, Raygui.LIST_ITEMS_HEIGHT, 48);

                    //buy button and insufficent score message
                    
                    if (player.score < weaponCost[selectedClass, selectedIndex])
                    {
                        Raylib.DrawText("Insufficent Score", 260, 600, 36, Color.Red);
                    }
                    else if (Raygui.GuiButton(buy, "Buy") == 1)
                    {
                        Buy();
                    }

                    //class selection
                    Raylib.DrawRectangleRec(r_Class, Color.LightGray);
                    Raylib.DrawRectangleLinesEx(r_Class, 2, Color.Gray);
                    switch (selectedClass)
                    {
                        case 0:
                            Raylib.DrawText("Gadgets", 350, 180, 32, Color.Black);
                            break;
                        case 1:
                            Raylib.DrawText("Melees", 360, 180, 32, Color.Black);
                            break;
                        case 2:
                            Raylib.DrawText("Guns", 380, 180, 32, Color.Black);
                            break;
                    }

                    if (Raygui.GuiButton(prev, "<-") == 1 && selectedClass > 0)
                    {
                        selectedClass--;
                        selectedIndex = 0;
                    }
                    if (Raygui.GuiButton(next, "->") == 1 && selectedClass < 2)
                    {
                        selectedClass++;
                        selectedIndex = 0;
                    }
                    //weapon selections
                    temp = Raygui.GuiListView(list, weaponList[selectedClass], ref scroll, selectedIndex);
                    if (temp != -1)
                    {
                        selectedIndex = temp;
                    }

                    //display stats
                    Raygui.GuiSetStyle(Raygui.DEFAULT, Raygui.TEXT_SIZE, 32);
                    Raygui.GuiGroupBox(stats, weapon[selectedClass, selectedIndex]);

                    Raylib.DrawText($"Energy Usage: {weaponEnergyUsage[selectedClass, selectedIndex]}", 680, 280, 32, Color.Black);
                    Raylib.DrawText($"Cost: {weaponCost[selectedClass, selectedIndex]}", 680, 380, 32, Color.Black);
                    Raylib.DrawText($"Description:\n {weaponDescription[selectedClass, selectedIndex]}", 680, 480, 32, Color.Black);
                    
                }



            }
            else
            {
                open = false;
            }
            //reset text size
            Raygui.GuiSetStyle(Raygui.DEFAULT, Raygui.TEXT_SIZE, 32);
        }
        
        private void Buy()
        {
            if (player.score >= weaponCost[selectedClass, selectedIndex])
            {
                player.SetSlot(selectedClass, selectedIndex);
            }
        }
    }
}

