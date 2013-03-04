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

namespace XNA_ENGINE.Game.Objects.Concept2
{
    class Menu
    {
        enum PlayerInput
        {
            Click,
            RightClick
        }

        private ContentManager Content;
        private Texture2D m_TexMode, m_TexTile1, m_TexTile2, m_TexTile3, m_TexTile4, m_TexAttack, m_TexDefend, m_TexGather;
        private Rectangle m_RectMode, m_RectTile1, m_RectTile2, m_RectTile3, m_RectTile4, m_RectAttack, m_RectDefend, m_RectGather;

        public int m_ModeSelected = 0;
        private int m_SelectedTile = 0;

        public Menu(ContentManager content)
        {
            Content = content;

            m_TexMode = Content.Load<Texture2D>("switch");
            m_TexTile1 = Content.Load<Texture2D>("tile1");
            m_TexTile2 = Content.Load<Texture2D>("tile2");
            m_TexTile3 = Content.Load<Texture2D>("tile3");
            m_TexTile4 = Content.Load<Texture2D>("tile4");

            m_TexAttack = Content.Load<Texture2D>("attack");
            m_TexDefend = Content.Load<Texture2D>("defend");
            m_TexGather = Content.Load<Texture2D>("gather");

            m_RectMode = new Rectangle(5, 820, m_TexMode.Width, m_TexMode.Height);
            m_RectTile1 = new Rectangle(30, 900, m_TexTile1.Width, m_TexTile1.Height);
            m_RectTile2 = new Rectangle(140, 900, m_TexTile2.Width, m_TexTile2.Height);
            m_RectTile3 = new Rectangle(250, 900, m_TexTile3.Width, m_TexTile3.Height);
            m_RectTile4 = new Rectangle(360, 900, m_TexTile4.Width, m_TexTile4.Height);

            m_RectAttack = new Rectangle(30, 900, m_TexAttack.Width, m_TexAttack.Height);
            m_RectDefend = new Rectangle(140, 900, m_TexDefend.Width, m_TexDefend.Height);
            m_RectGather = new Rectangle(250, 900, m_TexGather.Width, m_TexGather.Height);

            InputAction Click = new InputAction((int)PlayerInput.Click, TriggerState.Pressed);
            Click.MouseButton = MouseButtons.LeftButton;
            Click.GamePadButton = Buttons.X;
        }

        public void Update(RenderContext renderContext, InputManager inputManager)
        {
            Vector2 mousePos = new Vector2(renderContext.Input.CurrentMouseState.X,renderContext.Input.CurrentMouseState.Y);

            if (inputManager.GetAction((int)PlayerInput.Click).IsTriggered && CheckHitButton(mousePos, m_RectMode))
            {
                if (m_ModeSelected == 1) m_ModeSelected = 0;
                else m_ModeSelected = 1;
            }

            if (inputManager.GetAction((int)PlayerInput.Click).IsTriggered && CheckHitButton(mousePos, m_RectTile1))
            {
                m_SelectedTile = 1;
            }
            else if (inputManager.GetAction((int)PlayerInput.Click).IsTriggered && CheckHitButton(mousePos, m_RectTile2))
            {
                m_SelectedTile = 2;
            }
            else if (inputManager.GetAction((int)PlayerInput.Click).IsTriggered && CheckHitButton(mousePos, m_RectTile3))
            {
                m_SelectedTile = 3;
            }
            else if (inputManager.GetAction((int)PlayerInput.Click).IsTriggered && CheckHitButton(mousePos, m_RectTile4))
            {
                m_SelectedTile = 4;
            }
        }

        // Draw
        public void Draw(RenderContext renderContext)
        {
            renderContext.SpriteBatch.Draw(m_TexMode, m_RectMode, Color.White);

            if (m_ModeSelected == 1)
            {
                renderContext.SpriteBatch.Draw(m_TexTile1, m_RectTile1, Color.White);
                renderContext.SpriteBatch.Draw(m_TexTile2, m_RectTile2, Color.White);
                renderContext.SpriteBatch.Draw(m_TexTile3, m_RectTile3, Color.White);
                renderContext.SpriteBatch.Draw(m_TexTile4, m_RectTile4, Color.White);
            }
            else
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

        public int GetSelectedTile()
        {
            return m_SelectedTile;
        }

        public int GetSelectedMode()
        {
            return m_ModeSelected;
        }
    }
}
