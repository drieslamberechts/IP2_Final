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
using XNA_ENGINE.Game.Scenes;

namespace XNA_ENGINE.Game.Objects
{
    internal class Menu
    {

        //Object that holds the menu
        private static Menu m_Menu;

        private ContentManager Content;

        private readonly Texture2D m_TexSwitch,
                                   m_TexTileBlue,
                                   m_TexTileGold,
                                   m_TexTileRed,
                                   m_TexTile4,
                                   m_TexAttack,
                                   m_TexMove,
                                   m_TexSplit,
                                   m_TexBuild;

        private Rectangle m_RectSwitch,
                          m_RectTileBlue,
                          m_RectTileGold,
                          m_RectTileRed,
                          m_RectTile4,
                          m_RectAttack,
                          m_RectMove,
                          m_RectSplit,
                          m_RectBuild;

        public enum SubMenuSelected
        {
            MoveMode, //Attack, defend,...
            BuildMode, //Tile 1,2,3,...
            SettlementMode
        }

        public enum ModeSelected
        {
            None,
            Attack,
            Defend,
            Gather,
            TileBlue,
            TileGold,
            TileRed,
            Delete
        }

        private SubMenuSelected m_SubMenuSelected = SubMenuSelected.MoveMode;
        private ModeSelected m_SelectedMode = ModeSelected.None;
        private GridTile.TileType m_TileTypeSelected = GridTile.TileType.Normal1;
        private SpriteFont m_DebugFont;
        private Player m_Player;

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

            m_TexSwitch = Content.Load<Texture2D>("switch");

            m_TexTileBlue = Content.Load<Texture2D>("BuildBase");
            m_TexTileGold = Content.Load<Texture2D>("BuildTile");
            m_TexTileRed = Content.Load<Texture2D>("Tile3");
            m_TexTile4 = Content.Load<Texture2D>("Tile4");

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
           
        }

        public bool HandleInput(RenderContext renderContext)
        {
            var mousePos = new Vector2(renderContext.Input.CurrentMouseState.X, renderContext.Input.CurrentMouseState.Y);
            var inputManager = FinalScene.GetInputManager();

            if (inputManager.GetAction((int)FinalScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectSwitch))
            {
                if (m_SubMenuSelected == SubMenuSelected.MoveMode) m_SubMenuSelected = SubMenuSelected.BuildMode;
                else m_SubMenuSelected = SubMenuSelected.MoveMode;
                return true;
            }

            switch (m_SubMenuSelected)
            {
                case SubMenuSelected.BuildMode:
                    if (inputManager.GetAction((int)FinalScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectTileBlue))
                    {
                        // SET DECREASE RESOURCES
                        m_Player.GetResources().DecreaseWood(10);

                        m_SelectedMode = ModeSelected.TileBlue;
                        return true;
                    }

                    if (inputManager.GetAction((int)FinalScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectTileGold))
                    {
                        // SET DECREASE RESOURCES

                        m_SelectedMode = ModeSelected.TileGold;
                        return true;
                    }

                    if (inputManager.GetAction((int)FinalScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectTileRed))
                    {
                        // SET DECREASE RESOURCES

                        m_SelectedMode = ModeSelected.TileRed;
                        return true;
                    }

                    if (inputManager.GetAction((int)FinalScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectTile4))
                    {
                        m_SelectedMode = ModeSelected.Delete;
                        return true;
                    }
                    break;
                case SubMenuSelected.MoveMode:
                    if (inputManager.GetAction((int)FinalScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectAttack))
                    {
                        Console.WriteLine("Attack!");
                        m_Player.GetPlayerOptions().Attack();

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
                        m_Player.GetArmyList().Add(m_Player.GetPlayerOptions().SplitArmy(m_Player.GetSelectedArmy()));

                        m_SelectedMode = ModeSelected.Gather;
                        return true;
                    }
                    break;

                case SubMenuSelected.SettlementMode:
                    if (inputManager.GetAction((int)FinalScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectBuild))
                    {
                        Console.WriteLine("Build goblin");
                        m_Player.GetPlayerOptions().BuildTribe();
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
            m_RectSwitch = new Rectangle(10, renderContext.GraphicsDevice.Viewport.Height - 140, m_TexSwitch.Width,m_TexSwitch.Height);

            m_RectTileBlue = new Rectangle(40, renderContext.GraphicsDevice.Viewport.Height - 80, m_TexTileBlue.Width,m_TexTileBlue.Height);
            m_RectTileGold = new Rectangle(150, renderContext.GraphicsDevice.Viewport.Height - 80, m_TexTileGold.Width,m_TexTileGold.Height);
            m_RectTileRed = new Rectangle(260, renderContext.GraphicsDevice.Viewport.Height - 80, m_TexTileRed.Width,m_TexTileRed.Height);
            m_RectTile4 = new Rectangle(370, renderContext.GraphicsDevice.Viewport.Height - 80, m_TexTile4.Width,m_TexTile4.Height);

            m_RectAttack = new Rectangle(40, renderContext.GraphicsDevice.Viewport.Height - 80, m_TexAttack.Width,m_TexAttack.Height);
            m_RectMove = new Rectangle(150, renderContext.GraphicsDevice.Viewport.Height - 80, m_TexMove.Width,m_TexMove.Height);
            m_RectSplit = new Rectangle(260, renderContext.GraphicsDevice.Viewport.Height - 80, m_TexSplit.Width,m_TexSplit.Height);

            m_RectBuild = new Rectangle(40, renderContext.GraphicsDevice.Viewport.Height - 80, m_TexBuild.Width, m_TexBuild.Height);


            renderContext.SpriteBatch.Draw(m_TexSwitch,m_RectSwitch,Color.White);

            if (m_SubMenuSelected == SubMenuSelected.BuildMode)
            {
                renderContext.SpriteBatch.Draw(m_TexTileBlue, m_RectTileBlue, Color.White);
                renderContext.SpriteBatch.Draw(m_TexTileGold, m_RectTileGold, Color.White);
                renderContext.SpriteBatch.Draw(m_TexTileRed, m_RectTileRed, Color.White);
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
                renderContext.SpriteBatch.Draw(m_TexBuild, m_RectBuild, Color.White);
            }

            // DRAW EXTRA INFORMATION (RESOURCES,...)
            // resources
            renderContext.SpriteBatch.DrawString(m_DebugFont, "Wood: " + m_Player.GetResources().GetAllResources().ElementAt(0), new Vector2(renderContext.GraphicsDevice.Viewport.Width - 200, 10), Color.White);
            renderContext.SpriteBatch.DrawString(m_DebugFont, "Influence Points: " + m_Player.GetResources().GetAllResources().ElementAt(1), new Vector2(renderContext.GraphicsDevice.Viewport.Width - 200, 30), Color.White);

            // armysize
            renderContext.SpriteBatch.DrawString(m_DebugFont, "Army Size: " + m_Player.GetArmySize(), new Vector2(renderContext.GraphicsDevice.Viewport.Width / 2 - 25, 10), Color.White);
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
