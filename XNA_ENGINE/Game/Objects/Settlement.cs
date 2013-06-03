using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNA_ENGINE.Engine;
using XNA_ENGINE.Engine.Scenegraph;
using XNA_ENGINE.Game.Objects;
using XNA_ENGINE.Game.Managers;
using XNA_ENGINE.Game.Scenes;

namespace XNA_ENGINE.Game.Objects
{
    public class Settlement : Building
    {
        private const float GRIDHEIGHT = 32;
        private const float TIMEFORVILLAGER = 2;
        private double m_Timer = TIMEFORVILLAGER;

        private int m_AmountOfVillagersQueued = 0;

        public Settlement(List<GridTile> tileList)
        {
            m_PlaceableType = PlaceableType.Settlement;

            m_LinkedTileList = new List<GridTile>();

            foreach (var gridTile in tileList)
            {
                gridTile.SetIsUsedByStructure(true);
                gridTile.LevelOut(0,2);
                m_LinkedTileList.Add(gridTile);
            }
                
            m_Model = new GameModelGrid("Models/building_Settlement");
            m_Model.LocalPosition += new Vector3(m_LinkedTileList.ElementAt(0).Model.LocalPosition.X + 30, GRIDHEIGHT, m_LinkedTileList.ElementAt(0).Model.LocalPosition.Z);
            Quaternion rotation = new Quaternion(new Vector3(0,1,0), 0);
            m_Model.LocalRotation += rotation;
            m_Model.CanDraw = true;
            m_Model.LoadContent(PlayScene.GetContentManager());
            m_Model.UseTexture = true;

            m_Model.CreateBoundingBox(100, 64, 100, new Vector3(0, 20, -20));
            m_Model.DrawBoundingBox = false;

            ////Child Model
            //var childModel = new GameModelGrid("Models/building_Settlement");
            //childModel.LocalPosition += new Vector3(0, 0, -64);
            //childModel.CanDraw = true;
            //childModel.LoadContent(PlayScene.GetContentManager());
            //childModel.UseTexture = true;
            //m_Model.AddChild(childModel);

            GridFieldManager.GetInstance().GameScene.AddSceneObject(m_Model);

            Initialize();
            PlaceRallyPoint(SearchForDefaultRallyPointSpot());
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(RenderContext renderContext)
        {
            if (m_AmountOfVillagersQueued > 0)
            {
                m_Timer -= (renderContext.GameTime.ElapsedGameTime.Milliseconds / 1000.0);

                if (m_Timer <= 0)
                {
                    Console.WriteLine("Villager built");
                    m_Timer = TIMEFORVILLAGER;
                    --m_AmountOfVillagersQueued;
                    GridFieldManager.GetInstance().UserPlayer.AddPlaceable(new Villager(SearchForDefaultRallyPointSpot(), m_RallyPointTile));
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

            }

            base.OnSelected();
        }

        //Code to execute on Permanently selected
        public override void OnPermanentSelected()
        {
            //Get the inputmanager
            var inputManager = PlayScene.GetInputManager();
            var gridFieldManager = GridFieldManager.GetInstance();

            //RallyPoint
            m_Rallypoint.Translate(m_RallyPointTile.Model.WorldPosition);
            m_Rallypoint.CanDraw = true;

            Menu.GetInstance().SubMenu = Menu.SubMenuSelected.SettlementMode;

            if (Menu.GetInstance().m_Enable2)
            {
                Menu.GetInstance().m_Enable2 = false;
                Menu.GetInstance().m_Enable3 = true;
            }

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

        public override void QueueVillager(int amount = 1)
        {
            m_AmountOfVillagersQueued += amount;
        }


        public int GetAmountOfVillagersQueued()
        {
            return m_AmountOfVillagersQueued;
        }
    }
}
