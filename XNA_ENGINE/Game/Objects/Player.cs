using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using XNA_ENGINE.Engine;

namespace IP2_Xna_Template.Objects
{
    class Player
    {
        // Variables
        ContentManager Content;

        Texture2D m_Texture;
        public Rectangle m_Rectangle;

        Bullet[] m_Bullets;
        static int MAX_BULLETS = 20;
        int currentBullets = 0;
        Boolean m_CanCreateBullet = true;
        int m_PositionLastBullet;

        Rectangle m_PositionTarget;

        // Methods
        public Player(ContentManager content)
        {
            Content = content;
        }

        public void Initialize()
        {
            m_Texture = Content.Load<Texture2D>("protagtransparant");
            m_Rectangle = new Rectangle(30, 220, 144, 72);

            m_Bullets = new Bullet[MAX_BULLETS];

            //Keyboard.GetState(PlayerIndex.One).IsKeyDown(Keys.D1);
        }

        public void Update()
        {
            // Delete Bullets
            // Bullets get deleted when they aren't rendered on the screen (if they are to far in the X-position so they are out of the screen)
            for (int t = 0; t < MAX_BULLETS; ++t)
            {
                if (m_Bullets[t] != null)
                {
                    if (m_Bullets[t].GetPosition().X >= 1280) m_Bullets[t] = null;
                }
            }

            // UPDATE POSITION BULLETS
            for (int t = 0; t < MAX_BULLETS; ++t)
            {
                // Check if the bullet is a Homing Missile
                if (m_Bullets[t] != null)
                {
                    if (!m_Bullets[t].IsHomingMissile()) m_Bullets[t].Update(10, 0);
                    else m_Bullets[t].Update(m_PositionTarget.X, m_PositionTarget.Y);
                }
            }
        }

        public void Draw(RenderContext renderContext)
        {
            // This is all called in the MainScene (which is the actual Game)

            // The player gets drawn
            renderContext.SpriteBatch.Draw(m_Texture, m_Rectangle, Color.White);

            // A loop which checks for bullets that exist and if so draws them
            for (int t = 0; t < MAX_BULLETS; ++t)
            {
                if (m_Bullets[t] != null)
                {
                    m_Bullets[t].Draw(renderContext);
                }
            }
        }

        public void FireBullet()
        {
            if (m_CanCreateBullet)
            {
                if (currentBullets < MAX_BULLETS)
                {
                    // Check for the first place that is empty in the bullet array (maximum of 10 bullets in the scene)
                    for (int t = 0; t < MAX_BULLETS; ++t)
                    {
                        if (m_Bullets[t] == null)
                        {
                            // Create the bullet and go out the for loop
                            m_Bullets[t] = new Bullet(Content);
                            m_Bullets[t].m_Position = new Vector2(m_Rectangle.X + 110, m_Rectangle.Y + 35);
                            m_Bullets[t].InitializePos();
                            m_PositionLastBullet = t;
                            break;
                        }
                    }
                }
            }

            // Because the Loop would otherwise make 10 Bullets at a time you have to set a boolean which enables or
            // diables the creation of bullets
            // When the Shoot-Button (A) is released this variable is set to true so another bullet can be created
            m_CanCreateBullet = false;
        }

        // GET FUNCTIONS
        public Bullet[] GetBullets() { return m_Bullets; }
        public Rectangle GetPosition() { return m_Rectangle; }
        public int GetLastBullet() { return m_PositionLastBullet; }

        // SET FUNCTIONS
        public void SetPosition(int posX, int posY) { m_Rectangle.X = posX; m_Rectangle.Y = posY; }
        public void SetCanCreateBullet(bool value) { m_CanCreateBullet = value; }
        public void SetPositionTarget(int posX, int posY) { m_PositionTarget.X = posX; m_PositionTarget.Y = posY; }

    }
}
