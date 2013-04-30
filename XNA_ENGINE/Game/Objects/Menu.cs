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
                                   m_TexUnitList,
                                   m_TexSwitch,
                                   m_TexSettlement,
                                   m_TexSchool,
                                   m_TexShrine,
                                   m_TexBuildTile;

        private Rectangle m_RectMenuBackground,
                          m_RectWoodResource,
                          m_RectInfluenceResource,
                          m_RectDelete,
                          m_RectCharacterStats,
                          m_RectHoverVillager,
                          m_RectUnitList,
                          m_RectSwitch,
                          m_RectSettlement,
                          m_RectSchool,
                          m_RectShrine,
                          m_RectBuildTile;

        public enum SubMenuSelected
        {
            BaseMode,
            SoldierMode,
            ShrineMode,
            SettlementMode,
            ShamanMode,
            VillagerMode
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

        private SubMenuSelected m_SubMenuSelected = SubMenuSelected.BaseMode;
        private ModeSelected m_SelectedMode = ModeSelected.None;
        private GridTile.TileType m_TileTypeSelected = GridTile.TileType.Normal1;
        private SpriteFont m_DebugFont;
        private Player m_Player;

        private const int COSTOFWOOD_SETTLEMENT = 20;
        private const int COSTOFWOOD_SCHOOL = 30;
        private const int COSTOFWOOD_SHRINE = 50;
        private const int COSTOFINFLUENCE_SETTLEMENT = 0;
        private const int COSTOFINFLUENCE_SCHOOL = 0;
        private const int COSTOFINFLUENCE_SHRINE = 0;
        private const int COSTOFWOOD_TILE1 = 0;
        private const int COSTOFINFLUENCE_TILE1 = 20;

        // HOVERING
        private bool m_bShowVillagerHover;

        //Singleton implementation
        static public Menu GetInstance()
        {
            if (m_Menu == null)
                m_Menu = new Menu();
            return m_Menu;
        }

        private Menu()
        {
            Content = FinalScene.GetContentManager();

            m_bShowVillagerHover = false;

            m_TexSwitch = Content.Load<Texture2D>("switch");

            // MENU ONDERKANT BACKGROUND
            m_TexMenuBackground = Content.Load<Texture2D>("final Menu/grootMenu_Onderkant");

            // RESOURCE STATS
            m_TexWoodResource = Content.Load<Texture2D>("final Menu/resources_wood");
            m_TexInfluenceResource = Content.Load<Texture2D>("final Menu/resources_influence");

            // CHARACTER STATS
            m_TexCharacterStats = Content.Load<Texture2D>("final Menu/characterStats");
            m_TexUnitList = Content.Load<Texture2D>("final Menu/unitList");

            // ICONS
            m_TexDelete = Content.Load<Texture2D>("final Menu/iconStandard");

            // HOVERING
            m_TexHoverVillager = Content.Load<Texture2D>("final Menu/hoverVillager");

            // BUILDING ICONS
            m_TexSettlement = Content.Load<Texture2D>("final Menu/iconStandard");
            m_TexSchool = Content.Load<Texture2D>("final Menu/iconStandard");
            m_TexShrine = Content.Load<Texture2D>("final Menu/iconStandard");

            m_TexBuildTile = Content.Load<Texture2D>("final Menu/iconStandard");

            m_TexAttack = Content.Load<Texture2D>("Attack");
            m_TexMove = Content.Load<Texture2D>("Move");
            m_TexSplit = Content.Load<Texture2D>("Split_Army");

            m_TexBuild = Content.Load<Texture2D>("Build");

            m_DebugFont = Content.Load<SpriteFont>("Fonts/DebugFont");
        }

        public void SetPlayer(Player player)
        {
            m_Player = player;
        }

        public void SetFont(SpriteFont font)
        {
            m_DebugFont = font;
        }

        public void Update(RenderContext renderContext)
        {
            if (m_SelectedMode == ModeSelected.None && GridFieldManager.GetInstance(SceneManager.ActiveScene).CreativeMode == false)
                GridFieldManager.GetInstance(SceneManager.ActiveScene).SelectionModeMeth = GridFieldManager.SelectionMode.select1x1;
        }

        public bool HandleInput(RenderContext renderContext)
        {
            var mousePos = new Vector2(renderContext.Input.CurrentMouseState.X, renderContext.Input.CurrentMouseState.Y);
            var inputManager = FinalScene.GetInputManager();
            Placeable selectedPlaceable = GridFieldManager.GetInstance(SceneManager.ActiveScene).GetPermanentSelectedPlaceable();

            // HOVERING BUTTONS  --> dit is een test
            if (CheckHitButton(mousePos, m_RectDelete) && m_SubMenuSelected == SubMenuSelected.SettlementMode)
            {
                m_bShowVillagerHover = true;
            }
            else m_bShowVillagerHover = false;

            /*
            if (inputManager.GetAction((int)FinalScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectSwitch))
            {
                if (m_SubMenuSelected == SubMenuSelected.MoveMode) m_SubMenuSelected = SubMenuSelected.VillagerMode;
                else m_SubMenuSelected = SubMenuSelected.MoveMode;
                return true;
            }
            */

            switch (m_SubMenuSelected)
            {
                // --------------------------------------------
                // BASE MODE
                // --------------------------------------------
                case SubMenuSelected.BaseMode:
                    if (inputManager.GetAction((int)FinalScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectDelete))
                    {
                        // DELETE ITEM THAT WILL BE SELECTED
                        return true;
                    }
                    break;

                // --------------------------------------------
                // VILLAGER MODE
                // --------------------------------------------
                case SubMenuSelected.VillagerMode:
                    if (inputManager.GetAction((int)FinalScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectSchool))
                    {
                        // SET DECREASE RESOURCES
                        m_Player.GetResources().DecreaseWood(COSTOFWOOD_SCHOOL);
                        m_Player.GetResources().DecreaseInfluence(COSTOFINFLUENCE_SCHOOL);
                        GridFieldManager.GetInstance(SceneManager.ActiveScene).SelectionModeMeth = GridFieldManager.SelectionMode.select2x2;
                        m_SelectedMode = ModeSelected.BuildSchool;
                        return true;
                    }

                    if (inputManager.GetAction((int)FinalScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectShrine))
                    {
                        // SET DECREASE RESOURCES
                        m_Player.GetResources().DecreaseWood(COSTOFWOOD_SHRINE);
                        m_Player.GetResources().DecreaseInfluence(COSTOFINFLUENCE_SHRINE);
                        GridFieldManager.GetInstance(SceneManager.ActiveScene).SelectionModeMeth = GridFieldManager.SelectionMode.select1x1;
                        m_SelectedMode = ModeSelected.BuildShrine;
                        return true;
                    }
                    break;

                // --------------------------------------------
                // SHAMAN MODE
                // --------------------------------------------
                case SubMenuSelected.ShamanMode:
                    // BUILD TILES WITH SHAMAN
                    if (inputManager.GetAction((int)FinalScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectBuildTile))
                    {
                        Console.WriteLine("Create Tile 1");
                        m_Player.GetResources().DecreaseWood(COSTOFWOOD_TILE1);
                        m_Player.GetResources().DecreaseInfluence(COSTOFINFLUENCE_TILE1);
                        GridFieldManager.GetInstance(SceneManager.ActiveScene).SelectionModeMeth = GridFieldManager.SelectionMode.select1x1;
                        m_SelectedMode = ModeSelected.BuildTile1;
                        return true;
                    }
                   
                   /* if (inputManager.GetAction((int)FinalScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectTile2))
                    {
                        Console.WriteLine("Create Tile 2");
                        GridFieldManager.GetInstance(SceneManager.ActiveScene).SelectionModeMeth = GridFieldManager.SelectionMode.select1x1;
                        m_SelectedMode = ModeSelected.BuildTile2;
                        return true;
                    }

                    if (inputManager.GetAction((int)FinalScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectTile3))
                    {
                        Console.WriteLine("Create Tile 3");
                        GridFieldManager.GetInstance(SceneManager.ActiveScene).SelectionModeMeth = GridFieldManager.SelectionMode.select1x1;
                        m_SelectedMode = ModeSelected.BuildTile3;
                        return true;
                    }

                    if (inputManager.GetAction((int)FinalScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectTile4))
                    {
                        Console.WriteLine("Create Tile 4");
                        GridFieldManager.GetInstance(SceneManager.ActiveScene).SelectionModeMeth = GridFieldManager.SelectionMode.select1x1;
                        m_SelectedMode = ModeSelected.BuildTile4;
                        return true;
                    }*/

                    break;

                // --------------------------------------------
                // SOLDIER MODE
                // --------------------------------------------
                case SubMenuSelected.SoldierMode:
                    if (inputManager.GetAction((int)FinalScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectAttack))
                    {
                        var selectedArmy = GridFieldManager.GetInstance(SceneManager.ActiveScene).GetPermanentSelectedPlaceable();

                        Console.WriteLine("Attack!");
                        m_Player.GetPlayerOptions().Attack();

                        if (selectedArmy != null && selectedArmy.PlaceableTypeMeth == Placeable.PlaceableType.Army)
                        {
                            var newArmy = new Army(SceneManager.ActiveScene, selectedArmy.GetTargetTile());
                            Player.NewPlaceable(newArmy);
                            
                            SceneManager.AddGameScene(new AttackScene(Content, (Army)selectedArmy, newArmy));
                        }
                        

                        m_SelectedMode = ModeSelected.Attack;
                        return true;
                    }

                    if (inputManager.GetAction((int)FinalScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectMove))
                    {
                        Console.WriteLine("Move!");
                        m_Player.GetPlayerOptions().Move();

                        m_SelectedMode = ModeSelected.Defend;
                        return true;
                    }

                    if (inputManager.GetAction((int)FinalScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectSplit))
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
                    if (inputManager.GetAction((int)FinalScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectDelete))
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

                // HOVERING
                m_RectHoverVillager = new Rectangle(m_RectDelete.X, m_RectDelete.Y - m_TexHoverVillager.Height / 2 - 10,
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

                // HOVERING
                m_RectHoverVillager = new Rectangle(m_RectDelete.X, m_RectDelete.Y - m_TexHoverVillager.Height - 20,
                                                           m_TexHoverVillager.Width,
                                                           m_TexHoverVillager.Height);
            }

            

            m_RectSwitch = new Rectangle(10, renderContext.GraphicsDevice.Viewport.Height - 140, m_TexSwitch.Width,m_TexSwitch.Height);

            // BUILDINGS
            //m_RectSettlement = new Rectangle(40, renderContext.GraphicsDevice.Viewport.Height - 80, m_TexTileBlue.Width,m_TexTileBlue.Height);
            //m_RectSchool = new Rectangle(150, renderContext.GraphicsDevice.Viewport.Height - 80, m_TexTileGold.Width,m_TexTileGold.Height);
            //m_RectShrine = new Rectangle(260, renderContext.GraphicsDevice.Viewport.Height - 80, m_TexTileRed.Width,m_TexTileRed.Height);

            // SHAMAN MENU
            //m_RectTile1 = new Rectangle(40, renderContext.GraphicsDevice.Viewport.Height - 80, m_TexTile1.Width, m_TexTile1.Height);
            //m_RectTile2 = new Rectangle(150, renderContext.GraphicsDevice.Viewport.Height - 80, m_TexTile2.Width, m_TexTile2.Height);
            //m_RectTile3 = new Rectangle(260, renderContext.GraphicsDevice.Viewport.Height - 80, m_TexTile3.Width, m_TexTile3.Height);
            //m_RectTile4 = new Rectangle(370, renderContext.GraphicsDevice.Viewport.Height - 80, m_TexTile4.Width,m_TexTile4.Height);

            // ATTACK MOVE SPLIT
            //m_RectAttack = new Rectangle(40, renderContext.GraphicsDevice.Viewport.Height - 80, m_TexAttack.Width,m_TexAttack.Height);
            //m_RectMove = new Rectangle(150, renderContext.GraphicsDevice.Viewport.Height - 80, m_TexMove.Width,m_TexMove.Height);

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
                    renderContext.SpriteBatch.Draw(m_TexSwitch, m_RectSwitch, Color.White);
                    break;

                // --------------------------------------------
                // SETTLEMENT MODE
                // --------------------------------------------
                case SubMenuSelected.SettlementMode:
                    break;

                    m_RectSplit = new Rectangle(260, renderContext.GraphicsDevice.Viewport.Height - 80, m_TexSplit.Width,
                                                m_TexSplit.Height);

                    m_RectBuild = new Rectangle(40, renderContext.GraphicsDevice.Viewport.Height - 80, m_TexBuild.Width,
                                                m_TexBuild.Height);

                    // HOVERING
                    if (m_bShowVillagerHover)
                    {
                        renderContext.SpriteBatch.Draw(m_TexHoverVillager, m_RectHoverVillager, Color.White);
                    }

                    

                    if (m_SubMenuSelected == SubMenuSelected.VillagerMode)
                    {
                        
                        renderContext.SpriteBatch.Draw(m_TexTile4, m_RectTile4, Color.White);
                    }
                    else if (m_SubMenuSelected == SubMenuSelected.MoveMode)
                    {
                        renderContext.SpriteBatch.Draw(m_TexAttack, m_RectAttack, Color.White);
                        renderContext.SpriteBatch.Draw(m_TexMove, m_RectMove, Color.White);
                        renderContext.SpriteBatch.Draw(m_TexSplit, m_RectSplit, Color.White);
                    }
                    else if (m_SubMenuSelected == SubMenuSelected.SettlementMode)
                    {
                        // renderContext.SpriteBatch.Draw(m_TexBuild, m_RectBuild, Color.White);
                    }
                    else if (m_SubMenuSelected == SubMenuSelected.SjamanMode)
                    {
                        renderContext.SpriteBatch.Draw(m_TexBuild, m_RectBuild, Color.White);
                    }
            }

            // DRAW EXTRA INFORMATION (RESOURCES,...) IN TEXT
            // resources
            if (renderContext.GraphicsDevice.Viewport.Height <
                GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height)
            {
                renderContext.SpriteBatch.DrawString(m_DebugFont,
                                                     "" + m_Player.GetResources().GetAllResources().ElementAt(0),
                                                     new Vector2(
                                                         renderContext.GraphicsDevice.Viewport.Width / 2 - 45, 8),
                                                     Color.White);
                renderContext.SpriteBatch.DrawString(m_DebugFont,
                                                     "" + m_Player.GetResources().GetAllResources().ElementAt(1),
                                                     new Vector2(
                                                         renderContext.GraphicsDevice.Viewport.Width / 2 + 90, 8),
                                                     Color.White);
            }
            else
            {
                renderContext.SpriteBatch.DrawString(m_DebugFont,
                                                     "" + m_Player.GetResources().GetAllResources().ElementAt(0),
                                                     new Vector2(
                                                         renderContext.GraphicsDevice.Viewport.Width / 2 - 90, 20),
                                                     Color.White);
                renderContext.SpriteBatch.DrawString(m_DebugFont,
                                                     "" + m_Player.GetResources().GetAllResources().ElementAt(1),
                                                     new Vector2(
                                                         renderContext.GraphicsDevice.Viewport.Width / 2 + 180, 20),
                                                     Color.White);
            }

            // MENU ONDERKANT DRAW
            renderContext.SpriteBatch.Draw(m_TexMenuBackground, m_RectMenuBackground, Color.White);

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

        public Player Player
        {
            get { return m_Player; }
            //set { m_Player = value; }
        }

        public void NextTileType()
        {
            ++m_TileTypeSelected;
            if ((int)m_TileTypeSelected >= (int)GridTile.TileType.enumSize) m_TileTypeSelected = 0;
        }

        public void ResetSelectedMode()
        {
            m_SelectedMode = ModeSelected.None;
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
