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

        public GameSceneConcept1(ContentManager content)
            : base("GameSceneConcept1")
        {
            Content = content;
        }

        public override void Initialize()
        {
            // Create a new Player
            m_Player = new Player(Content);
            m_Player.Initialize();

            // Create an Enemy
            m_Enemy = new Enemy(Content);
            m_Enemy.Initialize();

            // Create Background
            m_TexBackground = Content.Load<Texture2D>("Backgrounds/PrimaryBackground");
            m_RectBackground = new Rectangle(0, 0, 1280, 720);

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

            // CREATE ENEMIES BASED ON THE POSITION OF THE CAMERA
            // THIS WILL INCREASE PREFORMANCE.
            // ALSO DELETE ENEMIES WHO ARE TO TE LEFT OF THE CAMERA AND AREN'T RENDERED ANYMORE

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
            spriteBatch.End();

            base.Draw2D(renderContext, drawBefore3D);
        }

        public override void Draw3D(RenderContext renderContext)
        {
            base.Draw3D(renderContext);
        }

        //...
    }
}
