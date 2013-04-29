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
    class PlayScene : GameScene
    {
        public enum PlayerInput
        {
            LeftClick,
            RightClick,
            LeftHold,
            ScrollWheelDown,
            ToggleCreativeMode,
            RotateClockWise,
            RotateCounterClockWise,
            ToggleSelectionMode,
            ToggleTileType
        }

        private static ContentManager m_Content;

        private SpriteFont m_DebugFont;

        private readonly Texture2D m_TexBackground;

        public PlayScene(ContentManager content, string map)
            : base("PlayScene")
        {
            //CONTENT
            m_Content = content;

            // FONT
            m_DebugFont = content.Load<SpriteFont>("Fonts/DebugFont");

            //TEXTURES
            m_TexBackground = content.Load<Texture2D>("Textures/Background");
        }

        public override void Initialize()
        {
            GridFieldManager.GetInstance()

            // Initialize player and AI
            m_Player = new Player(false);
            m_Ai = new Player(true);

            // Give menu the playerInfo
            Menu.GetInstance().SetPlayer(m_Player);

            //Input manager + inputs
            m_InputManager = new InputManager();

            InputAction leftClick = new InputAction((int)PlayerInput.LeftClick, TriggerState.Pressed);
            InputAction rightClick = new InputAction((int)PlayerInput.RightClick, TriggerState.Pressed);
            InputAction leftHold = new InputAction((int)PlayerInput.LeftHold, TriggerState.Down);
            InputAction scrollWheelDown = new InputAction((int)PlayerInput.ScrollWheelDown, TriggerState.Down);
            InputAction toggleCreativeMode = new InputAction((int)PlayerInput.ToggleCreativeMode, TriggerState.Pressed);
            InputAction rotateClockwise = new InputAction((int)PlayerInput.RotateClockWise, TriggerState.Down);
            InputAction rotateCounterClockwise = new InputAction((int)PlayerInput.RotateCounterClockWise, TriggerState.Down);
            InputAction toggleSelectionMode = new InputAction((int)PlayerInput.ToggleSelectionMode, TriggerState.Pressed);
            InputAction toggleTileType = new InputAction((int)PlayerInput.ToggleTileType, TriggerState.Pressed);


            leftClick.MouseButton = MouseButtons.LeftButton;
            rightClick.MouseButton = MouseButtons.RightButton;
            leftHold.MouseButton = MouseButtons.LeftButton;
            scrollWheelDown.MouseButton = MouseButtons.MiddleButton;
            toggleCreativeMode.KeyButton = Keys.C;
            rotateClockwise.KeyButton = Keys.A;
            rotateCounterClockwise.KeyButton = Keys.E;
            toggleSelectionMode.KeyButton = Keys.V;
            toggleTileType.KeyButton = Keys.B;


            m_InputManager.MapAction(leftClick);
            m_InputManager.MapAction(rightClick);
            m_InputManager.MapAction(leftHold);
            m_InputManager.MapAction(scrollWheelDown);
            m_InputManager.MapAction(toggleCreativeMode);
            m_InputManager.MapAction(rotateClockwise);
            m_InputManager.MapAction(rotateCounterClockwise);
            m_InputManager.MapAction(toggleSelectionMode);
            m_InputManager.MapAction(toggleTileType);

            //Initialize the GridFieldManager
            GridFieldManager.GetInstance(this).Initialize();

            //Adjust the camera position
            SceneManager.RenderContext.Camera.Translate(800, 1000, 800);
            SceneManager.RenderContext.Camera.Rotate(-45, 30, 150);


            m_CameraTargetPos = SceneManager.RenderContext.Camera.LocalPosition;

            // ADD SCENES
            // m_AttackScene = new AttackScene(m_Content, m_Player, m_Ai);
            //  SceneManager.AddGameScene(m_AttackScene);

            base.Initialize();

        }

        public override void Update(RenderContext renderContext)
        {

            base.Update(renderContext);
        }

        public override void Draw2D(RenderContext renderContext, bool drawBefore3D)
        {
           
            base.Draw2D(renderContext, drawBefore3D);
        }

        public override void Draw3D(RenderContext renderContext)
        {
            base.Draw3D(renderContext);
        }
    }
}
