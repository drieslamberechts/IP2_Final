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
    class Flag : Placeable
    {
        private const float GRIDHEIGHT = 32;

        public Flag(GridTile tile, GameScene pGameScene)
        {
            m_PlaceableType = PlaceableType.Flag;

            m_LinkedTile = tile;

            m_Model = new GameModelGrid("Models/settlement_TestSettlementBlue");
            m_Model.LocalPosition += new Vector3(0, GRIDHEIGHT, 0);
            m_Model.CanDraw = true;
            m_Model.LoadContent(FinalScene.GetContentManager());
            m_LinkedTile.Model.AddChild(m_Model);
        }
    }
}
