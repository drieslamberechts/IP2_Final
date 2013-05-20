using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNA_ENGINE.Game.Managers;
using XNA_ENGINE.Game.Objects;
using XNA_ENGINE.Game.Scenes;

namespace XNA_ENGINE.Game.TilePrefabs
{
    class PrefabNormalGrass : BasePrefab
    {
        public PrefabNormalGrass(GridTile tile)
        {
            m_TileModel = new GameModelGrid("Models/tile_Grass");
            m_TileModel.LoadContent(PlayScene.GetContentManager());
            m_TileModel.Texture2D = PlayScene.GetContentManager().Load<Texture2D>("Textures/tex_tile_Basic");
            m_TileModel.UseTexture = true;
            
            m_bOpen = true;

            foreach (var prop in m_PropList)
            {
                prop.LoadContent(PlayScene.GetContentManager());
            }
        }
    }
}
