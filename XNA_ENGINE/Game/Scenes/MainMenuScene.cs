using System;
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
        private Texture2D m_ButtonControls, m_ButtonControlsHovered;

        private Rectangle m_StartRect;
        private Rectangle m_ExitRect;
        private Rectangle m_DaeRect;
        private Rectangle m_BackgroundRect;
        private Rectangle m_RectButtonControls;
        private InputManager m_InputManager;
        private bool m_ExitGame;
        private  int m_Counter;

        private bool m_bStartButtonHovered;
        private bool m_bExitButtonHovered;
        private bool m_bControlsHovered;

        // Variables Controls Menu
        private bool m_bShowControls, m_bButtonReturnHovered, m_bShowControllerLayout, m_bShowKeyboardLayout;
        private bool m_bLayoutSwitch;

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
            m_bControlsHovered = false;
            m_bShowControls = false;

            m_bShowControls = false;
            m_bButtonReturnHovered = false;
            m_bShowControllerLayout = true;
            m_bShowKeyboardLayout = false;
            m_bLayoutSwitch = true;

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
            m_ButtonControls = m_Content.Load<Texture2D>("pause Menu/Button_ControlsUnhovered");
            m_ButtonControlsHovered = m_Content.Load<Texture2D>("pause Menu/Button_ControlsHovered");

            // CONTROLLER MENU
            m_TexControlsBackground = m_Content.Load<Texture2D>("ControllerMenu/Controller_Background");
            m_TexControlsKeyboard = m_Content.Load<Texture2D>("ControllerMenu/Controller_MouseAndKeyboard");
            m_TexControlsController = m_Content.Load<Texture2D>("ControllerMenu/Controller_xboxController");
            m_TexButtonReturn = m_Content.Load<Texture2D>("ControllerMenu/Controller_ReturnButtonUnhovered");
            m_TexButtonReturnHovered = m_Content.Load<Texture2D>("ControllerMenu/Controller_ReturnButtonHovered");
            m_TexButtonControllerSwitch = m_Content.Load<Texture2D>("ControllerMenu/Controller_xboxControllerButton");
            m_TexButtonKeyboardSwitch = m_Content.Load<Texture2D>("ControllerMenu/Controller_MouseAndKeyboardButton");

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
            m_RectButtonControls = new Rectangle(gameWidth - m_ButtonControls.Width - 50, gameHeight - m_ButtonControls.Height - 50, m_ButtonControls.Width, m_ButtonControls.Height);
            m_BackgroundRect = new Rectangle(0, 0, gameWidth, gameHeight);

            // CONTROLLER MENU
            m_RectControlsBackground = new Rectangle(gameWidth / 2 - m_TexControlsBackground.Width / 2, 40, m_TexControlsBackground.Width, m_TexControlsBackground.Height);
            m_RectControlsKeyboard = new Rectangle(gameWidth / 2 - m_TexControlsKeyboard.Width / 2, 300, m_TexControlsKeyboard.Width, m_TexControlsKeyboard.Height);
            m_RectControlsController = new Rectangle(gameWidth / 2 - m_TexControlsController.Width / 2, 300, m_TexControlsController.Width, m_TexControlsController.Height);
            m_RectButtonReturn = new Rectangle(gameWidth / 2 - 600, 900, m_TexButtonReturn.Width, m_TexButtonReturn.Height);
            m_RectButtonControllerSwitch = new Rectangle(gameWidth / 2 - m_TexButtonControllerSwitch.Width / 2, 160, m_TexButtonControllerSwitch.Width, m_TexButtonControllerSwitch.Height);
            m_RectButtonKeyboardSwitch = new Rectangle(gameWidth / 2 - m_TexButtonKeyboardSwitch.Width / 2, 160, m_TexButtonKeyboardSwitch.Width, m_TexButtonKeyboardSwitch.Height);

            if (gameWidth < renderContext.GraphicsDevice.Adapter.CurrentDisplayMode.Width)
            {
                m_StartRect = new Rectangle(gameWidth / 2 - m_StartButton.Width / 4, gameHeight / 2 - 85, m_StartButton.Width / 2, m_StartButton.Height / 2);
                m_ExitRect = new Rectangle(gameWidth / 2 - m_ExitButton.Width / 4, gameHeight / 2, m_ExitButton.Width / 2, m_ExitButton.Height / 2);
                m_RectButtonControls = new Rectangle(gameWidth - m_ButtonControls.Width / 2 - 25, gameHeight - m_ButtonControls.Height / 2 - 25, m_ButtonControls.Width / 2, m_ButtonControls.Height / 2);

                // CONTROLLER MENU
                m_RectControlsBackground = new Rectangle(gameWidth / 2 - m_TexControlsBackground.Width / 4, 20, m_TexControlsBackground.Width / 2, m_TexControlsBackground.Height / 2);
                m_RectControlsKeyboard = new Rectangle(gameWidth / 2 - m_TexControlsKeyboard.Width / 4, 150, m_TexControlsKeyboard.Width / 2, m_TexControlsKeyboard.Height / 2);
                m_RectControlsController = new Rectangle(gameWidth / 2 - m_TexControlsController.Width / 4, 150, m_TexControlsController.Width / 2, m_TexControlsController.Height / 2);
                m_RectButtonReturn = new Rectangle(gameWidth / 2 - 300, 450, m_TexButtonReturn.Width / 2, m_TexButtonReturn.Height / 2);
                m_RectButtonControllerSwitch = new Rectangle(gameWidth / 2 - m_TexButtonControllerSwitch.Width / 4, 70, m_TexButtonControllerSwitch.Width / 2, m_TexButtonControllerSwitch.Height / 2);
                m_RectButtonKeyboardSwitch = new Rectangle(gameWidth / 2 - m_TexButtonKeyboardSwitch.Width / 4, 70, m_TexButtonKeyboardSwitch.Width / 2, m_TexButtonKeyboardSwitch.Height / 2);
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

                if (m_bControlsHovered)
                    renderContext.SpriteBatch.Draw(m_ButtonControlsHovered, m_RectButtonControls, Color.White);
                else renderContext.SpriteBatch.Draw(m_ButtonControls, m_RectButtonControls, Color.White);
            }

            // DRAW CONTROLLER MENU
            if (m_bShowControls)
            {
                renderContext.SpriteBatch.Draw(m_TexControlsBackground, m_RectControlsBackground, Color.White);

                if (m_bShowControllerLayout)
                {
                    renderContext.SpriteBatch.Draw(m_TexButtonControllerSwitch, m_RectButtonControllerSwitch, Color.White);
                    renderContext.SpriteBatch.Draw(m_TexControlsController, m_RectControlsController, Color.White);
                }
                else if (m_bShowKeyboardLayout)
                {
                    renderContext.SpriteBatch.Draw(m_TexButtonKeyboardSwitch, m_RectButtonKeyboardSwitch, Color.White);
                    renderContext.SpriteBatch.Draw(m_TexControlsKeyboard, m_RectControlsKeyboard, Color.White);
                }

                // Return Button
                if (m_bButtonReturnHovered)
                    renderContext.SpriteBatch.Draw(m_TexButtonReturnHovered, m_RectButtonReturn, Color.White);
                else renderContext.SpriteBatch.Draw(m_TexButtonReturn, m_RectButtonReturn, Color.White);
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

            if (CheckHitButton(mousePos, m_RectButtonControls))
                m_bControlsHovered = true;
            else
                m_bControlsHovered = false;

            if (m_InputManager.GetAction((int)PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectButtonControls))
                m_bShowControls = true;

            if (CheckHitButton(mousePos, m_RectButtonReturn) && m_bShowControls)
                m_bButtonReturnHovered = true;
            else m_bButtonReturnHovered = false;

            // CONTROLS
            if (m_InputManager.GetAction((int)PlayScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectButtonReturn) && m_bShowControls)
                m_bShowControls = false;

            if (m_InputManager.GetAction((int)PlayScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectButtonControllerSwitch) && m_bShowControls && m_bShowControllerLayout && m_bLayoutSwitch)
            {
                m_bShowControllerLayout = false;
                m_bShowKeyboardLayout = true;
                m_bLayoutSwitch = false;
            }

            if (m_InputManager.GetAction((int)PlayScene.PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_RectButtonKeyboardSwitch) && m_bShowControls && m_bShowKeyboardLayout && m_bLayoutSwitch)
            {
                m_bShowKeyboardLayout = false;
                m_bShowControllerLayout = true;
                m_bLayoutSwitch = false;
            }

            m_bLayoutSwitch = true;

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
