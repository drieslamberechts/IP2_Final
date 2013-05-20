using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using XNA_ENGINE.Game.Objects;
using XNA_ENGINE.Game.Scenes;

namespace XNA_ENGINE.Game.TilePrefabs
{
    class PrefabTreeDead2 : BasePrefab
    {
        public PrefabTreeDead2(GridTile tile)
        {
            m_TileModel = new GameModelGrid("Models/tile_Grass");
            m_TileModel.LoadContent(PlayScene.GetContentManager());
            m_TileModel.UseTexture = true;

            var treeDead1 = new GameModelGrid("Models/tree_TreeDead2");
           // treeLong.Texture2D = PlayScene.GetContentManager().Load<Texture2D>("Textures/tex_tree_Tall1");
            treeDead1.UseTexture = true;

            m_PropList.Add(treeDead1);
           
            m_bOpen = true;
            m_WoodCount = 20;

            foreach (var prop in m_PropList)
                prop.LoadContent(PlayScene.GetContentManager());
        }
    }
}
