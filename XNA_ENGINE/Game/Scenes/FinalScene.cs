using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using XNA_ENGINE;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

using XNA_ENGINE.Engine;
using XNA_ENGINE.Engine.Scenegraph;
using XNA_ENGINE.Engine.Objects;

using XNA_ENGINE.Engine.Helpers;
using XNA_ENGINE.Game.Objects;

using Microsoft.Xna.Framework.Media;
using XNA_ENGINE.Game.Scenes;

using XNA_ENGINE.Game.Managers;

namespace XNA_ENGINE.Game.Scenes
{
    class FinalScene : GameScene
    {
        public enum PlayerInput
        {
            LeftClick,
            RightClick,
            ScrollWheelDown,
            ToggleCreativeMode,
            RotateClockWise,
            RotateCounterClockWise
        }

        private static ContentManager m_Content;

        // Player and AI
        private Player m_Player, m_Ai;

        // Controls
        GamePadState m_GamePadState;
        private static InputManager m_InputManager;

        private float m_ElapseTime;
        private int m_FrameCounter;
        private int m_Fps;
        private SpriteFont m_DebugFont;

        private const double CAMERAZOOMMIN = 0.2;
        private const double CAMERAZOOMMAX = 0.7;
        private const double ZOOMSTRENGTH = 0.001;
        private const double CAMERASTARTSCALE = 0.7;
        private double m_CameraScale = CAMERASTARTSCALE;
        private double m_CameraScaleTarget = CAMERASTARTSCALE;
        private double m_CameraRotationOffset;
        private Vector3 m_CameraTargetPos;

        // scene
        private AttackScene m_AttackScene; 

        public FinalScene(ContentManager content)
            : base("FinalScene")
        {
            //CONTENT
            m_Content = content;

            // FONT
            m_DebugFont = content.Load<SpriteFont>("Fonts/DebugFont");
        }


        public override void Initialize()
        {
            // Initialize player and AI
            m_Player = new Player(false);
            m_Ai = new Player(true);

            // Give menu the playerInfo
            Menu.GetInstance().SetPlayer(m_Player);

            //Input manager + inputs
            m_InputManager = new InputManager();

            InputAction leftClick = new InputAction((int)PlayerInput.LeftClick, TriggerState.Pressed);
            InputAction rightClick = new InputAction((int)PlayerInput.RightClick, TriggerState.Pressed);
            InputAction scrollWheelDown = new InputAction((int)PlayerInput.ScrollWheelDown, TriggerState.Down);
            InputAction toggleCreativeMode = new InputAction((int)PlayerInput.ToggleCreativeMode, TriggerState.Pressed);
            InputAction rotateClockwise = new InputAction((int)PlayerInput.RotateClockWise, TriggerState.Down);
            InputAction rotateCounterClockwise = new InputAction((int)PlayerInput.RotateCounterClockWise, TriggerState.Down);

            leftClick.MouseButton = MouseButtons.LeftButton;
            rightClick.MouseButton = MouseButtons.RightButton;
            scrollWheelDown.MouseButton = MouseButtons.MiddleButton;
            toggleCreativeMode.KeyButton = Keys.C;
            rotateClockwise.KeyButton = Keys.A;
            rotateCounterClockwise.KeyButton = Keys.E;
            
            m_InputManager.MapAction(leftClick);
            m_InputManager.MapAction(rightClick);
            m_InputManager.MapAction(scrollWheelDown);
            m_InputManager.MapAction(toggleCreativeMode);
            m_InputManager.MapAction(rotateClockwise);
            m_InputManager.MapAction(rotateCounterClockwise);

            //Initialize the GridFieldManager
            GridFieldManager.GetInstance(this).Initialize();
         
            //Adjust the camera position
            SceneManager.RenderContext.Camera.Translate(800, 1000, 800);
            SceneManager.RenderContext.Camera.Rotate(-45, 30, 150);

            m_CameraTargetPos = SceneManager.RenderContext.Camera.LocalPosition;

            // ADD SCENES
            m_AttackScene = new AttackScene(m_Content, m_Player, m_Ai);
            SceneManager.AddGameScene(m_AttackScene);

            base.Initialize();
        }

        public override void Update(RenderContext renderContext)
        {
            // FPS
            m_ElapseTime += (float)renderContext.GameTime.ElapsedGameTime.TotalSeconds;
            ++m_FrameCounter;

            if (m_ElapseTime > 1)
            {
                m_Fps = m_FrameCounter;
                m_FrameCounter = 0;
                m_ElapseTime = 0;
            }

            // UPDATE MENU
            Menu.GetInstance().Update(renderContext);

            // Handle Input
            HandleInput(renderContext);

            GridFieldManager.GetInstance(this).Update(renderContext);

            // Update Scenes
            if (m_Player.GetAttack())
            {
                Console.WriteLine("Player is Attacking");
                m_Player.ResetAttack();
                SceneManager.SetActiveScene("AttackScene");
            }
            if (m_Ai.GetAttack())
            {
                SceneManager.SetActiveScene("AttackScene");
                m_Ai.ResetAttack();
            }

            base.Update(renderContext);
        }

        public override void Draw2D(RenderContext renderContext, bool drawBefore3D)
        {
            // Show FPS 2
            renderContext.SpriteBatch.DrawString(m_DebugFont, "FPS: " + m_Fps, new Vector2(10, 10), Color.White);

            // DrawGUI
            Menu.GetInstance().Draw(renderContext);

            base.Draw2D(renderContext, drawBefore3D);

            // Show Selection
            renderContext.SpriteBatch.DrawString(m_DebugFont, "Selected: " + Menu.GetInstance().GetSelectedMode(), new Vector2(10, 30), Color.White);

            // Creative mode
            renderContext.SpriteBatch.DrawString(m_DebugFont, "Creative mode: " + GridFieldManager.GetInstance(this).CreativeMode, new Vector2(10, 50), Color.White);
        }

        public override void Draw3D(RenderContext renderContext)
        {
            base.Draw3D(renderContext);
        }

        private void HandleInput(RenderContext renderContext)
        {
            //Update inputManager
            m_InputManager.Update();

            //Check if the mouse is in the screen
            bool isMouseInScreen = IsMouseInScreen(renderContext);

            // Handle Keyboard Input
            KeyboardState keyboardState = renderContext.Input.CurrentKeyboardState;

            if (m_InputManager.IsActionTriggered((int) PlayerInput.ToggleCreativeMode))
                GridFieldManager.GetInstance(this).CreativeMode = !GridFieldManager.GetInstance(this).CreativeMode;

            // Handle GamePad Input
            m_GamePadState = GamePad.GetState(PlayerIndex.One);

            //CAMERA
            #region Camera
            //Camera Vectors
            //forward
            Vector3 forwardVecCam = GetForwardVectorOfQuaternion(renderContext.Camera.LocalRotation);
            forwardVecCam.Y = 0;
            forwardVecCam.Normalize();
            //right
            Vector3 rightVecCam;
            Matrix rotMatrix;
            rotMatrix = Matrix.CreateRotationY(MathHelper.ToRadians(-90));
            rightVecCam = Vector3.Transform(forwardVecCam, rotMatrix);
            rightVecCam.Normalize();

            //Gamepad
            if (m_GamePadState.IsConnected)
            {
                if (m_GamePadState.ThumbSticks.Left.Y > 0)
                    m_CameraTargetPos += -forwardVecCam * m_GamePadState.ThumbSticks.Left.Y * (float)m_CameraScale;
                if (m_GamePadState.ThumbSticks.Left.X < 0)
                    m_CameraTargetPos += rightVecCam * m_GamePadState.ThumbSticks.Left.X * (float)m_CameraScale;
                if (m_GamePadState.ThumbSticks.Left.Y < 0)
                    m_CameraTargetPos += forwardVecCam * m_GamePadState.ThumbSticks.Left.Y * (float)m_CameraScale;
                if (m_GamePadState.ThumbSticks.Left.X > 0)
                    m_CameraTargetPos += -rightVecCam * m_GamePadState.ThumbSticks.Left.X * (float)m_CameraScale;
            }


            //Keyboard
            float scrollStrength = 10*(float) m_CameraScale;

            if (keyboardState[Keys.Z] == KeyState.Down)
                m_CameraTargetPos += -forwardVecCam * scrollStrength;
            if (keyboardState[Keys.S] == KeyState.Down)
                m_CameraTargetPos += forwardVecCam * scrollStrength;
            if (keyboardState[Keys.Q] == KeyState.Down)
                m_CameraTargetPos += rightVecCam * scrollStrength;
            if (keyboardState[Keys.D] == KeyState.Down)
                m_CameraTargetPos += -rightVecCam * scrollStrength;

            //Rotate
            float camSpeed = 0.5f;
            if (m_InputManager.IsActionTriggered((int) PlayerInput.RotateClockWise))
                m_CameraRotationOffset += camSpeed;
            if (m_InputManager.IsActionTriggered((int)PlayerInput.RotateCounterClockWise))
                m_CameraRotationOffset -= camSpeed;

            SceneManager.RenderContext.Camera.Rotate(-45, 30 + (float)m_CameraRotationOffset, 150);

            int mouseX = renderContext.Input.CurrentMouseState.X;
            int mouseY = renderContext.Input.CurrentMouseState.Y;

            //MOUSE
            #region Mouse
            if (isMouseInScreen)
            {
                int offset = 5;

                Viewport vp = renderContext.GraphicsDevice.Viewport;

                if (mouseX < offset) m_CameraTargetPos += rightVecCam*scrollStrength;
                if (mouseY < offset) m_CameraTargetPos += -forwardVecCam*scrollStrength;

                if (mouseX > vp.Width - offset) m_CameraTargetPos += -rightVecCam*scrollStrength;
                if (mouseY > vp.Height - offset) m_CameraTargetPos += forwardVecCam*scrollStrength;
            }

            //Mouse wheel move
            if (m_InputManager.IsActionTriggered((int) PlayerInput.ScrollWheelDown))
            {
                m_CameraTargetPos += rightVecCam*(mouseX - renderContext.Input.OldMouseState.X)*(float) m_CameraScale*
                                     1.33f; //magic numbers
                m_CameraTargetPos += -forwardVecCam*(mouseY - renderContext.Input.OldMouseState.Y)*(float) m_CameraScale*
                                     1.8f; //magic number
            }

            //Move the actual camera with the vector
            renderContext.Camera.LocalPosition += (m_CameraTargetPos - renderContext.Camera.LocalPosition)/5;
                //Change the value to fiddle with the speed of the smooth transition

            //Zoom in and out camera
            double mouseScrollDifference = (double) m_InputManager.CurrentMouseState.ScrollWheelValue -
                                           (double) m_InputManager.OldMouseState.ScrollWheelValue;

            double newCameraScaleTarget = m_CameraScaleTarget + mouseScrollDifference*ZOOMSTRENGTH*-1;
            if (newCameraScaleTarget < CAMERAZOOMMIN)
                newCameraScaleTarget = CAMERAZOOMMIN;
            if (newCameraScaleTarget > CAMERAZOOMMAX)
                newCameraScaleTarget = CAMERAZOOMMAX;
            m_CameraScaleTarget = newCameraScaleTarget;

            m_CameraScale += (m_CameraScaleTarget - m_CameraScale)/10;
                //Change the value to fiddle with the speed of the smooth transition

            renderContext.Camera.Projection = CalculateProjectionMatrixOrthographic(renderContext);

            #endregion
            #endregion
            
            //Handle menu //If menu is hit don't do the grid test
            if (Menu.GetInstance().HandleInput(renderContext)) return; // hier in Menu -> klikken?

            //Raycast to grid
            if (isMouseInScreen)
                GridFieldManager.GetInstance(this).HitTestField(CalculateCursorRay(renderContext));
        }

        public static bool IsMouseInScreen(RenderContext renderContext)
        {
            int x = renderContext.Input.CurrentMouseState.X;
            if (x < 0) return false;

            int y = renderContext.Input.CurrentMouseState.Y;
            if (y < 0) return false;

            Viewport vp = renderContext.GraphicsDevice.Viewport;
            if (x > vp.Width) return false;
            if (y > vp.Height) return false;

            return true;
        }

        public static ContentManager GetContentManager()
        {
            return m_Content;
        }
        public static InputManager GetInputManager()
        {
            return m_InputManager;
        }

        public Matrix CalculateProjectionMatrixOrthographic(RenderContext renderContext)
        {
            float aspectRatio = (float)renderContext.GraphicsDevice.Viewport.Width/(float)renderContext.GraphicsDevice.Viewport.Height;
            return Matrix.CreateOrthographic((720 * aspectRatio) * (float)m_CameraScale, 720 * (float)m_CameraScale, 0.1f, 10000f);
        }

        public static Ray CalculateCursorRay(RenderContext renderContext)
        {
            Matrix view = renderContext.Camera.View;
            Matrix projection = renderContext.Camera.Projection;

            int mouseX = renderContext.Input.CurrentMouseState.X;
            int mouseY = renderContext.Input.CurrentMouseState.Y;

            // create 2 positions in screenspace using the cursor position. 0 is as
            // close as possible to the camera, 1 is as far away as possible.
            var nearSource = new Vector3(mouseX, mouseY, 0f);
            var farSource = new Vector3(mouseX, mouseY, 1f);

            // use Viewport.Unproject to tell what those two screen space positions
            // would be in world space. we'll need the projection matrix and view
            // matrix, which we have saved as member variables. We also need a world
            // matrix, which can just be identity.
            Vector3 nearPoint = renderContext.GraphicsDevice.Viewport.Unproject(nearSource,
                projection, view, Matrix.Identity);

            Vector3 farPoint = renderContext.GraphicsDevice.Viewport.Unproject(farSource,
                projection, view, Matrix.Identity);

            // find the direction vector that goes from the nearPoint to the farPoint
            // and normalize it....
            Vector3 direction = farPoint - nearPoint;
            direction.Normalize();

            // and then create a new ray using nearPoint as the source.
            return new Ray(nearPoint, direction);
        }
    
        //Quaternion conversion helper
        public static Vector3 GetForwardVectorOfQuaternion(Quaternion q) 
        {
            return new Vector3(2 * (q.X * q.Z + q.W * q.Y),
                            2 * (q.Y * q.X - q.W * q.X),
                            1 - 2 * (q.X * q.X + q.Y * q.Y));
        }
    }
}
