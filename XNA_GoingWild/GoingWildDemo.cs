using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace XNA_GoingWild
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class GoingWildDemo : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private const int HOR_ACCELERATION = 300;
        private const int HOR_MAX_SPEED = 200;

        private const int VER_ACCELERATION = 1000;

        private const int GROUND_LEVEL = 300;
        private const int JUMP_SPEED = 400;

        private Texture2D _texture;
        private int _direction = 1;
        private Vector2 _position = new Vector2(200, GROUND_LEVEL);
        private Vector2 _velocity = Vector2.Zero;

        public GoingWildDemo()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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

            // TODO: use this.Content to load your game content here
            _texture = Content.Load<Texture2D>("Hero");
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

            // TODO: Add your update logic here
            KeyboardState currKeyboardState = Keyboard.GetState();

            if (currKeyboardState.IsKeyDown(Keys.Right))
            {
                _direction = 1;
                _velocity.X += (float)(HOR_ACCELERATION * gameTime.ElapsedGameTime.TotalSeconds);
            }
            else if (currKeyboardState.IsKeyDown(Keys.Left))
            {
                _direction = -1;
                _velocity.X -= (float)(HOR_ACCELERATION * gameTime.ElapsedGameTime.TotalSeconds);
            }
            else
            {
                _velocity.X -= _direction * (float)(HOR_ACCELERATION * gameTime.ElapsedGameTime.TotalSeconds);

                if (_direction > 0 && _velocity.X < 0) _velocity.X = 0;
                else if (_direction < 0 && _velocity.X > 0) _velocity.X = 0;
            }

            _velocity.X = MathHelper.Clamp(_velocity.X, -HOR_MAX_SPEED, HOR_MAX_SPEED);


            if (currKeyboardState.IsKeyDown(Keys.Space) && _position.Y == GROUND_LEVEL)
            {
                _velocity.Y -= VER_ACCELERATION/2.0f;
            }
            else _velocity.Y += VER_ACCELERATION*(float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_position.Y > GROUND_LEVEL)
            {
                _velocity.Y = 0;
                _position.Y = GROUND_LEVEL;
            }


            _position += _velocity*(float)gameTime.ElapsedGameTime.TotalSeconds;

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
            spriteBatch.Begin();
            spriteBatch.Draw(_texture, _position, null, Color.White, 0, Vector2.Zero, Vector2.One, (_direction == 1) ? SpriteEffects.None : SpriteEffects.FlipHorizontally, 0);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
