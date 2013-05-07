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
using XNA_ENGINE.Game.Objects;
using XNA_ENGINE.Game.Managers;
using XNA_ENGINE.Game.Scenes;
using XNA_ENGINE.Game.TilePrefabs;

namespace XNA_ENGINE.Game.Objects
{
    public class GridTile
    {
        private GameModelGrid m_TileModel;

        private List<GameModelGrid> m_PropsList;

        private readonly int m_Row, m_Column;

        private const float GRIDWIDTH = 64;
        private const float GRIDDEPTH = 64;
        private const float GRIDHEIGHT = 32;
        private const int YOFFSETMIN = 0;
        private const int YOFFSETMAX = 15;

        private TileType m_TileType;

        private bool m_Selected;
        private bool m_PermanentSelected;
        private bool m_Open = true;
        private bool m_IsUsedbyStructure = false;

        private int m_WoodCount = 0;

        private readonly GameScene m_GameScene;

        public enum TileType
        {
            NormalGrass,
            TreeLong,
            TreeShort,
            Normal4,
            Water,
            Cliff,
            Spiked,

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
            m_PropsList = new List<GameModelGrid>();
            SetType(m_TileType);


            int woodCount = GridFieldManager.GetInstance().Random.Next(0, 10);
            if (woodCount == 1)
            {
                m_WoodCount = 20;
            } 
        }

        public void Update(RenderContext renderContext)
        {
            if (m_WoodCount > 0)
            {
               // m_Wood.DiffuseColor = new Vector3(0.36f, 0.25f, 0.20f);
              //  m_Wood.CanDraw = true;
            }
            else
            {
               // m_Wood.DiffuseColor = new Vector3(0.36f, 0.25f, 0.20f);
               // m_Wood.CanDraw = false;
            }

            //What to do if the tile is selected
            /*if (m_Selected)
            {
                m_TileModel.Selected = true;
            }
            else
            {
                m_TileModel.Selected = false;
            }*/

        
            
            OnSelected();
        }

        public bool HitTest(Ray ray)
        {
            if (m_TileModel.HitTest(ray))
            {
                System.Diagnostics.Debug.WriteLine("Row:" + m_Row.ToString() + " Column:" + m_Column.ToString());
                return true;
            }
            return false;
        }

        private bool OnSelected()
        {
            if (!m_Selected)
            {
                m_TileModel.Selected = false;
                return false;
            }

            m_TileModel.Selected = true;

            bool creativeMode = GridFieldManager.GetInstance().CreativeMode;
            //Get the inputmanager
            var inputManager = PlayScene.GetInputManager();
            //What mode is there selected in the menu to build?
            Menu.ModeSelected selectedMode = Menu.GetInstance().GetSelectedMode();

            if (creativeMode) //Creative mode off
            {
                if (inputManager.GetAction((int)PlayScene.PlayerInput.LeftHold).IsTriggered)
                {
                    SetType(Menu.GetInstance().TileTypeSelected); 
                }
            }
            else
            {
                if (inputManager.GetAction((int)PlayScene.PlayerInput.LeftClick).IsTriggered)
                {
                    Menu.GetInstance().SubMenu = Menu.SubMenuSelected.VillagerMode;
                }

                if (inputManager.GetAction((int)PlayScene.PlayerInput.RightClick).IsTriggered)
                {

                }
            }

            return true;
        }

        public void SetType(TileType type)
        {
            m_TileType = type;
            
            switch (type)
            {
                case TileType.NormalGrass:
                    LoadTileType(new PrefabNormalGrass(this));
                    break;
                case TileType.TreeLong:
                    LoadTileType(new PrefabTreeLong(this));
                    break;
                case TileType.TreeShort:
                    LoadTileType(new PrefabTreeShort(this));
                    break;
                case TileType.Normal4:
                    LoadTileType(new PrefabNormalGrass(this));
                    break;
                case TileType.Water:
                    LoadTileType(new PrefabNormalGrass(this));
                    break;
                case TileType.Cliff:
                    LoadTileType(new PrefabNormalGrass(this));
                    break;
                case TileType.Spiked:
                    LoadTileType(new PrefabNormalGrass(this));
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }

        private void LoadTileType(BasePrefab tilePrefab)
        {
            //Unload the previous model
            UnloadTileType();

            //Load new stuff
            if (m_IsUsedbyStructure == false)
                m_Open = tilePrefab.Open;
            else
                m_Open = false;

            m_TileModel = tilePrefab.TileModel;

            int yOffset = GridFieldManager.GetInstance().Random.Next(YOFFSETMIN, YOFFSETMAX);
            GridFieldManager.GetInstance().GameScene.AddSceneObject(m_TileModel);
            m_TileModel.Translate(new Vector3(GRIDWIDTH * m_Row, yOffset, GRIDDEPTH * m_Column));
            m_TileModel.CreateBoundingBox(GRIDWIDTH, 1, GRIDDEPTH, new Vector3(0, GRIDHEIGHT, 0));
            m_TileModel.DrawBoundingBox = false;

            foreach (var prop in tilePrefab.PropList)
            {
                prop.LocalPosition += new Vector3(0, GRIDHEIGHT, 0);
                //prop.CanDraw = true;
                prop.LoadContent(PlayScene.GetContentManager());
                prop.Rotate(0, -90, 0);
                prop.Translate(0, 0, 20);
                prop.DiffuseColor = new Vector3(1, 1, 1);
                m_TileModel.AddChild(prop);
            }
        }

        private void UnloadTileType()
        {
            foreach (var prop in m_PropsList)
            {
                m_TileModel.RemoveChild(prop);
            }

            GridFieldManager.GetInstance().GameScene.RemoveSceneObject(m_TileModel);

            m_PropsList.Clear();
        }

        public void SetIsUsedByStructure(bool value)
        {
            m_IsUsedbyStructure = value;
            SetType(m_TileType);
        }

        public bool IsOpen()
        {
            return m_Open;
        }

        public bool Selected
        {
            get { return m_Selected; }
            set { m_Selected = value; }
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

        public GameModelGrid Model
        {
            get { return m_TileModel; }
        }

        public bool PickupWood(Player player, bool pickup = true)
        {
            if (pickup)
            {
                player.GetResources().AddWood(m_WoodCount);
                m_WoodCount = 0;
                //m_Wood.CanDraw = false;

                return true;
            }
            else
            {
                if (m_WoodCount > 0)
                {
                    return true;
                }

                return false;
            }
        }
    }
}