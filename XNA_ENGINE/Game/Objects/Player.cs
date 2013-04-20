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
        const int INFLUENCE = 0;

        private const int LOW_WOOD = 10;
        private const int LOW_INFLUENCE = 10;

        private int m_ArmyCount;

        private readonly Resources m_Resources;

        // AI
        private AI m_Ai;
        private bool m_bIsAI;

        // Army
        private List<Army> m_ArmyList; 

        // METHODS
        public Player(bool isAI)
        {
            // Initialize
            m_bIsAI = isAI;
            m_Ai = new AI();

            m_ArmyCount = 0;
            m_ArmyList = new List<Army>();

            m_Resources = new Resources();
        }

        public void Update()
        {
            if (m_bIsAI)
            {
                // -------------------------
                // AI UPDATE
                // -------------------------
                if (m_Resources.GetAllResources()[WOOD] <= LOW_WOOD)
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

        public Resources GetResources()
        {
            return m_Resources;
        }

        public AI GetPlayerOptions()
        {
            return m_Ai;
        }

        public int GetArmySize()
        {
            return m_ArmyCount;
        }

        public bool GetAttack()
        {
           return m_Ai.GetAttack();
        }

        public void ResetAttack()
        {
            m_Ai.ResetAttack();
        }

        public Army GetSelectedArmy()
        {
            // Hier moet de selectie komen (welke van de legers is geselecteerd)

            m_ArmyList.Add(new Army()); // Om te testen

            return m_ArmyList[0];
        }

        public List<Army> GetArmyList()
        {
            return m_ArmyList;
        }
    }

    // ------------------------------------
    // ------------------------------------
    // RESOURCES
    // ------------------------------------
    // ------------------------------------
    class Resources
    {
        // VARIABLES
        float m_Wood, m_Influence;

        // METHODS
        public Resources()
        {
            m_Wood = 100;
            m_Influence = 100;
        }

        // GET RESOURCES
        public List<float> GetAllResources()
        {
            List<float> resourceArray = new List<float>();
            resourceArray.Add(m_Wood);
            resourceArray.Add(m_Influence);

            return resourceArray;
        }

        // ADD RESOURCES
        public void AddWood(float wood)
        {
            m_Wood += wood;
        }

        public void AddInfluence(float influence)
        {
            m_Influence += influence;
        }

        // DECREASE RESOURCES
        public void DecreaseWood(float wood)
        {
            m_Wood -= wood;
        }

        public void DecreaseInfluence(float influence)
        {
            m_Influence -= influence;
        }

    }
}
