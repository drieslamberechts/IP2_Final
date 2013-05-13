using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XNA_ENGINE.Engine;
using XNA_ENGINE.Engine.Helpers;
using XNA_ENGINE.Engine.Scenegraph;
using XNA_ENGINE.Game.Managers;
using XNA_ENGINE.Game.Scenes;

namespace XNA_ENGINE.Game.Objects
{
    internal class Menu
    {

        //Object that holds the menu
        private static Menu m_Menu;

        private ContentManager Content;

        private readonly Texture2D m_TexMenuBackground,
                                   m_TexWoodResource,
                                   m_TexInfluenceResource,
                                   m_TexDelete,
                                   m_TexCharacterStats,
                                   m_TexHoverVillager,
                                   m_TexVillager,
                                   m_TexVillagerHover,
                                   m_TexShaman,
                                   m_TexShamanHover,
                                   m_TexUnitList,
                                   m_TexSettlement,
                                   m_TexSettlementHover,
                                   m_TexSettlementInfo,
                                   m_TexSchool,
                                   m_TexShrine,
                                   m_TexBuildTile,
                                   m_TexSplit;

        private Rectangle m_RectMenuBackground,
                          m_RectWoodResource,
                          m_RectInfluenceResource,
                          m_RectDelete,
                          m_RectCharacterStats,
                          m_RectHoverVillager,
                          m_RectVillager,
                          m_RectShaman,
                          m_RectUnitList,
                          m_RectSettlement,
                          m_RectSettlementInfo,
                          m_RectSchool,
                          m_RectShrine,
                          m_RectBuildTile,
                          m_RectSplit;

        public enum SubMenuSelected
        {
            BaseMode,
            SoldierMode,
            ShrineMode,
            SettlementMode,
            ShamanMode,
            VillagerMode,
            SchoolMode
        }

        public enum ModeSelected
        {
            None,
            Attack,
            Defend,
            Gather,
            BuildSettlement,
            BuildShrine,
            BuildSchool,
            Delete,
            BuildTile1,
            BuildTile2,
            BuildTile3,
            BuildTile4,

            enumSize
        }

        private SubMenuSelected m_SubMenuSelected = SubMenuSelected.VillagerMode;
        private ModeSelected m_SelectedMode = ModeSelected.None;
        private GridTile.TileType m_TileTypeSelected = GridTile.TileType.NormalGrass;
        private SpriteFont m_DebugFont;

        private const int COSTOFWOOD_SETTLEMENT = 20;
        private const int COSTOFWOOD_SCHOOL = 30;
        private const int COSTOFWOOD_SHRINE = 50;
        private const int COSTOFINFLUENCE_SETTLEMENT = 0;
        private const int COSTOFINFLUENCE_SCHOOL = 0;
        private const int COSTOFINFLUENCE_SHRINE = 0;
        private const int COSTOFWOOD_TILE1 = 0;
        private const int COSTOFINFLUENCE_TILE1 = 20;

        // HOVERING
        private bool m_bShowVillagerHover,
                     m_bShowSettlementHover,
                     m_bShowShamanHover;

        //Singleton implementation
        static public Menu GetInstance()
        {
            if (m_Menu == null)
                m_Menu = new Menu();
            return m_Menu;
        }

        private Menu()
        {
            Content = PlayScene.GetContentManager();

            m_bShowVillagerHover = false;
            m_bShowSettlementHover = false;
            m_bShowShamanHover = false;

            // MENU ONDERKANT BACKGROUND
            m_TexMenuBackground = Content.Load<Texture2D>("final Menu/grootMenu_Onderkant");

            // RESOURCE STATS
            m_TexWoodResource = Content.Load<Texture2D>("final Menu/resources_wood");
            m_TexInfluenceResource = Content.Load<Texture2D>("final Menu/resources_influence");

            // CHARACTER STATS
            m_TexCharacterStats = Content.Load<Texture2D>("final Menu/characterStats");
            m_TexUnitList = Content.Load<Texture2D>("final Menu/unitList");

            // ICONS
            m_TexDelete = Content.Load<Texture2D>("final Menu/Button_Delete");
            m_TexSplit = Content.Load<Texture2D>("final Menu/iconStandard");
            m_TexVillager = Content.Load<Texture2D>("final Menu/Button_AddVillager");
            m_TexShaman = Content.Load<Texture2D>("final Menu/Button_AddShaman");
            m_TexVillagerHover = Content.Load<Texture2D>("final Menu/Button_AddVillagerHover");
            m_TexShamanHover = Content.Load<Texture2D>("final Menu/Button_AddShamanHover");

            // HOVERING
            m_TexHoverVillager = Content.Load<Texture2D>("final Menu/hoverVillager");
            m_TexSettlementInfo = Content.Load<Texture2D>("final Menu/hoverSettlementInfo");

            // BUILDING ICONS
            m_TexSettlement = Content.Load<Texture2D>("final Menu/Button_AddSettlement");
            m_TexSettlementHover = Content.Load<Texture2D>("final Menu/Button_AddSettlement");
            m_TexSchool = Content.Load<Texture2D>("final Menu/Button_AddSchool");
            m_TexShrine = Content.Load<Texture2D>("final Menu/iconStandard");

            m_TexBuildTile = Content.Load<Texture2D>("final Menu/iconStandard");

            m_DebugFont = Content.Load<SpriteFont>("Fonts/DebugFont");
        }

        public void SetFont(SpriteFont font)
        {
            m_DebugFont = font;
        }

        public void Update(RenderContext renderContext)
        {
            if (m_SelectedMode == ModeSelected.None && GridFieldManager.GetInstance().CreativeMode == false)
                GridFieldManager.GetInstance().SelectionModeMeth = GridFieldManager.SelectionMode.select1x1;
        }

        public bool HandleInput(RenderContext renderContext)
        {
            Player userPlayer = GridFieldManager.GetInstance().UserPlayer;

            var mousePos = new Vector2(renderContext.Input.CurrentMouseState.X, renderContext.Input.CurrentMouseState.Y);
            var inputManager = PlayScene.GetInputManager();
            Placeable selectedPlaceable = GridFieldManager.GetInstance().GetPermanentSelected();

            //if (inputManager.GetAction((int)PlayScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectSwitch))

            // HOVER VILLAGER BUTTON
            if (CheckHitButton(mousePos, m_RectVillager) && m_SubMenuSelected == SubMenuSelected.SettlementMode)
            {
                m_bShowVillagerHover = true;
            }
            else m_bShowVillagerHover = false;

            // HOVER SHAMAN BUTTON
            if (CheckHitButton(mousePos, m_RectShaman) && m_SubMenuSelected == SubMenuSelected.ShrineMode)
            {
                m_bShowShamanHover = true;
            }
            else m_bShowShamanHover = false;

            // HOVER SETTLEMENT BUTTON
            if (CheckHitButton(mousePos, m_RectSettlement) && m_SubMenuSelected == SubMenuSelected.VillagerMode)
            {
                m_bShowSettlementHover = true;
            }
            else m_bShowSettlementHover = false;

            /*
            {
                if (m_SubMenuSelected == SubMenuSelected.MoveMode) m_SubMenuSelected = SubMenuSelected.VillagerMode;
                else m_SubMenuSelected = SubMenuSelected.MoveMode;
                return true;
            }
            */
            // --------------------------------------------
            if (inputManager.GetAction((int)PlayScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectSettlement))
            switch (m_SubMenuSelected)
            {

                case SubMenuSelected.BaseMode:
                    if (inputManager.GetAction((int)PlayScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectDelete))
                    {
                        // DELETE ITEM THAT WILL BE SELECTED
                        userPlayer.GetResources().DecreaseWood(COSTOFWOOD_SETTLEMENT);
                        userPlayer.GetResources().DecreaseInfluence(COSTOFINFLUENCE_SETTLEMENT);
                        GridFieldManager.GetInstance().SelectionModeMeth = GridFieldManager.SelectionMode.select2x2;
                        return true;
                    }
                    break;

                // --------------------------------------------
                // VILLAGER MODE
                // --------------------------------------------
                case SubMenuSelected.VillagerMode:
                    if (inputManager.GetAction((int)PlayScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectSchool))
                    {
                        // SET DECREASE RESOURCES
                        userPlayer.GetResources().DecreaseWood(COSTOFWOOD_SCHOOL);
                        userPlayer.GetResources().DecreaseInfluence(COSTOFINFLUENCE_SCHOOL);
                        GridFieldManager.GetInstance().SelectionModeMeth = GridFieldManager.SelectionMode.select2x2;
                        m_SelectedMode = ModeSelected.BuildSchool;
                        return true;
                    }

                    if (inputManager.GetAction((int)PlayScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectSettlement))
                    {
                        // SET DECREASE RESOURCES
                        userPlayer.GetResources().DecreaseWood(COSTOFWOOD_SETTLEMENT);
                        userPlayer.GetResources().DecreaseInfluence(COSTOFINFLUENCE_SETTLEMENT);
                        GridFieldManager.GetInstance().SelectionModeMeth = GridFieldManager.SelectionMode.select2x2;
                        m_SelectedMode = ModeSelected.BuildSettlement;
                        return true;
                    }

                    if (inputManager.GetAction((int)PlayScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectShrine))
                    {
                        // SET DECREASE RESOURCES
                        userPlayer.GetResources().DecreaseWood(COSTOFWOOD_SHRINE);
                        userPlayer.GetResources().DecreaseInfluence(COSTOFINFLUENCE_SHRINE);
                        GridFieldManager.GetInstance().SelectionModeMeth = GridFieldManager.SelectionMode.select1x1;
                        m_SelectedMode = ModeSelected.BuildShrine;
                        return true;
                    }
                    break;

                // --------------------------------------------
                // SHRINE MODE
                // --------------------------------------------
                case SubMenuSelected.ShrineMode:
                    if (inputManager.GetAction((int)PlayScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectShaman))
                    {
                        if (selectedPlaceable != null && selectedPlaceable.PlaceableTypeMeth == Placeable.PlaceableType.Settlement)
                        {
                            Console.WriteLine("Build Shaman");
                        }
                        return true;
                    }
                    break;

                // --------------------------------------------
                // SHAMAN MODE
                // --------------------------------------------
                case SubMenuSelected.ShamanMode:
                    // BUILD TILES WITH SHAMAN
                    if (inputManager.GetAction((int)PlayScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectBuildTile))
                    {
                        Console.WriteLine("Create Tile 1");
                        userPlayer.GetResources().DecreaseWood(COSTOFWOOD_TILE1);
                        userPlayer.GetResources().DecreaseInfluence(COSTOFINFLUENCE_TILE1);
                        GridFieldManager.GetInstance().SelectionModeMeth = GridFieldManager.SelectionMode.select1x1;
                        m_SelectedMode = ModeSelected.BuildTile1;
                        return true;
                    }
                    break;

                // --------------------------------------------
                // SOLDIER MODE
                // --------------------------------------------
                case SubMenuSelected.SoldierMode:
                    if (inputManager.GetAction((int)PlayScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectSplit))
                    {
                        Console.WriteLine("Split!");
                        // m_Player.GetArmyList().Add(m_Player.GetPlayerOptions().SplitArmy(m_Player.GetSelectedArmy()));

                        // Get the Split Army working
                        // m_Player.GetPlayerOptions().SplitArmy(/* Add selected army */, new Army(SceneManager.ActiveScene, new GridTile(SceneManager.ActiveScene, 10, 10)));

                        m_SelectedMode = ModeSelected.Gather;
                        return true;
                    }
                    break;

                // --------------------------------------------
                // SETTLEMENT MODE
                // --------------------------------------------
                case SubMenuSelected.SettlementMode:
                    if (inputManager.GetAction((int)PlayScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectVillager))
                    {
                        if (selectedPlaceable != null && selectedPlaceable.PlaceableTypeMeth == Placeable.PlaceableType.Settlement)
                        {
                            selectedPlaceable.QueueVillager();
                            Console.WriteLine("Build Villager");
                        }

                        return true;
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            return false;
        }

        // Draw
        public void Draw(RenderContext renderContext)
        {
            Player userPlayer = GridFieldManager.GetInstance().UserPlayer;

            
            // ------------------------------------------
            // WINDOWED
            // ------------------------------------------
            if (renderContext.GraphicsDevice.Viewport.Height < GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height)
            {
                // MENU ONDERKANT RECTANGLES
                m_RectMenuBackground = new Rectangle(0, renderContext.GraphicsDevice.Viewport.Height - m_TexMenuBackground.Height / 2, m_TexMenuBackground.Width / 2,
                                                     m_TexMenuBackground.Height / 2);

                // RESOURCE STATS RECTANGLES
                m_RectWoodResource = new Rectangle(renderContext.GraphicsDevice.Viewport.Width / 2 - m_TexWoodResource.Width / 2,
                                                  5, m_TexWoodResource.Width / 2,
                                                   m_TexWoodResource.Height / 2);

                m_RectInfluenceResource = new Rectangle(renderContext.GraphicsDevice.Viewport.Width / 2 + 10,
                                                  5, m_TexInfluenceResource.Width / 2,
                                                   m_TexInfluenceResource.Height / 2);

                // CHARACTER STATS RECTANGLES
                m_RectCharacterStats = new Rectangle(renderContext.GraphicsDevice.Viewport.Width - m_TexCharacterStats.Width/2,
                                  0, m_TexCharacterStats.Width/2, m_TexCharacterStats.Height/2);

                m_RectUnitList = new Rectangle(renderContext.GraphicsDevice.Viewport.Width - m_TexCharacterStats.Width / 2 + 10, 10,
                                  m_TexUnitList.Width/2, m_TexUnitList.Height/2);

                // ICONS
                m_RectDelete = new Rectangle(10, renderContext.GraphicsDevice.Viewport.Height - m_TexDelete.Height + 45,
                                                           m_TexDelete.Width / 2,
                                                           m_TexDelete.Height / 2);

                m_RectVillager = new Rectangle(10, renderContext.GraphicsDevice.Viewport.Height - m_TexDelete.Height + 45,
                                                           m_TexDelete.Width / 2,
                                                           m_TexDelete.Height / 2);

                m_RectShaman = new Rectangle(10, renderContext.GraphicsDevice.Viewport.Height - m_TexShaman.Height + 45,
                                                           m_TexShaman.Width / 2,
                                                           m_TexShaman.Height / 2);

                m_RectSchool = new Rectangle(10, renderContext.GraphicsDevice.Viewport.Height - m_TexSchool.Height + 45,
                                                           m_TexSchool.Width / 2,
                                                           m_TexSchool.Height / 2);

                m_RectShrine = new Rectangle(10 * 2 + m_TexSchool.Width / 2, renderContext.GraphicsDevice.Viewport.Height - m_TexShrine.Height + 45,
                                                           m_TexShrine.Width / 2,
                                                           m_TexShrine.Height / 2);

                m_RectSettlement = new Rectangle(10 * 3 + m_TexSchool.Width, renderContext.GraphicsDevice.Viewport.Height - m_TexSettlement.Height + 45,
                                                           m_TexSettlement.Width / 2,
                                                           m_TexSettlement.Height / 2);

                m_RectSplit = new Rectangle(10, renderContext.GraphicsDevice.Viewport.Height - m_TexSplit.Height + 45,
                                                           m_TexSplit.Width / 2,
                                                           m_TexSplit.Height / 2);

                // HOVERING
                m_RectHoverVillager = new Rectangle(m_RectDelete.X, m_RectDelete.Y - m_TexHoverVillager.Height / 2 - 10,
                                                           m_TexHoverVillager.Width / 2,
                                                           m_TexHoverVillager.Height / 2);

                m_RectSettlementInfo = new Rectangle(m_RectDelete.X, m_RectDelete.Y - m_TexHoverVillager.Height / 2 - 10,
                                                           m_TexHoverVillager.Width / 2,
                                                           m_TexHoverVillager.Height / 2);

            }
            // ------------------------------------------
            // FULLSCREEN
            // ------------------------------------------
            else
            {
                // MENU ONDERKANT RECTANGLES
                m_RectMenuBackground = new Rectangle(0, renderContext.GraphicsDevice.Viewport.Height - m_TexMenuBackground.Height, m_TexMenuBackground.Width,
                                                     m_TexMenuBackground.Height);

                // RESOURCE STATS RECTANGLES
                m_RectWoodResource = new Rectangle(renderContext.GraphicsDevice.Viewport.Width / 2 - m_TexWoodResource.Width,
                                                  5, m_TexWoodResource.Width,
                                                   m_TexWoodResource.Height);

                m_RectInfluenceResource = new Rectangle(renderContext.GraphicsDevice.Viewport.Width / 2 + 10,
                                                  5, m_TexInfluenceResource.Width,
                                                   m_TexInfluenceResource.Height);

                // CHARACTER STATS RECTANGLES
                m_RectCharacterStats = new Rectangle(renderContext.GraphicsDevice.Viewport.Width - m_TexCharacterStats.Width,
                                  0, m_TexCharacterStats.Width, m_TexCharacterStats.Height);

                m_RectUnitList = new Rectangle(renderContext.GraphicsDevice.Viewport.Width - m_TexCharacterStats.Width + 20, 20,
                                  m_TexUnitList.Width, m_TexUnitList.Height);

                // ICONS
                m_RectDelete = new Rectangle(10, renderContext.GraphicsDevice.Viewport.Height - m_TexDelete.Height - 20,
                                                           m_TexDelete.Width,
                                                           m_TexDelete.Height);

                m_RectVillager = new Rectangle(10, renderContext.GraphicsDevice.Viewport.Height - m_TexDelete.Height - 20,
                                                           m_TexDelete.Width,
                                                           m_TexDelete.Height);

                m_RectShaman = new Rectangle(10, renderContext.GraphicsDevice.Viewport.Height - m_TexShaman.Height - 20,
                                                           m_TexShaman.Width,
                                                           m_TexShaman.Height);

                m_RectSettlement = new Rectangle(10, renderContext.GraphicsDevice.Viewport.Height - m_TexSettlement.Height - 20,
                                                           m_TexSettlement.Width,
                                                           m_TexSettlement.Height);

                m_RectSchool = new Rectangle(10 + m_TexSettlement.Width + 10, renderContext.GraphicsDevice.Viewport.Height - m_TexSchool.Height - 20,
                                                           m_TexSchool.Width,
                                                           m_TexSchool.Height);

                m_RectShrine = new Rectangle(10 * 3 + m_TexShrine.Width * 2, renderContext.GraphicsDevice.Viewport.Height - m_TexShrine.Height - 20,
                                                           m_TexShrine.Width,
                                                           m_TexShrine.Height);

                // HOVERING
                m_RectHoverVillager = new Rectangle(m_RectDelete.X, m_RectDelete.Y - m_TexHoverVillager.Height - 20,
                                                           m_TexHoverVillager.Width,
                                                           m_TexHoverVillager.Height);

                m_RectSettlementInfo = new Rectangle(m_RectDelete.X, m_RectDelete.Y - m_TexHoverVillager.Height - 20,
                                                           m_TexHoverVillager.Width,
                                                           m_TexHoverVillager.Height);
            }

            // MENU ONDERKANT DRAW
            renderContext.SpriteBatch.Draw(m_TexMenuBackground, m_RectMenuBackground, Color.White);

            switch (m_SubMenuSelected)
            {
                // --------------------------------------------
                // BASE MODE
                // --------------------------------------------
                case SubMenuSelected.BaseMode:
                    renderContext.SpriteBatch.Draw(m_TexDelete, m_RectDelete, Color.White);
                    break;

                // --------------------------------------------
                // VILLAGER MODE
                // --------------------------------------------
                case SubMenuSelected.VillagerMode:
                    if (m_bShowSettlementHover)
                    {
                        renderContext.SpriteBatch.Draw(m_TexSettlementHover, m_RectSettlement, Color.White);
                        renderContext.SpriteBatch.Draw(m_TexSettlementInfo, m_RectSettlementInfo, Color.White);
                    }
                    else
                        renderContext.SpriteBatch.Draw(m_TexSettlement, m_RectSettlement, Color.White);

                    renderContext.SpriteBatch.Draw(m_TexSchool, m_RectSchool, Color.White);
                    renderContext.SpriteBatch.Draw(m_TexShrine, m_RectShrine, Color.White);
                    break;

                // --------------------------------------------
                // SHAMAN MODE
                // --------------------------------------------
                case SubMenuSelected.ShamanMode:
                    m_RectBuildTile = new Rectangle(40, renderContext.GraphicsDevice.Viewport.Height - 80, m_TexBuildTile.Width, m_TexBuildTile.Height);
                    break;

                // --------------------------------------------
                // SOLDIER MODE
                // --------------------------------------------
                case SubMenuSelected.SoldierMode:
                    
                    break;

                // --------------------------------------------
                // SHRINE MODE
                // --------------------------------------------
                case SubMenuSelected.ShrineMode:
                    if (m_bShowShamanHover)
                        renderContext.SpriteBatch.Draw(m_TexShamanHover, m_RectShaman, Color.White);
                    else
                        renderContext.SpriteBatch.Draw(m_TexShaman, m_RectShaman, Color.White);
                    break;
                // --------------------------------------------
                // SETTLEMENT MODE
                // --------------------------------------------
                case SubMenuSelected.SettlementMode:
                    // HOVERING
                    if (m_bShowVillagerHover)
                    {
                        renderContext.SpriteBatch.Draw(m_TexHoverVillager, m_RectHoverVillager, Color.White);
                        renderContext.SpriteBatch.Draw(m_TexVillagerHover, m_RectVillager, Color.White);
                    }
                    else
                        renderContext.SpriteBatch.Draw(m_TexVillager, m_RectVillager, Color.White);
                    break;

                case SubMenuSelected.SchoolMode:
                    
                    break;
            }

            // DRAW EXTRA INFORMATION (RESOURCES,...) IN TEXT
            // resources
            if (renderContext.GraphicsDevice.Viewport.Height <
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height)
            {
                renderContext.SpriteBatch.DrawString(m_DebugFont,
                                                     "" + userPlayer.GetResources().GetAllResources().ElementAt(0),
                                                     new Vector2(
                                                         renderContext.GraphicsDevice.Viewport.Width / 2 - 45, 8),
                                                     Color.White);
                renderContext.SpriteBatch.DrawString(m_DebugFont,
                                                     "" + userPlayer.GetResources().GetAllResources().ElementAt(1),
                                                     new Vector2(
                                                         renderContext.GraphicsDevice.Viewport.Width / 2 + 90, 8),
                                                     Color.White);
            }
            else
            {
                renderContext.SpriteBatch.DrawString(m_DebugFont,
                                                     "" + userPlayer.GetResources().GetAllResources().ElementAt(0),
                                                     new Vector2(
                                                         renderContext.GraphicsDevice.Viewport.Width / 2 - 90, 20),
                                                     Color.White);
                renderContext.SpriteBatch.DrawString(m_DebugFont,
                                                     "" + userPlayer.GetResources().GetAllResources().ElementAt(1),
                                                     new Vector2(
                                                         renderContext.GraphicsDevice.Viewport.Width / 2 + 180, 20),
                                                     Color.White);
            }

            
           /* renderContext.SpriteBatch.DrawString(m_DebugFont, "Influence Points: " + userPlayer.GetResources().GetAllResources().ElementAt(1), new Vector2(renderContext.GraphicsDevice.Viewport.Width - 200, 30), Color.White);*/

            // RESOURCE STATS
            renderContext.SpriteBatch.Draw(m_TexWoodResource, m_RectWoodResource, Color.White);
            renderContext.SpriteBatch.Draw(m_TexInfluenceResource, m_RectInfluenceResource, Color.White);

            // CHARACTER STATS
            renderContext.SpriteBatch.Draw(m_TexCharacterStats, m_RectCharacterStats, Color.White);
            renderContext.SpriteBatch.Draw(m_TexUnitList, m_RectUnitList, Color.White);

            // ARMY STATS
            // renderContext.SpriteBatch.DrawString(m_DebugFont, "Army Size: " + m_Player.GetArmySize(), new Vector2(renderContext.GraphicsDevice.Viewport.Width / 2 - 25, 10), Color.White);
        }

        private bool CheckHitButton(Vector2 mousePos, Rectangle buttonRect)
        {
            if ((mousePos.X > buttonRect.X && mousePos.X <= buttonRect.X + buttonRect.Width) &&
                (mousePos.Y > buttonRect.Y && mousePos.Y <= buttonRect.Y + buttonRect.Height))
            {
                return true;
            }

            return false;
        }

        public ModeSelected GetSelectedMode()
        {
            return m_SelectedMode;
        }

        public void SetSelectedMode(ModeSelected mode)
        {
            m_SelectedMode = mode;
        }

        public void NextTileType()
        {
            ++m_TileTypeSelected;
            if ((int)m_TileTypeSelected >= (int)GridTile.TileType.enumSize) m_TileTypeSelected = 0;
        }

        public SubMenuSelected SubMenu
        {
            get { return m_SubMenuSelected; }
            set { m_SubMenuSelected = value; }
        }

        public GridTile.TileType TileTypeSelected
        {
            get { return m_TileTypeSelected; }
            set { m_TileTypeSelected = value; }
        }
    }
}
