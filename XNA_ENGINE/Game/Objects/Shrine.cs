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
    public class Shrine : Placeable
    {
        private GridTile m_DirectionTile;

        private const float GRIDHEIGHT = 32;
        private const float TIMEPERINFLUENCEPOINT = 5;

        private double m_InfluenceTimer = TIMEPERINFLUENCEPOINT;

        private readonly GameScene m_GameScene;

        private readonly ShrineType m_ShrineType;

        public enum ShrineType
        {
            Basic1,

            //<----Add new types in front of this comment 
            enumSize
        }

        public Shrine(GridTile tile, GameScene pGameScene, ShrineType shrineType)
        {
            m_PlaceableType = PlaceableType.Shrine;
            m_ShrineType = shrineType;

            m_LinkedTile = tile;

            switch (m_ShrineType)
            {
                case ShrineType.Basic1:
                    m_Model = new GameModelGrid("Models/building_Shrine");
                    m_Model.LocalPosition += new Vector3(0, GRIDHEIGHT, 0);
                    Quaternion rotation = new Quaternion(new Vector3(0,1,0), 0);
                    m_Model.LocalRotation += rotation;
                    m_Model.CanDraw = true;
                    m_Model.LoadContent(FinalScene.GetContentManager());
                    m_Model.DiffuseColor = new Vector3(0.1f, 0.1f, 0.5f);
                    m_LinkedTile.Model.AddChild(m_Model);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("shrineType");
            }

            m_GameScene = pGameScene;

            m_DirectionTile = m_LinkedTile;
        }

        public override void Update(RenderContext renderContext)
        {
            m_InfluenceTimer -= (renderContext.GameTime.ElapsedGameTime.Milliseconds/1000.0);

            if (m_InfluenceTimer <= 0)
            {
                m_InfluenceTimer = TIMEPERINFLUENCEPOINT;
                Menu.GetInstance().Player.GetResources().AddInfluence(1);
            }

            //Appearance of the tile
            switch (m_ShrineType)
            {
                case ShrineType.Basic1:
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
                m_DirectionTile.ShowFlag(true);
                Menu.GetInstance().SubMenu = Menu.SubMenuSelected.SettlementMode;
            }
            else
            {
                m_Model.PermanentSelected = false;
                m_DirectionTile.ShowFlag(false);
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

            base.OnSelected();

            return true;
        }

        public void PlaceRallyPoint(GridTile gridTile)
        {
            m_DirectionTile.ShowFlag(false);
            m_DirectionTile = gridTile;
        }
    }
}
