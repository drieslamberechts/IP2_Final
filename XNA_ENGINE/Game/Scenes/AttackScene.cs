using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using XNA_ENGINE.Engine;
using XNA_ENGINE.Engine.Scenegraph;

using XNA_ENGINE.Engine.Helpers;
using XNA_ENGINE.Game.Managers;
using XNA_ENGINE.Game.Objects;

namespace XNA_ENGINE.Game.Scenes
{
    class AttackScene : GameScene
    {
        public enum PlayerInput
        {
            LeftClick,
            RightClick,
            ScrollWheelDown,
            ToggleCreativeMode
        }

        private static ContentManager m_Content;
        private readonly SpriteFont m_DebugFont;
        private float m_ElapseTime;
        private decimal m_FrameCounter;
        private decimal m_Fps;

        private static InputManager m_InputManager;

        // Player and AI
        private Army m_Attacker, m_Defender;

        // Dice
        private int m_AttackersDice;
        private int m_DefendersDice;
        private bool m_bEnd;
        private bool m_AttackerWon;

        public AttackScene(ContentManager content, Army attackerArmy, Army defenderArmy)
            : base("AttackScene")
        {
            m_bEnd = false;
            m_AttackerWon = false;

            //CONTENT
            m_Content = content;

            // SET PLAYER AND AI
            m_Attacker = attackerArmy;
            m_Defender = defenderArmy;

            // FONT
            m_DebugFont = m_Content.Load<SpriteFont>("Fonts/DebugFont");

            // Execute Everything in the Scene
            Start();
        }

        public void Start()
        {
            //Input manager + input
            m_InputManager = new InputManager();

            var rightClick = new InputAction((int)PlayerInput.RightClick, TriggerState.Pressed);

            rightClick.MouseButton = MouseButtons.LeftButton;
            m_InputManager.MapAction(rightClick);

            // Throw Dice
            ThrowDice();

            // End the scene
            EndAttack();
        }

        public override void Update(RenderContext renderContext)
        {
            //Update inputManager
            m_InputManager.Update();

            // FPS
            m_ElapseTime += (float)renderContext.GameTime.ElapsedGameTime.TotalSeconds;
            m_FrameCounter++;

            if (m_ElapseTime > 1)
            {
                m_Fps = m_FrameCounter;
                m_FrameCounter = 0;
                m_ElapseTime = 0;
            }

            // Exit Attack scene
            if (m_InputManager.IsActionTriggered((int) PlayerInput.RightClick))
            {
                SceneManager.SetActiveScene("PlayScene");
            }

            base.Update(renderContext);
        }

        public override void Draw2D(RenderContext renderContext, bool drawBefore3D)
        {
            // Show FPS 2
            // renderContext.SpriteBatch.DrawString(m_DebugFont, "FPS: " + m_Fps, new Vector2(10, 10), Color.White);
            renderContext.SpriteBatch.DrawString(m_DebugFont, "Press Right Mouse Button to return to the main game", new Vector2(10, 50), Color.White);

            renderContext.SpriteBatch.DrawString(m_DebugFont, "Player Armysize: " + m_Attacker.ArmySize, new Vector2(10, 100), Color.White);
            renderContext.SpriteBatch.DrawString(m_DebugFont, "Ai Armysize: " + m_Defender.ArmySize, new Vector2(10, 120), Color.White);

            // Draw Dice
            renderContext.SpriteBatch.DrawString(m_DebugFont, "Attackers Dice: " + m_AttackersDice, new Vector2(300, 100), Color.White);
            renderContext.SpriteBatch.DrawString(m_DebugFont, "Defenders Dice: " + m_DefendersDice, new Vector2(300, 120), Color.White);

            if (m_bEnd)
            {
                if (m_AttackersDice > m_DefendersDice)
                {
                    renderContext.SpriteBatch.DrawString(m_DebugFont, "Attacker won!", new Vector2(300, 300),Color.White);
                    m_AttackerWon = true;
                }
                else
                {
                    renderContext.SpriteBatch.DrawString(m_DebugFont, "Defender won!", new Vector2(300, 300),Color.White);
                    m_AttackerWon = false;
                }
            }

            base.Draw2D(renderContext, drawBefore3D);
        }

        public override void Draw3D(RenderContext renderContext)
        {
            base.Draw3D(renderContext);
        }

        // Throws Dice for both the Attackers as Defenders
        private void ThrowDice()
        {
            var random = GridFieldManager.GetInstance().Random;

            for (var t = 0; t < m_Attacker.ArmySize; ++t)
            {
                var attackersDice = random.Next(1, 6);

                if(attackersDice > m_AttackersDice)
                    m_AttackersDice = attackersDice;
            }

            for (var t = 0; t < m_Defender.ArmySize; ++t)
            {
                var defendersDice = random.Next(1, 6);

                if (defendersDice > m_DefendersDice)
                    m_DefendersDice = defendersDice;
            }

            if (m_AttackersDice > m_DefendersDice)
                if (m_Defender.ArmySize > 0) m_Defender.ArmySize = m_Defender.ArmySize;
            else
                if (m_Attacker.ArmySize > 0) m_Attacker.ArmySize = m_Attacker.ArmySize;
        }

        private void EndAttack()
        {
            Army loser = GetLoserObject();
            GridFieldManager.GetInstance().UnboundArmy(loser);
            m_bEnd = true;
            loser.SetTargetTileOverride(null);
            loser.GetOwner().RemovePlaceable(loser);
           // GridFieldManager.GetInstance().GameScene.RemoveSceneObject(loser.Model);
            SceneManager.RemoveGameScene(this);
            //SceneManager.SetActiveScene("FinalScene");
        }

        public string GetWinner()
        {
            if (m_AttackerWon)
            {
                return "Attacker";
            }
            else
            {
                return "Defender";
            }
        }

        public Army GetLoserObject()
        {
            if (!m_AttackerWon)
            {
                return m_Attacker;
            }
            else
            {
                return m_Defender;
            }
        }

        public int GetAttackerDice()
        {
            return m_AttackersDice;
        }

        public int GetDefenderDice()
        {
            return m_DefendersDice;
        }

        public int GetHighestDieThrown()
        {
            if (m_AttackerWon)
                return m_AttackersDice;
            else
                return m_DefendersDice;
        }

        public int GetAttackerArmySize()
        {
            return m_Attacker.ArmySize;
        }

        public int GetDefenderArmySize()
        {
            return m_Defender.ArmySize;
        }
    }
}
