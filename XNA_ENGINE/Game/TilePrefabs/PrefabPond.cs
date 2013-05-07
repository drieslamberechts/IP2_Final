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
    class PrefabPond : BasePrefab
    {
        public PrefabPond(GridTile tile)
        {
            m_TileModel = new GameModelGrid("Models/tile_Pond");
            m_TileModel.LoadContent(PlayScene.GetContentManager());
            m_TileModel.UseTexture = true;

            //Rock1
            var rock1 = new GameModelGrid("Models/prop_Rock2");
            rock1.LocalScale = new Vector3(0.5f,0.5f,0.5f);
            rock1.Rotate(0, -90, 0);
            rock1.LocalPosition += new Vector3(25,-5,0);
            rock1.UseTexture = true;
            m_PropList.Add(rock1);

            //Rock2
            var rock2 = new GameModelGrid("Models/prop_Rock2");
            rock2.LocalScale = new Vector3(0.3f, 0.3f, 0.3f);
           // rock2.Rotate(0, -90, 0);
            rock2.LocalPosition += new Vector3(25, -5, 7);
            rock2.UseTexture = true;
            m_PropList.Add(rock2);

            //Rock3
            var rock3 = new GameModelGrid("Models/prop_Rock2");
            rock3.LocalScale = new Vector3(0.4f, 0.4f, 0.4f);
            rock3.Rotate(0, 90, 0);
            rock3.LocalPosition += new Vector3(30, -5, 20);
            rock3.UseTexture = true;
            m_PropList.Add(rock3);

            //Rock4
            var rock4 = new GameModelGrid("Models/prop_Rock2");
            rock4.LocalScale = new Vector3(0.7f, 0.7f, 0.7f);
            rock4.Rotate(0, 90, 0);
            rock4.LocalPosition += new Vector3(28, -20, 28);
            rock4.UseTexture = true;
            m_PropList.Add(rock4);

            //Rock5
            var rock5 = new GameModelGrid("Models/prop_Rock2");
            rock5.LocalScale = new Vector3(0.6f, 0.6f, 0.6f);
            rock5.Rotate(0, 0, 0);
            rock5.LocalPosition += new Vector3(28, -20, 0);
            rock5.UseTexture = true;
            m_PropList.Add(rock5);


            //Rock5
            var grass1 = new GameModelGrid("Models/prop_Grass3");
            grass1.LocalScale = new Vector3(1, 1, 1);
            grass1.Rotate(0, 0, 0);
            grass1.LocalPosition += new Vector3(0, 0, 0);
            grass1.UseTexture = true;
            m_PropList.Add(grass1);

            m_bOpen = false;
            foreach (var prop in m_PropList)
                prop.LoadContent(PlayScene.GetContentManager());
        }
    }
}
