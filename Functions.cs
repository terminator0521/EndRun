using System.Net.Http.Headers;
using System.Numerics;
using Raylib_cs;

namespace EndRun
{
    public static class Functions
    {

        /// <summary>
        /// Checks if the edges of the "inner" rectangle are within the bounds of the "outter" rectangle.
        /// </summary>
        /// <param name="inner">The rectangle whose edges are to be checked.</param>
        /// <param name="outter">The rectangle that defines the bounds.</param>
        /// <returns>false if the edges of "inner" are within the bounds of "outter", true otherwise.</returns>
        /// 
        public static bool CheckCollisionEdges(Rectangle inner, Rectangle outter)
        {
            if (inner.X >= outter.X && inner.Y >= outter.Y && inner.X + inner.Width <= outter.X + outter.Width && inner.Y + inner.Height <= outter.Y + outter.Height)
            {
                return false;
            }

            return true;
        }

        public static bool CheckCollisionsQuad(Rectangle rect1, float angle1, Rectangle rect2, float angle2)
        {
            //arrays for line points
            Vector2[][] lines1 = new Vector2[4][];
            Vector2[][] lines2 = new Vector2[4][];

            //vector for line to line collision
            Vector2 CP = new Vector2(0);

            //declare lines from first rect 
            lines1[0] =
            [
                new Vector2(rect1.X, rect1.Y),
                new Vector2(rect1.X + (rect1.Width * MathF.Cos(angle1)), rect1.Y - (rect1.Width * MathF.Sin(angle1)))
            ];
            lines1[1] =
            [
                new Vector2(rect1.X, rect1.Y),
                new Vector2(rect1.X + (rect1.Height * MathF.Sin(angle1)), rect1.Y + (rect1.Height * MathF.Cos(angle1)))
            ];
            lines1[2] =
            [
                lines1[1][1],
                new Vector2(lines1[1][1].X + (rect1.Width * MathF.Cos(angle1)), lines1[1][1].Y - (rect1.Width * MathF.Sin(angle1)))
            ];
            lines1[3] =
            [
                lines1[2][1],
                new Vector2(lines1[2][1].X - (rect1.Height * MathF.Sin(angle1)), lines1[2][1].Y - (rect1.Height * MathF.Cos(angle1)))
            ];

            //declare lines from second rect
            lines2[0] =
            [
                new Vector2(rect2.X, rect2.Y),
                new Vector2(rect2.X + (rect2.Width * MathF.Cos(angle2)), rect2.Y - (rect2.Width * MathF.Sin(angle2)))
            ];
            lines2[1] =
            [
                new Vector2(rect2.X, rect2.Y),
                new Vector2(rect2.X + (rect2.Height * MathF.Sin(angle2)), rect2.Y + (rect2.Height * MathF.Cos(angle2)))
            ];
            lines2[2] =
            [
                lines2[1][1],
                new Vector2(lines2[1][1].X + (rect2.Width * MathF.Cos(angle2)), lines2[1][1].Y - (rect2.Width * MathF.Sin(angle2)))
            ];
            lines2[3] =
            [
                lines2[2][1],
                new Vector2(lines2[2][1].X - (rect2.Height * MathF.Sin(angle2)), lines2[2][1].Y - (rect2.Height * MathF.Cos(angle2)))
            ];

            //checks
            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (Raylib.CheckCollisionLines(lines1[i][0], lines1[i][1], lines2[j][0], lines2[j][1], ref CP))
                    {
                        return true;
                    }
                }

            }

            //for checking what rect looks like in logic
            //for (int i = 0; i < 4; i++)
            //{
            //    Raylib.DrawLineV(lines1[i][0], lines1[i][1], Color.Black);
            //}

            //no lines collided
            return false;

        }
    }
}
