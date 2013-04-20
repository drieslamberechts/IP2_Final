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
        
        //Props
        private GameModel m_TreeShort1;
        private GameModel m_TreeTall1;

        private Army m_Army;

        private List<GameModel> m_PropsList; 
        private readonly int m_Row, m_Column;

        private const float GRIDWIDTH = 64;
        private const float GRIDDEPTH = 64;
        private const float GRIDHEIGHT = 32;
        private const int YOFFSETMIN = 0;
        private const int YOFFSETMAX = 15;

        private TileType m_TileType = TileType.Normal1;
        private string m_TileSettlement;

        private bool m_Selected;
        private bool m_PermanentSelected;

        private readonly GameScene m_GameScene;

        public enum TileType
        {
            Normal1,
            Normal2,
            Normal3,
            Normal4,
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

            m_TileModel = new GameModel("Models/tile_Normal");

            m_PropsList = new List<GameModel>();

            m_TreeShort1 = new GameModel("Models/tree_TreeShort");
            m_TreeTall1 = new GameModel("Models/tree_TreeTall");
            m_PropsList.Add(m_TreeShort1);
            m_PropsList.Add(m_TreeTall1);
            InitializeProps();

            m_TileModel.Translate(new Vector3(GRIDWIDTH * m_Row, yOffset, GRIDDEPTH * m_Column));
            m_GameScene.AddSceneObject(m_TileModel);

            m_TileModel.CreateBoundingBox(GRIDWIDTH, 1, GRIDDEPTH, new Vector3(0, GRIDHEIGHT, 0));
            m_TileModel.DrawBoundingBox = false;
        }

        public void Update(RenderContext renderContext)
        {
            //Appearance of the tile
            switch (m_TileType)
            {
                case TileType.Normal1:
                    ResetPropListParameters();

                    m_TileModel.Texture2D = FinalScene.GetContentManager().Load<Texture2D>("Textures/tex_tile_Basic");
                    m_TileModel.UseTexture = true;
                  
                    m_TileModel.CanDraw = true;
                   
                    m_TileModel.DiffuseColor = new Vector3(1.0f,1.0f,1.0f);
                    break;

                case TileType.Normal2:
                    ResetPropListParameters();
                    m_TileModel.Texture2D = FinalScene.GetContentManager().Load<Texture2D>("Textures/tex_tile_BasicWithDirt");
                    m_TileModel.UseTexture = true;

                    m_TileModel.CanDraw = true;
                   
                    m_TileModel.DiffuseColor = new Vector3(1.0f, 1.0f, 1.0f);
                    break;

                case TileType.Normal3:
                    ResetPropListParameters();
                    m_TileModel.Texture2D = FinalScene.GetContentManager().Load<Texture2D>("Textures/tex_tile_Basic");
                    m_TileModel.UseTexture = true;
                    
                    m_TileModel.CanDraw = true;
                    m_TreeShort1.Texture2D = FinalScene.GetContentManager().Load<Texture2D>("Textures/tex_tree_TreeShort1");
                    m_TreeShort1.UseTexture = true;
                    m_TreeShort1.CanDraw = true;

                    m_TileModel.DiffuseColor = new Vector3(1.0f, 1.0f, 1.0f);
                    break;

                case TileType.Normal4:
                    ResetPropListParameters();
                    m_TileModel.Texture2D = FinalScene.GetContentManager().Load<Texture2D>("Textures/tex_tile_Basic");
                    m_TileModel.UseTexture = true;

                    m_TileModel.CanDraw = true;
                    m_TreeTall1.Texture2D = FinalScene.GetContentManager().Load<Texture2D>("Textures/tex_tree_TreeShort1");
                    m_TreeTall1.UseTexture = true;
                    m_TreeTall1.CanDraw = true;

                    m_TileModel.DiffuseColor = new Vector3(1.0f, 1.0f, 1.0f);
                    break;

                case TileType.Water:
                    ResetPropListParameters();
                    m_TileModel.CanDraw = true;

                    m_TileModel.DiffuseColor = new Vector3(1.0f, 1.0f, 1.0f);
                    break;

                case TileType.Cliff:
                    ResetPropListParameters();
                    m_TileModel.CanDraw = false;
                   
                    m_TileModel.DiffuseColor = new Vector3(0.5f, 0.0f, 0.0f);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            //What to do if the tile is selected
            if (m_Selected)
            {
                m_TileModel.Selected = true;
                if (m_SettlementDisplayModel != null)
                    m_SettlementDisplayModel.Selected = true;
            }
            else
            {
                m_TileModel.Selected = false;
                if (m_SettlementDisplayModel != null)
                    m_SettlementDisplayModel.Selected = false;
                
            }

            //What to do if the tile is permanently selected (until the tile is deselected)
            if (m_PermanentSelected)
            {
                m_TileModel.PermanentSelected = true;
                if (m_SettlementDisplayModel != null)
                    m_SettlementDisplayModel.PermanentSelected = true;
            }
            else
            {
                m_TileModel.PermanentSelected = false;
                if (m_SettlementDisplayModel != null)
                    m_SettlementDisplayModel.PermanentSelected = false;
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
                        case Menu.ModeSelected.None:
                            GridFieldManager.GetInstance(m_GameScene).PermanentSelect(m_Row,m_Column);
                            break;
                        case Menu.ModeSelected.Attack:
                            break;
                        case Menu.ModeSelected.Defend:
                            break;
                        case Menu.ModeSelected.Gather:
                            break;
                        case Menu.ModeSelected.TileBlue:
                            ChangeSettlementModel("Models/settlement_TestSettlementBlue");
                            Menu.GetInstance().ResetSelectedMode();
                            break;
                        case Menu.ModeSelected.TileGold:
                            ChangeSettlementModel("Models/settlement_TestSettlementGold");
                            Menu.GetInstance().ResetSelectedMode();
                            break;
                        case Menu.ModeSelected.TileRed:
                            ChangeSettlementModel("Models/settlement_TestSettlementRed");
                            Menu.GetInstance().ResetSelectedMode();
                            break;
                        case Menu.ModeSelected.Delete:
                            RemoveSettlementModel();
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

        private void ChangeSettlementModel(string asset)
        {
            GameModel newModel = new GameModel(asset);
            newModel.LocalPosition += new Vector3(0, GRIDHEIGHT, 0);
            newModel.CanDraw = true;
            newModel.LoadContent(FinalScene.GetContentManager());
            m_TileModel.RemoveChild(m_SettlementDisplayModel);

            m_SettlementDisplayModel = newModel;
            m_TileModel.AddChild(newModel);
        }

        private void InitializeProps()
        {
            foreach (GameModel prop in m_PropsList)
            {
                prop.LocalPosition += new Vector3(0, GRIDHEIGHT, 0);
                prop.CanDraw = true;
                prop.LoadContent(FinalScene.GetContentManager());
                prop.Rotate(0, -90, 0);
                prop.Translate(0, 0, 20);
                prop.DiffuseColor = new Vector3(1, 1, 1);
                m_TileModel.AddChild(prop);
            }
        }

        private void ResetPropListParameters()
        {
            foreach (GameModel prop in m_PropsList)
            {
                prop.CanDraw = false;
                prop.DiffuseColor = new Vector3(1, 1, 1);
            }
        }

        private void RemoveSettlementModel()
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

        public bool PermanentSelect
        {
            get { return m_PermanentSelected; }
            set { m_PermanentSelected = value; }
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