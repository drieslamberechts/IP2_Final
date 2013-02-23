using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IP2_Xna_Template.Objects
{
    class Bullet
    {
        // Variables
        ContentManager Content;

        Texture2D m_TextureBullet;
        Rectangle m_RectBullet;

        public Vector2 m_Position;

        // Methods
        public Bullet(ContentManager content)
        {
            Content = content;
            m_TextureBullet = Content.Load<Texture2D>("bullet");
        }

        public void InitializePos()
        {
            m_RectBullet = new Rectangle((int)m_Position.X, (int)m_Position.Y, 30, 7);
        }

        public void Update()
        {
            m_RectBullet.X += 10;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(m_TextureBullet, m_RectBullet, Color.White);
        }

        // GET FUNCTIONS
        // Returns Rectangle (Check Position or Collisions)
        public Rectangle GetPosition() { return m_RectBullet; }
    }
}
