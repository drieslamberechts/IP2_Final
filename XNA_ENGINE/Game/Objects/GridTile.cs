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
        private readonly int m_Row, m_Column;

        private const float GRIDWIDTH = 64;
        private const float GRIDDEPTH = 64;
        private const float GRIDHEIGHT = 32;
        private const int YOFFSETMIN = 0;
        private const int YOFFSETMAX = 15;

        private TileType m_TileType = TileType.Normal;
        private string m_TileSettlement;

        private bool m_Selected;

        private readonly GameScene m_GameScene;

        public enum TileType
        {
            Normal,
            Water,
            Cliff,

            //<----Add new types in front of this comment 
            enumSize
        }

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
            m_TileModel.DrawBoundingBox = false;
        }

        public void Update(RenderContext renderContext)
        {
           // m_TileModel.Texture2D = FinalScene.GetContentManager().Load<Texture2D>("Textures/RainbowTexture");
            //Appearance of the tile
            switch (m_TileType)
            {
                case TileType.Normal:
                    m_TileModel.CanDraw = true;
                    m_TileModel.DiffuseColor = new Vector3(0.0f,0.5f,0.0f);
                    break;
                case TileType.Water:
                    m_TileModel.CanDraw = true;
                    m_TileModel.DiffuseColor = new Vector3(0.0f, 0.0f, 0.5f);
                    break;
                case TileType.Cliff:
                    m_TileModel.CanDraw = false;
                    m_TileModel.DiffuseColor = new Vector3(0.5f, 0.0f, 0.0f);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            //What to do if the tile is selected
            if (m_Selected)
            {
                //m_TileModel.Texture2D = FinalScene.GetContentManager().Load<Texture2D>("Textures/RainbowTexture");
                //m_TileModel.UseTexture = true;
                m_TileModel.Selected = true;
                if (m_SettlementDisplayModel != null)
                    m_SettlementDisplayModel.Selected = true;
            }
            else
            {
                m_TileModel.Selected = false;
                if (m_SettlementDisplayModel != null)
                    m_SettlementDisplayModel.Selected = false;
                //m_TileModel.UseTexture = false;
            }
           
            m_Selected = false;
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
            bool creativeMode = GridFieldManager.GetInstance(m_GameScene).CreativeMode;
            //Get the inputmanager
            var inputManager = FinalScene.GetInputManager();
            //What mode is there selected in the menu to build?
            Menu.ModeSelected selectedMode = Menu.GetInstance().GetSelectedMode();


            if (creativeMode) //Creative mode off
            {

                if (inputManager.GetAction((int)FinalScene.PlayerInput.LeftClick).IsTriggered)
                {
                    ++m_TileType;
                    if ((int) m_TileType >= (int)TileType.enumSize) m_TileType = 0;
                }

                if (inputManager.GetAction((int)FinalScene.PlayerInput.RightClick).IsTriggered)
                {
                    --m_TileType;
                    if ((int) m_TileType < 0) m_TileType = TileType.enumSize-1;
                }
            }
            else
            {
                if (inputManager.GetAction((int)FinalScene.PlayerInput.LeftClick).IsTriggered)
                {
                    switch (selectedMode)
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
                            RemoveChildModel();
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                if (inputManager.GetAction((int)FinalScene.PlayerInput.RightClick).IsTriggered)
                {

                }
            }

            m_Selected = true;
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

        private void RemoveChildModel()
        {
            m_TileModel.RemoveChild(m_SettlementDisplayModel);
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
            get{return m_Selected;}
            set{m_Selected = value;} 
        }

        public int Row
        {
            get { return m_Row; }
        }

        public int Column
        {
            get { return m_Column; }
        }

        public TileType TileTypeValue
        {
            get { return m_TileType; }
            set { m_TileType = value; }
        }
    }
}