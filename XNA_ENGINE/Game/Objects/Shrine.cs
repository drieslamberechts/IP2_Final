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
    public class Shrine : Building
    {
        private const float GRIDHEIGHT = 32;
        private const float TIMEPERINFLUENCEPOINT = 5;

        private double m_InfluenceTimer = TIMEPERINFLUENCEPOINT;

        private const float TIMEFORSHAMAN = 2;
        private double m_Timer = TIMEFORSHAMAN;
        private int m_AmountOfShamansQueued = 0;

        public Shrine(List<GridTile> tileList)
        {
            m_PlaceableType = PlaceableType.Shrine;

            m_LinkedTileList = new List<GridTile>();

            foreach (var gridTile in tileList)
            {
                gridTile.SetIsUsedByStructure(true);
                gridTile.LevelOut(0, 2);
                m_LinkedTileList.Add(gridTile);
            }
            m_RallyPointTile = tileList.ElementAt(0);

            m_Model = new GameModelGrid("Models/building_Shrine");
            m_Model.LocalPosition += new Vector3(m_LinkedTileList.ElementAt(0).Model.LocalPosition.X, GRIDHEIGHT, m_LinkedTileList.ElementAt(0).Model.LocalPosition.Z);
            Quaternion rotation = new Quaternion(new Vector3(0,1,0), 0);
            m_Model.LocalRotation += rotation;
            m_Model.CanDraw = true;
            m_Model.LoadContent(PlayScene.GetContentManager());
            m_Model.UseTexture = true;

            m_Model.CreateBoundingBox(50, 64, 50, new Vector3(0, 50, 0));
            m_Model.DrawBoundingBox = false;

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
            if (m_AmountOfShamansQueued > 0)
            {
                m_Timer -= (renderContext.GameTime.ElapsedGameTime.Milliseconds / 1000.0);

                if (m_Timer <= 0)
                {
                    Console.WriteLine("Shaman built");
                    m_Timer = TIMEFORSHAMAN;
                    --m_AmountOfShamansQueued;
                    GridFieldManager.GetInstance().UserPlayer.AddPlaceable(new Shaman(SearchForDefaultRallyPointSpot(),m_RallyPointTile));
                }
            }

            m_InfluenceTimer -= (renderContext.GameTime.ElapsedGameTime.Milliseconds/1000.0);

            if (m_InfluenceTimer <= 0)
            {
                m_InfluenceTimer = TIMEPERINFLUENCEPOINT;
                m_Owner.GetResources().AddInfluence(1);
            }

            if (Menu.GetInstance().m_Enable8)
            {
                Menu.GetInstance().m_Enable8 = false;
                Menu.GetInstance().m_Enable9 = true;
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
                    permaSelected.GetOwner().RemovePlaceable(permaSelected);
                    Sacrifice();
                }
            }

            base.OnSelected();
        }

        private void Sacrifice()
        {
            m_Owner.GetResources().AddInfluence(10);
        }

        public override void QueueShaman(int amount = 1)
        {
            int amountOfShamansInPlayer = 0;
            foreach (Placeable placeable in m_Owner.GetOwnedList())
                if (placeable.PlaceableTypeMeth == PlaceableType.Shaman) ++amountOfShamansInPlayer;

            if (m_AmountOfShamansQueued == 0 && amountOfShamansInPlayer == 0)
                m_AmountOfShamansQueued += amount;
        }

        //Code to execute on Permanently selected
        public override void OnPermanentSelected()
        {
            //Get the inputmanager
            var inputManager = PlayScene.GetInputManager();
            var gridFieldManager = GridFieldManager.GetInstance();

            Menu.GetInstance().SubMenu = Menu.SubMenuSelected.ShrineMode;

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

        public override int GetQueuedShaman()
        {
            return m_AmountOfShamansQueued;
        }
    }
}
