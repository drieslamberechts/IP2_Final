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

        // Methods
        public Enemy(ContentManager content)
        {
            Content = content;
        }

        public void Initialize()
        {
            m_TexEnemy = Content.Load<Texture2D>("ball");
            m_RectEnemy = new Rectangle(800, 350, 64, 64);
        }

        public void Update()
        { }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(m_TexEnemy, m_RectEnemy, Color.White);
        }

        // GET FUNTIONS
        public Rectangle GetPosition() { return m_RectEnemy; }
    }
}
