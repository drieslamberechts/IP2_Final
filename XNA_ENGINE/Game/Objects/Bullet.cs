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
        //test
        // Variables
        ContentManager Content;

        Texture2D m_TextureBullet;
        Rectangle m_RectBullet;

        public Vector2 m_Position;

        Boolean m_bHomingMissile = false;

        private Target m_Target;

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
                Vector2 bulletVec = ConvertRectangleToVector2(m_RectBullet);

                Vector2 targetVec = ConvertRectangleToVector2(m_Target.GetPosition());
                Vector2 inbetweenVec = targetVec - bulletVec;

                bulletVec += inbetweenVec / 20;

                m_RectBullet.X = (int)bulletVec.X;
                m_RectBullet.Y = (int)bulletVec.Y;

            }
        }

        public void Draw(RenderContext renderContext)
        {
            renderContext.SpriteBatch.Draw(m_TextureBullet, m_RectBullet, Color.White);
        }

        private Vector2 ConvertRectangleToVector2(Rectangle pos)
        {
            Vector2 returnVector;
            returnVector.X = pos.X;
            returnVector.Y = pos.Y;
            return returnVector;
        }

        // GET FUNCTIONS
        // Returns Rectangle (Check Position or Collisions)
        public Rectangle GetPosition() { return m_RectBullet; }
        public bool IsHomingMissile() { return m_bHomingMissile; }

        // SET FUNCIONS
        // Sets the bullet to a homing missile
        public void SetHomingMissile(Target targetPos) { m_bHomingMissile = true; m_Target = targetPos; }
    }
}
