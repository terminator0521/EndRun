using System;
using System.Collections.Generic;
using System.Text;
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
        /// <returns>True if the edges of "inner" are within the bounds of "outter", false otherwise.</returns>
        /// 
        public static bool CheckEdges(Rectangle inner, Rectangle outter)
        {
            if (inner.X < outter.X || inner.Y < outter.Y || inner.X + inner.Width > outter.X + outter.Width || inner.Y + inner.Height > outter.Y + outter.Height)
            {
                return false;
            }

            return true;
        }

        public static bool SATCheckCollisionRecs()
        {

        }
    }
}
