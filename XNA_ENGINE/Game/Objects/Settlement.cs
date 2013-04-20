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
    public class Settlement : Placeable
    {
        private GridTile m_DirectionTile;

        //Props
        private readonly int m_Row, m_Column;

        private const float GRIDHEIGHT = 32;

        private readonly GameScene m_GameScene;

        private readonly SettlementType m_SettlementType;

        public enum SettlementType
        {
            Basic1,

            //<----Add new types in front of this comment 
            enumSize
        }

        public Settlement(GridTile tile, GameScene pGameScene, SettlementType settlementType)
        {
            m_PlaceableType = PlaceableType.Settlement;
            m_SettlementType = settlementType;

            m_LinkedTile = tile;

            switch (m_SettlementType)
            {
                case SettlementType.Basic1:
                    m_Model = new GameModelGrid("Models/settlement_TestSettlementBlue");
                    m_Model.LocalPosition += new Vector3(0, GRIDHEIGHT, 0);
                    m_Model.CanDraw = true;
                    m_Model.LoadContent(FinalScene.GetContentManager());
                    m_LinkedTile.Model.AddChild(m_Model);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("settlementType");
            }

            m_Column = m_LinkedTile.Column;
            m_Row = m_LinkedTile.Row;

            m_GameScene = pGameScene;

            m_DirectionTile = m_LinkedTile;
        }

        public override void Update(RenderContext renderContext)
        {
            //Appearance of the tile
            switch (m_SettlementType)
            {
                case SettlementType.Basic1:
                    //m_Model.Texture2D = FinalScene.GetContentManager().Load<Texture2D>("Textures/tex_tile_Basic");
                   // m_Model.UseTexture = true;

                   // m_Model.CanDraw = true;

                   // m_Model.DiffuseColor = new Vector3(1.0f, 1.0f, 1.0f);
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
        public override void OnHit()
        {
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

            base.OnHit();
        }

        public void PlaceDirectionFlag(GridTile gridTile)
        {
            m_DirectionTile.RemoveFlag();
            m_DirectionTile = gridTile;
        }
    }
}
