using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace XNA_ENGINE.Game.Objects
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
            m_RectBeat = new Rectangle((int)position.X, (int)position.Y, (int)m_TexBeat.Width, (int)m_TexBeat.Height);
        }

        public void Initialize()
        {
            
        }

        public void Update()
        {
 
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }
    }
}
