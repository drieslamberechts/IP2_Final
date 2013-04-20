using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XNA_ENGINE.Game.Objects;


namespace XNA_ENGINE.Game.Objects
{
    class Army : Placeable
    {
        public Vector2 GetCurrentPosition()
        {
            // Remove this
            return new Vector2(0, 0);
        }
    }
}
