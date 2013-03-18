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


namespace XNA_ENGINE.Game.Objects.Concept2
{
    public class GridTile
    {
        private GameModel m_TileModel;

        public GridTile(GameScene pGameScene)
        {            
            m_TileModel = new GameModel("Models/tile_Template");
            pGameScene.AddSceneObject(m_TileModel);
        }

        public void Initialize()
        {
            
        }

        public void Update()
        {

        }
    }
}