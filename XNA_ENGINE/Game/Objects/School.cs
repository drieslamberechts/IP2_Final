using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using XNA_ENGINE.Engine;
using XNA_ENGINE.Engine.Scenegraph;
using XNA_ENGINE.Game.Objects;
using XNA_ENGINE.Game.Managers;
using XNA_ENGINE.Game.Scenes;

namespace XNA_ENGINE.Game.Objects
{
    public class School : Placeable
    {
        private GridTile m_RallyPointTile;

        //Props
        private readonly int m_Row, m_Column;

        private const float GRIDHEIGHT = 32;

        private const float TIMEFORVILLAGER = 2;
        private double m_Timer = TIMEFORVILLAGER;
        private int m_AmountOfSoldiersQueued = 0;

        private readonly GameScene m_GameScene;

        private readonly SchoolType m_SchoolType;

        public enum SchoolType
        {
            Basic1,

            //<----Add new types in front of this comment 
            enumSize
        }

        public School(GridTile tile, GameScene pGameScene, SchoolType schoolType)
        {
            m_PlaceableType = PlaceableType.School;
            m_SchoolType = schoolType;

            m_LinkedTile = tile;

            switch (m_SchoolType)
            {
                case SchoolType.Basic1:
                    m_Model = new GameModelGrid("Models/building_School");
                    m_Model.LocalPosition += new Vector3(30, GRIDHEIGHT, 10);
                    Quaternion rotation = new Quaternion(new Vector3(0,1,0), 0);
                    m_Model.LocalRotation += rotation;
                    m_Model.CanDraw = true;
                    m_Model.LoadContent(FinalScene.GetContentManager());
                    m_Model.DiffuseColor = new Vector3(0.1f, 0.1f, 0.5f);
                    m_LinkedTile.Model.AddChild(m_Model);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("schoolType");
            }

            m_Column = m_LinkedTile.Column;
            m_Row = m_LinkedTile.Row;

            m_GameScene = pGameScene;

            m_RallyPointTile = m_LinkedTile;
        }

        public override void Update(RenderContext renderContext)
        {
            if (m_AmountOfSoldiersQueued > 0)
            {
                m_Timer -= (renderContext.GameTime.ElapsedGameTime.Milliseconds / 1000.0);

                if (m_Timer <= 0)
                {
                    //Console.WriteLine("Soldier built");
                    m_Timer = TIMEFORVILLAGER;
                    --m_AmountOfSoldiersQueued;
                    Menu.GetInstance().Player.NewPlaceable(new Army(SceneManager.ActiveScene, m_RallyPointTile));
                }
            }

            //Appearance of the tile
            switch (m_SchoolType)
            {
                case SchoolType.Basic1:
                    //m_Model.Texture2D = FinalScene.GetContentManager().Load<Texture2D>("Textures/tex_tile_Basic");
                    //m_Model.UseTexture = true;

                    //m_Model.CanDraw = true;

                    //m_Model.DiffuseColor = new Vector3(1.0f, 1.0f, 1.0f);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            //What to do if the tile is selected
            if (m_LinkedTile.Selected)
            {
                m_Model.Selected = true;
            }
            else
            {
                m_Model.Selected = false;
            }

            //What to do if the tile is permanently selected (until the tile is deselected)
            if (m_LinkedTile.PermanentSelected)
            {
                m_Model.PermanentSelected = true;
                m_RallyPointTile.ShowFlag(true);
                Menu.GetInstance().SubMenu = Menu.SubMenuSelected.SettlementMode;
            }
            else
            {
                m_Model.PermanentSelected = false;
                m_RallyPointTile.ShowFlag(false);
            }

            base.Update(renderContext);
        }

        //Code to execute on hit with mouse
        public override bool OnSelected()
        {
            if (!m_LinkedTile.Selected) return false;

            //Get the inputmanager
            var inputManager = FinalScene.GetInputManager();
            //What mode is there selected in the menu to build?
            Menu.ModeSelected selectedMode = Menu.GetInstance().GetSelectedMode();

            if (inputManager.GetAction((int)FinalScene.PlayerInput.LeftClick).IsTriggered)
            {
                
            }

            if (inputManager.GetAction((int)FinalScene.PlayerInput.RightClick).IsTriggered)
            {

            }

            if (Menu.GetInstance().m_Enable5)
            {
                Menu.GetInstance().m_Enable5 = false;
                Menu.GetInstance().m_Enable6 = true;
            }

            base.OnSelected();

            return true;
        }

        public override void QueueSoldier(int amount = 1)
        {
            m_AmountOfSoldiersQueued += amount;
        }

        public void PlaceRallyPoint(GridTile gridTile)
        {
            m_RallyPointTile.ShowFlag(false);
            m_RallyPointTile = gridTile;
        }
    }
}
