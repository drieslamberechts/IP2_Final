﻿using System;
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
    class PrefabRock3 : BasePrefab
    {
        public PrefabRock3(GridTile tile)
        {
            m_TileModel = new GameModelGrid("Models/tile_GrassRock2");
            m_TileModel.LoadContent(PlayScene.GetContentManager());
            m_TileModel.Texture2D = PlayScene.GetContentManager().Load<Texture2D>("Textures/tex_tile_BasicWithDirt3");
            m_TileModel.UseTexture = true;

            m_bOpen = true;

            foreach (var prop in m_PropList)
                prop.LoadContent(PlayScene.GetContentManager());
        }
    }
}