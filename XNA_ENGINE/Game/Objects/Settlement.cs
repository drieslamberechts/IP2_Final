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
    class Settlement : Placeable
    {
        private readonly GridTile m_LinkedTile;

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
                Menu.GetInstance().SubMenu = Menu.SubMenuSelected.SettlementMode;
            }
            else
            {
                m_Model.PermanentSelected = false;
                Menu.GetInstance().SubMenu = Menu.SubMenuSelected.BuildMode;
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
                int test = 0;
            }

            if (inputManager.GetAction((int)FinalScene.PlayerInput.RightClick).IsTriggered)
            {

            }

            base.OnHit();
        }
        /*
        private void ChangeSettlementModel(string asset)
        {
            GameModel newModel = new GameModel(asset);
            newModel.LocalPosition += new Vector3(0, GRIDHEIGHT, 0);
            newModel.CanDraw = true;
            newModel.LoadContent(FinalScene.GetContentManager());
            m_TileModel.RemoveChild(m_SettlementDisplayModel);

            m_SettlementDisplayModel = newModel;
            m_TileModel.AddChild(newModel);
        }*/
/*
        public void SetTileSettlement(string type)
        {
            m_TileSettlement = type;
        }

        public string GetTileSettlement()
        {
            return m_TileSettlement;
        }*/

        public int Row
        {
            get { return m_Row; }
        }

        public int Column
        {
            get { return m_Column; }
        }


    }
}
