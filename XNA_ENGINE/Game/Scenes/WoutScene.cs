using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XNA_ENGINE.Engine.Objects;
using XNA_ENGINE.Engine;
using XNA_ENGINE.Engine.Scenegraph;
using XNA_ENGINE.Game.Objects;
//using VampirePuzzle.Framework;

namespace XNA_ENGINE.Game.Scenes
{
    class WoutScene : GameScene
    {
        private GameSprite m_SmileySprite;

        public WoutScene() : base("WoutScene") { }

        public override void Initialize()
        {
            m_SmileySprite = new GameSprite("protagtransparant");
           // m_SmileySprite.Translate(100,100);
            m_SmileySprite.Scale(0.2f, 0.2f);
          //  m_SmileySprite.Rotate(45);
            AddSceneObject(m_SmileySprite);
        }
        
        public override void Update(RenderContext renderContext)
        {
            //m_SmileySprite.Translate(-m_SmileySprite.Width * m_SmileySprite.LocalScale.X / 2, -m_SmileySprite.Width * m_SmileySprite.LocalScale.Y / 2);
            //m_SmileySprite.Rotate(180);
           
           // m_SmileySprite.Rotate(renderContext.GameTime.TotalGameTime);

          //m_SmileySprite.Translate(200, 200);
            base.Update(renderContext);
        }
    }
}
