using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XNA_ENGINE.Engine.Scenegraph;
using XNA_ENGINE.Game.Objects;
using XNA_ENGINE.Game.Scenes;


namespace XNA_ENGINE.Game.Objects
{
    public class RallyPoint : Placeable
    {
        private const float GRIDHEIGHT = 32;

        public RallyPoint(GridTile tile, GameScene pGameScene)
        {
            m_PlaceableType = PlaceableType.RallyPoint;

           // m_LinkedTile = tile;

            m_Model = new GameModelGrid("Models/building_FlagNormal");
            m_Model.LocalPosition += new Vector3(0, GRIDHEIGHT, 0);
            m_Model.CanDraw = true;
            m_Model.LoadContent(PlayScene.GetContentManager());
            m_Model.DiffuseColor = new Vector3(0.0f, 0.8f, 0.0f);
            m_Model.Scale(0.2f, 0.2f, 0.2f);
          //  m_LinkedTile.Model.AddChild(m_Model);
        }
    }
}
