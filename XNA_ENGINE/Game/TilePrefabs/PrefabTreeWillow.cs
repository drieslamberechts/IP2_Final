﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using XNA_ENGINE.Game.Objects;
using XNA_ENGINE.Game.Scenes;

namespace XNA_ENGINE.Game.TilePrefabs
{
    class PrefabTreeWillow : BasePrefab
    {
        public PrefabTreeWillow(GridTile tile)
        {
            m_TileModel = new GameModelGrid("Models/tile_Grass");
            m_TileModel.LoadContent(PlayScene.GetContentManager());
            m_TileModel.UseTexture = true;

            var treeWillow = new GameModelGrid("Models/tree_TreeWillow");
           // treeLong.Texture2D = PlayScene.GetContentManager().Load<Texture2D>("Textures/tex_tree_Tall1");
            treeWillow.UseTexture = true;

            m_PropList.Add(treeWillow);
           
            m_bOpen = true;
            m_WoodCount = 20;

            foreach (var prop in m_PropList)
                prop.LoadContent(PlayScene.GetContentManager());
        }
    }
}
