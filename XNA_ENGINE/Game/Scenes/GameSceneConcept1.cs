using System;
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

using XNA_ENGINE.Game.Objects;

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

        // Controls
        GamePadState gamePadState;

        // Game Objects
        Player m_Player;
        Bullet[] m_Bullets;

        private List<Target> m_TargetList = new List<Target>();
        private List<MusicBeat> m_BeatList = new List<MusicBeat>();

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

        // Time interval for spawning targets
        private double m_TimeIntervalTarget = 0;
        private double m_TimeBetweenEachSpawnTarget = 0.8;

        // Time interval for spawning beats
        private double m_TimeIntervalBeat = 0;
        private double m_TimeBetweenEachSpawnBeat = 0.6;

        // Controller bool
        Boolean m_bUsingController = false;

        // ScoreStuff
        private double m_Accuracy;
        private double m_TotalScore;


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
          //  MediaPlayer.Play(song);
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
            // Update The Game Objects from Player
            m_Player.Update();
            SetPositionClosestTarget();

            //Handle all player input
            HandleInput(renderContext);

            // Update the spawntiming
            m_TimeIntervalTarget += (double)renderContext.GameTime.ElapsedGameTime.Milliseconds/1000;
            if (m_TimeIntervalTarget >= m_TimeBetweenEachSpawnTarget)
            {
                Random random = new Random();
                int randomNumber = random.Next(50, 500);

                m_TimeIntervalTarget = 0;

                m_TargetList.Add(new Target(Content, new Vector2(1400, randomNumber)));
            }

            m_TimeIntervalBeat += (double)renderContext.GameTime.ElapsedGameTime.Milliseconds / 1000;
            if (m_TimeIntervalBeat >= m_TimeBetweenEachSpawnBeat)
            {
                m_TimeIntervalBeat = 0;
                m_BeatList.Add(new MusicBeat(Content, new Vector2(1300, 500)));
            }

            //Scroll and destroy targets
            HandleTargets(renderContext);

            //Scroll and destroy beats
            HandleBeats(renderContext);

            //Scroll background
            m_RectBackground.Offset(-(int)m_BackgroundScrollSpeed, 0);
            if (m_RectBackground.Left <= -1280) m_RectBackground.Offset(1280, 0);

            
            base.Update(renderContext);
        }

        public override void Draw2D(RenderContext renderContext, bool drawBefore3D)
        {
            // Draw Game Objects
            // Background
            renderContext.SpriteBatch.Draw(m_TexBackground, m_RectBackground, Color.White);

            Rectangle rectangle;
            rectangle = m_RectBackground;
            rectangle.Offset(1280, 0);
            renderContext.SpriteBatch.Draw(m_TexBackground, rectangle, Color.White);

            // Player & Possible Bullets
            m_Player.Draw(renderContext);


            // Targets
            DrawTargets(renderContext);

            // Beats
            DrawBeats(renderContext);

            // DebugScreen
            if (m_bShowDebugScreen) DrawDebugScreen(renderContext);

            base.Draw2D(renderContext, drawBefore3D);
        }

        public override void Draw3D(RenderContext renderContext)
        {
            base.Draw3D(renderContext);
        }

        private void HandleInput(RenderContext renderContext)
        {
            gamePadState = GamePad.GetState(PlayerIndex.One);

            // CHECK FOR EXTRA PRESSED BUTTONS (PAUSE BUTTON, ...)
            KeyboardState keyboardState = renderContext.Input.CurrentKeyboardState;

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


            /*-----------------------------------------*/
            /*---------------PLAYER--------------------*/
            /*-----------------------------------------*/
            if (gamePadState.IsConnected) m_bUsingController = true;
            else m_bUsingController = false;

            Rectangle playerPos = m_Player.GetPosition();

            //GamePad
            if (m_bUsingController)
            {
                // UP or DOWN  Movement
                if (gamePadState.ThumbSticks.Left.Y > 0)
                    m_Player.SetPosition(playerPos.X, playerPos.Y - 3);
                if (gamePadState.ThumbSticks.Left.Y < 0)
                    m_Player.SetPosition(playerPos.X, playerPos.Y +3);
            }

            //Keyboard
            if (keyboardState[Keys.Z] == KeyState.Down)
                m_Player.SetPosition(playerPos.X, playerPos.Y - 3);
            if (keyboardState[Keys.S] == KeyState.Down)
                m_Player.SetPosition(playerPos.X, playerPos.Y + 3);

            // Fire a bullet
            if (gamePadState.Buttons.A == ButtonState.Pressed || keyboardState[Keys.Space] == KeyState.Down)
            {
                m_Player.FireBullet();
            }
                

            switch (m_bUsingController)
            {
                case true:
                    if (gamePadState.Buttons.A == ButtonState.Released)
                        m_Player.SetCanCreateBullet(true);
                    break;

                case false:
                    if (keyboardState[Keys.Space] == KeyState.Up)
                        m_Player.SetCanCreateBullet(true);
                    break;
            }

            m_OldKeyboardState = keyboardState;
        }

        private void DrawDebugScreen(RenderContext renderContext)
        {
            int yPos = 0;
            int offset = 15;

            renderContext.SpriteBatch.DrawString(m_DebugFont, "DEBUG:", new Vector2(10, yPos += offset), Color.Green);
            renderContext.SpriteBatch.DrawString(m_DebugFont, "Framerate: " + 1000.0/(double)renderContext.GameTime.ElapsedGameTime.Milliseconds, new Vector2(15, yPos += offset), Color.Green);
            renderContext.SpriteBatch.DrawString(m_DebugFont, "HeroPosition: " + m_Player.m_Rectangle.Location.ToString(), new Vector2(15, yPos += offset), Color.Green);
            renderContext.SpriteBatch.DrawString(m_DebugFont, "CameraWorldPosition: " + renderContext.Camera.WorldPosition.ToString(), new Vector2(15, yPos += offset), Color.Green);
            renderContext.SpriteBatch.DrawString(m_DebugFont, "CameraLocalPosition: " + renderContext.Camera.LocalPosition.ToString(), new Vector2(15, yPos += offset), Color.Green);
            renderContext.SpriteBatch.DrawString(m_DebugFont, "Time Interval: " + m_TimeIntervalTarget.ToString(), new Vector2(15, yPos += offset), Color.Green);
            renderContext.SpriteBatch.DrawString(m_DebugFont, "TargetCount: " + m_TargetList.Count.ToString(), new Vector2(15, yPos += offset), Color.Green);
            renderContext.SpriteBatch.DrawString(m_DebugFont, "BeatCount: " + m_BeatList.Count.ToString(), new Vector2(15, yPos += offset), Color.Green);
            
            renderContext.SpriteBatch.DrawString(m_DebugFont, "INSTRUCTIONS:", new Vector2(10, yPos += offset + 10), Color.Green);
            renderContext.SpriteBatch.DrawString(m_DebugFont, "Disbale debug: D", new Vector2(15, yPos += offset), Color.Green);
            renderContext.SpriteBatch.DrawString(m_DebugFont, "Up and down: Z/S", new Vector2(15, yPos += offset), Color.Green);
            renderContext.SpriteBatch.DrawString(m_DebugFont, "Shoot: Spacebar", new Vector2(15, yPos += offset), Color.Green);
            renderContext.SpriteBatch.DrawString(m_DebugFont, "Pause game: P", new Vector2(15, yPos += offset), Color.Green);

            renderContext.SpriteBatch.DrawString(m_DebugFont, "Accuracy: " + m_Accuracy, new Vector2(200,m_Player.GetPosition().Y-20), Color.Yellow);
            renderContext.SpriteBatch.DrawString(m_DebugFont, "Score: " + m_TotalScore, new Vector2(200, m_Player.GetPosition().Y - 35), Color.Yellow);
            
            

        }

        //TARGETS functions
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
                for (int t = 0; t < 20; ++t)
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

        //BEATS functions
        private void HandleBeats(RenderContext renderContext)
        {
            m_Bullets = m_Player.GetBullets();

            List<MusicBeat> beatsToDelete = new List<MusicBeat>();

            foreach (MusicBeat beat in m_BeatList)
            {
                beat.Update(renderContext);

                //Move the enemies
                beat.OffsetPosition(-(int)m_BackgroundScrollSpeed, 0);

                //Delete the enemy if it reached the end of the screen
                if (beat.GetPosition().X < -200) beatsToDelete.Add(beat);

                //Go over all bullets and check collisionz with the enemy
   
                // Delete beats if they aren't triggered and are behind the player
                if (beat.GetPosition().X + beat.GetPosition().Width < 0)
                {
                    // Delete both Objects if they hit
                    // Add to list the objects that need to be deleted
                    beatsToDelete.Add(beat);
                }

                // Delete the beat if the player is able to shoot on the beat of the music
                if (m_Player.GetPosition().Intersects(beat.GetPosition()) && (gamePadState.Buttons.A == ButtonState.Pressed || m_OldKeyboardState.IsKeyDown(Keys.Space)))
                {
                    m_Accuracy = ((m_Player.GetPosition().X+140 - beat.GetPosition().X)-100)*-1;

                    m_TotalScore += m_Accuracy;

                    beatsToDelete.Add(beat);

                    // Sets the last created bullet shot to Homing Missile
                    Bullet latestBullet = m_Player.GetBullets().ElementAt<Bullet>(m_Player.GetLastBullet());
                    if (latestBullet != null)
                    {
                        Target targetToFollow = GetNearestTarget(latestBullet);

                        latestBullet.SetHomingMissile(targetToFollow);
                    }
                        
                }
            }

            //Delete the enemies
            foreach (MusicBeat beats in beatsToDelete)
            {
                m_BeatList.Remove(beats);
            }
        }
        private void DrawBeats(RenderContext renderContext)
        {
            foreach (MusicBeat beat in m_BeatList)
            {
                if (beat != null) beat.Draw(renderContext);
            }
        }

        private void SetPositionClosestTarget()
        {
            Rectangle bufferPos;
            bufferPos = new Rectangle(300, 0, 1, 1);

            foreach (Target target in m_TargetList)
            {
                if (target.GetPosition().X < 200 && target.GetPosition().X >= 100)
                {
                    bufferPos = target.GetPosition();
                    m_Player.SetPositionTarget(bufferPos.X, bufferPos.Y);
                    break;
                }
            }
        }

        private Target GetNearestTarget(Bullet bullet)
        { 
            Target returnTarget;
            returnTarget = m_TargetList.ElementAt<Target>(0);

            Vector2 posBullet;
            posBullet.X = bullet.GetPosition().X + bullet.GetPosition().Width;
            posBullet.Y = bullet.GetPosition().Y + bullet.GetPosition().Height;

            Vector2 oldDistance;
            oldDistance.X = 10000;
            oldDistance.Y = 10000;
            Vector2 posTarget;

            foreach (Target target in m_TargetList)
            {
                posTarget.X = target.GetPosition().X;
                posTarget.Y = target.GetPosition().Y;

                if ((posBullet - posTarget).Length() < oldDistance.Length())
                {
                    oldDistance = posBullet - posTarget;
                    returnTarget = target;
                }
            }

            return returnTarget;
        }
    }

    
}
