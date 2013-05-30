﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XNA_ENGINE.Engine;
using XNA_ENGINE.Engine.Helpers;
using XNA_ENGINE.Engine.Scenegraph;

namespace XNA_ENGINE.Game.Scenes
{
    class MainMenuScene : GameScene
    {
        public enum PlayerInput
        {
            LeftClick,
            RightClick,
            ScrollWheelDown,
            ToggleCreativeMode,
            RotateClockWise,
            RotateCounterClockWise
        }

        private readonly ContentManager m_Content;

        private Texture2D m_StartButton;
        private Texture2D m_ExitButton;
        private Texture2D m_StartButtonHovered;
        private Texture2D m_ExitButtonHovered;
        private Texture2D m_DaeScreen;
        private Texture2D m_BackgroundTexture;

        private Rectangle m_StartRect;
        private Rectangle m_ExitRect;
        private Rectangle m_DaeRect;
        private Rectangle m_BackgroundRect;
        private InputManager m_InputManager;
        private bool m_ExitGame;
        private  int m_Counter;

        private bool m_bStartButtonHovered;
        private bool m_bExitButtonHovered;

        private const int SPLASHSCREENTIME = 200;

        public MainMenuScene(ContentManager content)
            : base("MainMenuScene")
        {
            //CONTENT
            m_Content = content;
        }

        public override void Initialize()
        {
            m_bStartButtonHovered = false;
            m_bExitButtonHovered = false;

            // background
            m_BackgroundTexture = m_Content.Load<Texture2D>("final Menu/Background_StartMenu");

            // DAE
            m_Counter = 0;
            m_DaeScreen = m_Content.Load<Texture2D>("DaeSplashScreen");

            // Buttons
            m_StartButton = m_Content.Load<Texture2D>("final Menu/Button_StartMenu_StartButtonUnhovered");
            m_ExitButton = m_Content.Load<Texture2D>("final Menu/Button_StartMenu_ExitButtonUnhovered");
            m_StartButtonHovered = m_Content.Load<Texture2D>("final Menu/Button_StartMenu_StartButtonHovered");
            m_ExitButtonHovered = m_Content.Load<Texture2D>("final Menu/Button_StartMenu_ExitButtonHovered");

            // InputManager
            m_InputManager = new InputManager();
            InputAction leftClick = new InputAction((int)PlayerInput.LeftClick, TriggerState.Pressed);
            leftClick.MouseButton = MouseButtons.LeftButton;
            m_InputManager.MapAction(leftClick);

            m_ExitGame = false;

            base.Initialize();
        }

        public override void Update(RenderContext renderContext)
        {
            HandleInput(renderContext);

            base.Update(renderContext);
        }

        public override void Draw2D(RenderContext renderContext, bool drawBefore3D)
        {
            int gameHeight = renderContext.GraphicsDevice.Viewport.Height;
            int gameWidth = renderContext.GraphicsDevice.Viewport.Width;

            m_DaeRect = new Rectangle(0, 0, gameWidth, gameHeight);
            m_StartRect = new Rectangle(gameWidth / 2 - m_StartButton.Width / 2, gameHeight / 2 - 175, m_StartButton.Width, m_StartButton.Height);
            m_ExitRect = new Rectangle(gameWidth / 2 - m_ExitButton.Width / 2, gameHeight / 2, m_ExitButton.Width, m_ExitButton.Height);
            m_BackgroundRect = new Rectangle(0, 0, gameWidth, gameHeight);

            if (gameWidth < renderContext.GraphicsDevice.Adapter.CurrentDisplayMode.Width)
            {
                m_StartRect = new Rectangle(gameWidth / 2 - m_StartButton.Width / 4, gameHeight / 2 - 85, m_StartButton.Width / 2, m_StartButton.Height / 2);
                m_ExitRect = new Rectangle(gameWidth / 2 - m_ExitButton.Width / 4, gameHeight / 2, m_ExitButton.Width / 2, m_ExitButton.Height / 2);
            }

            if (m_Counter < SPLASHSCREENTIME)
            {
                ++m_Counter;
                renderContext.SpriteBatch.Draw(m_DaeScreen, m_DaeRect, Color.White);
            }
            else
            {
                renderContext.SpriteBatch.Draw(m_BackgroundTexture, m_BackgroundRect, Color.White);

                if (m_bStartButtonHovered == false)
                    renderContext.SpriteBatch.Draw(m_StartButton, m_StartRect, Color.White);
                else
                    renderContext.SpriteBatch.Draw(m_StartButtonHovered, m_StartRect, Color.White);  

                if (m_bExitButtonHovered == false)
                    renderContext.SpriteBatch.Draw(m_ExitButton, m_ExitRect, Color.White);
                else
                    renderContext.SpriteBatch.Draw(m_ExitButtonHovered, m_ExitRect, Color.White);
            }

            base.Draw2D(renderContext, drawBefore3D);
        }

        public bool HandleInput(RenderContext renderContext)
        {
            //Update inputManager
            m_InputManager.Update();

            var mousePos = new Vector2(renderContext.Input.CurrentMouseState.X, renderContext.Input.CurrentMouseState.Y);

            // HOVER
            if (CheckHitButton(mousePos, m_StartRect))
            {
                m_bStartButtonHovered = true;
            }
            else
                m_bStartButtonHovered = false;

            if (CheckHitButton(mousePos, m_ExitRect))
            {
                m_bExitButtonHovered = true;
            }
            else
            {
                m_bExitButtonHovered = false;
            }

            if (m_InputManager.GetAction((int)PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_StartRect))
            {
                Console.WriteLine("Start button");

                var playScene = new PlayScene(m_Content, "GeneratedTileMap");
                SceneManager.AddGameScene(playScene);
                SceneManager.SetActiveScene("PlayScene");
                playScene.Initialize();

                return true;
            }
            else if (m_InputManager.GetAction((int)PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_ExitRect))
            {
                Console.WriteLine("Exit button");
                ExitGame = true;
                SceneManager.MainGame.Exit();
                return true;
            }
            else return false;
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

        public bool ExitGame
        {
            get { return m_ExitGame; }
            set { m_ExitGame = value; }
        }
    }
}
