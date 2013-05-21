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
        private const float GRIDHEIGHT = 32;

        private GridTile m_TargetTile;
        private GridTile m_CurrentTile;
        private List<GridTile> m_PathToFollow;

        private const int m_MoveRadius = 1;
        private float MOVEMENTSPEED = 0.5f; //seconds per tile

        private float m_PreviousDistanceToTile;

        public Shaman(GridTile startTile)
        {
            m_LinkedTileList = null;

            m_PlaceableType = PlaceableType.Shaman;

            m_Model = new GameModelGrid("Models/char_Goblin_Shaman");
            m_Model.LocalPosition += new Vector3(0, 0, 0);
           // m_Model.LocalScale = new Vector3(0.4f, 0.4f, 0.4f);
            // Quaternion rotation = new Quaternion(new Vector3(0, 1, 0), 0);
            // m_Model.LocalRotation += rotation;
            m_Model.CanDraw = true;
            m_Model.LoadContent(PlayScene.GetContentManager());
            m_Model.UseTexture = true;
         
            GridFieldManager.GetInstance().GameScene.AddSceneObject(m_Model);

            m_Model.CreateBoundingBox(30, 64, 30, new Vector3(0, 0, 0));
            m_Model.DrawBoundingBox = false;

            m_TargetTile = startTile;
            m_CurrentTile = startTile;

            m_Model.Translate(m_CurrentTile.Model.LocalPosition);

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


            if (m_PathToFollow != null && m_PathToFollow.Any())
                SetTargetTile(m_PathToFollow.ElementAt(0));

            //Do smooth movement
            //Check if the targetTile exists
            //Check if the targetTile and the Currenttile are the same
            if (m_TargetTile != null && m_CurrentTile != m_TargetTile)
            {
                //Values
                float deltaTime = renderContext.GameTime.ElapsedGameTime.Milliseconds / 1000.0f;
                Vector3 targetPos = m_TargetTile.Model.WorldPosition;
                Vector3 worldPos = m_Model.WorldPosition;
                Vector3 currentTilePos = m_CurrentTile.Model.WorldPosition;
                //Offset the soldier so it has the correct position
                targetPos.Y += 32;

                //Calculate the distance for 1 tile to the other
                Vector3 distanceVector = targetPos - currentTilePos;
                //Calculate the direction
                Vector3 directionVector = (targetPos - worldPos);
                directionVector.Normalize();
                distanceVector = distanceVector.Length() * directionVector;

                //Calculate a new vector without y value
                Vector3 directionVectorCalc = directionVector;
                directionVectorCalc.Y = 0;
                directionVectorCalc.Normalize();

                //Do the right rotation
                int add = 0;
                if (directionVectorCalc.X != 0) add = 270;
                m_Model.Rotate(0, (directionVectorCalc.X * 90 * -1) + (directionVectorCalc.Z * 90) + -90 + add, 0); //  90*dot - 90

                //If the model is in the proximity, stick it to the tile
                if (m_PreviousDistanceToTile < (targetPos - worldPos).Length())
                {
                    m_CurrentTile.Model.GreenHighLight = false;
                    m_TargetTile.Model.GreenHighLight = true;
                    m_CurrentTile = m_TargetTile;
                    m_Model.Translate(targetPos);
                    m_PathToFollow.Remove(m_TargetTile);

                    m_PreviousDistanceToTile = 100000.0f;
                }
                else //else just move it towards it
                {
                    m_Model.Translate(worldPos + (distanceVector * (deltaTime / MOVEMENTSPEED)));
                    m_PreviousDistanceToTile = (targetPos - worldPos).Length();
                }
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
            m_Rallypoint.CanDraw = false;

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
                GoToTile(selectedTile);
            }

            base.OnPermanentSelected();
        }

        public override void GoToTile(GridTile targetTile)
        {
            m_PathToFollow = GridFieldManager.GetInstance().CalculatePath(m_CurrentTile, targetTile);
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
