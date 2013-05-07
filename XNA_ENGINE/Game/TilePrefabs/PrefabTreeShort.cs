using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using XNA_ENGINE.Game.Objects;
using XNA_ENGINE.Game.Scenes;

namespace XNA_ENGINE.Game.TilePrefabs
{
    class PrefabTreeShort : BasePrefab
    {
        public PrefabTreeShort(GridTile tile)
        {
            m_TileModel = new GameModelGrid("Models/tile_Normal");
            m_TileModel.LoadContent(PlayScene.GetContentManager());
            m_TileModel.UseTexture = true;

            var m_TreeShort = new GameModelGrid("Models/tree_TreeShort1");
            m_TreeShort.UseTexture = true;

            m_PropList.Add(m_TreeShort);

            m_bOpen = false;

            foreach (var prop in m_PropList)
                prop.LoadContent(PlayScene.GetContentManager());
        }
    }
}
