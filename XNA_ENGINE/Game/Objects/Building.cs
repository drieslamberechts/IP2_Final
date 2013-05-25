using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XNA_ENGINE.Engine;
using XNA_ENGINE.Game.Managers;
using XNA_ENGINE.Game.Scenes;

//----------------
//This class will be used as an abstract class for placeable things on the grid
//----------------

namespace XNA_ENGINE.Game.Objects
{
    public abstract class Building : Placeable
    {
        protected GridTile m_RallyPointTile;
        protected GameModelGrid m_Rallypoint;

        public override void Initialize()
        {
            //Load a rallyPointFlag
            m_Rallypoint = new GameModelGrid("Models/prop_Flag");
            m_Rallypoint.CanDraw = true;
            m_Rallypoint.LoadContent(PlayScene.GetContentManager());
            m_Rallypoint.DiffuseColor = new Vector3(0.0f, 0.8f, 0.0f);
            GridFieldManager.GetInstance().GameScene.AddSceneObject(m_Rallypoint);
         
            base.Initialize();
        }

        public override void Update(RenderContext renderContext)
        {
            if (Model.Selected)
                OnSelected();

            if (Model.PermanentSelected)
                OnPermanentSelected();
            else
                m_Rallypoint.CanDraw = false;
        }

        public override void OnSelected()
        {
            base.OnSelected();
        }

        public override void OnPermanentSelected()
        {
            base.OnPermanentSelected();
        }

        public virtual GridTile SearchForDefaultRallyPointSpot()
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
                if (surroundingTile.IsOpen() && surroundingTile.GetIsUsedByStructure() == false)
                    return surroundingTile;

            return null;
        }

        public virtual bool PlaceRallyPoint(GridTile gridTile)
        {
            if (gridTile.IsOpen() && gridTile.GetIsUsedByStructure() == false)
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
