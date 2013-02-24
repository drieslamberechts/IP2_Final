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
    class Target
    {
        // Variables
        ContentManager Content;

        Texture2D m_TexTarget;
        Rectangle m_RectTarget;

        double m_PresizeYPos;

        int m_WaveOffset;

        // Methods
        public Target(ContentManager content, Vector2 startPosition)
        {
            Content = content;
            
            //Load bitmap
            m_TexTarget = Content.Load<Texture2D>("cloud");
            m_RectTarget = new Rectangle((int)startPosition.X, (int)startPosition.Y, 192, 100);

            //Hold a more presize position of the rectangle
            m_PresizeYPos = m_RectTarget.Y;
            
            //Generate Offset for waves
            Random random = new Random();
            m_WaveOffset = random.Next(0, 6280);

            Initialize();
        }

        public void Initialize()
        {

        }

        public void Update(RenderContext renderContext)
        {
            // Waving Movement
            double toAdd = Math.Sin((((double)(renderContext.GameTime.TotalGameTime.Milliseconds + m_WaveOffset)) / 1000.0 + ((double)renderContext.GameTime.TotalGameTime.Seconds)) * 2);
            
            m_PresizeYPos += toAdd;
            m_RectTarget.Y = (int)m_PresizeYPos;
        }

        public void Draw(RenderContext renderContext)
        {
            renderContext.SpriteBatch.Draw(m_TexTarget, m_RectTarget, Color.White);
        }

        // GET FUNTIONS
        public Rectangle GetPosition() { return m_RectTarget; }

        // SET FUNTIONS
        public void OffsetPosition(int xPos, int yPos) { m_RectTarget.Offset(xPos,yPos); }
    }
}