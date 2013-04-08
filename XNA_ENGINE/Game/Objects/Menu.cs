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
                                   m_TexTile1,
                                   m_TexTile2,
                                   m_TexTile3,
                                   m_TexTile4,
                                   m_TexAttack,
                                   m_TexDefend,
                                   m_TexGather;

        private Rectangle m_RectSwitch,
                          m_RectTile1,
                          m_RectTile2,
                          m_RectTile3,
                          m_RectTile4,
                          m_RectAttack,
                          m_RectDefend,
                          m_RectGather;

        public enum SubMenuSelected
        {
            MoveMode, //Attack, defend,...
            BuildMode //Tile 1,2,3,...
        }

        public enum ModeSelected
        {
            Attack,
            Defend,
            Gather,
            Tile1,
            Tile2,
            Tile3,
            Tile4
        }

        private SubMenuSelected m_ModeSelected = SubMenuSelected.MoveMode;
        private ModeSelected m_SelectedTile = ModeSelected.Attack;
        private readonly SpriteFont m_DebugFont;

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

            m_TexTile1 = Content.Load<Texture2D>("tile1");
            m_TexTile2 = Content.Load<Texture2D>("tile2");
            m_TexTile3 = Content.Load<Texture2D>("tile3");
            m_TexTile4 = Content.Load<Texture2D>("tile4");

            m_TexAttack = Content.Load<Texture2D>("attack");
            m_TexDefend = Content.Load<Texture2D>("defend");
            m_TexGather = Content.Load<Texture2D>("gather");

            m_DebugFont = Content.Load<SpriteFont>("Fonts/DebugFont");

            var click = new InputAction((int) FinalScene.PlayerInput.LeftClick, TriggerState.Pressed);
            click.MouseButton = MouseButtons.LeftButton;
            click.GamePadButton = Buttons.X;
        }

        public void Update(RenderContext renderContext)
        {
           
        }

        public bool HandleInput(RenderContext renderContext,InputManager inputManager)
        {
            var mousePos = new Vector2(renderContext.Input.CurrentMouseState.X, renderContext.Input.CurrentMouseState.Y);

            if (inputManager.GetAction((int)FinalScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectSwitch))
            {
                if (m_ModeSelected == SubMenuSelected.MoveMode) m_ModeSelected = SubMenuSelected.BuildMode;
                else m_ModeSelected = SubMenuSelected.MoveMode;
                return true;
            }

            switch (m_ModeSelected)
            {
                case SubMenuSelected.BuildMode:
                    if (inputManager.GetAction((int)FinalScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectTile1))
                    {
                        m_SelectedTile = ModeSelected.Tile1;
                        return true;
                    }

                    if (inputManager.GetAction((int)FinalScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectTile2))
                    {
                        m_SelectedTile = ModeSelected.Tile2;
                        return true;
                    }

                    if (inputManager.GetAction((int)FinalScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectTile3))
                    {
                        m_SelectedTile = ModeSelected.Tile3;
                        return true;
                    }

                    if (inputManager.GetAction((int)FinalScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectTile4))
                    {
                        m_SelectedTile = ModeSelected.Tile4;
                        return true;
                    }
                    break;
                case SubMenuSelected.MoveMode:
                    if (inputManager.GetAction((int)FinalScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectAttack))
                    {
                        m_SelectedTile = ModeSelected.Attack;
                        return true;
                    }

                    if (inputManager.GetAction((int)FinalScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectDefend))
                    {
                        m_SelectedTile = ModeSelected.Defend;
                        return true;
                    }

                    if (inputManager.GetAction((int)FinalScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectGather))
                    {
                        m_SelectedTile = ModeSelected.Gather;
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
            m_RectSwitch = new Rectangle(10, renderContext.GraphicsDevice.Viewport.Height - 140, m_TexSwitch.Width,
                                       m_TexSwitch.Height);

            m_RectTile1 = new Rectangle(40, renderContext.GraphicsDevice.Viewport.Height - 80, m_TexTile1.Width,
                                        m_TexTile1.Height);
            m_RectTile2 = new Rectangle(150, renderContext.GraphicsDevice.Viewport.Height - 80, m_TexTile2.Width,
                                        m_TexTile2.Height);
            m_RectTile3 = new Rectangle(260, renderContext.GraphicsDevice.Viewport.Height - 80, m_TexTile3.Width,
                                        m_TexTile3.Height);
            m_RectTile4 = new Rectangle(370, renderContext.GraphicsDevice.Viewport.Height - 80, m_TexTile4.Width,
                                        m_TexTile4.Height);

            m_RectAttack = new Rectangle(40, renderContext.GraphicsDevice.Viewport.Height - 80, m_TexAttack.Width,
                                         m_TexAttack.Height);
            m_RectDefend = new Rectangle(150, renderContext.GraphicsDevice.Viewport.Height - 80, m_TexDefend.Width,
                                         m_TexDefend.Height);
            m_RectGather = new Rectangle(260, renderContext.GraphicsDevice.Viewport.Height - 80, m_TexGather.Width,
                                         m_TexGather.Height);


            renderContext.SpriteBatch.Draw(m_TexSwitch,m_RectSwitch,Color.White);

            if (m_ModeSelected == SubMenuSelected.BuildMode)
            {
                renderContext.SpriteBatch.Draw(m_TexTile1, m_RectTile1, Color.White);
                renderContext.SpriteBatch.Draw(m_TexTile2, m_RectTile2, Color.White);
                renderContext.SpriteBatch.Draw(m_TexTile3, m_RectTile3, Color.White);
                renderContext.SpriteBatch.Draw(m_TexTile4, m_RectTile4, Color.White);
            }
            else if (m_ModeSelected == SubMenuSelected.MoveMode)
            {
                renderContext.SpriteBatch.Draw(m_TexAttack, m_RectAttack, Color.White);
                renderContext.SpriteBatch.Draw(m_TexDefend, m_RectDefend, Color.White);
                renderContext.SpriteBatch.Draw(m_TexGather, m_RectGather, Color.White);
            }
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

        public ModeSelected GetSelectedTile()
        {
            return m_SelectedTile;
        }
    }
}
