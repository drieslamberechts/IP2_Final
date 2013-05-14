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
    class PrefabDirt1 : BasePrefab
    {
        public PrefabDirt1(GridTile tile)
        {
            m_TileModel = new GameModelGrid("Models/tile_Dirt1");
            m_TileModel.LoadContent(PlayScene.GetContentManager());
            m_TileModel.UseTexture = true;

            m_bOpen = true;

            foreach (var prop in m_PropList)
                prop.LoadContent(PlayScene.GetContentManager());
        }
    }
}
