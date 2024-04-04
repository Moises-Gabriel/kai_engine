using Raylib_cs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kai_Engine.ENGINE.Systems
{
    internal class InputHandling
    {
        public int Horizontal()
        {
            int direction = 0;
            
            //Determines if the player is moving left or right
            //-1 is left & 1 is right
            if (Raylib.IsKeyPressed(KeyboardKey.A))
                direction = -1;

            if (Raylib.IsKeyPressed(KeyboardKey.D))
                direction = 1;

            return direction;
        }
    }
}
