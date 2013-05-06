using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XNA_ENGINE.Engine.Scenegraph;
using XNA_ENGINE.Game.Managers;
using XNA_ENGINE.Game.Scenes;

namespace XNA_ENGINE.Game.Objects
{
    class Shaman : Placeable
    {
        private GridTile m_TargetTile;

        private const float GRIDHEIGHT = 32;

        public Shaman(GridTile startTile)
        {
            m_LinkedTileList = null;

            m_PlaceableType = PlaceableType.Shaman;

            m_Model = new GameModelGrid("Models/char_Goblin_Shaman");
            m_Model.LocalPosition += new Vector3(30, GRIDHEIGHT + 64, 64);
           // m_Model.LocalScale = new Vector3(0.4f, 0.4f, 0.4f);
            // Quaternion rotation = new Quaternion(new Vector3(0, 1, 0), 0);
            // m_Model.LocalRotation += rotation;
            m_Model.CanDraw = true;
            m_Model.LoadContent(PlayScene.GetContentManager());
            m_Model.DiffuseColor = new Vector3(0.1f, 0.1f, 0.5f);
            GridFieldManager.GetInstance().GameScene.AddSceneObject(m_Model);

            m_Model.CreateBoundingBox(45, 128, 45, new Vector3(0, GRIDHEIGHT + 30, 0));
            //m_Model.DrawBoundingBox = true;

            m_TargetTile = startTile;

            m_Model.Translate(m_TargetTile.Model.WorldPosition);

            Initialize();
        }

        public override void Update(Engine.RenderContext renderContext)
        {
            //Get the inputmanager
            var inputManager = PlayScene.GetInputManager();
            var gridFieldManager = GridFieldManager.GetInstance();
            m_Rallypoint.CanDraw = false;

            GridTile selectedTile;
            if (gridFieldManager.GetSelectedTiles() != null && gridFieldManager.GetSelectedTiles().Any())
                selectedTile = gridFieldManager.GetSelectedTiles().ElementAt(0);
            else
                selectedTile = null;

            Menu.GetInstance().SubMenu = Menu.SubMenuSelected.MoveMode;

            if (inputManager.GetAction((int)PlayScene.PlayerInput.LeftClick).IsTriggered)
            {

            }

            if (inputManager.GetAction((int)PlayScene.PlayerInput.RightClick).IsTriggered)
            {
                if (selectedTile != null)
                    SetTargetTile(selectedTile);
            }

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
