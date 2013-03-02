using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XNA_ENGINE.Engine.Scenegraph;
using XNA_ENGINE.Game.Scenes;

//Test

namespace XNA_ENGINE.Game
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MainGame : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            Window.Title = "IP2";

            // Dries Test
            graphics.PreferredBackBufferHeight = 1000;
            graphics.PreferredBackBufferWidth = 1400;
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
            //SceneManager.AddGameScene(new GameSceneConcept1(Content));
            SceneManager.AddGameScene(new GameSceneConcept2(Content));
            SceneManager.AddGameScene(new PauseScreen(Content));
            SceneManager.AddGameScene(new WoutScene());


            SceneManager.SetActiveScene("GameSceneConcept2");
            //SceneManager.SetActiveScene("GameSceneConcept2");
            //SceneManager.SetActiveScene("WoutScene");
            //SceneManager.SetActiveScene("DriesScene");
            //SceneManager.SetActiveScene("FiddleDemoScene");
            //SceneManager.SetActiveScene("GameAnimatedSpriteDemo");
            //SceneManager.SetActiveScene("GameModelDemo");
            //SceneManager.SetActiveScene("GoingWildDemo");
            //SceneManager.SetActiveScene("GameSpriteDemo");
            
           
            SceneManager.Initialize();

            IsMouseVisible = true;

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
                SceneManager.SetActiveScene("GameSceneConcept1");
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.D2))
                SceneManager.SetActiveScene("GameSceneConcept2");
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.D3))
                SceneManager.SetActiveScene("WoutScene");
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.D4))
                SceneManager.SetActiveScene("InteractionScene");
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.D5))
                SceneManager.SetActiveScene("FiddleDemoScene");
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.D6))
                SceneManager.SetActiveScene("GameAnimatedSpriteDemo");
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.D7))
                SceneManager.SetActiveScene("GoingWildDemo");
            if (Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.D8))
                SceneManager.SetActiveScene("GameSpriteDemo");

            // TODO: Add your update logic here
            SceneManager.Update(gameTime);

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
