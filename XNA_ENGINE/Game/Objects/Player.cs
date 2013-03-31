using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNA_ENGINE.Game.Objects
{
    class Player
    {
        // VARIABLES
        const int WOOD = 0;
        const int FOOD = 1;
        const int MONEY = 2;

        private Resources m_Resources;

        // METHODS
        public Player()
        {
            m_Resources = new Resources();
        }

        public List<float> GetResources()
        {
            return m_Resources.GetResources();
        }
    }

    class Resources
    {
        // VARIABLES
        float m_Wood, m_Food, m_Money;

        // METHODS
        public Resources()
        {
            m_Wood = 100;
            m_Food = 100;
            m_Money = 100;
        }

        public List<float> GetResources()
        {
            List<float> resourceArray = new List<float>();
            resourceArray.Add(m_Wood);
            resourceArray.Add(m_Food);
            resourceArray.Add(m_Money);

            return resourceArray;
        }

        // ADD RESOURCES
        public void AddWood(float wood)
        {
            m_Wood += wood;
        }

        public void AddFood(float food)
        {
            m_Food += food;
        }

        public void AddMoney(float money)
        {
            m_Money = money;
        }

        // DECREASE RESOURCES
        public void DecreaseWood(float wood)
        {
            m_Wood -= wood;
        }

        public void DecreaseFood(float food)
        {
            m_Food -= food;
        }

        public void DecreaseMoney(float money)
        {
            m_Money -= money;
        }
    }
}
