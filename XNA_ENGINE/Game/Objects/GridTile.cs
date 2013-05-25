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
        private bool m_Open = true;
        private bool m_IsUsedbyStructure = false;

        private int m_WoodCount = 0;

        private float m_YOffset;

        private float m_YOffsetTarget;
        private float m_OffsetTransitionTime;

        private readonly GameScene m_GameScene;

        private Army m_BoundArmy;
        private bool m_ShamanGoal;
        private bool m_IsInUse = false;

        //PathFinding
        private int m_PFH;
        private int m_PFG;
        private GridTile m_PFParent;

        public enum TileType
        {
            NormalGrass,
            TreeLong,
            TreeShort,
            Pond,
            Water,
            Cliff,
            Spiked,
            Rock1,
            Rock2,
            Rock3,
            Rock4,
            DirtGrass1,
            Dirt1,
            Dirt2,
            TreeWillow,
            TreeDead1,
            TreeDead2,
            OrangeGrass,
            TreeShort3,
            TreeShort4,
            TreeShort5,
            TreeShort6,

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

            m_YOffset = GridFieldManager.GetInstance().Random.Next(YOFFSETMIN, YOFFSETMAX);
            m_YOffsetTarget = m_YOffset;
            
            SetType(m_TileType);

            m_BoundArmy = null;
        }

        public void Update(RenderContext renderContext)
        {
            if (m_YOffset != m_YOffsetTarget)
            {
                float delta = m_YOffsetTarget - m_YOffset;
                delta = delta * (m_OffsetTransitionTime*(renderContext.GameTime.ElapsedGameTime.Milliseconds/1000.0f));
                m_YOffset += delta;
                m_TileModel.Translate(m_TileModel.LocalPosition.X, m_YOffset, m_TileModel.LocalPosition.Z);
            }

            m_TileModel.ShamanGoal = m_ShamanGoal;
            m_TileModel.GreenHighLight = m_IsInUse;

            m_IsInUse = false;

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
                    if (Menu.GetInstance().GetSelectedMode() == Menu.ModeSelected.None)
                    {
                        GridFieldManager.GetInstance().PermanentDeselect();
                        Menu.GetInstance().SubMenu = Menu.SubMenuSelected.BaseMode;
                        Menu.GetInstance().SetSelectedMode(Menu.ModeSelected.None);
                    }
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
                    LoadTileType(new PrefabTreeShort1(this));
                    break;
                case TileType.Pond:
                    LoadTileType(new PrefabPond(this));
                    break;
                case TileType.Water:
                    LoadTileType(new PrefabNormalGrass(this));
                    break;
                case TileType.Cliff:
                    LoadTileType(new PrefabCliff(this));
                    break;
                case TileType.Spiked:
                    LoadTileType(new PrefabNormalGrass(this));
                    break;
                case TileType.Rock1:
                    LoadTileType(new PrefabRock1(this));
                    break;
                case TileType.Rock2:
                    LoadTileType(new PrefabRock2(this));
                    break;
                case TileType.Rock3:
                    LoadTileType(new PrefabRock3(this));
                    break;
                case TileType.Rock4:
                    LoadTileType(new PrefabRock4(this));
                    break;
                case TileType.DirtGrass1:
                    LoadTileType(new PrefabDirtGrass1(this));
                    break;
                case TileType.Dirt1:
                    LoadTileType(new PrefabDirt1(this));
                    break;
                case TileType.Dirt2:
                    LoadTileType(new PrefabDirt2(this));
                    break;
                case TileType.TreeWillow:
                    LoadTileType(new PrefabTreeWillow(this));
                    break;
                case TileType.TreeDead1:
                    LoadTileType(new PrefabTreeDead1(this));
                    break;
                case TileType.TreeDead2:
                    LoadTileType(new PrefabTreeDead2(this));
                    break;
                case TileType.OrangeGrass:
                    LoadTileType(new PrefabOrangeGrass(this));
                    break;
                case TileType.TreeShort3:
                    LoadTileType(new PrefabTreeShort3(this));
                    break;
                case TileType.TreeShort4:
                    LoadTileType(new PrefabTreeShort4(this));
                    break;
                case TileType.TreeShort5:
                    LoadTileType(new PrefabTreeShort5(this));
                    break;
                case TileType.TreeShort6:
                    LoadTileType(new PrefabTreeShort6(this));
                    break;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }

        private void LoadTileType(BasePrefab tilePrefab)
        {
            bool isDangerous = false;
            if (m_TileModel!= null)
                isDangerous = m_TileModel.Danger;

            //Unload the previous model
            UnloadTileType();

            //Load new stuff
            if (m_IsUsedbyStructure == false)
                m_Open = tilePrefab.Open;
            else
                m_Open = false;

            m_WoodCount = tilePrefab.WoodCount;
            m_TileModel = tilePrefab.TileModel;

            GridFieldManager.GetInstance().GameScene.AddSceneObject(m_TileModel);
            m_TileModel.Translate(new Vector3(GRIDWIDTH * m_Row, m_YOffset, GRIDDEPTH * m_Column));

            //Give it a random spin
            int rand = GridFieldManager.GetInstance().Random.Next(0,4);
            m_TileModel.Rotate( 0, 90 * rand, 0);


            m_TileModel.CreateBoundingBox(GRIDWIDTH, 1, GRIDDEPTH, new Vector3(0, GRIDHEIGHT, 0));
            m_TileModel.DrawBoundingBox = false;

            foreach (var prop in tilePrefab.PropList)
            {
                prop.LocalPosition += new Vector3(0, GRIDHEIGHT, 0);

                prop.DiffuseColor = new Vector3(1, 1, 1);
                m_TileModel.AddChild(prop);
            }

            if (m_TileModel != null)
                m_TileModel.Danger = isDangerous;

            if (m_ShamanGoal)
                m_TileModel.ShamanGoal = true;
            else
                m_TileModel.ShamanGoal = false;
        }

        public void PFResetValues()
        {
            m_PFG = 0;
            m_PFH = 0;
            m_PFParent = null;
        }

        public GridTile PFParent
        {
            get { return m_PFParent; }
            set { m_PFParent = value; }
        }


        public int PFH
        {
            get { return m_PFH; }
            set { m_PFH = value; }
        }

        public int PFG
        {
            get { return m_PFG; }
            set { m_PFG = value; }
        }

        public int PFGetF()
        {
            return m_PFG + m_PFH;
        }

        private void UnloadTileType()
        {
            foreach (var prop in m_PropsList)
                m_TileModel.RemoveChild(prop);

            GridFieldManager.GetInstance().GameScene.RemoveSceneObject(m_TileModel);

            m_PropsList.Clear();
        }

        public void SetIsUsedByStructure(bool value)
        {
            m_IsUsedbyStructure = value;
            SetType(m_TileType);
        }

        public bool GetIsUsedByStructure()
        {
            return m_IsUsedbyStructure;
        }

        public bool IsOpen()
        {
            return m_Open;
        }

        public bool IsWalkable()
        {
            if (m_Open && m_IsUsedbyStructure == false)
                return true;

            return false;
        }
        public bool ShamanGoal
        {
            get { return m_ShamanGoal; }
            set { m_ShamanGoal = value; }
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

        public bool IsInUse
        {
            get { return m_IsInUse; }
            set { m_IsInUse = value; }
        }

        public GameModelGrid Model
        {
            get { return m_TileModel; }
        }

        public void LevelOut(int offset, float time)
        {
            m_YOffsetTarget = offset;
            m_OffsetTransitionTime = time;
        }

        public void BoundArmy(Army army)
        {
            m_TileModel.Danger = true;
            m_BoundArmy = army;
        }

        public void UnBoundArmy(Army army = null)
        {
            m_TileModel.Danger = false;
            m_BoundArmy = null;
        }

        public Army IsBoundArmy()
        {
            return m_BoundArmy;
        }

        public bool PickupWood(Player player, bool pickup = true)
        {
            if (pickup)
            {
                player.GetResources().AddWood(m_WoodCount);

                if (m_WoodCount > 0)
                    SetType(TileType.NormalGrass);
                m_WoodCount = 0;

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