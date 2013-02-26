using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNA_ENGINE.Engine;
using XNA_ENGINE.Engine.Scenegraph;

namespace XNA_ENGINE.Game.Scenes
{
    class InteractionScene : GameScene
    {
        // ------------------------------
        // Variables
        // ------------------------------
        private ContentManager Content;

        readonly SpriteFont m_DebugFont;
        int m_Attackers;
        int m_Defenders;

        // ------------------------------
        // Methods
        // ------------------------------
        public InteractionScene(ContentManager content)
            :base("InteractionScene")
        {
            Content = content;
            m_DebugFont = Content.Load<SpriteFont>("Fonts/DebugFont");
        }

        public void Initialize(int attackers, int defenders)
        {
            m_Attackers = attackers;
            m_Defenders = defenders;
        }

        // Update
        public override void Update(RenderContext renderContext)
        {

        }

        // Draw
        public override void Draw2D(RenderContext renderContext, bool drawBefore3D)
        {
            // TEST DATA
            //m_Attackers = 15;
            //m_Defenders = 5;

            // Draw Text
            renderContext.SpriteBatch.DrawString(m_DebugFont, "-- INTERACTION SCREEN --", new Vector2(10, 10), Color.White);

            // Information about attacker
            renderContext.SpriteBatch.DrawString(m_DebugFont, "-- Attackers --", new Vector2(10, 40), Color.White);
            renderContext.SpriteBatch.DrawString(m_DebugFont, "Nr of Attackers: ", new Vector2(10, 55), Color.White);
            renderContext.SpriteBatch.DrawString(m_DebugFont, Convert.ToString(m_Attackers), new Vector2(150, 55), Color.White);

            // Information about defenders
            renderContext.SpriteBatch.DrawString(m_DebugFont, "-- Defenders --", new Vector2(10, 85), Color.White);
            renderContext.SpriteBatch.DrawString(m_DebugFont, "Nr of Defenders: ", new Vector2(10, 100), Color.White);
            renderContext.SpriteBatch.DrawString(m_DebugFont, Convert.ToString(m_Defenders), new Vector2(150, 100), Color.White);
        }

        public List<int> GetOutcome()
        {
            var outcome = new List<int>();

            outcome[0] = m_Attackers;
            outcome[1] = m_Defenders;

            return outcome;
        }
    }
}
