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
        private Texture2D m_ExitButton ;

        private Rectangle m_StartRect;
        private Rectangle m_ExitRect;
        private InputManager m_InputManager;
        private bool m_ExitGame;

        public MainMenuScene(ContentManager content)
            : base("MainMenuScene")
        {
            //CONTENT
            m_Content = content;
        }

        public override void Initialize()
        {
            // Buttons
            m_StartButton = m_Content.Load<Texture2D>("menu/startButton");
            m_ExitButton = m_Content.Load<Texture2D>("menu/exitButton");

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

            m_StartRect = new Rectangle(gameWidth / 2 - m_StartButton.Width / 2, gameHeight / 2 - 175, m_StartButton.Width, m_StartButton.Height);
            m_ExitRect = new Rectangle(gameWidth / 2 - m_ExitButton.Width / 2, gameHeight / 2, m_ExitButton.Width, m_ExitButton.Height);

            renderContext.SpriteBatch.Draw(m_StartButton, m_StartRect, Color.White);
            renderContext.SpriteBatch.Draw(m_ExitButton, m_ExitRect, Color.White);

            base.Draw2D(renderContext, drawBefore3D);
        }

        public bool HandleInput(RenderContext renderContext)
        {
            //Update inputManager
            m_InputManager.Update();

            var mousePos = new Vector2(renderContext.Input.CurrentMouseState.X, renderContext.Input.CurrentMouseState.Y);

            if (m_InputManager.GetAction((int)PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_StartRect))
            {
                Console.WriteLine("Start button");
                SceneManager.SetActiveScene("FinalScene");
                return true;
            }
            else if (m_InputManager.GetAction((int)PlayerInput.LeftClick).IsTriggered && CheckHitButton(mousePos, m_ExitRect))
            {
                Console.WriteLine("Exit button");
                ExitGame = true;
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
