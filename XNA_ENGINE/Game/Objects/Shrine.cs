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
    public class Shrine : Placeable
    {
        private GridTile m_RallyPointTile;

        private const float GRIDHEIGHT = 32;
        private const float TIMEPERINFLUENCEPOINT = 5;

        private double m_InfluenceTimer = TIMEPERINFLUENCEPOINT;

        public Shrine(List<GridTile> tileList)
        {
            m_PlaceableType = PlaceableType.Shrine;

            m_LinkedTileList = new List<GridTile>();

            foreach (var gridTile in tileList)
                m_LinkedTileList.Add(gridTile);

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
            SearchForDefaultRallyPointSpot();
        }

        public virtual void Initialize()
        {
            base.Initialize();
        }

        public override void Update(RenderContext renderContext)
        {
            m_InfluenceTimer -= (renderContext.GameTime.ElapsedGameTime.Milliseconds/1000.0);

            if (m_InfluenceTimer <= 0)
            {
                m_InfluenceTimer = TIMEPERINFLUENCEPOINT;
                //Menu.GetInstance().Player.GetResources().AddInfluence(1);
            }

            base.Update(renderContext);
        }

        //Code to execute on hit with mouse
        public override void OnSelected()
        {
            //if (!m_LinkedTile.Selected) return false;

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

        private void SearchForDefaultRallyPointSpot()
        {
            List<GridTile> totalSurroundingTiles = new List<GridTile>();
            List<GridTile> surroundingTiles = new List<GridTile>();
            foreach (var structureTile in m_LinkedTileList)
            {
                surroundingTiles.Clear();
                surroundingTiles = GridFieldManager.GetInstance().GetAllSurroundingTiles(structureTile);

                //Loop over surrounding tiles
                foreach (var surroundingTile in surroundingTiles)
                    totalSurroundingTiles.Add(surroundingTile);
            }

            List<GridTile> removeList = new List<GridTile>();
            foreach (var surroundingTile in totalSurroundingTiles)
            {
                foreach (var structureTile in m_LinkedTileList)
                {
                    //If the tile is a tile on the structure
                    if (structureTile == surroundingTile)
                        removeList.Add(structureTile);
                }
            }

            //Remove the elements form the list
            foreach (var gridTile in removeList)
            {
                totalSurroundingTiles.Remove(gridTile);
            }

            foreach (var surroundingTile in totalSurroundingTiles)
            {
                if (PlaceRallyPoint(surroundingTile))
                    return;
            }
        }

        //Code to execute on Permanently selected
        public override void OnPermanentSelected()
        {
            //Get the inputmanager
            var inputManager = PlayScene.GetInputManager();
            Menu.GetInstance().SubMenu = Menu.SubMenuSelected.ShrineMode;

            base.OnPermanentSelected();
        }

        public bool PlaceRallyPoint(GridTile gridTile)
        {
            if (gridTile.IsOpen())
            {
                m_RallyPointTile = gridTile;
                m_Rallypoint.Translate(m_RallyPointTile.Model.WorldPosition);

                return true;
            }
            //Couldn't place the rallypoint
            return false;
        }

    }
}
