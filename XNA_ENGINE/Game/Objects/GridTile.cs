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

namespace XNA_ENGINE.Game.Objects
{
    public class GridTile
    {
        private GameModelGrid m_TileModel;

        //Props
        private GameModelGrid m_TreeShort1;
        private GameModelGrid m_TreeTall1;
        private GameModelGrid m_Wood;

        private Army m_Army;

        private List<GameModelGrid> m_PropsList;
        private List<Placeable> m_LinkedPlaceables;

        private readonly int m_Row, m_Column;

        private const float GRIDWIDTH = 64;
        private const float GRIDDEPTH = 64;
        private const float GRIDHEIGHT = 32;
        private const int YOFFSETMIN = 0;
        private const int YOFFSETMAX = 15;

        private TileType m_TileType = TileType.Normal1;

        private bool m_Selected;
        private bool m_PermanentSelected;
        private bool m_Open = true;

        private int m_WoodCount = 0;

        private readonly GameScene m_GameScene;

        public enum TileType
        {
            Normal1,
            Normal2,
            Normal3,
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
            int yOffset = GridFieldManager.GetInstance(m_GameScene).Random.Next(YOFFSETMIN, YOFFSETMAX);

            m_TileModel = new GameModelGrid("Models/tile_Normal");

            m_PropsList = new List<GameModelGrid>();
            m_LinkedPlaceables = new List<Placeable>();
            m_LinkedPlaceables.Add(new RallyPoint(this, m_GameScene));
            ShowFlag(false);

            m_TreeShort1 = new GameModelGrid("Models/tree_TreeShort");
            m_TreeTall1 = new GameModelGrid("Models/tree_TreeTall");
            m_Wood = new GameModelGrid("Models/prop_Wood");
            m_PropsList.Add(m_TreeShort1);
            m_PropsList.Add(m_TreeTall1);
            m_PropsList.Add(m_Wood);
            InitializeProps();

            m_Wood.LocalPosition = new Vector3(16, 32, -16);
            m_Wood.UseTexture = false;
            m_Wood.Scale(new Vector3(0.5f, 0.5f, 0.5f));


            m_TileModel.Translate(new Vector3(GRIDWIDTH*m_Row, yOffset, GRIDDEPTH*m_Column));
            m_GameScene.AddSceneObject(m_TileModel);

            m_TileModel.CreateBoundingBox(GRIDWIDTH, 1, GRIDDEPTH, new Vector3(0, GRIDHEIGHT, 0));
            m_TileModel.DrawBoundingBox = false;

            int woodCount = GridFieldManager.GetInstance(SceneManager.ActiveScene).Random.Next(0, 10);

            if (woodCount == 1)
            {
                m_WoodCount = 20;
            }
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

                    m_TileModel.DiffuseColor = new Vector3(1.0f, 1.0f, 1.0f);
                    break;

                case TileType.Normal2:
                    ResetPropListParameters();
                    m_TileModel.Texture2D =
                        FinalScene.GetContentManager().Load<Texture2D>("Textures/tex_tile_BasicWithDirt");
                    m_TileModel.UseTexture = true;

                    m_TileModel.CanDraw = true;

                    m_TileModel.DiffuseColor = new Vector3(1.0f, 1.0f, 1.0f);
                    break;

                case TileType.Normal3:
                    ResetPropListParameters();
                    m_TileModel.Texture2D = FinalScene.GetContentManager().Load<Texture2D>("Textures/tex_tile_Basic");
                    m_TileModel.UseTexture = true;

                    m_TileModel.CanDraw = true;
                    m_TreeShort1.Texture2D =
                        FinalScene.GetContentManager().Load<Texture2D>("Textures/tex_tree_Tall1");
                    m_TreeShort1.UseTexture = true;
                    m_TreeShort1.CanDraw = true;

                    m_TileModel.DiffuseColor = new Vector3(1.0f, 1.0f, 1.0f);
                    break;

                case TileType.Normal4:
                    ResetPropListParameters();
                    m_TileModel.Texture2D = FinalScene.GetContentManager().Load<Texture2D>("Textures/tex_tile_Basic");
                    m_TileModel.UseTexture = true;

                    m_TileModel.CanDraw = true;
                    m_TreeTall1.Texture2D =
                    FinalScene.GetContentManager().Load<Texture2D>("Textures/tex_tree_Tall1");
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

                case TileType.Spiked:
                    ResetPropListParameters();
                    m_TileModel.Texture2D = FinalScene.GetContentManager().Load<Texture2D>("Textures/tex_tile_Basic");
                    m_TileModel.UseTexture = true;

                    m_TileModel.CanDraw = true;
                    m_TreeTall1.Texture2D =
                    FinalScene.GetContentManager().Load<Texture2D>("Textures/tex_tree_Tall1");
                    m_TreeTall1.UseTexture = true;
                    m_TreeTall1.CanDraw = true;

                    m_TileModel.DiffuseColor = new Vector3(1.0f, 1.0f, 1.0f);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (m_WoodCount > 0)
            {
                m_Wood.DiffuseColor = new Vector3(0.36f, 0.25f, 0.20f);
                m_Wood.CanDraw = true;
            }
            else
            {
                m_Wood.DiffuseColor = new Vector3(0.36f, 0.25f, 0.20f);
                m_Wood.CanDraw = false;
            }

            //What to do if the tile is selected
            if (m_Selected)
            {
                m_TileModel.Selected = true;
            }
            else
            {
                m_TileModel.Selected = false;
            }

            //What to do if the tile is permanently selected (until the tile is deselected)
            if (m_PermanentSelected)
            {
                m_TileModel.PermanentSelected = true;
            }
            else
            {
                m_TileModel.PermanentSelected = false;
            }

            foreach (var placeable in m_LinkedPlaceables)
            {
                placeable.Update(renderContext);
            }

            OnSelected();

            if (this.PermanentSelected)
                Menu.GetInstance().SubMenu = Menu.SubMenuSelected.BaseMode;
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
            if (!m_Selected) return false;

            bool creativeMode = GridFieldManager.GetInstance(m_GameScene).CreativeMode;
            //Get the inputmanager
            var inputManager = FinalScene.GetInputManager();
            //What mode is there selected in the menu to build?
            Menu.ModeSelected selectedMode = Menu.GetInstance().GetSelectedMode();

            if (creativeMode) //Creative mode off
            {
                if (inputManager.GetAction((int) FinalScene.PlayerInput.LeftHold).IsTriggered)
                {
                    m_TileType = Menu.GetInstance().TileTypeSelected;
                }
            }
            else
            {
                if (inputManager.GetAction((int) FinalScene.PlayerInput.LeftClick).IsTriggered)
                {

                    // BUILD BUILDINGS

                    // DELETE

                    // CREATE TILES WITH SHAMAN
                    /*  case Menu.ModeSelected.BuildTile1:
                            SetTileSpiked();
                            break;
                        case Menu.ModeSelected.BuildTile2:
                            break;
                        case Menu.ModeSelected.BuildTile3:
                            break;
                        case Menu.ModeSelected.BuildTile4:
                            break;*/

                    // DEFAULT

                }

                if (inputManager.GetAction((int) FinalScene.PlayerInput.RightClick).IsTriggered)
                {

                }
            }

            foreach (var placeable in m_LinkedPlaceables)
                placeable.OnSelected();

            return true;
        }

        private void InitializeProps()
        {
            foreach (GameModelGrid prop in m_PropsList)
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
            foreach (GameModelGrid prop in m_PropsList)
            {
                prop.CanDraw = false;
                prop.DiffuseColor = new Vector3(1, 1, 1);
            }
        }

        public void AddSettlement(Settlement.SettlementType settlementType)
        {
            m_LinkedPlaceables.Add(new Settlement(this, m_GameScene, settlementType));
        }

        public void AddSchool(School.SchoolType schoolType)
        {
            m_LinkedPlaceables.Add(new School(this, m_GameScene, schoolType));
        }

        public void AddShrine(Shrine.ShrineType shrineType)
        {
            m_LinkedPlaceables.Add(new Shrine(this, m_GameScene, shrineType));
        }

        // CHANGE TILE WITH SHAMAN
        public void SetTileSpiked()
        {
            m_TileType = TileType.Spiked;
        }

        /* private void RemoveSettlementModel()
        {
            foreach (var placeable in m_Placeables)
            {
                if (placeable.PlaceableTypeMeth == Placeable.PlaceableType.Settlement)
                {
                    m_Placeables.Remove(placeable);
                    return;
                }
            }
        }*/

        public Settlement HasSettlement()
        {
            foreach (var placeable in m_LinkedPlaceables)
            {
                if (placeable.PlaceableTypeMeth == Placeable.PlaceableType.Settlement)
                    return (Settlement) placeable;
            }

            return null;
        }

        public School HasSchool()
        {
            foreach (var placeable in m_LinkedPlaceables)
            {
                if (placeable.PlaceableTypeMeth == Placeable.PlaceableType.School)
                    return (School) placeable;
            }

            return null;
        }

        public Shrine HasShrine()
        {
            foreach (var placeable in m_LinkedPlaceables)
            {
                if (placeable.PlaceableTypeMeth == Placeable.PlaceableType.Shrine)
                    return (Shrine) placeable;
            }

            return null;
        }

        public bool PermanentSelected
        {
            get { return m_PermanentSelected; }
            set { m_PermanentSelected = value; }
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
            set { m_TileModel = value; }
        }

        public List<Placeable> LinkedPlaceables
        {
            get { return m_LinkedPlaceables; }
        }

        public void ShowFlag(bool value)
        {
            foreach (var placeable in m_LinkedPlaceables)
            {
                if (placeable.PlaceableTypeMeth == Placeable.PlaceableType.Flag)
                {
                    placeable.Model.CanDraw = value;
                }
            }
        }

        public bool PickupWood(Player player, bool pickup = true)
        {
            if (pickup)
            {
                player.GetResources().AddWood(m_WoodCount);
                m_WoodCount = 0;
                m_Wood.CanDraw = false;

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