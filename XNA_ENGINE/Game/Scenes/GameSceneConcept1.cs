using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNA_ENGINE;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using XNA_ENGINE.Engine.Scenegraph;
using XNA_ENGINE.Engine;
using IP2_Xna_Template.Objects;
using Microsoft.Xna.Framework.Media;

namespace XNA_ENGINE.Game
{
    public class GameSceneConcept1 : GameScene
    {
        ContentManager Content;
        SpriteBatch spriteBatch;

        Boolean m_bCanSwitchScene = true;

        // Background
        Texture2D m_TexBackground;
        Rectangle m_RectBackground;

        // Game Objects
        Player m_Player;
        Enemy m_Enemy;
        Bullet[] m_Bullets;

        // Music
        Song song;

        //DebugScreen
        SpriteFont m_DebugFont;
        Boolean m_bShowDebugScreen = true;


        KeyboardState m_OldKeyboardState;

        public GameSceneConcept1(ContentManager content)
            : base("GameSceneConcept1")
        {
            Content = content;
            m_DebugFont = Content.Load<SpriteFont>("Fonts/DebugFont");
        }

        public override void Initialize()
        {
            // Create a new Player
            m_Player = new Player(Content);
            m_Player.Initialize();

            // Create an Enemy
            m_Enemy = new Enemy(Content, new Vector2(800, 450));
            m_Enemy.Initialize();

            // Create Background
            m_TexBackground = Content.Load<Texture2D>("Backgrounds/PrimaryBackground");
            m_RectBackground = new Rectangle(0, 0, 1280, 720);

            // Set Up Music
            song = Content.Load<Song>("Music");
            MediaPlayer.Play(song);
            MediaPlayer.Volume = 0.05f;

            // Set the old state of the keyboard
            m_OldKeyboardState = Keyboard.GetState(PlayerIndex.One);

            base.Initialize();
        }

        public override void Update(RenderContext renderContext)
        {
            // Update The Game Objects
            m_Player.Update();
            if (m_Enemy != null) m_Enemy.Update();

            // Check if a bullets hits an enemy and destroy both (bullet and enemy)
            // Get Bullets from Player
            m_Bullets = m_Player.GetBullets();
            for (int t = 0; t < 10; ++t)
            {
                if (m_Bullets[t] != null && m_Enemy != null)
                {
                    // Check intersection based on rectangles
                    if (m_Bullets[t].GetPosition().Intersects(m_Enemy.GetPosition()))
                    {
                        // Delete both Objects if they hit
                        m_Enemy = null;
                        m_Bullets[t] = null;
                    }
                }
            }

            // CHECK FOR EXTRA PRESSED BUTTONS (PAUSE BUTTON, ...)
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState(PlayerIndex.One);


            //GamePad
            if (gamePadState.IsConnected)
            {
                if (gamePadState.Buttons.Start == ButtonState.Pressed && m_bCanSwitchScene)
                {
                    SceneManager.SetActiveScene("PauseScreen");
                    m_bCanSwitchScene = false;
                }

                if (gamePadState.Buttons.Start == ButtonState.Released && !m_bCanSwitchScene)
                    m_bCanSwitchScene = true;
            }

            //Keyboard
            if (keyboardState[Keys.P] == KeyState.Down && m_bCanSwitchScene)
            {
                SceneManager.SetActiveScene("PauseScreen");
                m_bCanSwitchScene = false;
            }

            if (keyboardState[Keys.P] == KeyState.Up && !m_bCanSwitchScene)
                m_bCanSwitchScene = true;

            //Debug
            if (keyboardState.IsKeyDown(Keys.D))
            {
                // If not down last update, key has just been pressed.
                if (!m_OldKeyboardState.IsKeyDown(Keys.D))
                {
                    m_bShowDebugScreen = !m_bShowDebugScreen;
                }
            }
            else if (m_OldKeyboardState.IsKeyDown(Keys.D))
            {
                // Key was down last update, but not down now, so
                // it has just been released.
            }

            // CREATE ENEMIES BASED ON THE POSITION OF THE CAMERA
            // THIS WILL INCREASE PREFORMANCE.
            // ALSO DELETE ENEMIES WHO ARE TO TE LEFT OF THE CAMERA AND AREN'T RENDERED ANYMORE

            m_OldKeyboardState = keyboardState;
            base.Update(renderContext);
        }

        public override void Draw2D(RenderContext renderContext, bool drawBefore3D)
        {
            spriteBatch = new SpriteBatch(renderContext.GraphicsDevice);

            // Draw Game Objects
            spriteBatch.Begin();
            // Background
            spriteBatch.Draw(m_TexBackground, m_RectBackground, Color.White);

            // Player & Possible Bullets
            m_Player.Draw(spriteBatch);

            // Enemies
            if (m_Enemy != null) m_Enemy.Draw(spriteBatch);

            //DebugScreen
            if (m_bShowDebugScreen) DrawDebugScreen();

            spriteBatch.End();

            base.Draw2D(renderContext, drawBefore3D);
        }

        public override void Draw3D(RenderContext renderContext)
        {
            base.Draw3D(renderContext);
        }

        private void DrawDebugScreen()
        {
            int yPos = 0;
            int offset = 15;
            spriteBatch.DrawString(m_DebugFont,"DEBUG:", new Vector2(10, yPos += offset), Color.Green);
            spriteBatch.DrawString(m_DebugFont, "Heroposition: (" + m_Player.m_Rectangle.X.ToString() + ", " + m_Player.m_Rectangle.Y.ToString() + ")", new Vector2(15, yPos += offset), Color.Green);
            spriteBatch.DrawString(m_DebugFont, "INSTRUCTIONS:", new Vector2(10, yPos += offset+10), Color.Green);
            spriteBatch.DrawString(m_DebugFont, "Disbale debug: D", new Vector2(15, yPos += offset), Color.Green);
            spriteBatch.DrawString(m_DebugFont, "Up and down: Z/S", new Vector2(15, yPos += offset), Color.Green);
            spriteBatch.DrawString(m_DebugFont, "Shoot: Spacebar", new Vector2(15, yPos += offset), Color.Green);
            spriteBatch.DrawString(m_DebugFont, "Pause game: P", new Vector2(15, yPos += offset), Color.Green);
        }

        //...
    }
}
