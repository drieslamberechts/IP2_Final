using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace XNA_ENGINE.Game.Objects
{
    class Army
    {
        private Vector2 m_Position; // Same as the grid tile
        private int m_ArmyCount;

        public Army()
        {
            m_ArmyCount = 0;
            m_Position = new Vector2(0, 0);
        }

        public void SetPosition(Vector2 pos)
        {
            m_Position = pos;
        }

        public Vector2 GetCurrentPosition()
        {
            return m_Position;
        }

        public void SplitArmy()
        {
            m_ArmyCount /= 2;
        }

        public int GetArmyCount()
        {
            return m_ArmyCount;
        }

        public void SetArmyCount(int armyCount)
        {
            m_ArmyCount = armyCount;
        }
    }
}
