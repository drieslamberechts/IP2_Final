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
        //Singleton implementation
        private static GridFieldManager m_GridFieldManager;

        private List<List<GridTile>> m_GridField;

        private const int GRID_ROW_LENGTH = 30;
        private const int GRID_COLUMN_LENGTH = 30;
        private const int GRID_OFFSET = 65;


        private GridFieldManager(GameScene pGameScene)
        {
            //GENERATE GRIDFIELD
            m_GridField = new List<List<GridTile>>();
            for (int i = 0; i < GRID_ROW_LENGTH; ++i)
            {
                List<GridTile> tempList = new List<GridTile>();
                for (int j = 0; j < GRID_COLUMN_LENGTH; ++j)
                {
                    tempList.Add(new GridTile(pGameScene, new Vector3((i * GRID_OFFSET),0,(j * GRID_OFFSET)), i,j));
                }
                m_GridField.Add(tempList);
            }
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
                    m_GridField[i][j].Update(renderContext);
                }
            }
        }

    }
}
