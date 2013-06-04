using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XNA_ENGINE.Engine;
using XNA_ENGINE.Engine.Scenegraph;
using XNA_ENGINE.Game.Managers;
using XNA_ENGINE.Game.Scenes;

namespace XNA_ENGINE.Game.Objects
{
    public class Player
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

        // Army
        private List<Placeable> m_OwnedPlaceablesList;

        private List<Placeable> m_ObjectsNeedToBeRemoved;
        private List<Placeable> m_ObjectsNeedToBeAdded;

        private PlayerColor m_Color;

        public enum PlayerColor
        {
            Blue,
            Red,

            enumSize
        }

        // METHODS
        public Player(PlayerColor color)
        {
            m_Color = color;

            // Initialize
            m_Ai = new AI();

            m_ArmyCount = 0;
            m_OwnedPlaceablesList = new List<Placeable>();
            m_ObjectsNeedToBeRemoved = new List<Placeable>();
            m_ObjectsNeedToBeAdded = new List<Placeable>();

            m_Resources = new Resources();
        }

        public void Update(RenderContext renderContext)
        {
            //Push the objects from the add array to the owned array and clear the add array. We use this because sometimes stuff can be added in the update function
            foreach (var placeable in m_ObjectsNeedToBeAdded)
                m_OwnedPlaceablesList.Add(placeable);
            m_ObjectsNeedToBeAdded.Clear();

            //Remove every object that is in the removed array
            foreach (var placeable in m_ObjectsNeedToBeRemoved)
                m_OwnedPlaceablesList.Remove(placeable);
            m_ObjectsNeedToBeRemoved.Clear();

            //Update every placeable in the list
            foreach (var placeable in m_OwnedPlaceablesList)
            {
                placeable.Update(renderContext);
            }

           /* if (m_bIsAI)
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
            }*/
        }

        public Placeable HitTestPlaceables(Ray ray)
        {
            foreach (var placeable in m_OwnedPlaceablesList)
                if (placeable.Model.HitTest(ray)) return placeable;

            return null;
        }

        public bool HandleInput(RenderContext renderContext)
        {
            var inputManager = PlayScene.GetInputManager();
            bool isMouseInScreen = PlayScene.IsMouseInScreen(renderContext);

            //Raycast to grid
            if (isMouseInScreen)
            {
                var hittedPlaceable = HitTestPlaceables(PlayScene.CalculateCursorRay(renderContext));
                if (hittedPlaceable != null)
                {
                    if (inputManager.IsActionTriggered((int)PlayScene.PlayerInput.LeftClick))
                    {
                        bool value = hittedPlaceable.Model.PermanentSelected;
                        GridFieldManager.GetInstance().PermanentDeselect();
                        hittedPlaceable.Model.PermanentSelected = !value;
                        
                        return true;
                    }
                }
            }

            return false;
        }

        public void AddPlaceable(Placeable placeable)
        {
            m_ObjectsNeedToBeAdded.Add(placeable);
            placeable.SetOwner(this);
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
            var count = 0;

            foreach (Placeable placeable in m_OwnedPlaceablesList)
            {
                if (placeable.PlaceableTypeMeth == Placeable.PlaceableType.Army)
                {
                    Army army = (Army) placeable;
                    count += army.ArmySize;
                }
            }

            m_ArmyCount = count;

            return m_ArmyCount;
        }

        public int GetSelectedArmySize()
        {
            var permaSelected = GridFieldManager.GetInstance().GetPermanentSelected();
            if (permaSelected != null && permaSelected.PlaceableTypeMeth == Placeable.PlaceableType.Army)
            {
                Army army = (Army) permaSelected;

                return army.ArmySize;
            }
            return 0;
        }

        public int GetVillagerCount()
        {
            var count = 0;
            for (int t = 0; t < m_OwnedPlaceablesList.Count; ++t)
            {
                if (m_OwnedPlaceablesList.ElementAt(t).PlaceableTypeMeth == Placeable.PlaceableType.Villager)
                    count++;
            }

            return count;
        }

        public int GetShamanCount()
        {
            var count = 0;
            for (int t = 0; t < m_OwnedPlaceablesList.Count; ++t)
            {
                if (m_OwnedPlaceablesList.ElementAt(t).PlaceableTypeMeth == Placeable.PlaceableType.Shaman)
                    count++;
            }

            return count;
        }
        
        public bool GetAttack()
        {
           return m_Ai.GetAttack();
        }

        public void ResetAttack()
        {
            m_Ai.ResetAttack();
        }

       /* public Army GetSelectedArmy()
        {
            // Hier moet de selectie komen (welke van de legers is geselecteerd)

          //  m_OwnedPlaceablesList.Add(new Army()); // Om te testen



           // return m_OwnedPlaceablesList[0];
        }*/


        public void RemovePlaceable(Placeable placeable)
        {
            GridFieldManager.GetInstance().GameScene.RemoveSceneObject(placeable.Model);
            m_ObjectsNeedToBeRemoved.Add(placeable);
        }

        public List<Placeable> GetOwnedList()
        {
            return m_OwnedPlaceablesList;
        }
    }

    // ------------------------------------
    // ------------------------------------
    // RESOURCES
    // ------------------------------------
    // ------------------------------------
    public class Resources
    {
        // VARIABLES
        float m_Wood, m_Influence;

        // METHODS
        public Resources()
        {
            m_Wood = 100;
            m_Influence = 30;
        }

        // GET RESOURCES
        public List<float> GetAllResources()
        {
            List<float> resourceArray = new List<float>();
            resourceArray.Add(m_Wood);
            resourceArray.Add(m_Influence);

            return resourceArray;
        }

        public float GetWood()
        {
            return m_Wood;
        }

        public float GetInfluence()
        {
            return m_Influence;
        }

        // ADD RESOURCES
        public void AddWood(int wood)
        {
            m_Wood += wood;
        }

        public void AddInfluence(int influence)
        {
            m_Influence += influence;
        }

        // DECREASE RESOURCES
        public void DecreaseWood(int wood)
        {
            m_Wood -= wood;
        }

        public void DecreaseInfluence(float influence)
        {
            m_Influence -= influence;
        }

    }
}
