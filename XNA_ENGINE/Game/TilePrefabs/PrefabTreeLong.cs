using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using XNA_ENGINE.Game.Objects;
using XNA_ENGINE.Game.Scenes;

namespace XNA_ENGINE.Game.TilePrefabs
{
    class PrefabTreeLong : BasePrefab
    {
        public PrefabTreeLong(GridTile tile)
        {
            m_TileModel = new GameModelGrid("Models/tile_Normal");
            m_TileModel.LoadContent(PlayScene.GetContentManager());
            m_TileModel.UseTexture = true;

            var treeLong = new GameModelGrid("Models/tree_TreeTall1");
           // treeLong.Texture2D = PlayScene.GetContentManager().Load<Texture2D>("Textures/tex_tree_Tall1");
            treeLong.UseTexture = true;

            m_PropList.Add(treeLong);
           
            m_bOpen = true;
            m_WoodCount = 20;

            foreach (var prop in m_PropList)
                prop.LoadContent(PlayScene.GetContentManager());
        }
    }
}
