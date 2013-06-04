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

        private Placeable m_SelectedPlaceable;

        private readonly Texture2D m_TexMenuBackground,
                                   m_TexWoodResource,
                                   m_TexInfluenceResource,
                                   m_TexDelete,
                                   m_TexDeleteHover,
                                   m_TexCharacterStats,
                                   m_TexHoverVillager,
                                   m_TexVillager,
                                   m_TexVillagerHover,
                                   m_TexSoldierQueue,
                                   m_TexShamanQueue,
                                   m_TexVillagerQueue,
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
                                   m_TexShrineHover,
                                   m_TexShrineInfo,
                                   m_TexBuildTile,
                                   m_TexSplit,
                                   m_TexMenuExit,
                                   m_TexMenuExitHovered,
                                   m_TexMenuFullscreen,
                                   m_TexMenuWindowed,
                                   m_TexMenuFullscreenHovered,
                                   m_TexMenuWindowedHovered,
                                   m_TexMainMenu,
                                   m_TexMainMenuHovered,
                                   m_TexMenuRestart,
                                   m_TexMenuRestartHovered,
                                   m_TexMenuResume,
                                   m_TexMenuResumeHovered,
                                   m_TexMenuSave,
                                   m_TexMenuSaveHovered,
                                   m_TexMenuControls,
                                   m_TexMenuControlsHovered,
                                   m_TexPauseMenuBackground,
                                   m_TexSplitIcon,
                                   m_TexSplitIconHover;

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
                          m_RectSplit,
                          m_RectMenuExit,
                          m_RectMenuFullscreen,
                          m_RectMenuWindowed,
                          m_RectMenuControls,
                          m_RectMainMenu,
                          m_RectMenuRestart,
                          m_RectMenuResume,
                          m_RectMenuSave,
                          m_RectPauseMenuBackground;

        // Variables Controls Menu
        private bool m_bShowControls, m_bButtonReturnHovered, m_bShowControllerLayout, m_bShowKeyboardLayout;

        private Texture2D m_TexControlsBackground,
                          m_TexControlsKeyboard,
                          m_TexControlsController,
                          m_TexButtonReturn,
                          m_TexButtonReturnHovered,
                          m_TexButtonControllerSwitch,
                          m_TexButtonKeyboardSwitch;

        private Rectangle m_RectControlsBackground,
                          m_RectControlsKeyboard,
                          m_RectControlsController,
                          m_RectButtonReturn,
                          m_RectButtonControllerSwitch,
                          m_RectButtonKeyboardSwitch;

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
            Split,
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

        //In-game Menu
        private bool m_IngameMenu = false;

        // HOVERING
        private bool m_bShowVillagerHover,
                     m_bShowSettlementHover,
                     m_bShowShamanHover,
                     m_bShowShrineHover,
                     m_bShowSchoolHover,
                     m_bShowMenuFullscreenHover,
                     m_bShowMenuExitHover,
                     m_bShowMainMenuHover,
                     m_bShowMenuSaveHover,
                     m_bShowMenuResumeHover,
                     m_bShowMenuRestartHover,
                     m_bShowMenuControlsHover,
                     m_bShowSplitHover;

        private bool m_bDrawMenu;
        private bool m_bLayoutSwitch;

        // ATTACKING INFO
        private bool m_bAttackingScene;

        private Texture2D m_TexBackgroundVictory,
                          m_TexBackgroundDefeat;

        private Rectangle m_RectBackgroundVictory,
                          m_RectBackgroundDefeat;

        // VARIABLES WHEN USING A CONTROLLER
        private bool m_bResumeSelected, m_bExitSelected, m_bControlsSelected, m_bSaveSelected, m_bFullscreenWindowedSelected, m_bRestartSelected, m_bMainMenuSelected, m_bReturnSelected;
        private static GamePadState m_GamePadState;
        private bool m_bCanSelectNextButton;

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
            m_bShowMenuFullscreenHover = false;
            m_bShowMenuExitHover = false;
            m_bShowMenuSaveHover = false;
            m_bShowMenuResumeHover = false;
            m_bShowMenuRestartHover = false;
            m_bShowMenuControlsHover = false;
            m_bShowMainMenuHover = false;
            m_bShowSplitHover = false;

            m_bShowControls = false;
            m_bButtonReturnHovered = false;
            m_bShowControllerLayout = true;
            m_bShowKeyboardLayout = false;
            m_bLayoutSwitch = true;

            m_bAttackingScene = false;

            // CONTROLLER USAGE
            m_bResumeSelected = false;
            m_bExitSelected = false;
            m_bControlsSelected = false;
            m_bSaveSelected = true;
            m_bFullscreenWindowedSelected = false;
            m_bRestartSelected = false;
            m_bMainMenuSelected = false;
            m_bCanSelectNextButton = true;
            m_bReturnSelected = false;

            m_GamePadState = GamePad.GetState(PlayerIndex.One);

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
            m_TexVillager = Content.Load<Texture2D>("final Menu/Button_AddVillager");
            m_TexShaman = Content.Load<Texture2D>("final Menu/Button_AddShaman");
            m_TexVillagerHover = Content.Load<Texture2D>("final Menu/Button_AddVillagerHover");
            m_TexShamanHover = Content.Load<Texture2D>("final Menu/Button_AddShamanHover");
            m_TexSplitIcon = Content.Load<Texture2D>("final Menu/Button_SplitArmyUnhovered");
            m_TexSplitIconHover = Content.Load<Texture2D>("final Menu/Button_SplitArmyHovered"); 

            m_TexSoldierQueue = Content.Load<Texture2D>("final Menu/Information_NrUnitsWarrior");
            m_TexShamanQueue = Content.Load<Texture2D>("final Menu/Information_NrUnitsShaman");
            m_TexVillagerQueue = Content.Load<Texture2D>("final Menu/Information_NrUnitsVillager");

            // HOVERING
            m_TexShamanInfo = Content.Load<Texture2D>("final Menu/hoverShaman");
            m_TexHoverVillager = Content.Load<Texture2D>("final Menu/hoverVillager");
            m_TexSettlementInfo = Content.Load<Texture2D>("final Menu/hoverSettlementInfo");
            m_TexSchoolInfo = Content.Load<Texture2D>("final Menu/hoverSchool");
            m_TexShrineInfo = Content.Load<Texture2D>("final Menu/hoverShrine");

            // BUILDING ICONS
            m_TexSettlement = Content.Load<Texture2D>("final Menu/Button_AddSettlement");
            m_TexSettlementHover = Content.Load<Texture2D>("final Menu/Button_AddSettlementHover");
            m_TexSchool = Content.Load<Texture2D>("final Menu/Button_AddSchool");
            m_TexSchoolHover = Content.Load<Texture2D>("final Menu/Button_AddSchoolHover");
            m_TexShrine = Content.Load<Texture2D>("final Menu/Button_AddShrineUnhovered");
            m_TexShrineHover = Content.Load<Texture2D>("final Menu/Button_AddShrineHovered");

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

            // INGAME MENU
            m_bDrawMenu = false;

            m_TexMenuExit = Content.Load<Texture2D>("pause Menu/Button_ExitUnhovered");
            m_TexMenuFullscreen = Content.Load<Texture2D>("pause Menu/Button_FullscreenUnhovered");
            m_TexMenuWindowed = Content.Load<Texture2D>("pause Menu/Button_WindowedUnhovered");
            m_TexMainMenu = Content.Load<Texture2D>("pause Menu/Button_MainMenuUnhovered");
            m_TexMenuRestart = Content.Load<Texture2D>("pause Menu/Button_RestartUnhovered");
            m_TexMenuResume = Content.Load<Texture2D>("pause Menu/Button_ResumeUnhovered");
            m_TexMenuSave = Content.Load<Texture2D>("pause Menu/Button_SaveUnhovered");
            m_TexMenuFullscreenHovered = Content.Load<Texture2D>("pause Menu/Button_FullscreenHovered");
            m_TexMenuWindowedHovered = Content.Load<Texture2D>("pause Menu/Button_WindowedHovered");
            m_TexMainMenuHovered = Content.Load<Texture2D>("pause Menu/Button_MainMenuHovered");
            m_TexMenuRestartHovered = Content.Load<Texture2D>("pause Menu/Button_RestartHovered");
            m_TexMenuResumeHovered = Content.Load<Texture2D>("pause Menu/Button_ResumeHovered");
            m_TexMenuSaveHovered = Content.Load<Texture2D>("pause Menu/Button_SaveHovered");
            m_TexMenuExitHovered = Content.Load<Texture2D>("pause Menu/Button_ExitHovered");
            m_TexPauseMenuBackground = Content.Load<Texture2D>("pause Menu/Background_GamePause");
            m_TexMenuControls = Content.Load<Texture2D>("pause Menu/Button_ControlsUnhovered");
            m_TexMenuControlsHovered = Content.Load<Texture2D>("pause Menu/Button_ControlsHovered");

            // CONTROLLER MENU
            m_TexControlsBackground = Content.Load<Texture2D>("ControllerMenu/Controller_Background");
            m_TexControlsKeyboard = Content.Load<Texture2D>("ControllerMenu/Controller_MouseAndKeyboard");
            m_TexControlsController = Content.Load<Texture2D>("ControllerMenu/Controller_xboxController");
            m_TexButtonReturn = Content.Load<Texture2D>("ControllerMenu/Controller_ReturnButtonUnhovered");
            m_TexButtonReturnHovered = Content.Load<Texture2D>("ControllerMenu/Controller_ReturnButtonHovered");
            m_TexButtonControllerSwitch = Content.Load<Texture2D>("ControllerMenu/Controller_xboxControllerButton");
            m_TexButtonKeyboardSwitch = Content.Load<Texture2D>("ControllerMenu/Controller_MouseAndKeyboardButton");

            // ATTAKING
            m_TexBackgroundVictory = Content.Load<Texture2D>("VictoryScreen");
            m_TexBackgroundDefeat = Content.Load<Texture2D>("DefeatScreen");

            // FONT
            m_DebugFont = Content.Load<SpriteFont>("Fonts/DebugFont");
        }

        public void SetFont(SpriteFont font)
        {
            m_DebugFont = font;
        }

        public void Update(RenderContext renderContext)
        {
            m_SelectedPlaceable = GridFieldManager.GetInstance().GetPermanentSelected();

            if (m_SelectedMode == ModeSelected.None && GridFieldManager.GetInstance().CreativeMode == false)
                GridFieldManager.GetInstance().SelectionModeMeth = GridFieldManager.SelectionMode.select1x1;

            // CONTROLLER USAGE
            if (m_GamePadState.IsConnected)
            {
                if (m_bSaveSelected)
                {
                    m_bShowMenuSaveHover = true;
                    m_bShowMenuResumeHover = false;
                    m_bShowMenuRestartHover = false;
                    m_bShowMenuControlsHover = false;
                    m_bShowMenuExitHover = false;
                    m_bShowMainMenuHover = false;
                    m_bShowMenuFullscreenHover = false;
                }
                if (m_bResumeSelected)
                {
                    m_bShowMenuSaveHover = false;
                    m_bShowMenuResumeHover = true;
                    m_bShowMenuRestartHover = false;
                    m_bShowMenuControlsHover = false;
                    m_bShowMenuExitHover = false;
                    m_bShowMainMenuHover = false;
                    m_bShowMenuFullscreenHover = false;
                }
                if (m_bRestartSelected)
                {
                    m_bShowMenuSaveHover = false;
                    m_bShowMenuResumeHover = false;
                    m_bShowMenuRestartHover = true;
                    m_bShowMenuControlsHover = false;
                    m_bShowMenuExitHover = false;
                    m_bShowMainMenuHover = false;
                    m_bShowMenuFullscreenHover = false;
                }
                if (m_bMainMenuSelected)
                {
                    m_bShowMenuSaveHover = false;
                    m_bShowMenuResumeHover = false;
                    m_bShowMenuRestartHover = false;
                    m_bShowMenuControlsHover = false;
                    m_bShowMenuExitHover = false;
                    m_bShowMainMenuHover = true;
                    m_bShowMenuFullscreenHover = false;
                }
                if (m_bFullscreenWindowedSelected)
                {
                    m_bShowMenuSaveHover = false;
                    m_bShowMenuResumeHover = false;
                    m_bShowMenuRestartHover = false;
                    m_bShowMenuControlsHover = false;
                    m_bShowMenuExitHover = false;
                    m_bShowMainMenuHover = false;
                    m_bShowMenuFullscreenHover = true;
                }
                if (m_bControlsSelected)
                {
                    m_bShowMenuSaveHover = false;
                    m_bShowMenuResumeHover = false;
                    m_bShowMenuRestartHover = false;
                    m_bShowMenuControlsHover = true;
                    m_bShowMenuExitHover = false;
                    m_bShowMainMenuHover = false;
                    m_bShowMenuFullscreenHover = false;
                }
                if (m_bExitSelected)
                {
                    m_bShowMenuSaveHover = false;
                    m_bShowMenuResumeHover = false;
                    m_bShowMenuRestartHover = false;
                    m_bShowMenuControlsHover = false;
                    m_bShowMenuExitHover = true;
                    m_bShowMainMenuHover = false;
                    m_bShowMenuFullscreenHover = false;
                }

                if (m_bShowControls)
                    m_bButtonReturnHovered = true;
            }
        }

        public bool HandleInput(RenderContext renderContext)
        {
            Player userPlayer = GridFieldManager.GetInstance().UserPlayer;

            var mousePos = new Vector2(renderContext.Input.CurrentMouseState.X, renderContext.Input.CurrentMouseState.Y);
            var inputManager = PlayScene.GetInputManager();
            
            //if (inputManager.GetAction((int)PlayScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectSwitch))

            // HOVER VILLAGER BUTTON
            if (CheckHitButton(mousePos, m_RectVillager) && m_SubMenuSelected == SubMenuSelected.SettlementMode)
                m_bShowVillagerHover = true;
            else m_bShowVillagerHover = false;

            // HOVER SHAMAN BUTTON
            if (CheckHitButton(mousePos, m_RectShaman) && m_SubMenuSelected == SubMenuSelected.ShrineMode)
                m_bShowShamanHover = true;
            else m_bShowShamanHover = false;

            // HOVER SHRINE BUTTON
            if (CheckHitButton(mousePos, m_RectShrine) && m_SubMenuSelected == SubMenuSelected.VillagerMode)
                m_bShowShrineHover = true;
            else m_bShowShrineHover = false;

            // HOVER SCHOOL BUTTON
            if (CheckHitButton(mousePos, m_RectSchool) && m_SubMenuSelected == SubMenuSelected.VillagerMode)
                m_bShowSchoolHover = true;
            else m_bShowSchoolHover = false;

            // HOVER SETTLEMENT BUTTON
            if (CheckHitButton(mousePos, m_RectSettlement) && m_SubMenuSelected == SubMenuSelected.VillagerMode)
                m_bShowSettlementHover = true;
            else m_bShowSettlementHover = false;

            // HOVER INGAME MENU WITHOUT CONTROLLER
            if (!GamePad.GetState(PlayerIndex.One).IsConnected)
            {
                // exit
                if (CheckHitButton(mousePos, m_RectMenuExit) && m_bDrawMenu)
                    m_bShowMenuExitHover = true;
                else m_bShowMenuExitHover = false;

                // fullscreen / windowed
                if (CheckHitButton(mousePos, m_RectMenuFullscreen) && m_bDrawMenu)
                    m_bShowMenuFullscreenHover = true;
                else m_bShowMenuFullscreenHover = false;

                // save
                if (CheckHitButton(mousePos, m_RectMenuSave) && m_bDrawMenu)
                    m_bShowMenuSaveHover = true;
                else m_bShowMenuSaveHover = false;

                // restart
                if (CheckHitButton(mousePos, m_RectMenuRestart) && m_bDrawMenu)
                    m_bShowMenuRestartHover = true;
                else m_bShowMenuRestartHover = false;

                // main menu
                if (CheckHitButton(mousePos, m_RectMainMenu) && m_bDrawMenu)
                    m_bShowMainMenuHover = true;
                else m_bShowMainMenuHover = false;

                // controls
                if (CheckHitButton(mousePos, m_RectMenuControls) && m_bDrawMenu)
                    m_bShowMenuControlsHover = true;
                else m_bShowMenuControlsHover = false;

                // resume
                if (CheckHitButton(mousePos, m_RectMenuResume) && m_bDrawMenu)
                    m_bShowMenuResumeHover = true;
                else m_bShowMenuResumeHover = false;

                // CONTROLLER MENU
                // return
                if (CheckHitButton(mousePos, m_RectButtonReturn) && m_bShowControls)
                    m_bButtonReturnHovered = true;
                else m_bButtonReturnHovered = false;
            }

            // Split Icon
            if (CheckHitButton(mousePos, m_RectDelete) && m_SubMenuSelected == SubMenuSelected.SoldierMode)
                m_bShowSplitHover = true;
            else
                m_bShowSplitHover = false;

            // CONTROLLER USAGE
            if (m_GamePadState.IsConnected)
            {
                // GO DOWN
                if (GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed && m_bSaveSelected && m_bCanSelectNextButton)
                {
                    m_bCanSelectNextButton = false;
                    m_bSaveSelected = false;
                    m_bResumeSelected = true;
                    m_bRestartSelected = false;
                    m_bMainMenuSelected = false;
                    m_bFullscreenWindowedSelected = false;
                    m_bControlsSelected = false;
                    m_bExitSelected = false;
                }
                else if (GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed && m_bResumeSelected && m_bCanSelectNextButton)
                {
                    m_bCanSelectNextButton = false;
                    m_bSaveSelected = false;
                    m_bResumeSelected = false;
                    m_bRestartSelected = true;
                    m_bMainMenuSelected = false;
                    m_bFullscreenWindowedSelected = false;
                    m_bControlsSelected = false;
                    m_bExitSelected = false;
                }
                else if (GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed && m_bRestartSelected && m_bCanSelectNextButton)
                {
                    m_bCanSelectNextButton = false;
                    m_bSaveSelected = false;
                    m_bResumeSelected = false;
                    m_bRestartSelected = false;
                    m_bMainMenuSelected = true;
                    m_bFullscreenWindowedSelected = false;
                    m_bControlsSelected = false;
                    m_bExitSelected = false;
                }
                else if (GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed && m_bMainMenuSelected && m_bCanSelectNextButton)
                {
                    m_bCanSelectNextButton = false;
                    m_bSaveSelected = false;
                    m_bResumeSelected = false;
                    m_bRestartSelected = false;
                    m_bMainMenuSelected = false;
                    m_bFullscreenWindowedSelected = true;
                    m_bControlsSelected = false;
                    m_bExitSelected = false;
                }
                else if (GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed && m_bFullscreenWindowedSelected && m_bCanSelectNextButton)
                {
                    m_bCanSelectNextButton = false;
                    m_bSaveSelected = false;
                    m_bResumeSelected = false;
                    m_bRestartSelected = false;
                    m_bMainMenuSelected = false;
                    m_bFullscreenWindowedSelected = false;
                    m_bControlsSelected = true;
                    m_bExitSelected = false;
                }
                else if (GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Pressed && m_bControlsSelected && m_bCanSelectNextButton)
                {
                    m_bCanSelectNextButton = false;
                    m_bSaveSelected = false;
                    m_bResumeSelected = false;
                    m_bRestartSelected = false;
                    m_bMainMenuSelected = false;
                    m_bFullscreenWindowedSelected = false;
                    m_bControlsSelected = false;
                    m_bExitSelected = true;
                }

                    // GO UP
                else if (GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed && m_bExitSelected && m_bCanSelectNextButton)
                {
                    m_bCanSelectNextButton = false;
                    m_bSaveSelected = false;
                    m_bResumeSelected = false;
                    m_bRestartSelected = false;
                    m_bMainMenuSelected = false;
                    m_bFullscreenWindowedSelected = false;
                    m_bControlsSelected = true;
                    m_bExitSelected = false;
                }
                else if (GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed && m_bControlsSelected && m_bCanSelectNextButton)
                {
                    m_bCanSelectNextButton = false;
                    m_bSaveSelected = false;
                    m_bResumeSelected = false;
                    m_bRestartSelected = false;
                    m_bMainMenuSelected = false;
                    m_bFullscreenWindowedSelected = true;
                    m_bControlsSelected = false;
                    m_bExitSelected = false;
                }
                else if (GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed && m_bFullscreenWindowedSelected && m_bCanSelectNextButton)
                {
                    m_bCanSelectNextButton = false;
                    m_bSaveSelected = false;
                    m_bResumeSelected = false;
                    m_bRestartSelected = false;
                    m_bMainMenuSelected = true;
                    m_bFullscreenWindowedSelected = false;
                    m_bControlsSelected = false;
                    m_bExitSelected = false;
                }
                else if (GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed && m_bMainMenuSelected && m_bCanSelectNextButton)
                {
                    m_bCanSelectNextButton = false;
                    m_bSaveSelected = false;
                    m_bResumeSelected = false;
                    m_bRestartSelected = true;
                    m_bMainMenuSelected = false;
                    m_bFullscreenWindowedSelected = false;
                    m_bControlsSelected = false;
                    m_bExitSelected = false;
                }
                else if (GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed && m_bRestartSelected && m_bCanSelectNextButton)
                {
                    m_bCanSelectNextButton = false;
                    m_bSaveSelected = false;
                    m_bResumeSelected = true;
                    m_bRestartSelected = false;
                    m_bMainMenuSelected = false;
                    m_bFullscreenWindowedSelected = false;
                    m_bControlsSelected = false;
                    m_bExitSelected = false;
                }
                else if (GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Pressed && m_bResumeSelected && m_bCanSelectNextButton)
                {
                    m_bCanSelectNextButton = true;
                    m_bSaveSelected = true;
                    m_bResumeSelected = false;
                    m_bRestartSelected = false;
                    m_bMainMenuSelected = false;
                    m_bFullscreenWindowedSelected = false;
                    m_bControlsSelected = false;
                    m_bExitSelected = false;
                }

                if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Released && GamePad.GetState(PlayerIndex.One).DPad.Up == ButtonState.Released && GamePad.GetState(PlayerIndex.One).DPad.Down == ButtonState.Released && GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Released && GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Released)
                    m_bCanSelectNextButton = true;

                // SELECT A BUTTON
                if (!m_bShowControls)
                {
                    if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed && m_bSaveSelected)
                    {
                        // geen idee
                    }
                    else if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed && m_bResumeSelected)
                    {
                        m_bDrawMenu = false;
                    }
                    else if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed && m_bRestartSelected)
                    {
                        // geen idee
                    }
                    else if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed && m_bMainMenuSelected)
                    {
                        SceneManager.SetActiveScene("MainMenuScene");
                    }
                    else if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed && m_bFullscreenWindowedSelected)
                    {
                        if (renderContext.GraphicsDevice.Viewport.Height < renderContext.GraphicsDevice.Adapter.CurrentDisplayMode.Height)
                        {
                            MainGame.graphics.IsFullScreen = true;
                            MainGame.graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                            MainGame.graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                            MainGame.graphics.ApplyChanges();
                        }
                        else
                        {
                            MainGame.graphics.IsFullScreen = false;
                            MainGame.graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2;
                            MainGame.graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2;
                            MainGame.graphics.ApplyChanges();
                        }
                    }
                    else if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed && m_bControlsSelected)
                    {
                        m_bShowControls = true;
                        m_bCanSelectNextButton = false;
                    }
                    else if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed && m_bExitSelected)
                    {
                        SceneManager.MainGame.Exit();
                    }
                }

                // CONTROL LAYOUT
                if (m_bShowControls)
                {
                    m_bReturnSelected = true;

                    if (GamePad.GetState(PlayerIndex.One).Buttons.A == ButtonState.Pressed && m_bReturnSelected && m_bCanSelectNextButton)
                    {
                        m_bShowControls = false;
                        m_bCanSelectNextButton = false;
                    }

                    if (GamePad.GetState(PlayerIndex.One).DPad.Right == ButtonState.Pressed || GamePad.GetState(PlayerIndex.One).DPad.Left == ButtonState.Pressed && m_bCanSelectNextButton)
                    {
                        if (m_bShowKeyboardLayout)
                        {
                            m_bShowKeyboardLayout = false;
                            m_bShowControllerLayout = true;
                            m_bCanSelectNextButton = false;
                        }
                        else
                        {
                            m_bShowKeyboardLayout = true;
                            m_bShowControllerLayout = false;
                            m_bCanSelectNextButton = false;
                        }
                    }
                }
            }

            //if (inputManager.GetAction((int)PlayScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectSettlement))
            switch (m_SubMenuSelected)
            {
                case SubMenuSelected.BaseMode:
                    if (inputManager.GetAction((int)PlayScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectDelete) 
                        && userPlayer.GetResources().GetWood() >= StandardCost.COSTOFWOOD_SETTLEMENT
                        && userPlayer.GetResources().GetInfluence() >= StandardCost.COSTOFINFLUENCE_SETTLEMENT)
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
                    if (inputManager.GetAction((int)PlayScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectSchool)
                        && userPlayer.GetResources().GetWood() >= StandardCost.COSTOFWOOD_SCHOOL
                        && userPlayer.GetResources().GetInfluence() >= StandardCost.COSTOFINFLUENCE_SCHOOL)
                    {
                        // SET DECREASE RESOURCES
                        userPlayer.GetResources().DecreaseWood(StandardCost.COSTOFWOOD_SCHOOL);
                        userPlayer.GetResources().DecreaseInfluence(StandardCost.COSTOFINFLUENCE_SCHOOL);
                        GridFieldManager.GetInstance().SelectionModeMeth = GridFieldManager.SelectionMode.select2x2;
                        m_SelectedMode = ModeSelected.BuildSchool;
                        return true;
                    }

                    if (inputManager.GetAction((int)PlayScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectSettlement)
                        && userPlayer.GetResources().GetWood() >= StandardCost.COSTOFWOOD_SETTLEMENT
                        && userPlayer.GetResources().GetInfluence() >= StandardCost.COSTOFINFLUENCE_SETTLEMENT)
                    {
                        // SET DECREASE RESOURCES
                        userPlayer.GetResources().DecreaseWood(StandardCost.COSTOFWOOD_SETTLEMENT);
                        userPlayer.GetResources().DecreaseInfluence(StandardCost.COSTOFINFLUENCE_SETTLEMENT);
                        GridFieldManager.GetInstance().SelectionModeMeth = GridFieldManager.SelectionMode.select2x2;
                        m_SelectedMode = ModeSelected.BuildSettlement;
                        return true;
                    }

                    if (inputManager.GetAction((int)PlayScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectShrine)

                        && userPlayer.GetResources().GetWood() >= StandardCost.COSTOFWOOD_SHRINE
                        && userPlayer.GetResources().GetInfluence() >= StandardCost.COSTOFINFLUENCE_SHRINE)
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
                        if (m_SelectedPlaceable != null && m_SelectedPlaceable.PlaceableTypeMeth == Placeable.PlaceableType.Shrine)
                        {
                                Console.WriteLine("Build Shaman");
                                m_SelectedPlaceable.QueueShaman();
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
                    if (inputManager.GetAction((int)PlayScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectDelete))
                    {
                        Console.WriteLine("Split!");
                        


                        m_SelectedMode = ModeSelected.Split;
                        return true;
                    }
                    break;

                // --------------------------------------------
                // SETTLEMENT MODE
                // --------------------------------------------
                case SubMenuSelected.SettlementMode:
                    if (inputManager.GetAction((int)PlayScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectVillager))
                    {
                        if (m_SelectedPlaceable != null && m_SelectedPlaceable.PlaceableTypeMeth == Placeable.PlaceableType.Settlement)
                        {
                            m_SelectedPlaceable.QueueVillager();
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

            // INGAME MENU INPUT
            if (inputManager.GetAction((int)PlayScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectMenuFullscreen) && m_bDrawMenu)
            {
                if (renderContext.GraphicsDevice.Viewport.Height < renderContext.GraphicsDevice.Adapter.CurrentDisplayMode.Height)
                {
                    MainGame.graphics.IsFullScreen = true;
                    MainGame.graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                    MainGame.graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                    MainGame.graphics.ApplyChanges();
                }
                else
                {
                    MainGame.graphics.IsFullScreen = false;
                    MainGame.graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2;
                    MainGame.graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2;
                    MainGame.graphics.ApplyChanges();
                }
            }

            if (inputManager.GetAction((int)PlayScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectMenuExit) && m_bDrawMenu)
                SceneManager.MainGame.Exit();

            if (inputManager.GetAction((int)PlayScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectMainMenu) && m_bDrawMenu)
                // Switchen doet iets raar met de game
                SceneManager.SetActiveScene("MainMenuScene");

            if (inputManager.GetAction((int)PlayScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectMenuResume) && m_bDrawMenu)
               m_bDrawMenu = false;

            if (inputManager.GetAction((int)PlayScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectMenuControls) && m_bDrawMenu)
                m_bShowControls = true;

            if (inputManager.GetAction((int)PlayScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectButtonReturn) && m_bShowControls)
                m_bShowControls = false;

            if (inputManager.GetAction((int) PlayScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectMenuSave) && m_bDrawMenu)
                MapLoadSave.GetInstance().SaveMap(GridFieldManager.GetInstance().GridField);

            if (inputManager.GetAction((int)PlayScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectButtonControllerSwitch) && m_bShowControls && m_bShowControllerLayout && m_bLayoutSwitch)
            {
                m_bShowControllerLayout = false;
                m_bShowKeyboardLayout = true;
                m_bLayoutSwitch = false;
            }

            if (inputManager.GetAction((int)PlayScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectButtonKeyboardSwitch) && m_bShowControls && m_bShowKeyboardLayout && m_bLayoutSwitch)
            {
                m_bShowKeyboardLayout = false;
                m_bShowControllerLayout = true;
                m_bLayoutSwitch = false;
            }

            m_bLayoutSwitch = true;

            if (m_bAttackingScene)
            {
                if (inputManager.GetAction((int)PlayScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectBackgroundVictory))
                {
                    m_bAttackingScene = false;
                }
            }


            return false;
        }

        // Draw
        public void Draw(RenderContext renderContext)
        {
            Player userPlayer = GridFieldManager.GetInstance().UserPlayer;
            var vp = renderContext.GraphicsDevice.Viewport;
            int vpHeight = vp.Height;
            int vpWidth = vp.Width;
            SpriteBatch spriteBatch = renderContext.SpriteBatch;

            // --------------------------------------------
            // TUTORIAL
            // --------------------------------------------
            // windowed
            if (vpHeight < GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height)
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
            if (vpHeight < GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height)
            {
                // MENU ONDERKANT RECTANGLES
                m_RectMenuBackground = new Rectangle(0,vpHeight -m_TexMenuBackground.Height/2, m_TexMenuBackground.Width/2,m_TexMenuBackground.Height/2);

                // RESOURCE STATS RECTANGLES
                m_RectWoodResource = new Rectangle(renderContext.GraphicsDevice.Viewport.Width/2 - m_TexWoodResource.Width/2, 5, m_TexWoodResource.Width/2, m_TexWoodResource.Height/2);

                m_RectInfluenceResource = new Rectangle(renderContext.GraphicsDevice.Viewport.Width/2 + 10, 5, m_TexInfluenceResource.Width/2, m_TexInfluenceResource.Height/2);

                // CHARACTER STATS RECTANGLES
                m_RectCharacterStats =new Rectangle(renderContext.GraphicsDevice.Viewport.Width - m_TexCharacterStats.Width/2, 0, m_TexCharacterStats.Width/2, m_TexCharacterStats.Height/2);
                m_RectUnitList =new Rectangle(renderContext.GraphicsDevice.Viewport.Width - m_TexCharacterStats.Width/2 + 10, 10, m_TexUnitList.Width/2, m_TexUnitList.Height/2);

                // ICONS
                m_RectDelete        = new Rectangle(10, vpHeight - m_TexDelete.Height + 45,m_TexDelete.Width/2, m_TexDelete.Height/2);
                m_RectDeleteHover   = new Rectangle(10, vpHeight - m_TexDeleteHover.Height +45, m_TexDeleteHover.Width/2, m_TexDeleteHover.Height/2);
                m_RectVillager      = new Rectangle(10, vpHeight - m_TexDelete.Height + 45,m_TexDelete.Width/2, m_TexDelete.Height/2);
                m_RectShaman        = new Rectangle(10, vpHeight - m_TexShaman.Height + 45, m_TexShaman.Width/2, m_TexShaman.Height/2);
                m_RectSchool        = new Rectangle(10, vpHeight - m_TexSchool.Height + 45,m_TexSchool.Width/2, m_TexSchool.Height/2);
                m_RectSchoolHover   = new Rectangle(10, vpHeight - m_TexSchool.Height + 45,m_TexSchool.Width/2, m_TexSchool.Height/2);
                m_RectSchoolInfo    = new Rectangle(m_RectSchoolInfo.X, m_RectDelete.Y - m_TexSchoolInfo.Height/2 - 10, m_TexSchoolInfo.Width/2, m_TexSchoolInfo.Height/2);
                m_RectShrine        = new Rectangle(10*2 + m_TexSchool.Width/2, vpHeight - m_TexShrine.Height + 45,m_TexShrine.Width/2, m_TexShrine.Height/2);
                m_RectSettlement    = new Rectangle(10*3 + m_TexSchool.Width, vpHeight - m_TexSettlement.Height +45, m_TexSettlement.Width/2, m_TexSettlement.Height/2);
                m_RectSplit         = new Rectangle(10, vpHeight - m_TexSplit.Height + 45, m_TexSplit.Width/2, m_TexSplit.Height/2);


                // HOVERING
                m_RectHoverVillager  = new Rectangle(m_RectDelete.X, m_RectDelete.Y - m_TexHoverVillager.Height/2 - 10,m_TexHoverVillager.Width/2, m_TexHoverVillager.Height/2);
                m_RectSettlementInfo = new Rectangle(m_RectDelete.X, m_RectDelete.Y - m_TexHoverVillager.Height/2 - 10, m_TexHoverVillager.Width/2, m_TexHoverVillager.Height/2);
                m_RectShrineInfo = new Rectangle(m_RectDelete.X, m_RectDelete.Y - m_TexShrineInfo.Height / 2 - 10, m_TexShrineInfo.Width / 2, m_TexShrineInfo.Height / 2);

                // INGAME MENU
                m_RectMenuControls      = new Rectangle(vpWidth / 2 - m_TexMenuExit.Width / 8, 175 + m_TexMenuExit.Height / 4 * 5, m_TexMenuExit.Width / 4, m_TexMenuExit.Height / 4);
                m_RectMenuExit          = new Rectangle(vpWidth / 2 - m_TexMenuExit.Width / 8, 175 + m_TexMenuExit.Height/ 4 * 6, m_TexMenuExit.Width / 4, m_TexMenuExit.Height / 4);
                m_RectMenuFullscreen    = new Rectangle(vpWidth / 2 - m_TexMenuExit.Width / 8, 175 + m_TexMenuExit.Height/ 4 * 4, m_TexMenuExit.Width / 4, m_TexMenuExit.Height / 4);
                m_RectMenuWindowed      = new Rectangle(vpWidth / 2 - m_TexMenuExit.Width / 8, 175 + m_TexMenuExit.Height/ 4 * 4, m_TexMenuExit.Width / 4, m_TexMenuExit.Height / 4);
                m_RectMainMenu          = new Rectangle(vpWidth / 2 - m_TexMenuExit.Width / 8, 175 + m_TexMenuExit.Height/ 4 * 3, m_TexMenuExit.Width / 4, m_TexMenuExit.Height / 4);
                m_RectMenuRestart       = new Rectangle(vpWidth / 2 - m_TexMenuExit.Width / 8, 175 + m_TexMenuExit.Height/ 4 * 2, m_TexMenuExit.Width / 4, m_TexMenuExit.Height / 4);
                m_RectMenuResume        = new Rectangle(vpWidth / 2 - m_TexMenuExit.Width / 8, 175 + m_TexMenuExit.Height/ 4, m_TexMenuExit.Width / 4, m_TexMenuExit.Height / 4);
                m_RectMenuSave          = new Rectangle(vpWidth / 2 - m_TexMenuExit.Width / 8, 175, m_TexMenuExit.Width / 4, m_TexMenuExit.Height / 4);
                m_RectPauseMenuBackground = new Rectangle(vpWidth / 2 - m_TexPauseMenuBackground.Width / 8, 152, m_TexPauseMenuBackground.Width / 4, m_TexPauseMenuBackground.Height / 4);

                // CONTROLLER MENU
                m_RectControlsBackground = new Rectangle(vpWidth / 2 - m_TexControlsBackground.Width / 4, 20, m_TexControlsBackground.Width / 2, m_TexControlsBackground.Height / 2);
                m_RectControlsKeyboard = new Rectangle(vpWidth / 2 - m_TexControlsKeyboard.Width / 4, 150, m_TexControlsKeyboard.Width / 2, m_TexControlsKeyboard.Height / 2);
                m_RectControlsController = new Rectangle(vpWidth / 2 - m_TexControlsController.Width / 4, 150, m_TexControlsController.Width / 2, m_TexControlsController.Height / 2);
                m_RectButtonReturn = new Rectangle(vpWidth / 2 - 300, 450, m_TexButtonReturn.Width / 2, m_TexButtonReturn.Height / 2);
                m_RectButtonControllerSwitch = new Rectangle(vpWidth / 2 - m_TexButtonControllerSwitch.Width / 4, 70, m_TexButtonControllerSwitch.Width / 2, m_TexButtonControllerSwitch.Height / 2);
                m_RectButtonKeyboardSwitch = new Rectangle(vpWidth / 2 - m_TexButtonKeyboardSwitch.Width / 4, 70, m_TexButtonKeyboardSwitch.Width / 2, m_TexButtonKeyboardSwitch.Height / 2);

                // ATTACKER
                m_RectBackgroundVictory = new Rectangle(vpWidth / 2 - m_TexBackgroundVictory.Width / 4, 0, m_TexBackgroundVictory.Width / 2, m_TexBackgroundVictory.Height / 2);
                m_RectBackgroundDefeat  = new Rectangle(vpWidth / 2 - m_TexBackgroundDefeat.Width / 4, 0, m_TexBackgroundDefeat.Width / 2, m_TexBackgroundDefeat.Height / 2);
            }
                // ------------------------------------------
                // FULLSCREEN
                // ------------------------------------------
            else
            {
                // MENU ONDERKANT RECTANGLES
                m_RectMenuBackground = new Rectangle(0,vpHeight - m_TexMenuBackground.Height, m_TexMenuBackground.Width, m_TexMenuBackground.Height);

                // RESOURCE STATS RECTANGLES
                m_RectWoodResource = new Rectangle(vpWidth / 2 - m_TexWoodResource.Width, 5, m_TexWoodResource.Width, m_TexWoodResource.Height);
                m_RectInfluenceResource = new Rectangle(vpWidth / 2 + 10, 5,m_TexInfluenceResource.Width, m_TexInfluenceResource.Height);

                // CHARACTER STATS RECTANGLES
                m_RectCharacterStats = new Rectangle(vpWidth - m_TexCharacterStats.Width, 0, m_TexCharacterStats.Width, m_TexCharacterStats.Height);
                m_RectUnitList = new Rectangle(vpWidth - m_TexCharacterStats.Width + 20, 20, m_TexUnitList.Width, m_TexUnitList.Height);

                // ICONS
                m_RectDelete = new Rectangle(10, vpHeight - m_TexDelete.Height - 20, m_TexDelete.Width, m_TexDelete.Height);
                m_RectDeleteHover = new Rectangle(10,vpHeight - m_TexDelete.Height - 20, m_TexDelete.Width, m_TexDelete.Height);
                m_RectVillager = new Rectangle(10,vpHeight - m_TexDelete.Height - 20,m_TexDelete.Width, m_TexDelete.Height);
                m_RectShaman = new Rectangle(10, vpHeight - m_TexShaman.Height - 20, m_TexShaman.Width, m_TexShaman.Height);
                m_RectSettlement = new Rectangle(10, vpHeight - m_TexSettlement.Height -20, m_TexSettlement.Width, m_TexSettlement.Height);
                m_RectSchool = new Rectangle(10 + m_TexSchoolHover.Width + 4,vpHeight - m_TexSchool.Height - 20, m_TexSchool.Width, m_TexSchool.Height);
                m_RectSchoolHover = new Rectangle(10 + m_TexSchoolHover.Width + 4,vpHeight - m_TexSchoolHover.Height -17, m_TexSchoolHover.Width, m_TexSchoolHover.Height);
                m_RectShrine = new Rectangle(10*3 + m_TexShrine.Width*2,vpHeight - m_TexShrine.Height - 20,m_TexShrine.Width, m_TexShrine.Height);
                m_RectBuildTile = new Rectangle(10, vpHeight - m_TexBuildTile.Height -20, m_TexBuildTile.Width, m_TexBuildTile.Height);

                // HOVERING
                
                m_RectHoverVillager = new Rectangle(m_RectDelete.X, m_RectDelete.Y - m_TexHoverVillager.Height - 20,m_TexHoverVillager.Width, m_TexHoverVillager.Height);
                m_RectSettlementInfo = new Rectangle(m_RectDelete.X, m_RectDelete.Y - m_TexHoverVillager.Height - 20, m_TexHoverVillager.Width, m_TexHoverVillager.Height);
                m_RectSchoolInfo = new Rectangle(m_RectDelete.X, m_RectDelete.Y - m_TexSchoolInfo.Height - 20, m_TexSchoolInfo.Width, m_TexSchoolInfo.Height);
                m_RectShamanInfo = new Rectangle(m_RectDelete.X, m_RectDelete.Y - m_TexShamanInfo.Height - 20,m_TexShamanInfo.Width, m_TexShamanInfo.Height);
                m_RectShrineInfo = new Rectangle(m_RectDelete.X, m_RectDelete.Y - m_TexShrineInfo.Height - 20, m_TexShrineInfo.Width, m_TexShrineInfo.Height);

                // INGAME MENU
                m_RectMenuControls      = new Rectangle(vpWidth / 2 - m_TexMenuExit.Width / 4, 350 + m_TexMenuExit.Height / 2 * 5 + 20, m_TexMenuExit.Width / 2, m_TexMenuExit.Height / 2);
                m_RectMenuExit          = new Rectangle(vpWidth / 2 - m_TexMenuExit.Width / 4, 350 + m_TexMenuExit.Height / 2 * 6 + 20   , m_TexMenuExit.Width / 2, m_TexMenuExit.Height / 2);
                m_RectMenuFullscreen    = new Rectangle(vpWidth / 2 - m_TexMenuExit.Width / 4, 350 + m_TexMenuExit.Height / 2 * 4 + 20   , m_TexMenuExit.Width / 2, m_TexMenuExit.Height / 2);
                m_RectMenuWindowed      = new Rectangle(vpWidth / 2 - m_TexMenuExit.Width / 4, 350 + m_TexMenuExit.Height / 2 * 4 + 20   , m_TexMenuExit.Width / 2, m_TexMenuExit.Height / 2);
                m_RectMainMenu          = new Rectangle(vpWidth / 2 - m_TexMenuExit.Width / 4, 350 + m_TexMenuExit.Height / 2 * 3 + 20   , m_TexMenuExit.Width / 2, m_TexMenuExit.Height / 2);
                m_RectMenuRestart       = new Rectangle(vpWidth / 2 - m_TexMenuExit.Width / 4, 350 + m_TexMenuExit.Height / 2 * 2 + 20   , m_TexMenuExit.Width / 2, m_TexMenuExit.Height / 2);
                m_RectMenuResume        = new Rectangle(vpWidth / 2 - m_TexMenuExit.Width / 4, 350 + m_TexMenuExit.Height / 2 + 20   , m_TexMenuExit.Width / 2, m_TexMenuExit.Height / 2);
                m_RectMenuSave          = new Rectangle(vpWidth / 2 - m_TexMenuExit.Width / 4, 350 + 20                                  , m_TexMenuExit.Width / 2, m_TexMenuExit.Height / 2);
                m_RectPauseMenuBackground = new Rectangle(vpWidth / 2 - m_TexPauseMenuBackground.Width / 4, 325, m_TexPauseMenuBackground.Width / 2, m_TexPauseMenuBackground.Height / 2);

                // CONTROLLER MENU
                m_RectControlsBackground        = new Rectangle(vpWidth / 2 - m_TexControlsBackground.Width / 2, 40, m_TexControlsBackground.Width, m_TexControlsBackground.Height);
                m_RectControlsKeyboard          = new Rectangle(vpWidth / 2 - m_TexControlsKeyboard.Width / 2, 300, m_TexControlsKeyboard.Width, m_TexControlsKeyboard.Height);
                m_RectControlsController        = new Rectangle(vpWidth / 2 - m_TexControlsController.Width / 2, 300, m_TexControlsController.Width, m_TexControlsController.Height);
                m_RectButtonReturn              = new Rectangle(vpWidth / 2 - 600, 900, m_TexButtonReturn.Width, m_TexButtonReturn.Height);
                m_RectButtonControllerSwitch    = new Rectangle(vpWidth / 2 - m_TexButtonControllerSwitch.Width / 2, 160, m_TexButtonControllerSwitch.Width, m_TexButtonControllerSwitch.Height);
                m_RectButtonKeyboardSwitch      = new Rectangle(vpWidth / 2 - m_TexButtonKeyboardSwitch.Width / 2, 160, m_TexButtonKeyboardSwitch.Width, m_TexButtonKeyboardSwitch.Height);

                // ATTACKER
                m_RectBackgroundVictory = new Rectangle(vpWidth / 2 - m_TexBackgroundVictory.Width / 2, 0, m_TexBackgroundVictory.Width, m_TexBackgroundVictory.Height);
                m_RectBackgroundDefeat  = new Rectangle(vpWidth / 2 - m_TexBackgroundDefeat.Width / 2, 0, m_TexBackgroundDefeat.Width, m_TexBackgroundDefeat.Height);
            }

            // MENU ONDERKANT DRAW
            renderContext.SpriteBatch.Draw(m_TexMenuBackground, m_RectMenuBackground, Color.White);

            switch (m_SubMenuSelected)
            {
                    // --------------------------------------------
                    // BASE MODE
                    // --------------------------------------------
                case SubMenuSelected.BaseMode:
                    spriteBatch.Draw(m_TexDelete, m_RectDelete, Color.White);
                    break;

                    // --------------------------------------------
                    // VILLAGER MODE
                    // --------------------------------------------
                case SubMenuSelected.VillagerMode:
                    if (m_bShowSettlementHover)
                    {
                        spriteBatch.Draw(m_TexSettlementHover, m_RectSettlement, Color.White);
                        spriteBatch.Draw(m_TexSettlementInfo, m_RectSettlementInfo, Color.White);
                    }
                    else
                        spriteBatch.Draw(m_TexSettlement, m_RectSettlement, Color.White);

                    if (m_bShowSchoolHover)
                    {
                        spriteBatch.Draw(m_TexSchoolHover, m_RectSchoolHover, Color.White);
                        spriteBatch.Draw(m_TexSchoolInfo, m_RectSchoolInfo, Color.White);
                    }
                    else
                        spriteBatch.Draw(m_TexSchool, m_RectSchool, Color.White);

                    if (m_bShowShrineHover)
                    {
                        spriteBatch.Draw(m_TexShrineHover, m_RectShrine, Color.White);
                        spriteBatch.Draw(m_TexShrineInfo, m_RectShrineInfo, Color.White);
                    }
                    else
                        spriteBatch.Draw(m_TexShrine, m_RectShrine, Color.White);
                    break;

                    // --------------------------------------------
                    // SHAMAN MODE
                    // --------------------------------------------
                case SubMenuSelected.ShamanMode:
                    spriteBatch.Draw(m_TexBuildTile, m_RectBuildTile, Color.White);
                    break;

                    // --------------------------------------------
                    // SOLDIER MODE
                    // --------------------------------------------
                case SubMenuSelected.SoldierMode:
                    if (m_bShowSplitHover)
                        spriteBatch.Draw(m_TexSplitIconHover, m_RectDelete, Color.White);
                    else 
                        spriteBatch.Draw(m_TexSplitIcon, m_RectDelete, Color.White);
                    break;

                    // --------------------------------------------
                    // SHRINE MODE
                    // --------------------------------------------
                case SubMenuSelected.ShrineMode:
                    if (m_bShowShamanHover)
                    {
                        spriteBatch.Draw(m_TexShamanHover, m_RectShaman, Color.White);
                        spriteBatch.Draw(m_TexShamanInfo, m_RectShamanInfo, Color.White);
                    }
                    else
                        spriteBatch.Draw(m_TexShaman, m_RectShaman, Color.White);

                    // SHOW QUEUE
                    if (m_SelectedPlaceable != null)
                    {
                        for (int t = 0; t < m_SelectedPlaceable.GetQueuedShaman(); ++t)
                        {
                            m_RectVillager = new Rectangle(265 + t * 50, 475, m_TexShamanQueue.Width / 2 + 10, m_TexShamanQueue.Height / 2 + 10);
                            spriteBatch.Draw(m_TexShamanQueue, m_RectVillager, Color.White);
                        }
                    }
                    break;
                    // --------------------------------------------
                    // SETTLEMENT MODE
                    // --------------------------------------------
                case SubMenuSelected.SettlementMode:
                    // HOVERING
                    if (m_bShowVillagerHover)
                    {
                        spriteBatch.Draw(m_TexHoverVillager, m_RectHoverVillager, Color.White);
                        spriteBatch.Draw(m_TexVillagerHover, m_RectVillager, Color.White);
                    }
                    else
                        spriteBatch.Draw(m_TexVillager, m_RectVillager, Color.White);

                    // SHOW QUEUE
                    if (m_SelectedPlaceable != null)
                    {
                        for (int t = 0; t < m_SelectedPlaceable.GetQueuedVillager(); ++t)
                        {
                            m_RectVillager = new Rectangle(265 + t * 50, 475, m_TexVillagerQueue.Width / 2 + 10, m_TexVillagerQueue.Height / 2 + 10);
                            spriteBatch.Draw(m_TexVillagerQueue, m_RectVillager, Color.White);
                        }
                    }
                    break;

                    // --------------------------------------------
                    // SCHOOL MODE
                    // --------------------------------------------
                case SubMenuSelected.SchoolMode:
                    // SHOW QUEUE
                    if (m_SelectedPlaceable != null)
                    {
                        for (int t = 0; t < m_SelectedPlaceable.GetQueuedSoldiers(); ++t)
                        {
                            m_RectVillager = new Rectangle(265 + t * 50, 475, m_TexSoldierQueue.Width / 2 + 10, m_TexSoldierQueue.Height / 2 + 10);
                            spriteBatch.Draw(m_TexSoldierQueue, m_RectVillager, Color.White);
                        }
                    }
                    break;
            }

            // DRAW TUTORIAL
            if (m_Enable1)
                spriteBatch.Draw(m_TexScreen1, m_RectScreen1, Color.White);
            if (m_Enable2)
                spriteBatch.Draw(m_TexScreen2, m_RectScreen2, Color.White);
            if (m_Enable3)
                spriteBatch.Draw(m_TexScreen3, m_RectScreen3, Color.White);
            if (m_Enable4)
                spriteBatch.Draw(m_TexScreen4, m_RectScreen4, Color.White);
            if (m_Enable5)
                spriteBatch.Draw(m_TexScreen5, m_RectScreen5, Color.White);
            if (m_Enable6)
                spriteBatch.Draw(m_TexScreen6, m_RectScreen6, Color.White);
            if (m_Enable7)
                spriteBatch.Draw(m_TexScreen7, m_RectScreen7, Color.White);
            if (m_Enable8)
                spriteBatch.Draw(m_TexScreen8, m_RectScreen8, Color.White);
            if (m_Enable9)
                spriteBatch.Draw(m_TexScreen9, m_RectScreen9, Color.White);
            if (m_Enable10)
                spriteBatch.Draw(m_TexScreen10, m_RectScreen10, Color.White);
            if (m_Enable11)
                spriteBatch.Draw(m_TexScreen11, m_RectScreen11, Color.White);

            // DRAW EXTRA INFORMATION (RESOURCES,...) IN TEXT
            // resources
            if (vpHeight < GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height)
            {
                spriteBatch.DrawString(m_DebugFont, "" + userPlayer.GetResources().GetAllResources().ElementAt(0), new Vector2(vpWidth / 2 - 45, 8), Color.White);
                spriteBatch.DrawString(m_DebugFont, "" + userPlayer.GetResources().GetAllResources().ElementAt(1), new Vector2(vpWidth / 2 + 90, 8), Color.White);
            }
            else
            {
                spriteBatch.DrawString(m_DebugFont, "" + userPlayer.GetResources().GetAllResources().ElementAt(0), new Vector2(vpWidth/2 - 90, 20), Color.White);
                spriteBatch.DrawString(m_DebugFont, "" + userPlayer.GetResources().GetAllResources().ElementAt(1), new Vector2(vpWidth / 2 + 180, 20), Color.White);
            }

            // RESOURCE STATS
            spriteBatch.Draw(m_TexWoodResource, m_RectWoodResource, Color.White);
            spriteBatch.Draw(m_TexInfluenceResource, m_RectInfluenceResource, Color.White);

            // CHARACTER STATS
            spriteBatch.Draw(m_TexCharacterStats, m_RectCharacterStats, Color.White);
            spriteBatch.Draw(m_TexUnitList, m_RectUnitList, Color.White);

            // COUNT STATS
            if (vpHeight < GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height)
            {
                spriteBatch.DrawString(m_DebugFont, "" + userPlayer.GetVillagerCount(), new Vector2(vpWidth - 50, 25), Color.White);
                spriteBatch.DrawString(m_DebugFont, "" + userPlayer.GetArmySize() + "  (" + userPlayer.GetSelectedArmySize()+")", new Vector2(vpWidth - 50, 65), Color.White);
                spriteBatch.DrawString(m_DebugFont, "" + userPlayer.GetShamanCount(), new Vector2(vpWidth - 50, 105), Color.White);
            }
            else
            {
                spriteBatch.DrawString(m_DebugFont, "" + userPlayer.GetVillagerCount(), new Vector2(vpWidth - 100, 50), Color.White);
                spriteBatch.DrawString(m_DebugFont, "" + userPlayer.GetArmySize() + "  (" + userPlayer.GetSelectedArmySize() + ")", new Vector2(vpWidth - 100, 130), Color.White);
                spriteBatch.DrawString(m_DebugFont, "" + userPlayer.GetShamanCount(), new Vector2(vpWidth - 100, 210), Color.White);
            }

            // DRAW IN-GAME MENU
            if (m_bDrawMenu)
            {
                spriteBatch.Draw(m_TexPauseMenuBackground, m_RectPauseMenuBackground, Color.White);

                if (vpHeight < GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height)
                    if (m_bShowMenuFullscreenHover)
                        spriteBatch.Draw(m_TexMenuFullscreenHovered, m_RectMenuFullscreen, Color.White);
                    else
                        spriteBatch.Draw(m_TexMenuFullscreen, m_RectMenuFullscreen, Color.White);
                else
                    if (m_bShowMenuFullscreenHover)
                        spriteBatch.Draw(m_TexMenuWindowedHovered, m_RectMenuFullscreen, Color.White);
                    else
                        spriteBatch.Draw(m_TexMenuWindowed, m_RectMenuFullscreen, Color.White);

                if (m_bShowMenuResumeHover)
                    spriteBatch.Draw(m_TexMenuResumeHovered, m_RectMenuResume, Color.White);
                else
                    spriteBatch.Draw(m_TexMenuResume, m_RectMenuResume, Color.White);

                if (m_bShowMenuRestartHover)
                    spriteBatch.Draw(m_TexMenuRestartHovered, m_RectMenuRestart, Color.White);
                else
                    spriteBatch.Draw(m_TexMenuRestart, m_RectMenuRestart, Color.White);

                if (m_bShowMainMenuHover)
                    spriteBatch.Draw(m_TexMainMenuHovered, m_RectMainMenu, Color.White);
                else
                    spriteBatch.Draw(m_TexMainMenu, m_RectMainMenu, Color.White);

                if (m_bShowMenuControlsHover)
                    spriteBatch.Draw(m_TexMenuControlsHovered, m_RectMenuControls, Color.White);
                else
                    spriteBatch.Draw(m_TexMenuControls, m_RectMenuControls, Color.White);

                if (m_bShowMenuExitHover)
                    spriteBatch.Draw(m_TexMenuExitHovered, m_RectMenuExit, Color.White);
                else
                    spriteBatch.Draw(m_TexMenuExit, m_RectMenuExit, Color.White);

                if (m_bShowMenuSaveHover)
                    spriteBatch.Draw(m_TexMenuSaveHovered, m_RectMenuSave, Color.White);
                else
                    spriteBatch.Draw(m_TexMenuSave, m_RectMenuSave, Color.White);
            }

            // DRAW CONTROLLER MENU
            if (m_bShowControls)
            {
                spriteBatch.Draw(m_TexControlsBackground, m_RectControlsBackground, Color.White);

                if (m_bShowControllerLayout)
                {
                    spriteBatch.Draw(m_TexButtonControllerSwitch, m_RectButtonControllerSwitch, Color.White);
                    spriteBatch.Draw(m_TexControlsController, m_RectControlsController, Color.White);
                }
                else if (m_bShowKeyboardLayout)
                {
                    spriteBatch.Draw(m_TexButtonKeyboardSwitch, m_RectButtonKeyboardSwitch, Color.White);
                    spriteBatch.Draw(m_TexControlsKeyboard, m_RectControlsKeyboard, Color.White);
                }

                // Return Button
                if (m_bButtonReturnHovered)
                    spriteBatch.Draw(m_TexButtonReturnHovered, m_RectButtonReturn, Color.White);
                else spriteBatch.Draw(m_TexButtonReturn, m_RectButtonReturn, Color.White);
            }

            // SHOW ATTACKER SCENE
            if (m_bAttackingScene)
            {
                AttackScene tempScene = null;

                for (int t = 0; t < SceneManager.GameScenes.Count; ++t)
                {
                    if (SceneManager.GameScenes.ElementAt(t).SceneName == "AttackScene")
                    {
                        tempScene = (AttackScene)SceneManager.GameScenes.ElementAt(t);
                        break;
                    }
                }

                // DRAW INFORMATION
                if (tempScene != null)
                {
                    if (tempScene.GetWinner() == "Attacker")
                        spriteBatch.Draw(m_TexBackgroundVictory, m_RectBackgroundVictory, Color.White);
                    else
                        spriteBatch.Draw(m_TexBackgroundDefeat, m_RectBackgroundDefeat, Color.White);

                    if (vpWidth < renderContext.GraphicsDevice.Adapter.CurrentDisplayMode.Width)
                    {
                        spriteBatch.DrawString(m_DebugFont, "" + tempScene.GetAttackerArmySize(), new Vector2(vpWidth / 2 + 100, 140), Color.Black);
                        spriteBatch.DrawString(m_DebugFont, "" + tempScene.GetDefenderArmySize(), new Vector2(vpWidth / 2 + 100, 180), Color.Black);
                        spriteBatch.DrawString(m_DebugFont, "" + tempScene.GetHighestDieThrown(), new Vector2(vpWidth / 2 + 100, 220), Color.Black);
                    }
                    else if (vpWidth == renderContext.GraphicsDevice.Adapter.CurrentDisplayMode.Width)
                    {
                        spriteBatch.DrawString(m_DebugFont, "" + tempScene.GetAttackerArmySize(), new Vector2(vpWidth / 2 + 200, 280), Color.Black);
                        spriteBatch.DrawString(m_DebugFont, "" + tempScene.GetDefenderArmySize(), new Vector2(vpWidth / 2 + 200, 360), Color.Black);
                        spriteBatch.DrawString(m_DebugFont, "" + tempScene.GetHighestDieThrown(), new Vector2(vpWidth / 2 + 200, 440), Color.Black);
                    }
                }
            }
        }

        public void ToggleIngameMenu()
        {
            m_IngameMenu = !m_IngameMenu;
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

        public void ShowInGameMenu(bool show)
        {
            m_bDrawMenu = show;
        }

        public bool GetShowInGameMenu()
        {
            return m_bDrawMenu;
        }

        public void SetAttackingScene()
        {
            m_bAttackingScene = true;
        }
    }
}