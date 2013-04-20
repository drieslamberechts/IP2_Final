using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XNA_ENGINE.Game.Objects
{
    class Sjaman : Placeable
    {
        private Vector2 m_Position;

        public Sjaman()
        {
            m_Position = new Vector2(0, 0);
        }

        public void SetPosition(Vector2 position)
        {
            m_Position = position;
        }

        public Vector2 GetPosition()
        {
            return m_Position;
        }
    }
}
