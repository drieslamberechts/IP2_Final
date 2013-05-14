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
    public class Army : Placeable
    {
        private GridTile m_TargetTile;

        private const float GRIDHEIGHT = 32;

        private int m_ArmySize = 1;

        public Army(GridTile startTile)
        {
            m_LinkedTileList = null;

            m_PlaceableType = PlaceableType.Army;

            m_Model = new GameModelGrid("Models/char_Soldier");
            m_Model.LocalPosition += new Vector3(30, GRIDHEIGHT + 64, 64);
            m_Model.LocalScale = new Vector3(0.4f, 0.4f, 0.4f);
            // Quaternion rotation = new Quaternion(new Vector3(0, 1, 0), 0);
            // m_Model.LocalRotation += rotation;
            m_Model.CanDraw = true;
            m_Model.LoadContent(PlayScene.GetContentManager());
            m_Model.DiffuseColor = new Vector3(0.1f, 0.1f, 0.5f);
            GridFieldManager.GetInstance().GameScene.AddSceneObject(m_Model);

            m_Model.CreateBoundingBox(45, 128, 45, new Vector3(0, GRIDHEIGHT + 30, 0));
            m_Model.DrawBoundingBox = false;

            m_TargetTile = startTile;

            m_Model.Translate(m_TargetTile.Model.WorldPosition);

            Initialize();
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

            List<Placeable> selectedPlaceableList = gridFieldManager.GetSelectedPlaceables();

            if (gridFieldManager.GetSelectedTiles() != null && gridFieldManager.GetSelectedTiles().Any())
                selectedTile = gridFieldManager.GetSelectedTiles().ElementAt(0);
            else
                selectedTile = null;

            Menu.GetInstance().SubMenu = Menu.SubMenuSelected.SoldierMode;

            if (inputManager.GetAction((int)PlayScene.PlayerInput.LeftClick).IsTriggered)
            {

            }

            if (inputManager.GetAction((int)PlayScene.PlayerInput.RightClick).IsTriggered)
            {
                if (selectedTile != null)
                    SetTargetTile(selectedTile);


                if (selectedPlaceableList != null)
                    if (selectedPlaceableList.ElementAt(0).PlaceableTypeMeth == PlaceableType.Army)
                    {
                        SceneManager.AddGameScene(new AttackScene(PlayScene.GetContentManager(), this, (Army)selectedPlaceableList.ElementAt(0)));
                        SceneManager.SetActiveScene("AttackScene");
                    }
            }

            base.OnPermanentSelected();
        }

        public override void Update(Engine.RenderContext renderContext)
        {
            Vector3 newPos = m_TargetTile.Model.WorldPosition;
            newPos.Y += 32;
            m_Model.Translate(newPos);

            if (m_Model.PermanentSelected)
                Menu.GetInstance().SubMenu = Menu.SubMenuSelected.SoldierMode;

            if (Menu.GetInstance().m_Enable6)
            {
                Menu.GetInstance().m_Enable6 = false;
                Menu.GetInstance().m_Enable7 = true;
            }

            base.Update(renderContext);
        }

        public override void SetTargetTile(GridTile targetTile)
        {
            if (targetTile.IsWalkable())
                m_TargetTile = targetTile;
        }

        public void MergeArmies(Army otherArmy)
        {
            ArmySize += otherArmy.ArmySize; 
            m_Owner.RemovePlaceable(otherArmy);
        }

        public override GridTile GetTargetTile()
        {
            return m_TargetTile;
        }

        public int ArmySize
        {
            get { return m_ArmySize; }
            set { m_ArmySize = value; }
        }
    }
}
