using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XNA_ENGINE.Engine.Scenegraph;
using XNA_ENGINE.Game.Objects;

namespace XNA_ENGINE.Game.Managers
{
    class GridFieldManager
    {
        public enum SelectionMode
        {
            select1x1,
            select2x2,
            select3x3,

            enumSize
        }

        //Singleton implementation
        private static GridFieldManager m_GridFieldManager;

        private GridTile[,] m_GridField;


        private const int GRID_ROW_LENGTH = 30;
        private const int GRID_COLUMN_LENGTH = 30;

        private SelectionMode m_SelectionMode = SelectionMode.select1x1;

        // private int GRID_OFFSET = 64;

        private Random m_Random;

        private GridFieldManager(GameScene pGameScene)
        {
            CreativeMode = false;

            // Load Map
            m_GridField = MapLoadSave.GetInstance().LoadMap(pGameScene, "GeneratedTileMap");
        }

        static public GridFieldManager GetInstance(GameScene pGameScene)
        {
            if (m_GridFieldManager == null)
                m_GridFieldManager = new GridFieldManager(pGameScene);

            return m_GridFieldManager;
        }

        public void Initialize()
        {
            m_Random = new Random();

            //Iterate over every GridTile
            for (int i = 0; i < GRID_ROW_LENGTH; ++i)
            {
                for (int j = 0; j < GRID_COLUMN_LENGTH; ++j)
                {
                    m_GridField[i, j].Initialize();
                }
            }
        }

        public void Update(Engine.RenderContext renderContext)
        {
            //Iterate over every GridTile
            for (int i = 0; i < GRID_ROW_LENGTH; ++i)
            {
                for (int j = 0; j < GRID_COLUMN_LENGTH; ++j)
                {
                    m_GridField[i,j].Update(renderContext);
                }
            }
        }

        public GridTile HitTestField(Ray ray)
        {
            //Iterate over every GridTile
            for (int i = 0; i < GRID_ROW_LENGTH; ++i)
            {
                for (int j = 0; j < GRID_COLUMN_LENGTH; ++j)
                {
                    if (m_GridField[i,j].HitTest(ray))
                        return m_GridField[i, j];
                }
            }
            return null;
        }

        public GridTile GetSelectedTile()
        {
            foreach (var gridTile in m_GridField)
            {
                if (gridTile.PermanentSelected)
                    return gridTile;
            }

            return null;
        }

        public void PermanentSelect(GridTile tile)
        {
            bool value = tile.PermanentSelected;
            foreach (var gridTile in m_GridField)
            {
                gridTile.PermanentSelected = false;
            }

            tile.PermanentSelected = !value;
        }

        public void Select(GridTile tile)
        {
            switch (m_SelectionMode)
            {
                case SelectionMode.select1x1:
                    tile.Selected = true;
                    break;
                case SelectionMode.select2x2:
                    tile.Selected = true;
                    if (GetNWTile(tile) != null) GetNWTile(tile).Selected = true;
                    if (GetNTile(tile) != null) GetNTile(tile).Selected = true;
                    if (GetNETile(tile) != null) GetNETile(tile).Selected = true;
                    break;
                case SelectionMode.select3x3:
                    tile.Selected = true;
                    if (GetNWTile(tile) != null) GetNWTile(tile).Selected = true;
                    if (GetNTile(tile) != null) GetNTile(tile).Selected = true;
                    if (GetNETile(tile) != null) GetNETile(tile).Selected = true;
                    if (GetETile(tile) != null) GetETile(tile).Selected = true;
                    if (GetSETile(tile) != null) GetSETile(tile).Selected = true;
                    if (GetSTile(tile) != null) GetSTile(tile).Selected = true;
                    if (GetSWTile(tile) != null) GetSWTile(tile).Selected = true;
                    if (GetWTile(tile) != null) GetWTile(tile).Selected = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("SelectionMode (1x1 or 2x2) is out of range in GridFieldManager");
            }
        }

        public void Deselect()
        {
            foreach (var gridTile in m_GridField)
                gridTile.Selected = false;
        }

        //Functions that pick a surrounding tile of another tile
        #region Surrounding tiles

        
        //NW and following names stand for north west....
        //NorthWest
        public GridTile GetNWTile(GridTile tile)
        {
            if (tile.Row <= 0) 
                return null;

            return m_GridField[tile.Row - 1, tile.Column];
        }

        //North
        public GridTile GetNTile(GridTile tile)
        {
            if (tile.Row <= 0 || tile.Column <= 0)
                return null;

            return m_GridField[tile.Row - 1, tile.Column - 1];
        }

        //NorthEast
        public GridTile GetNETile(GridTile tile)
        {
            if (tile.Column <= 0)
                return null;

            return m_GridField[tile.Row, tile.Column - 1];
        }

        //East
        public GridTile GetETile(GridTile tile)
        {
            if (tile.Row >= GRID_ROW_LENGTH - 1 || tile.Column <= 0)
                return null;

            return m_GridField[tile.Row +1, tile.Column - 1];
        }

        //SouthEast
        public GridTile GetSETile(GridTile tile)
        {
            if (tile.Row >= GRID_ROW_LENGTH -1)
                return null;

            return m_GridField[tile.Row +1, tile.Column];
        }

        //South
        public GridTile GetSTile(GridTile tile)
        {
            if (tile.Row >= GRID_ROW_LENGTH - 1 || tile.Column >= GRID_COLUMN_LENGTH - 1)
                return null;

            return m_GridField[tile.Row + 1, tile.Column + 1];
        }

        //SouthWest
        public GridTile GetSWTile(GridTile tile)
        {
            if (tile.Column >= GRID_COLUMN_LENGTH -1)
                return null;

            return m_GridField[tile.Row , tile.Column +1];
        }

        //West
        public GridTile GetWTile(GridTile tile)
        {
            if (tile.Row <= 0 || tile.Column >= GRID_COLUMN_LENGTH - 1)
                return null;

            return m_GridField[tile.Row -1 , tile.Column + 1];
        }

        #endregion

        public SelectionMode SelectionModeMeth
        {
            get { return m_SelectionMode; }
            set { m_SelectionMode = value; }
        }

        public void NextSelectionMode()
        {
            ++m_SelectionMode;
            if ((int)m_SelectionMode >= (int)SelectionMode.enumSize) m_SelectionMode = 0;
        }

        public Random Random
        {
            get { return m_Random; }
        }
        public GridTile[,] GridField
        {
            get { return m_GridField; }
        }

        public bool CreativeMode { get; set; }

    }
}
