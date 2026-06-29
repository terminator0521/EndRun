using Raylib_cs;
using raygui_cs;

namespace Game;
public static class Game
{
    public static void Main()
    {
        Raylib.InitWindow(1280, 720, "EndRun");

        while(!Raylib.WindowShouldClose())
        {
            Update();
            Draw();
        }
    }

    public static void Update()
    {

    }

    public static void Draw()
    {

    }
}