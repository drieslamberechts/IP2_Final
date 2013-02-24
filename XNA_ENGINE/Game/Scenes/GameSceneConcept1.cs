﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNA_ENGINE;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

using XNA_ENGINE.Engine;
using XNA_ENGINE.Engine.Scenegraph;
using XNA_ENGINE.Engine.Objects;

using IP2_Xna_Template.Objects;
using Microsoft.Xna.Framework.Media;

namespace XNA_ENGINE.Game
{
    public class GameSceneConcept1 : GameScene
    {
        ContentManager Content;

        // PauseScreen
        Boolean m_bCanSwitchScene = true;

        // Background
        Texture2D m_TexBackground;
        Rectangle m_RectBackground;

        // Game Objects
        Player m_Player;
        Bullet[] m_Bullets;

        private List<Target> m_TargetList = new List<Target>();

        // Music
        Song song;
        Boolean m_bMusicPlaying = true;
        Boolean m_bCanChangeMusic = true;

        // DebugScreen
        SpriteFont m_DebugFont;
        Boolean m_bShowDebugScreen = true;

        // Old Keyboard State
        KeyboardState m_OldKeyboardState;

        // Scroll speed of level
        float m_BackgroundScrollSpeed = 5;

        // Time interval for spawning enemies
        private double m_TimeIntervalEnemies = 0;
        private double m_TimeBetweenEachSpawn = 0.8;

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

        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);
        }

        public override void Update(RenderContext renderContext)
        {
            // Update The Game Objects
            m_Player.Update();

            // Update the spawntiming
            m_TimeIntervalEnemies += (double)renderContext.GameTime.ElapsedGameTime.Milliseconds/1000;
            if (m_TimeIntervalEnemies >= m_TimeBetweenEachSpawn)
            {
                Random random = new Random();
                int randomNumber = random.Next(50, 500);

                m_TimeIntervalEnemies = 0;
                m_TargetList.Add(new Target(Content, new Vector2(1400, randomNumber)));
            }

            //Scroll and destroy targets
            HandleTargets(renderContext);

            //Scroll background
            m_RectBackground.Offset(new Point(-(int)m_BackgroundScrollSpeed, 0));
            if (m_RectBackground.Left <= -1280) m_RectBackground.Offset(new Point(1280, 0));


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

                if (gamePadState.Buttons.Y == ButtonState.Pressed && m_bCanChangeMusic == true)
                {
                    m_bMusicPlaying = !m_bMusicPlaying;
                    m_bCanChangeMusic = false;
                }
                if (gamePadState.Buttons.Y == ButtonState.Released) m_bCanChangeMusic = true;
                
                switch (m_bMusicPlaying)
                {
                    case true:
                        if (MediaPlayer.State == MediaState.Paused) MediaPlayer.Resume();
                        break;
                    case false:
                        MediaPlayer.Pause();
                        break;
                }
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
            // Draw Game Objects
            // Background
            renderContext.SpriteBatch.Draw(m_TexBackground, m_RectBackground, Color.White);

            Rectangle rectangle;
            rectangle = m_RectBackground;
            rectangle.Offset(new Point(1280, 0));
            renderContext.SpriteBatch.Draw(m_TexBackground, rectangle, Color.White);

            // Player & Possible Bullets
            m_Player.Draw(renderContext);

            // Targets
            DrawTargets(renderContext);

            //DebugScreen
            if (m_bShowDebugScreen) DrawDebugScreen(renderContext);

            base.Draw2D(renderContext, drawBefore3D);
        }

        public override void Draw3D(RenderContext renderContext)
        {
            base.Draw3D(renderContext);
        }

        private void DrawDebugScreen(RenderContext renderContext)
        {
            int yPos = 0;
            int offset = 15;

            renderContext.SpriteBatch.DrawString(m_DebugFont, "DEBUG:", new Vector2(10, yPos += offset), Color.Green);
            renderContext.SpriteBatch.DrawString(m_DebugFont, "HeroPosition: " + m_Player.m_Rectangle.Location.ToString(), new Vector2(15, yPos += offset), Color.Green);
            renderContext.SpriteBatch.DrawString(m_DebugFont, "CameraWorldPosition: " + renderContext.Camera.WorldPosition.ToString(), new Vector2(15, yPos += offset), Color.Green);
            renderContext.SpriteBatch.DrawString(m_DebugFont, "CameraLocalPosition: " + renderContext.Camera.LocalPosition.ToString(), new Vector2(15, yPos += offset), Color.Green);
            renderContext.SpriteBatch.DrawString(m_DebugFont, "Time Interval: " + m_TimeIntervalEnemies.ToString(), new Vector2(15, yPos += offset), Color.Green);
            renderContext.SpriteBatch.DrawString(m_DebugFont, "TargetCount: " + m_TargetList.Count.ToString(), new Vector2(15, yPos += offset), Color.Green);
            
            renderContext.SpriteBatch.DrawString(m_DebugFont, "INSTRUCTIONS:", new Vector2(10, yPos += offset + 10), Color.Green);
            renderContext.SpriteBatch.DrawString(m_DebugFont, "Disbale debug: D", new Vector2(15, yPos += offset), Color.Green);
            renderContext.SpriteBatch.DrawString(m_DebugFont, "Up and down: Z/S", new Vector2(15, yPos += offset), Color.Green);
            renderContext.SpriteBatch.DrawString(m_DebugFont, "Shoot: Spacebar", new Vector2(15, yPos += offset), Color.Green);
            renderContext.SpriteBatch.DrawString(m_DebugFont, "Pause game: P", new Vector2(15, yPos += offset), Color.Green);
        }

        private void HandleTargets(RenderContext renderContext)
        {
            m_Bullets = m_Player.GetBullets();

            List<Target> targetsToDelete = new List<Target>();

            foreach (Target target in m_TargetList)
            {
                target.Update(renderContext);

                //Move the enemies
                target.OffsetPosition(-(int)m_BackgroundScrollSpeed, 0);

                //Delete the enemy if it reached the end of the screen
                if (target.GetPosition().X < -200) targetsToDelete.Add(target);

                //Go over all bullets and check collision with the enemy
                for (int t = 0; t < 10; ++t)
                {
                    if (m_Bullets[t] != null && target != null)
                    {
                        // Check intersection based on rectangles
                        if (m_Bullets[t].GetPosition().Intersects(target.GetPosition()))
                        {
                            // Delete both Objects if they hit
                            // Add to list the objects that need to be deleted
                            targetsToDelete.Add(target);
                            m_Bullets[t] = null;
                        }
                    }
                }
            }

            //Delete the enemies
            foreach (Target target in targetsToDelete)
            {
                m_TargetList.Remove(target);
            }


        }

        private void DrawTargets(RenderContext renderContext)
        {
            foreach (Target target in m_TargetList)
            {
                if (target != null) target.Draw(renderContext);
            }
        }
        //...
    }
}
