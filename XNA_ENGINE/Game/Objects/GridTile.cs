using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XNA_ENGINE.Engine;
using XNA_ENGINE.Engine.Objects;
using XNA_ENGINE.Engine.Scenegraph;
using XNA_ENGINE.Game.Objects;


namespace XNA_ENGINE.Game.Objects
{
    public class GridTile
    {
        private GameModel m_TileModel;
        private int m_Row, m_Column;

        public GridTile(GameScene pGameScene, Vector3 position, int row, int column)
        {            
            m_TileModel = new GameModel("Models/tile_Template");
            m_TileModel.Translate(position);
            pGameScene.AddSceneObject(m_TileModel);

            m_Row = row;
            m_Column = column;
        }

        public void Initialize()
        {
            
        }

        public void Update()
        {

        }
    }
}