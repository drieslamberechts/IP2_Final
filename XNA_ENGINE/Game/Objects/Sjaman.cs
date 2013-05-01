using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XNA_ENGINE.Engine.Scenegraph;
using XNA_ENGINE.Game.Scenes;

namespace XNA_ENGINE.Game.Objects
{
    class Sjaman : Placeable
    {
        private GridTile m_TargetTile;

        private const float GRIDHEIGHT = 32;

        public Sjaman(GameScene gameScene ,GridTile startTile)
        {
            m_LinkedTile = null;
            m_Static = false;

            m_PlaceableType = PlaceableType.Sjaman;

            m_Model = new GameModelGrid("Models/char_Goblin_Shaman");
            m_Model.LocalPosition += new Vector3(30, GRIDHEIGHT + 64, 64);
           // m_Model.LocalScale = new Vector3(0.4f, 0.4f, 0.4f);
            // Quaternion rotation = new Quaternion(new Vector3(0, 1, 0), 0);
            // m_Model.LocalRotation += rotation;
            m_Model.CanDraw = true;
            m_Model.LoadContent(FinalScene.GetContentManager());
            m_Model.DiffuseColor = new Vector3(0.1f, 0.1f, 0.5f);
            gameScene.AddSceneObject(m_Model);

            m_Model.CreateBoundingBox(45, 128, 45, new Vector3(0, GRIDHEIGHT + 30, 0));
            //m_Model.DrawBoundingBox = true;

            m_TargetTile = startTile;

            m_Model.Translate(m_TargetTile.Model.WorldPosition);
        }

        public override void Update(Engine.RenderContext renderContext)
        {
            Vector3 newPos = m_TargetTile.Model.WorldPosition;
            newPos.Y += 32;
            m_Model.Translate(newPos);

            if (m_Model.PermanentSelected)
                Menu.GetInstance().SubMenu = Menu.SubMenuSelected.ShamanMode;

            base.Update(renderContext);
        }

        public override void SetTargetTile(GridTile targetTile)
        {
            m_TargetTile = targetTile;
        }

        public virtual bool OnSelected()
        {
            if (!m_Model.PermanentSelected) return false;

          

            return true;
        }
    }
}
