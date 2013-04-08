using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XNA_ENGINE.Engine.Helpers;
using XNA_ENGINE.Engine.Scenegraph;
using XNA_ENGINE.Game.Objects;
using XNA_ENGINE.Game.Scenes;

namespace XNA_ENGINE.Game
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MainGame : Microsoft.Xna.Framework.Game
    {
        enum PlayerInput
        {
            FullScreen
        }

        private InputManager m_InputManager;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.Title = "IP2";

            // Dries Test
            graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2;
            graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2;
            //graphics.IsFullScreen = true;
            graphics.ApplyChanges();
            // End Dries Test

            SceneManager.MainGame = this;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            SceneManager.AddGameScene(new FinalScene(Content));

            SceneManager.SetActiveScene("FinalScene");

            SceneManager.Initialize();

            IsMouseVisible = true;

            //INPUT
            m_InputManager = new InputManager();

            // Initialize InputAction to switch Screen
            InputAction Tap = new InputAction((int)PlayerInput.FullScreen, TriggerState.Pressed);
            Tap.KeyButton = Keys.F;

            m_InputManager.MapAction(Tap);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            SceneManager.RenderContext.GraphicsDevice = graphics.GraphicsDevice;
            SceneManager.RenderContext.SpriteBatch = spriteBatch;

            SceneManager.LoadContent(Content);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();


            //Change the active scene
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.D1))
                SceneManager.SetActiveScene("FinalScene");
       

            // TODO: Add your update logic here
            SceneManager.Update(gameTime);

            //-----------------------------------------------
            // SWITCH TO FULLSCREEN OR WINDOWED
            //-----------------------------------------------
            //INPUT
            m_InputManager.Update();

            if (m_InputManager.GetAction((int)PlayerInput.FullScreen).IsTriggered && graphics.IsFullScreen == false)
            {
                graphics.IsFullScreen = true;
                graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
                graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
                graphics.ApplyChanges();
            }

            else if (m_InputManager.GetAction((int)PlayerInput.FullScreen).IsTriggered && graphics.IsFullScreen)
            {
                graphics.IsFullScreen = false;
                graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height / 2;
                graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width / 2;
                graphics.ApplyChanges();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here
            SceneManager.Draw();

            base.Draw(gameTime);
        }
    }
}
