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
            m_TileModel.CanDraw = true;
            m_TileModel.LoadContent(PlayScene.GetContentManager());
            m_TileModel.UseTexture = true;

            var m_TreeLong = new GameModelGrid("Models/tree_TreeTall");
            m_TreeLong.Texture2D = PlayScene.GetContentManager().Load<Texture2D>("Textures/tex_tree_Tall1");
            m_TreeLong.UseTexture = true;

            m_PropList.Add(m_TreeLong);
           
            m_bOpen = false;

            foreach (var prop in m_PropList)
            {
                prop.LoadContent(PlayScene.GetContentManager());
            }
        }
    }
}
