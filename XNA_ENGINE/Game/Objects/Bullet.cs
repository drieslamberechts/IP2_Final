using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using XNA_ENGINE.Engine;

namespace IP2_Xna_Template.Objects
{
    class Bullet
    {
        // Variables
        ContentManager Content;

        Texture2D m_TextureBullet;
        Rectangle m_RectBullet;

        public Vector2 m_Position;

        Boolean m_bHomingMissile = false;

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

        public void Update(int posX, int posY)
        {
            m_RectBullet.X += posX;
            m_RectBullet.Y += posY;
            
            if (m_bHomingMissile)
            {
                if (m_RectBullet.X <= posX) m_RectBullet.X += 10;
                if (m_RectBullet.Y >= posY) m_RectBullet.Y -= 10;
                else if (m_RectBullet.Y <= posY) m_RectBullet.Y += 10;
            }
        }

        public void Draw(RenderContext renderContext)
        {
            renderContext.SpriteBatch.Draw(m_TextureBullet, m_RectBullet, Color.White);
        }

        // GET FUNCTIONS
        // Returns Rectangle (Check Position or Collisions)
        public Rectangle GetPosition() { return m_RectBullet; }
        public bool IsHomingMissile() { return m_bHomingMissile; }

        // SET FUNCIONS
        // Sets the bullet to a homing missile
        public void SetHomingMissile() { m_bHomingMissile = true; }
    }
}
