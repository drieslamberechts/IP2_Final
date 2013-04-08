using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XNA_ENGINE.Engine;
using XNA_ENGINE.Engine.Objects;
using XNA_ENGINE.Engine.Scenegraph;
using XNA_ENGINE.Game.Managers;
using XNA_ENGINE.Game.Objects;
using XNA_ENGINE.Game.Scenes;


namespace XNA_ENGINE.Game.Objects
{
    public class GridTile
    {
        private GameModel m_TileModel;
        private GameModel m_SettlementDisplayModel;
        private int m_Row, m_Column;

        private static float GRIDWIDTH = 64;
        private static float GRIDDEPTH = 64;
        private static float GRIDHEIGHT = 32;
        private const int YOFFSETMIN = -5;
        private const int YOFFSETMAX = 5;

        private string m_TileType;
        private string m_TileSettlement;

        private bool m_Selected;

        private GameScene m_GameScene;

        public GridTile(GameScene pGameScene, int row, int column)
        {
            m_Column = column;
            m_Row = row;

            m_Selected = false;

            m_GameScene = pGameScene;
        }

        public void Initialize()
        {
            int yOffset = GridFieldManager.GetInstance(m_GameScene).Random.Next(YOFFSETMIN, YOFFSETMAX);

            m_TileModel = new GameModel("Models/tile_Template");
            m_TileModel.Translate(new Vector3(GRIDWIDTH * m_Row, yOffset, GRIDDEPTH * m_Column));
            m_GameScene.AddSceneObject(m_TileModel);

            m_TileModel.CreateBoundingBox(GRIDWIDTH, 1, GRIDDEPTH, new Vector3(0, GRIDHEIGHT, 0));
            m_TileModel.DrawBoundingBox = true;

            m_TileType = "normal";
        }

        public void Update(Engine.RenderContext renderContext)
        {
            if (m_Selected)
                m_TileModel.Rotate(0, 45.0f*(float) renderContext.GameTime.TotalGameTime.TotalSeconds, 0);

            /*if (m_Selected)
                m_TileModel.SetColor(new Vector3(0.5f,1,1));
            else
                m_TileModel.SetColor(new Vector3(0.0f, 1, 0.0f));*/
        }

        public bool HitTest(Ray ray)
        {
            if (m_TileModel.HitTest(ray))
            {
                System.Diagnostics.Debug.WriteLine("Row:" + m_Row.ToString() + " Column:" + m_Column.ToString());
                OnHit();
                return true;
            }
            return false;
        }

        //Code to execute on hit with mouse
        private void OnHit()
        {
            Menu.ModeSelected selectedTile = Menu.GetInstance().GetSelectedTile();

            switch (selectedTile)
            {
                case Menu.ModeSelected.Attack:
                    break;
                case Menu.ModeSelected.Defend:
                    break;
                case Menu.ModeSelected.Gather:
                    break;
                case Menu.ModeSelected.TileBlue:
                    ChangeChildModel("Models/settlement_TestSettlementBlue");
                    break;
                case Menu.ModeSelected.TileGold:
                    ChangeChildModel("Models/settlement_TestSettlementGold");
                    break;
                case Menu.ModeSelected.TileRed:
                    ChangeChildModel("Models/settlement_TestSettlementRed");
                    break;
                case Menu.ModeSelected.Delete:
                    ChangeChildModel();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void ChangeChildModel(string asset)
        {
            GameModel newModel = new GameModel(asset);
            newModel.LocalPosition += new Vector3(0, GRIDHEIGHT, 0);
            newModel.CanDraw = true;
            newModel.LoadContent(FinalScene.GetContentManager());
            m_TileModel.RemoveChild(m_SettlementDisplayModel);

            m_SettlementDisplayModel = newModel;
            m_TileModel.AddChild(newModel);
        }

        private void ChangeChildModel()
        {
            m_TileModel.RemoveChild(m_SettlementDisplayModel);
        }

        public void SetTileType(string type)
        {
            m_TileType = type;
        }

        public string GetTileType()
        {
            return m_TileType;
        }

        public void SetTileSettlement(string type)
        {
            m_TileSettlement = type;
        }

        public string GetTileSettlement()
        {
            return m_TileSettlement;
        }

        public bool Selected
        {
            get
            {
                return m_Selected;
            }

            set
            {
                m_Selected = value;
            } 
        }
    }
}