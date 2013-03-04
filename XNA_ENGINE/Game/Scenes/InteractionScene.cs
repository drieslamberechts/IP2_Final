using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using XNA_ENGINE.Engine;
using XNA_ENGINE.Engine.Scenegraph;

namespace XNA_ENGINE.Game.Scenes
{
    class InteractionScene : GameScene
    {
        // ------------------------------
        // Variables
        // ------------------------------
        private readonly ContentManager m_Content;

        readonly SpriteFont m_DebugFont;
        int m_Attackers;
        int m_Defenders;
        private bool m_bDrawText = false;
        private int m_Countdown = 200;

        private Texture2D m_TexAttackers, m_TexDefenders, m_TexBackground;
        private Rectangle m_RectAttackers, m_RectDefenders, m_RectBackground;

        private int m_AttackersDice, m_DefendersDice;

        // ------------------------------
        // Methods
        // ------------------------------
        public InteractionScene(ContentManager content)
            :base("InteractionScene")
        {
            m_Content = content;
            m_DebugFont = m_Content.Load<SpriteFont>("Fonts/DebugFont");
        }

        public void Initialize(int attackers, int defenders)
        {
            m_Attackers = attackers;
            m_Defenders = defenders;

            m_TexAttackers = m_Content.Load<Texture2D>("megaman");
            m_TexDefenders = m_Content.Load<Texture2D>("megaman");

            m_RectAttackers = new Rectangle(0, 0, m_TexAttackers.Width, m_TexAttackers.Height);
            m_RectDefenders = new Rectangle(0, 0, m_TexDefenders.Width, m_TexDefenders.Height);
            m_RectBackground = new Rectangle(0, 0, 1400, 1000);
        }

        // Update
        public override void Update(RenderContext renderContext)
        {
            if (m_Attackers > 0 && m_Defenders > 0)
            {
                ThrowDice();
                Die();
            }

            if (m_Attackers == 0 || m_Defenders == 0)
            {
                m_bDrawText = true;
            }

            if (m_bDrawText) m_Countdown--;
            if (m_Countdown == 0) SceneManager.SetActiveScene("GameSceneConcept2");
        }

        // Draw
        public override void Draw2D(RenderContext renderContext, bool drawBefore3D)
        {
            //---------------------------------
            // INFO ABOUT ENCOUNTER
            //---------------------------------
            renderContext.SpriteBatch.DrawString(m_DebugFont, "-- INTERACTION SCREEN --", new Vector2(10, 10), Color.White);

            // Information about attacker
            renderContext.SpriteBatch.DrawString(m_DebugFont, "-- Attackers --", new Vector2(10, 40), Color.White);
            renderContext.SpriteBatch.DrawString(m_DebugFont, "Nr of Attackers: " + Convert.ToString(m_Attackers), new Vector2(10, 55), Color.White);

            // Information about defenders
            renderContext.SpriteBatch.DrawString(m_DebugFont, "-- Defenders --", new Vector2(10, 85), Color.White);
            renderContext.SpriteBatch.DrawString(m_DebugFont, "Nr of Defenders: " + Convert.ToString(m_Defenders), new Vector2(10, 100), Color.White);

            renderContext.SpriteBatch.DrawString(m_DebugFont, "-- Outcome Dice --", new Vector2(10, 130), Color.White);
            renderContext.SpriteBatch.DrawString(m_DebugFont, "Attackers throw: " + Convert.ToString(m_AttackersDice), new Vector2(10, 145), Color.White);
            renderContext.SpriteBatch.DrawString(m_DebugFont, "Defenders throw: " + Convert.ToString(m_DefendersDice), new Vector2(10, 160), Color.White);

            if (m_bDrawText)
                renderContext.SpriteBatch.DrawString(m_DebugFont,
                                                     "Attackers: " + Convert.ToString(m_Attackers) + " | Defenders: " +
                                                     Convert.ToString(m_Defenders), new Vector2(10, 900), Color.White);

            //---------------------------------
            // ACTUAL DRAWING
            //---------------------------------

            // Draw Attackers
            for (int t = 0; t < m_Attackers; ++t)
            {
                m_RectAttackers.X = 200 + ((m_RectAttackers.Width + 30) * t);
                m_RectAttackers.Y = 100;
                renderContext.SpriteBatch.Draw(m_TexAttackers, m_RectAttackers, Color.White);
            }

            // Draw Defenders
            for (int t = 0; t < m_Defenders; ++t)
            {
                m_RectDefenders.X = 200 + ((m_RectDefenders.Width + 30) * t);
                m_RectDefenders.Y = 400;
                renderContext.SpriteBatch.Draw(m_TexDefenders, m_RectDefenders, Color.White);
            }
        }

        // Returns the outcome of the last Interaction
        public List<int> GetOutcome()
        {
            var outcome = new List<int>();

            outcome[0] = m_Attackers;
            outcome[1] = m_Defenders;

            return outcome;
        }

        // Throws Dice for both the Attackers as Defenders
        private void ThrowDice()
        {
            var random = new Random();

            for (int t = 0; t < m_Attackers; ++t)
            {
                int attackersDice = random.Next(1, 6);
                if (attackersDice > m_AttackersDice)
                    m_AttackersDice = attackersDice;
            }

            for (int t = 0; t < m_Defenders; ++t)
            {
                int defendersDice = random.Next(1, 6);
                if (defendersDice > m_DefendersDice)
                    m_DefendersDice = defendersDice;
            }
        }

        private void Die()
        {
            if (m_DefendersDice > m_AttackersDice) m_Attackers--;
            else if (m_DefendersDice == m_AttackersDice) m_Attackers--;
            else m_Defenders--;
        }
    }
}
