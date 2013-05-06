using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XNA_ENGINE.Engine.Scenegraph;
using XNA_ENGINE.Game.Managers;
using XNA_ENGINE.Game.Objects;
using XNA_ENGINE.Game.Scenes;


namespace XNA_ENGINE.Game.Objects
{
    class Villager : Placeable
    {
        private GridTile m_TargetTile;

        private const float GRIDHEIGHT = 32;

        public Villager(GameScene gameScene, GridTile startTile)
        {
            m_LinkedTileList = null;

            m_PlaceableType = PlaceableType.Villager;

            m_Model = new GameModelGrid("Models/char_Villager");
            m_Model.LocalPosition += new Vector3(30, GRIDHEIGHT+64, 64);
            m_Model.LocalScale = new Vector3(0.4f, 0.4f, 0.4f);
           // Quaternion rotation = new Quaternion(new Vector3(0, 1, 0), 0);
           // m_Model.LocalRotation += rotation;
            m_Model.CanDraw = true;
            m_Model.LoadContent(PlayScene.GetContentManager());
            m_Model.DiffuseColor = new Vector3(0.1f, 0.1f, 0.5f);
            gameScene.AddSceneObject(m_Model);

            m_Model.CreateBoundingBox(45, 128, 45, new Vector3(0, GRIDHEIGHT+30, 0));
            m_Model.DrawBoundingBox = true;

            m_TargetTile = startTile;

            m_Model.Translate(m_TargetTile.Model.WorldPosition);

            Initialize();
        }

        public override void Update(Engine.RenderContext renderContext)
        {
            Vector3 newPos = m_TargetTile.Model.WorldPosition;
            newPos.Y += 32;
            m_Model.Translate(newPos);

            m_TargetTile.PickupWood(m_Owner);

           /* foreach (var placeable in m_TargetTile.LinkedPlaceables)
            {
                if (placeable.PlaceableTypeMeth == PlaceableType.School)
                {
                    placeable.QueueSoldier();
                    SceneManager.ActiveScene.RemoveSceneObject(m_Model);
                   // GridFieldManager.GetInstance().Player.RemovePlaceable(this);
                }

                if (placeable.PlaceableTypeMeth == PlaceableType.Shrine)
                {
                    SceneManager.ActiveScene.RemoveSceneObject(m_Model);
                  //  Menu.GetInstance().Player.RemovePlaceable(this);
                  //  Menu.GetInstance().Player.GetResources().AddInfluence(20);
                }
            }*/

            if (m_Model.PermanentSelected)
                Menu.GetInstance().SubMenu = Menu.SubMenuSelected.VillagerMode;

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
            m_Rallypoint.CanDraw = false;

            GridTile selectedTile;
            if (gridFieldManager.GetSelectedTiles() != null && gridFieldManager.GetSelectedTiles().Any())
                selectedTile = gridFieldManager.GetSelectedTiles().ElementAt(0);
            else
                selectedTile = null;

            Menu.GetInstance().SubMenu = Menu.SubMenuSelected.VillagerMode;

            if (inputManager.GetAction((int)PlayScene.PlayerInput.LeftClick).IsTriggered)
            {
                
            }

            if (inputManager.GetAction((int)PlayScene.PlayerInput.RightClick).IsTriggered)
            {
                if (selectedTile != null)
                    SetTargetTile(selectedTile);
            }

            base.OnPermanentSelected();
        }

        public override void SetTargetTile(GridTile targetTile)
        {
            m_TargetTile = targetTile;
        }
    }
}
