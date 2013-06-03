using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNA_ENGINE.Engine;
using XNA_ENGINE.Engine.Scenegraph;
using XNA_ENGINE.Game.Helpers;
using XNA_ENGINE.Game.Objects;
using XNA_ENGINE.Game.Managers;
using XNA_ENGINE.Game.Scenes;

namespace XNA_ENGINE.Game.Objects
{
    public class School : Building
    {
        private const float GRIDHEIGHT = 32;
        private const float TIMEFORVILLAGER = 2;
        private double m_Timer = TIMEFORVILLAGER;

        private int m_AmountOfSoldiersQueued = 0;

        private List<Villager> m_DesignatedVillagerList;

        public School(List<GridTile> tileList)
        {
            m_PlaceableType = PlaceableType.School;

            m_LinkedTileList = new List<GridTile>();

            foreach (var gridTile in tileList)
            {
                gridTile.SetIsUsedByStructure(true);
                gridTile.LevelOut(0, 2);
                m_LinkedTileList.Add(gridTile);
            }

            m_RallyPointTile = tileList.ElementAt(0);

            m_Model = new GameModelGrid("Models/building_School");
            m_Model.LocalPosition += new Vector3(m_LinkedTileList.ElementAt(0).Model.LocalPosition.X + 30, GRIDHEIGHT, m_LinkedTileList.ElementAt(0).Model.LocalPosition.Z + 10);
            Quaternion rotation = new Quaternion(new Vector3(0,1,0), 0);
            m_Model.LocalRotation += rotation;
            m_Model.CanDraw = true;
            m_Model.LoadContent(PlayScene.GetContentManager());
            m_Model.UseTexture = true;

            m_Model.CreateBoundingBox(100, 32, 100, new Vector3(0, 0, 0));
            m_Model.DrawBoundingBox = false;

            GridFieldManager.GetInstance().GameScene.AddSceneObject(m_Model);

            Initialize();
            PlaceRallyPoint(SearchForDefaultRallyPointSpot());

            m_DesignatedVillagerList = new List<Villager>();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(RenderContext renderContext)
        {
            if (m_DesignatedVillagerList.Any())
            {
                List<Villager> deleteList = new List<Villager>();

                foreach (Villager villager in m_DesignatedVillagerList)
                {
                    if (villager.CurrentTile == SearchForDefaultRallyPointSpot() && villager.GetOwner().GetResources().GetWood() >= StandardCost.COSTOFWOOD_SOLDIER)
                    {
                        villager.GetOwner().RemovePlaceable(villager);
                        m_Owner.GetResources().DecreaseWood(StandardCost.COSTOFWOOD_SOLDIER);
                        m_Owner.GetResources().DecreaseInfluence(StandardCost.COSTOFINFLUENCE_SOLDIER);
                        QueueSoldier();
                        deleteList.Add(villager);
                    }
                }

                foreach (Villager villager in deleteList)
                {
                    m_DesignatedVillagerList.Remove(villager);
                }
            }

            if (m_AmountOfSoldiersQueued > 0)
            {
                m_Timer -= (renderContext.GameTime.ElapsedGameTime.Milliseconds / 1000.0);

                if (m_Timer <= 0)
                {
                    Console.WriteLine("Soldier built");
                    m_Timer = TIMEFORVILLAGER;
                    --m_AmountOfSoldiersQueued;
                    GetOwner().AddPlaceable(new Army(SearchForDefaultRallyPointSpot() ,m_RallyPointTile));
                }
            }

            base.Update(renderContext);
        }

        //Code to execute on hit with mouse
        public override void OnSelected()
        {
            //Get the inputmanager
            var inputManager = PlayScene.GetInputManager();
            //What mode is there selected in the menu to build?
            Menu.ModeSelected selectedMode = Menu.GetInstance().GetSelectedMode();

            if (inputManager.GetAction((int)PlayScene.PlayerInput.LeftClick).IsTriggered)
            {
                
            }

            if (inputManager.GetAction((int)PlayScene.PlayerInput.RightClick).IsTriggered)
            {
                Placeable permaSelected = GridFieldManager.GetInstance().GetPermanentSelected();
                if (permaSelected != null && permaSelected.PlaceableTypeMeth == PlaceableType.Villager)
                {
                    permaSelected.GoToTile(SearchForDefaultRallyPointSpot());
                    m_DesignatedVillagerList.Add((Villager)permaSelected);
                }
            }

            if (Menu.GetInstance().m_Enable5)
            {
                Menu.GetInstance().m_Enable5 = false;
                Menu.GetInstance().m_Enable6 = true;
            }

            base.OnSelected();
        }

        //Code to execute on Permanently selected
        public override void OnPermanentSelected()
        {
            //Get the inputmanager
            var inputManager = PlayScene.GetInputManager();
            var gridFieldManager = GridFieldManager.GetInstance();

            Menu.GetInstance().SubMenu = Menu.SubMenuSelected.SchoolMode;

            //RallyPoint
            m_Rallypoint.Translate(m_RallyPointTile.Model.WorldPosition);
            m_Rallypoint.CanDraw = true;

            GridTile selectedTile;
            if (gridFieldManager.GetSelectedTiles() != null && gridFieldManager.GetSelectedTiles().Any())
                selectedTile = gridFieldManager.GetSelectedTiles().ElementAt(0);
            else
                selectedTile = null;


            if (inputManager.GetAction((int)PlayScene.PlayerInput.RightClick).IsTriggered)
            {
                if (selectedTile != null)
                    PlaceRallyPoint(selectedTile);
            }

            base.OnPermanentSelected();
        }

        public override void QueueSoldier(int amount = 1)
        {
            m_AmountOfSoldiersQueued += amount;
        }

        public override int GetQueuedSoldiers()
        {
            return m_AmountOfSoldiersQueued;
        }
    }
}
