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
    public class Army : Unit
    {
        private int m_ArmySize;

        public Army(GridTile startTile, GridTile goToTile, int armySize = 1)
        {
            m_ArmySize = armySize;

            m_LinkedTileList = null;

            m_PlaceableType = PlaceableType.Army;

            m_Model = new GameModelGrid("Models/char_Goblin_Soldier2");
            m_Model.LocalPosition += new Vector3(30, GRIDHEIGHT + 64, 64);
            m_Model.LocalScale = new Vector3(0.4f, 0.4f, 0.4f);
            // Quaternion rotation = new Quaternion(new Vector3(0, 1, 0), 0);
            // m_Model.LocalRotation += rotation;
            m_Model.CanDraw = true;
            m_Model.LoadContent(PlayScene.GetContentManager());

            m_Model.UseTexture = true;
            GridFieldManager.GetInstance().GameScene.AddSceneObject(m_Model);

            m_Model.CreateBoundingBox(45, 128, 45, new Vector3(0, GRIDHEIGHT + 30, 0));
            m_Model.DrawBoundingBox = false;

            m_CurrentTile = startTile;
            GoToTile(goToTile);

            m_Model.Translate(m_CurrentTile.Model.LocalPosition);

            MOVEMENTSPEED = 0.5f;

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
                GoToTile(selectedTile);

                if (selectedPlaceableList != null && selectedPlaceableList.ElementAt(0).PlaceableTypeMeth == PlaceableType.Army)
                {
                    if (selectedPlaceableList.ElementAt(0).GetOwner() != m_Owner)
                    {
                        //SceneManager.AddGameScene(new AttackScene(PlayScene.GetContentManager(), this, (Army)selectedPlaceableList.ElementAt(0)));
                        //SceneManager.SetActiveScene("AttackScene");
                    }
                    else
                    {
                        MergeArmies((Army)selectedPlaceableList.ElementAt(0),true);   
                    }
                }
            }

            base.OnPermanentSelected();
        }

        public override void Update(Engine.RenderContext renderContext)
        {
          //  if (m_TargetTile == null) return;

            if (m_Owner == GridFieldManager.GetInstance().UserPlayer)
            {
                Army boundArmy = m_CurrentTile.IsBoundArmy();
                if (boundArmy != null)
                {
                    SceneManager.AddGameScene(new AttackScene(PlayScene.GetContentManager(), this, boundArmy));
                    SceneManager.SetActiveScene("AttackScene");
                }
            }

            base.Update(renderContext);
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

        public void SetTargetTileOverride(GridTile targetTile)
        {
            m_TargetTile = targetTile;
        }

        public void MergeArmies(Army otherArmy, bool deleteThisArmy = false)
        {
            if (deleteThisArmy)
            {
                otherArmy.ArmySize+=ArmySize;
                m_Owner.RemovePlaceable(this);
            }
            else
            {
                ArmySize += otherArmy.ArmySize;
                m_Owner.RemovePlaceable(otherArmy);
            }
        }

        public override void GoToTile(GridTile targetTile)
        {
            m_PathToFollow = GridFieldManager.GetInstance().CalculatePath(m_CurrentTile, targetTile);
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
