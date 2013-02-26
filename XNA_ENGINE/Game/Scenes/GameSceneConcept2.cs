using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNA_ENGINE;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

using XNA_ENGINE.Engine;
using XNA_ENGINE.Engine.Scenegraph;
using XNA_ENGINE.Engine.Objects;

using XNA_ENGINE.Engine.Helpers;

using XNA_ENGINE.Game.Objects;

using IP2_Xna_Template.Objects;
using Microsoft.Xna.Framework.Media;

namespace XNA_ENGINE.Game
{
    public class GameSceneConcept2 : GameScene
    {
        enum PlayerInput
        {
            GoUp,
            GoDown,
            Shoot
        }

        private ContentManager Content;
        private InputManager m_InputManager;

        public GameSceneConcept2(ContentManager content)
            : base("GameSceneConcept2")
        {
            Content = content;
            m_InputManager = new InputManager();

            InputAction GoUp = new InputAction((int)PlayerInput.GoUp, TriggerState.Down);
            GoUp.KeyButton = Keys.Z;
            GoUp.GamePadButton = Buttons.LeftThumbstickUp;

            InputAction GoDown = new InputAction((int)PlayerInput.GoDown, TriggerState.Down);
            GoDown.KeyButton = Keys.S;
            GoDown.GamePadButton = Buttons.LeftThumbstickDown;

            InputAction Shoot = new InputAction((int)PlayerInput.Shoot, TriggerState.Pressed);
            Shoot.KeyButton = Keys.Space;
            Shoot.GamePadButton = Buttons.RightTrigger;

            m_InputManager.MapAction(GoUp);
            m_InputManager.MapAction(GoDown);
            m_InputManager.MapAction(Shoot);

        }

        public override void Initialize()
        {
            Debug2D.LoadContent(Content);
            base.Initialize();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            
            base.LoadContent(contentManager);
        }

        public override void Update(RenderContext renderContext)
        {
            m_InputManager.Update();

            if (m_InputManager.GetAction((int)PlayerInput.GoUp).IsTriggered)
                System.Console.WriteLine("Up");

            if (m_InputManager.GetAction((int)PlayerInput.GoDown).IsTriggered)
                System.Console.WriteLine("Down");

            if (m_InputManager.GetAction((int)PlayerInput.Shoot).IsTriggered)
                System.Console.WriteLine("Shoot");

            base.Update(renderContext);
        }

        public override void Draw2D(RenderContext renderContext, bool drawBefore3D)
        {

            //MouseStateEx.IsButtonDown(MouseButtons.LeftButton)
          //  Debug2D.DrawRectangle(new Rectangle(20, 20, 100, 100), Color.Black);

            base.Draw2D(renderContext, drawBefore3D);
        }

        public override void Draw3D(RenderContext renderContext)
        {
            base.Draw3D(renderContext);
        }
    }

}
