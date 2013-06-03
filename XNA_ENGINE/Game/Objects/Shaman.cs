using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XNA_ENGINE.Engine.Objects;
using XNA_ENGINE.Engine.Scenegraph;
using XNA_ENGINE.Game.Managers;
using XNA_ENGINE.Game.Scenes;

namespace XNA_ENGINE.Game.Objects
{
    class Shaman : Unit
    {
        public Shaman(GridTile startTile, GridTile goToTile)
        {
            m_LinkedTileList = null;

            m_PlaceableType = PlaceableType.Shaman;

            m_Model = new GameModelGrid("Models/char_Goblin_Shaman_Animated",true);
            m_Model.LocalPosition += new Vector3(0, 0, 0);
           // m_Model.LocalScale = new Vector3(0.4f, 0.4f, 0.4f);
            // Quaternion rotation = new Quaternion(new Vector3(0, 1, 0), 0);
            // m_Model.LocalRotation += rotation;
            m_Model.CanDraw = true;
            m_Model.LoadContent(PlayScene.GetContentManager());
            m_Model.UseTexture = true;
            //m_Model.PlayAnimation("Take001");
         
            GridFieldManager.GetInstance().GameScene.AddSceneObject(m_Model);

            m_Model.CreateBoundingBox(30, 64, 30, new Vector3(0, 0, 0));
            m_Model.DrawBoundingBox = false;

            m_CurrentTile = startTile;
            GoToTile(goToTile);

            m_Model.Translate(m_CurrentTile.Model.LocalPosition);

            MOVEMENTSPEED = 1.0f;

            Initialize();
        }

        public override void Update(Engine.RenderContext renderContext)
        {
            if (m_CurrentTile.ShamanGoal)
            {
                GridFieldManager.GetInstance().Won = true;
            }

            if (m_CurrentTile.Model.Danger)
            {
                m_Owner.RemovePlaceable(this);
                return;
            }

            base.Update(renderContext);
        }

        //Code to execute on hit with mouse
        public override void OnSelected()
        {
            //Get the inputmanager
            var inputManager = PlayScene.GetInputManager();

            //What mode is there selected in the menu to build?
            Menu.ModeSelected selectedMode = Menu.GetInstance().GetSelectedMode();
            if (inputManager.GetAction((int)PlayScene.PlayerInput.LeftClick).IsTriggered)
            {

            }

            if (inputManager.GetAction((int)PlayScene.PlayerInput.RightClick).IsTriggered)
            {

            }

            base.OnSelected();
        }

        //Code to execute on Permanently selected
        public override void OnPermanentSelected()
        {
            //Get the inputmanager
            var inputManager = PlayScene.GetInputManager();
            var gridFieldManager = GridFieldManager.GetInstance();

            GridTile selectedTile;
            if (gridFieldManager.GetSelectedTiles() != null && gridFieldManager.GetSelectedTiles().Any())
                selectedTile = gridFieldManager.GetSelectedTiles().ElementAt(0);
            else
                selectedTile = null;

            Menu.GetInstance().SubMenu = Menu.SubMenuSelected.ShamanMode;

            List<GridTile> changeableTileList = gridFieldManager.GetSurroundingForShaman(m_CurrentTile);

            if (inputManager.GetAction((int)PlayScene.PlayerInput.LeftClick).IsTriggered)
            {

            }

            if (inputManager.GetAction((int)PlayScene.PlayerInput.RightClick).IsTriggered)
            {
                if (selectedTile != null) GoToTile(selectedTile);
            }

            base.OnPermanentSelected();
        }

        public override bool SetTargetTile(GridTile targetTile)
        {
            if (targetTile != null)
            {
                m_TargetTile = targetTile;

                return true;
            }
            return false;
        }
    }
}
