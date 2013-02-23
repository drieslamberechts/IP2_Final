using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IP2_Xna_Template.Objects
{
    class Enemy
    {
        // Variables
        ContentManager Content;

        Texture2D m_TexEnemy;
        Rectangle m_RectEnemy;

        Vector2 m_MovingPosition = new Vector2(0, 0);

        // Methods
        public Enemy(ContentManager content, Vector2 startPosition)
        {
            Content = content;
            m_RectEnemy = new Rectangle((int)startPosition.X, (int)startPosition.Y, 100, 100);
        }

        public void Initialize()
        {
            m_TexEnemy = Content.Load<Texture2D>("enemy");
        }

        public void Update()
        {
            // Waving Movement
            m_RectEnemy.Y += (int)m_MovingPosition.Y;
        
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(m_TexEnemy, m_RectEnemy, Color.White);
        }

        // GET FUNTIONS
        public Rectangle GetPosition() { return m_RectEnemy; }
    }
}
