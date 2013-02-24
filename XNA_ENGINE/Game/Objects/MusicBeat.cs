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
    class MusicBeat
    {
        // Variable
        ContentManager Content;

        Texture2D m_TexBeat;
        Rectangle m_RectBeat;

        // Methods
        public MusicBeat(ContentManager content, Vector2 position)
        {
            Content = content;

            m_TexBeat = Content.Load<Texture2D>("buttonBeat");
            m_RectBeat = new Rectangle((int)position.X, (int)position.Y, (int)m_TexBeat.Width/5, (int)m_TexBeat.Height/5);
        }

        public void Initialize()
        {
            
        }

        public void Update(RenderContext renderContext)
        {
 
        }

        public void Draw(RenderContext renderContext)
        {
            renderContext.SpriteBatch.Draw(m_TexBeat, m_RectBeat, Color.White);
        }

        // GET FUNTIONS
        public Rectangle GetPosition() { return m_RectBeat; }

        // SET FUNTIONS
        public void OffsetPosition(int xPos, int yPos) { m_RectBeat.Offset(xPos, yPos); }
    }
}
