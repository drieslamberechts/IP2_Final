using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

using XNA_ENGINE.Engine;
using XNA_ENGINE.Engine.Scenegraph;

using XNA_ENGINE.Engine.Helpers;
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
        private Player m_Player, m_Ai;

        // Dice
        private int m_AttackersDice;
        private int m_DefendersDice;

        public AttackScene(ContentManager content, Player player, Player ai)
            : base("AttackScene")
        {
            //CONTENT
            m_Content = content;

            // SET PLAYER AND AI
            m_Player = player;
            m_Ai = ai;

            // FONT
            m_DebugFont = m_Content.Load<SpriteFont>("Fonts/DebugFont");

            // Execute Everything in the Scene
            Start();
        }

        public void Start()
        {
            //Input manager + inputs
            m_InputManager = new InputManager();

            var rightClick = new InputAction((int)PlayerInput.RightClick, TriggerState.Pressed);

            rightClick.MouseButton = MouseButtons.RightButton;
            m_InputManager.MapAction(rightClick);

            // Throw Dice
            ThrowDice();
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
                SceneManager.SetActiveScene("FinalScene");
            }

            base.Update(renderContext);
        }

        public override void Draw2D(RenderContext renderContext, bool drawBefore3D)
        {
            // Show FPS 2
            renderContext.SpriteBatch.DrawString(m_DebugFont, "FPS: " + m_Fps, new Vector2(10, 10), Color.White);
            renderContext.SpriteBatch.DrawString(m_DebugFont, "Press Right Mouse Button to return to the main game",
                                                 new Vector2(10, 50), Color.White);

            renderContext.SpriteBatch.DrawString(m_DebugFont, "Player Armysize: " + m_Player.GetArmySize(),
                                                 new Vector2(10, 100), Color.White);
            renderContext.SpriteBatch.DrawString(m_DebugFont, "Ai Armysize: " + m_Ai.GetArmySize(),
                                                 new Vector2(10, 120), Color.White);

            // Draw Dice
            renderContext.SpriteBatch.DrawString(m_DebugFont, "Attackers Dice: " + m_AttackersDice,
                                                 new Vector2(300, 100), Color.White);
            renderContext.SpriteBatch.DrawString(m_DebugFont, "Defenders Dice: " + m_DefendersDice,
                                                 new Vector2(300, 120), Color.White);

            base.Draw2D(renderContext, drawBefore3D);
        }

        public override void Draw3D(RenderContext renderContext)
        {
            base.Draw3D(renderContext);
        }

        // Throws Dice for both the Attackers as Defenders
        private void ThrowDice()
        {
            var random = new Random();

            for (var t = 0; t < m_Player.GetArmySize(); ++t)
            {
                var attackersDice = random.Next(1, 6);
                m_AttackersDice = attackersDice;
            }

            for (var t = 0; t < m_Ai.GetArmySize(); ++t)
            {
                var defendersDice = random.Next(1, 6);
                m_DefendersDice = defendersDice;
            }
        }
    }
}
