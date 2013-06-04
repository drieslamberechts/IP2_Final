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
    class Villager : Unit
    {
        public Villager(GridTile startTile, GridTile goToTile)
        {
            m_LinkedTileList = null;

            m_PlaceableType = PlaceableType.Villager;

            m_Model = new GameModelGrid("Models/char_Goblin_Villager");
            m_Model.LocalPosition += new Vector3(0, 32, 0);
            m_Model.LocalScale = new Vector3(0.4f, 0.4f, 0.4f);
           // Quaternion rotation = new Quaternion(new Vector3(0, 1, 0), 0);
           // m_Model.LocalRotation += rotation;
            m_Model.CanDraw = true;
            m_Model.LoadContent(PlayScene.GetContentManager());
            m_Model.UseTexture = true;
            GridFieldManager.GetInstance().GameScene.AddSceneObject(m_Model);

            m_Model.CreateBoundingBox(45, 128, 45, new Vector3(0, GRIDHEIGHT+30, 0));
            m_Model.DrawBoundingBox = false;

            m_CurrentTile = startTile;
            GoToTile(goToTile);

            MOVEMENTSPEED = 0.8f;

            Initialize();
        }

        public override void Update(Engine.RenderContext renderContext)
        {
            if (m_CurrentTile.Model.Danger == true)
            {
                m_Owner.RemovePlaceable(this);
                return;
            }

            m_CurrentTile.PickupWood(m_Owner);

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
            
            GridTile selectedTile = null;
            if (gridFieldManager.GetSelectedTiles() != null)
                selectedTile = gridFieldManager.GetSelectedTiles().ElementAt(0);

          /*  if (gridFieldManager.GetSelectedPlaceables() != null)
            {
                foreach (Placeable placeables in gridFieldManager.GetSelectedPlaceables())
                {
                    selectedTile = placeables.LinkedTiles.ElementAt(0);
                }
            }*/

            if (Menu.GetInstance().m_Enable1)
            {
                Menu.GetInstance().m_Enable1 = false;
                Menu.GetInstance().m_Enable2 = true;
            }
            
            Menu.GetInstance().SubMenu = Menu.SubMenuSelected.VillagerMode;

            if (inputManager.GetAction((int)PlayScene.PlayerInput.LeftClick).IsTriggered)
            {
                
            }

            if (inputManager.GetAction((int)PlayScene.PlayerInput.RightClick).IsTriggered)
            {
                if (selectedTile != null) GoToTile(selectedTile);
            }

            base.OnPermanentSelected();
        }

        public List<GridTile> PathToFollow
        {
            get { return m_PathToFollow; }
            set { m_PathToFollow = value; }
        }

        public override bool SetTargetTile(GridTile targetTile)
        {
            if (targetTile != null)
            {
                m_TargetTile = targetTile;

                if (Menu.GetInstance().m_Enable4)
                {
                    Menu.GetInstance().m_Enable4 = false;
                    Menu.GetInstance().m_Enable5 = true;
                }
                return true;
            }
            return false;
        }
    }
}
