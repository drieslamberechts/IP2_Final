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
using XNA_ENGINE.Game.Helpers;
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
                                   m_TexDeleteHover,
                                   m_TexCharacterStats,
                                   m_TexHoverVillager,
                                   m_TexVillager,
                                   m_TexVillagerHover,
                                   m_TexShaman,
                                   m_TexShamanHover,
                                   m_TexShamanInfo,
                                   m_TexUnitList,
                                   m_TexSettlement,
                                   m_TexSettlementHover,
                                   m_TexSettlementInfo,
                                   m_TexSchool,
                                   m_TexSchoolHover,
                                   m_TexSchoolInfo,
                                   m_TexShrine,
                                   m_TexShrineInfo,
                                   m_TexBuildTile,
                                   m_TexSplit;

        private Rectangle m_RectMenuBackground,
                          m_RectWoodResource,
                          m_RectInfluenceResource,
                          m_RectDelete,
                          m_RectDeleteHover,
                          m_RectCharacterStats,
                          m_RectHoverVillager,
                          m_RectVillager,
                          m_RectShaman,
                          m_RectShamanInfo,
                          m_RectUnitList,
                          m_RectSettlement,
                          m_RectSettlementInfo,
                          m_RectSchool,
                          m_RectSchoolHover,
                          m_RectSchoolInfo,
                          m_RectShrine,
                          m_RectShrineInfo,
                          m_RectBuildTile,
                          m_RectSplit;

        // tutorial variables
        private readonly Texture2D m_TexScreen1,
                                    m_TexScreen2,
                                    m_TexScreen3,
                                    m_TexScreen4,
                                    m_TexScreen5,
                                    m_TexScreen6,
                                    m_TexScreen7,
                                    m_TexScreen8,
                                    m_TexScreen9,
                                    m_TexScreen10,
                                    m_TexScreen11;

        private Rectangle m_RectScreen1,
                          m_RectScreen2,
                          m_RectScreen3,
                          m_RectScreen4,
                          m_RectScreen5,
                          m_RectScreen6,
                          m_RectScreen7,
                          m_RectScreen8,
                          m_RectScreen9,
                          m_RectScreen10,
                          m_RectScreen11;

        public bool m_Enable1,
                     m_Enable2,
                     m_Enable3,
                     m_Enable4,
                     m_Enable5,
                     m_Enable6,
                     m_Enable7,
                     m_Enable8,
                     m_Enable9,
                     m_Enable10,
                     m_Enable11;

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

        // HOVERING
        private bool m_bShowVillagerHover,
                     m_bShowSettlementHover,
                     m_bShowShamanHover,
                     m_bShowShrineHover,
                     m_bShowSchoolHover;

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
            m_bShowShrineHover = false;
            m_bShowShamanHover = false;
            m_bShowSchoolHover = false;

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
            m_TexDeleteHover = Content.Load<Texture2D>("final Menu/Button_DeleteHover");
            m_TexSplit = Content.Load<Texture2D>("final Menu/iconStandard");
            m_TexVillager = Content.Load<Texture2D>("final Menu/Button_AddVillager");
            m_TexShaman = Content.Load<Texture2D>("final Menu/Button_AddShaman");
            m_TexVillagerHover = Content.Load<Texture2D>("final Menu/Button_AddVillagerHover");
            m_TexShamanHover = Content.Load<Texture2D>("final Menu/Button_AddShamanHover");

            // HOVERING
            m_TexShamanInfo = Content.Load<Texture2D>("final Menu/hoverShaman");
            m_TexHoverVillager = Content.Load<Texture2D>("final Menu/hoverVillager");
            m_TexSettlementInfo = Content.Load<Texture2D>("final Menu/hoverSettlementInfo");
            m_TexSchoolInfo = Content.Load<Texture2D>("final Menu/hoverSchool");
            m_TexShrineInfo = Content.Load<Texture2D>("final Menu/hoverSchool");

            // BUILDING ICONS
            m_TexSettlement = Content.Load<Texture2D>("final Menu/Button_AddSettlement");
            m_TexSettlementHover = Content.Load<Texture2D>("final Menu/Button_AddSettlementHover");
            m_TexSchool = Content.Load<Texture2D>("final Menu/Button_AddSchool");
            m_TexSchoolHover = Content.Load<Texture2D>("final Menu/Button_AddSchoolHover");
            m_TexShrine = Content.Load<Texture2D>("final Menu/iconStandard");

            m_TexBuildTile = Content.Load<Texture2D>("final Menu/iconStandard");

            // TUTORIAL
            m_Enable1 = true;
            m_Enable2 = false;
            m_Enable3 = false;
            m_Enable4 = false;
            m_Enable5 = false;
            m_Enable6 = false;
            m_Enable7 = false;
            m_Enable8 = false;
            m_Enable9 = false;
            m_Enable10 = false;
            m_Enable11 = false;
            
            m_TexScreen1 = Content.Load<Texture2D>("tutorial/screen1");
            m_TexScreen2 = Content.Load<Texture2D>("tutorial/screen2");
            m_TexScreen3 = Content.Load<Texture2D>("tutorial/screen3");
            m_TexScreen4 = Content.Load<Texture2D>("tutorial/screen4");
            m_TexScreen5 = Content.Load<Texture2D>("tutorial/screen5");
            m_TexScreen6 = Content.Load<Texture2D>("tutorial/screen6");
            m_TexScreen7 = Content.Load<Texture2D>("tutorial/screen7");
            m_TexScreen8 = Content.Load<Texture2D>("tutorial/screen8");
            m_TexScreen9 = Content.Load<Texture2D>("tutorial/screen9");
            m_TexScreen10 = Content.Load<Texture2D>("tutorial/screen10");
            m_TexScreen11 = Content.Load<Texture2D>("tutorial/screen11");

            // FONT
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

            // HOVER SHRINE BUTTON
            if (CheckHitButton(mousePos, m_RectShrine) && m_SubMenuSelected == SubMenuSelected.VillagerMode)
            {
                m_bShowShrineHover = true;
            }
            else m_bShowShrineHover = false;

            // HOVER SCHOOL BUTTON
            if (CheckHitButton(mousePos, m_RectSchool) && m_SubMenuSelected == SubMenuSelected.VillagerMode)
            {
                m_bShowSchoolHover = true;
            }
            else m_bShowSchoolHover = false;

            // HOVER SETTLEMENT BUTTON
            if (CheckHitButton(mousePos, m_RectSettlement) && m_SubMenuSelected == SubMenuSelected.VillagerMode)
            {
                m_bShowSettlementHover = true;
            }
            else m_bShowSettlementHover = false;

            //if (inputManager.GetAction((int)PlayScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectSettlement))
            switch (m_SubMenuSelected)
            {

                case SubMenuSelected.BaseMode:
                    if (inputManager.GetAction((int)PlayScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectDelete))
                    {
                        // DELETE ITEM THAT WILL BE SELECTED
                        userPlayer.GetResources().DecreaseWood(StandardCost.COSTOFWOOD_SETTLEMENT);
                        userPlayer.GetResources().DecreaseInfluence(StandardCost.COSTOFINFLUENCE_SETTLEMENT);
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
                        userPlayer.GetResources().DecreaseWood(StandardCost.COSTOFWOOD_SCHOOL);
                        userPlayer.GetResources().DecreaseInfluence(StandardCost.COSTOFINFLUENCE_SCHOOL);
                        GridFieldManager.GetInstance().SelectionModeMeth = GridFieldManager.SelectionMode.select2x2;
                        m_SelectedMode = ModeSelected.BuildSchool;
                        return true;
                    }

                    if (inputManager.GetAction((int)PlayScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectSettlement))
                    {
                        // SET DECREASE RESOURCES
                        userPlayer.GetResources().DecreaseWood(StandardCost.COSTOFWOOD_SETTLEMENT);
                        userPlayer.GetResources().DecreaseInfluence(StandardCost.COSTOFINFLUENCE_SETTLEMENT);
                        GridFieldManager.GetInstance().SelectionModeMeth = GridFieldManager.SelectionMode.select2x2;
                        m_SelectedMode = ModeSelected.BuildSettlement;
                        return true;
                    }

                    if (inputManager.GetAction((int)PlayScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectShrine))
                    {
                        // SET DECREASE RESOURCES
                        userPlayer.GetResources().DecreaseWood(StandardCost.COSTOFWOOD_SHRINE);
                        userPlayer.GetResources().DecreaseInfluence(StandardCost.COSTOFINFLUENCE_SHRINE);
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
                            // Actually build Shaman
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
                        userPlayer.GetResources().DecreaseWood(StandardCost.COSTOFWOOD_TILE1);
                        userPlayer.GetResources().DecreaseInfluence(StandardCost.COSTOFINFLUENCE_TILE1);
                        GridFieldManager.GetInstance().SelectionModeMeth = GridFieldManager.SelectionMode.select1x1;
                        m_SelectedMode = ModeSelected.BuildTile1;

                        if (m_Enable10)
                        {
                            m_Enable10 = false;
                            m_Enable11 = true;
                        }
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

                        if (m_Enable3)
                        {
                            m_Enable3 = false;
                            m_Enable4 = true;
                        }

                        return true;
                    }
                    break;

                // --------------------------------------------
                // SCHOOL MODE
                // --------------------------------------------
                case SubMenuSelected.SchoolMode:
                    // do nothing
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (inputManager.GetAction((int)PlayScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectScreen11) && m_Enable11)
            {
                m_Enable11 = false;
            }

            return false;
        }

        // Draw
        public void Draw(RenderContext renderContext)
        {
            Player userPlayer = GridFieldManager.GetInstance().UserPlayer;

            // --------------------------------------------
            // TUTORIAL
            // --------------------------------------------
            // windowed
            if (renderContext.GraphicsDevice.Viewport.Height < GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height)
            {
                if (m_Enable1)
                    m_RectScreen1 = new Rectangle(5, 5, m_TexScreen1.Width/2, m_TexScreen1.Height/2);
                if (m_Enable2)
                    m_RectScreen2 = new Rectangle(5, 5, m_TexScreen2.Width/2, m_TexScreen2.Height/2);
                if (m_Enable3)
                    m_RectScreen3 = new Rectangle(5, 5, m_TexScreen3.Width/2, m_TexScreen3.Height/2);
                if (m_Enable4)
                    m_RectScreen4 = new Rectangle(5, 5, m_TexScreen4.Width/2, m_TexScreen4.Height/2);
                if (m_Enable5)
                    m_RectScreen5 = new Rectangle(5, 5, m_TexScreen5.Width/2, m_TexScreen5.Height/2);
                if (m_Enable6)
                    m_RectScreen6 = new Rectangle(5, 5, m_TexScreen6.Width/2, m_TexScreen6.Height/2);
                if (m_Enable7)
                    m_RectScreen7 = new Rectangle(5, 5, m_TexScreen7.Width/2, m_TexScreen7.Height/2);
                if (m_Enable8)
                    m_RectScreen8 = new Rectangle(5, 5, m_TexScreen8.Width/2, m_TexScreen8.Height/2);
                if (m_Enable9)
                    m_RectScreen9 = new Rectangle(5, 5, m_TexScreen9.Width/2, m_TexScreen9.Height/2);
                if (m_Enable10)
                    m_RectScreen10 = new Rectangle(5, 5, m_TexScreen10.Width/2, m_TexScreen10.Height/2);
                if (m_Enable11)
                    m_RectScreen11 = new Rectangle(5, 5, m_TexScreen11.Width/2, m_TexScreen11.Height/2);
            }
                // fullscreen
            else
            {
                if (m_Enable1)
                    m_RectScreen1 = new Rectangle(10, 10, m_TexScreen1.Width, m_TexScreen1.Height);
                if (m_Enable2)
                    m_RectScreen2 = new Rectangle(10, 10, m_TexScreen2.Width, m_TexScreen2.Height);
                if (m_Enable3)
                    m_RectScreen3 = new Rectangle(10, 10, m_TexScreen3.Width, m_TexScreen3.Height);
                if (m_Enable4)
                    m_RectScreen4 = new Rectangle(10, 10, m_TexScreen4.Width, m_TexScreen4.Height);
                if (m_Enable5)
                    m_RectScreen5 = new Rectangle(10, 10, m_TexScreen5.Width, m_TexScreen5.Height);
                if (m_Enable6)
                    m_RectScreen6 = new Rectangle(10, 10, m_TexScreen6.Width, m_TexScreen6.Height);
                if (m_Enable7)
                    m_RectScreen7 = new Rectangle(10, 10, m_TexScreen7.Width, m_TexScreen7.Height);
                if (m_Enable8)
                    m_RectScreen8 = new Rectangle(10, 10, m_TexScreen8.Width, m_TexScreen8.Height);
                if (m_Enable9)
                    m_RectScreen9 = new Rectangle(10, 10, m_TexScreen9.Width, m_TexScreen9.Height);
                if (m_Enable10)
                    m_RectScreen10 = new Rectangle(10, 10, m_TexScreen10.Width, m_TexScreen10.Height);
                if (m_Enable11)
                    m_RectScreen11 = new Rectangle(10, 10, m_TexScreen11.Width, m_TexScreen11.Height);
            }

            // ------------------------------------------
            // WINDOWED
            // ------------------------------------------
            if (renderContext.GraphicsDevice.Viewport.Height < GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height)
            {
                // MENU ONDERKANT RECTANGLES
                m_RectMenuBackground = new Rectangle(0,
                                                     renderContext.GraphicsDevice.Viewport.Height -
                                                     m_TexMenuBackground.Height/2, m_TexMenuBackground.Width/2,
                                                     m_TexMenuBackground.Height/2);

                // RESOURCE STATS RECTANGLES
                m_RectWoodResource =
                    new Rectangle(renderContext.GraphicsDevice.Viewport.Width/2 - m_TexWoodResource.Width/2, 5,
                                  m_TexWoodResource.Width/2, m_TexWoodResource.Height/2);

                m_RectInfluenceResource = new Rectangle(renderContext.GraphicsDevice.Viewport.Width/2 + 10, 5,
                                                        m_TexInfluenceResource.Width/2, m_TexInfluenceResource.Height/2);

                // CHARACTER STATS RECTANGLES
                m_RectCharacterStats =
                    new Rectangle(renderContext.GraphicsDevice.Viewport.Width - m_TexCharacterStats.Width/2, 0,
                                  m_TexCharacterStats.Width/2, m_TexCharacterStats.Height/2);

                m_RectUnitList =
                    new Rectangle(renderContext.GraphicsDevice.Viewport.Width - m_TexCharacterStats.Width/2 + 10, 10,
                                  m_TexUnitList.Width/2, m_TexUnitList.Height/2);

                // ICONS
                m_RectDelete = new Rectangle(10, renderContext.GraphicsDevice.Viewport.Height - m_TexDelete.Height + 45,
                                             m_TexDelete.Width/2, m_TexDelete.Height/2);

                m_RectDeleteHover = new Rectangle(10,
                                                  renderContext.GraphicsDevice.Viewport.Height - m_TexDeleteHover.Height +
                                                  45, m_TexDeleteHover.Width/2, m_TexDeleteHover.Height/2);

                m_RectVillager = new Rectangle(10,
                                               renderContext.GraphicsDevice.Viewport.Height - m_TexDelete.Height + 45,
                                               m_TexDelete.Width/2, m_TexDelete.Height/2);

                m_RectShaman = new Rectangle(10, renderContext.GraphicsDevice.Viewport.Height - m_TexShaman.Height + 45,
                                             m_TexShaman.Width/2, m_TexShaman.Height/2);

                m_RectSchool = new Rectangle(10, renderContext.GraphicsDevice.Viewport.Height - m_TexSchool.Height + 45,
                                             m_TexSchool.Width/2, m_TexSchool.Height/2);

                m_RectSchoolHover = new Rectangle(10,
                                                  renderContext.GraphicsDevice.Viewport.Height - m_TexSchool.Height + 45,
                                                  m_TexSchool.Width/2, m_TexSchool.Height/2);

                m_RectSchoolInfo = new Rectangle(m_RectSchoolInfo.X, m_RectDelete.Y - m_TexSchoolInfo.Height/2 - 10,
                                                 m_TexSchoolInfo.Width/2, m_TexSchoolInfo.Height/2);

                m_RectShrine = new Rectangle(10*2 + m_TexSchool.Width/2,
                                             renderContext.GraphicsDevice.Viewport.Height - m_TexShrine.Height + 45,
                                             m_TexShrine.Width/2, m_TexShrine.Height/2);

                m_RectSettlement = new Rectangle(10*3 + m_TexSchool.Width,
                                                 renderContext.GraphicsDevice.Viewport.Height - m_TexSettlement.Height +
                                                 45, m_TexSettlement.Width/2, m_TexSettlement.Height/2);

                m_RectSplit = new Rectangle(10, renderContext.GraphicsDevice.Viewport.Height - m_TexSplit.Height + 45,
                                            m_TexSplit.Width/2, m_TexSplit.Height/2);

                // HOVERING
                m_RectHoverVillager = new Rectangle(m_RectDelete.X, m_RectDelete.Y - m_TexHoverVillager.Height/2 - 10,
                                                    m_TexHoverVillager.Width/2, m_TexHoverVillager.Height/2);

                m_RectSettlementInfo = new Rectangle(m_RectDelete.X, m_RectDelete.Y - m_TexHoverVillager.Height/2 - 10,
                                                     m_TexHoverVillager.Width/2, m_TexHoverVillager.Height/2);

            }
                // ------------------------------------------
                // FULLSCREEN
                // ------------------------------------------
            else
            {
                // MENU ONDERKANT RECTANGLES
                m_RectMenuBackground = new Rectangle(0,
                                                     renderContext.GraphicsDevice.Viewport.Height -
                                                     m_TexMenuBackground.Height, m_TexMenuBackground.Width,
                                                     m_TexMenuBackground.Height);

                // RESOURCE STATS RECTANGLES
                m_RectWoodResource =
                    new Rectangle(renderContext.GraphicsDevice.Viewport.Width/2 - m_TexWoodResource.Width, 5,
                                  m_TexWoodResource.Width, m_TexWoodResource.Height);

                m_RectInfluenceResource = new Rectangle(renderContext.GraphicsDevice.Viewport.Width/2 + 10, 5,
                                                        m_TexInfluenceResource.Width, m_TexInfluenceResource.Height);

                // CHARACTER STATS RECTANGLES
                m_RectCharacterStats =
                    new Rectangle(renderContext.GraphicsDevice.Viewport.Width - m_TexCharacterStats.Width, 0,
                                  m_TexCharacterStats.Width, m_TexCharacterStats.Height);

                m_RectUnitList =
                    new Rectangle(renderContext.GraphicsDevice.Viewport.Width - m_TexCharacterStats.Width + 20, 20,
                                  m_TexUnitList.Width, m_TexUnitList.Height);

                // ICONS
                m_RectDelete = new Rectangle(10, renderContext.GraphicsDevice.Viewport.Height - m_TexDelete.Height - 20,
                                             m_TexDelete.Width, m_TexDelete.Height);

                m_RectDeleteHover = new Rectangle(10,
                                                  renderContext.GraphicsDevice.Viewport.Height - m_TexDelete.Height - 20,
                                                  m_TexDelete.Width, m_TexDelete.Height);

                m_RectVillager = new Rectangle(10,
                                               renderContext.GraphicsDevice.Viewport.Height - m_TexDelete.Height - 20,
                                               m_TexDelete.Width, m_TexDelete.Height);

                m_RectShaman = new Rectangle(10, renderContext.GraphicsDevice.Viewport.Height - m_TexShaman.Height - 20,
                                             m_TexShaman.Width, m_TexShaman.Height);

                m_RectSettlement = new Rectangle(10,
                                                 renderContext.GraphicsDevice.Viewport.Height - m_TexSettlement.Height -
                                                 20, m_TexSettlement.Width, m_TexSettlement.Height);

                m_RectSchool = new Rectangle(10 + m_TexSchoolHover.Width + 4,
                                             renderContext.GraphicsDevice.Viewport.Height - m_TexSchool.Height - 20,
                                             m_TexSchool.Width, m_TexSchool.Height);

                m_RectSchoolHover = new Rectangle(10 + m_TexSchoolHover.Width + 4,
                                                  renderContext.GraphicsDevice.Viewport.Height - m_TexSchoolHover.Height -
                                                  17, m_TexSchoolHover.Width, m_TexSchoolHover.Height);

                m_RectShrine = new Rectangle(10*3 + m_TexShrine.Width*2,
                                             renderContext.GraphicsDevice.Viewport.Height - m_TexShrine.Height - 20,
                                             m_TexShrine.Width, m_TexShrine.Height);

                m_RectBuildTile = new Rectangle(10,
                                                renderContext.GraphicsDevice.Viewport.Height - m_TexBuildTile.Height -
                                                20, m_TexBuildTile.Width, m_TexBuildTile.Height);

                // HOVERING
                m_RectHoverVillager = new Rectangle(m_RectDelete.X, m_RectDelete.Y - m_TexHoverVillager.Height - 20,
                                                    m_TexHoverVillager.Width, m_TexHoverVillager.Height);

                m_RectSettlementInfo = new Rectangle(m_RectDelete.X, m_RectDelete.Y - m_TexHoverVillager.Height - 20,
                                                     m_TexHoverVillager.Width, m_TexHoverVillager.Height);

                m_RectSchoolInfo = new Rectangle(m_RectDelete.X, m_RectDelete.Y - m_TexSchoolInfo.Height - 20,
                                                 m_TexSchoolInfo.Width, m_TexSchoolInfo.Height);

                m_RectShamanInfo = new Rectangle(m_RectDelete.X, m_RectDelete.Y - m_TexShamanInfo.Height - 20,
                                                 m_TexShamanInfo.Width, m_TexShamanInfo.Height);

                m_RectShrineInfo = new Rectangle(m_RectDelete.X, m_RectDelete.Y - m_TexShrineInfo.Height - 20,
                                                 m_TexShrineInfo.Width, m_TexShrineInfo.Height);
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

                    if (m_bShowSchoolHover)
                    {
                        renderContext.SpriteBatch.Draw(m_TexSchoolHover, m_RectSchoolHover, Color.White);
                        renderContext.SpriteBatch.Draw(m_TexSchoolInfo, m_RectSchoolInfo, Color.White);
                    }
                    else
                        renderContext.SpriteBatch.Draw(m_TexSchool, m_RectSchool, Color.White);

                    if (m_bShowShrineHover)
                    {
                        renderContext.SpriteBatch.Draw(m_TexShrine, m_RectShrine, Color.White);
                        renderContext.SpriteBatch.Draw(m_TexShrineInfo, m_RectShrineInfo, Color.White);
                    }
                    else
                        renderContext.SpriteBatch.Draw(m_TexShrine, m_RectShrine, Color.White);
                    break;

                    // --------------------------------------------
                    // SHAMAN MODE
                    // --------------------------------------------
                case SubMenuSelected.ShamanMode:
                    renderContext.SpriteBatch.Draw(m_TexBuildTile, m_RectBuildTile, Color.White);
                    break;

                    // --------------------------------------------
                    // SOLDIER MODE
                    // --------------------------------------------
                case SubMenuSelected.SoldierMode:
                    // Do nothing
                    break;

                    // --------------------------------------------
                    // SHRINE MODE
                    // --------------------------------------------
                case SubMenuSelected.ShrineMode:
                    if (m_bShowShamanHover)
                    {
                        renderContext.SpriteBatch.Draw(m_TexShamanHover, m_RectShaman, Color.White);
                        renderContext.SpriteBatch.Draw(m_TexShamanInfo, m_RectShamanInfo, Color.White);
                    }
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

            // DRAW TUTORIAL
            if (m_Enable1)
                renderContext.SpriteBatch.Draw(m_TexScreen1, m_RectScreen1, Color.White);
            if (m_Enable2)
                renderContext.SpriteBatch.Draw(m_TexScreen2, m_RectScreen2, Color.White);
            if (m_Enable3)
                renderContext.SpriteBatch.Draw(m_TexScreen3, m_RectScreen3, Color.White);
            if (m_Enable4)
                renderContext.SpriteBatch.Draw(m_TexScreen4, m_RectScreen4, Color.White);
            if (m_Enable5)
                renderContext.SpriteBatch.Draw(m_TexScreen5, m_RectScreen5, Color.White);
            if (m_Enable6)
                renderContext.SpriteBatch.Draw(m_TexScreen6, m_RectScreen6, Color.White);
            if (m_Enable7)
                renderContext.SpriteBatch.Draw(m_TexScreen7, m_RectScreen7, Color.White);
            if (m_Enable8)
                renderContext.SpriteBatch.Draw(m_TexScreen8, m_RectScreen8, Color.White);
            if (m_Enable9)
                renderContext.SpriteBatch.Draw(m_TexScreen9, m_RectScreen9, Color.White);
            if (m_Enable10)
                renderContext.SpriteBatch.Draw(m_TexScreen10, m_RectScreen10, Color.White);
            if (m_Enable11)
                renderContext.SpriteBatch.Draw(m_TexScreen11, m_RectScreen11, Color.White);

            // DRAW EXTRA INFORMATION (RESOURCES,...) IN TEXT
            // resources
            if (renderContext.GraphicsDevice.Viewport.Height < GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height)
            {
                renderContext.SpriteBatch.DrawString(m_DebugFont, "" + userPlayer.GetResources().GetAllResources().ElementAt(0), new Vector2(renderContext.GraphicsDevice.Viewport.Width/2 - 45, 8), Color.White);
                renderContext.SpriteBatch.DrawString(m_DebugFont, "" + userPlayer.GetResources().GetAllResources().ElementAt(1), new Vector2(renderContext.GraphicsDevice.Viewport.Width/2 + 90, 8), Color.White);
            }
            else
            {
                renderContext.SpriteBatch.DrawString(m_DebugFont, "" + userPlayer.GetResources().GetAllResources().ElementAt(0), new Vector2(renderContext.GraphicsDevice.Viewport.Width/2 - 90, 20), Color.White);
                renderContext.SpriteBatch.DrawString(m_DebugFont, "" + userPlayer.GetResources().GetAllResources().ElementAt(1), new Vector2(renderContext.GraphicsDevice.Viewport.Width/2 + 180, 20), Color.White);
            }

            // RESOURCE STATS
            renderContext.SpriteBatch.Draw(m_TexWoodResource, m_RectWoodResource, Color.White);
            renderContext.SpriteBatch.Draw(m_TexInfluenceResource, m_RectInfluenceResource, Color.White);

            // CHARACTER STATS
            renderContext.SpriteBatch.Draw(m_TexCharacterStats, m_RectCharacterStats, Color.White);
            renderContext.SpriteBatch.Draw(m_TexUnitList, m_RectUnitList, Color.White);

            // COUNT STATS
            if (renderContext.GraphicsDevice.Viewport.Height < GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height)
            {
                renderContext.SpriteBatch.DrawString(m_DebugFont, "" + GridFieldManager.GetInstance().UserPlayer.GetVillagerCount(), new Vector2(renderContext.GraphicsDevice.Viewport.Width - 50, 25), Color.White);
                renderContext.SpriteBatch.DrawString(m_DebugFont, "" + GridFieldManager.GetInstance().UserPlayer.GetArmySize(), new Vector2(renderContext.GraphicsDevice.Viewport.Width - 50, 65), Color.White);
                renderContext.SpriteBatch.DrawString(m_DebugFont, "" + GridFieldManager.GetInstance().UserPlayer.GetShamanCount(), new Vector2(renderContext.GraphicsDevice.Viewport.Width - 50, 105), Color.White);
            }
            else
            {
                renderContext.SpriteBatch.DrawString(m_DebugFont, "" + GridFieldManager.GetInstance().UserPlayer.GetVillagerCount(), new Vector2(renderContext.GraphicsDevice.Viewport.Width - 100, 50), Color.White);
                renderContext.SpriteBatch.DrawString(m_DebugFont, "" + GridFieldManager.GetInstance().UserPlayer.GetArmySize(), new Vector2(renderContext.GraphicsDevice.Viewport.Width - 100, 130), Color.White);
                renderContext.SpriteBatch.DrawString(m_DebugFont, "" + GridFieldManager.GetInstance().UserPlayer.GetShamanCount(), new Vector2(renderContext.GraphicsDevice.Viewport.Width - 100, 210), Color.White);
            }
        }

        private bool CheckHitButton(Vector2 mousePos, Rectangle buttonRect)
        {
            if ((mousePos.X > buttonRect.X && mousePos.X <= buttonRect.X + buttonRect.Width) && (mousePos.Y > buttonRect.Y && mousePos.Y <= buttonRect.Y + buttonRect.Height))
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

        public void NextTileType(bool previous = false)
        {
            if (!previous)
                ++m_TileTypeSelected;
            else
                --m_TileTypeSelected;
            
            if ((int)m_TileTypeSelected >= (int)GridTile.TileType.enumSize) m_TileTypeSelected = 0;
            if ((int)m_TileTypeSelected < 0) m_TileTypeSelected = GridTile.TileType.enumSize-1;
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