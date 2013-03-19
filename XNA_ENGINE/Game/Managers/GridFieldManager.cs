using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XNA_ENGINE.Engine.Scenegraph;
using XNA_ENGINE.Game.Helpers;
using XNA_ENGINE.Game.Objects;

namespace XNA_ENGINE.Game.Managers
{
    class GridFieldManager
    {
        //Singleton implementation
        private static GridFieldManager m_GridFieldManager;

        private GridTile[,] m_GridField;

        private int GRID_ROW_LENGTH = 30;
        private int GRID_COLUMN_LENGTH = 30;
        private int GRID_OFFSET = 64;


        private GridFieldManager(GameScene pGameScene)
        {
            // Generate a new Map
            m_GridField = MapLoadSave.GetInstance().GenerateMap(pGameScene);

            /*//LOAD GRIDFIELD
            LoadGridField(pGameScene, "tilemap.xml");
            
            //GENERATE GRIDFIELD
            m_GridField = new List<List<GridTile>>();
            for (int i = 0; i < GRID_ROW_LENGTH; ++i)
            {
                List<GridTile> tempList = new List<GridTile>();
                for (int j = 0; j < GRID_COLUMN_LENGTH; ++j)
                {
                    tempList.Add(new GridTile(pGameScene, i, j));
                }
                m_GridField.Add(tempList);
            }*/
        }

        static public GridFieldManager GetInstance(GameScene pGameScene)
        {
            if (m_GridFieldManager == null)
                m_GridFieldManager = new GridFieldManager(pGameScene);

            return m_GridFieldManager;
        }

        public void Initialize()
        {
            
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

        public void LoadGridField(GameScene pGameScene, string XMLFile)
        {
            m_GridField = MapLoadSave.GetInstance().LoadMap(XMLFile, pGameScene);

            //Count columns and rows
            //int rows = m_GridField.Count(list => true);
            //int columns = m_GridField[0].Count(tile => true);

            //System.Diagnostics.Debug.WriteLine("Rows: " + rows + " Columns: " + columns);

           // GRID_ROW_LENGTH = rows;
            //GRID_COLUMN_LENGTH = rows;
        }

        public bool HitTestField(Ray ray)
        {
            //Iterate over every GridTile
            for (int i = 0; i < GRID_ROW_LENGTH; ++i)
            {
                for (int j = 0; j < GRID_COLUMN_LENGTH; ++j)
                {
                    if (m_GridField[i,j].HitTest(ray))
                        return true;
                }
            }
            return false;
        }

    }
}
