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

        private const int LOW_WOOD = 10;
        private const int LOW_FOOD = 10;
        private const int LOW_MONEY = 10;

        private readonly Resources m_Resources;

        // AI
        private AI m_Ai;
        private bool m_bIsAI;

        // METHODS
        public Player(bool isAI)
        {
            m_bIsAI = isAI;

            m_Resources = new Resources();
        }

        public void Update()
        {
            if (m_bIsAI)
            {
                // -------------------------
                // AI UPDATE
                // -------------------------
                if (m_Resources.GetResources()[WOOD] <= LOW_WOOD)
                {
                    // Search for resources
                    m_Ai.Scout();
                }

                // need to be able to get current tile for army
            }
            else
            {
                // REGULAR UPDATES
            }
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
