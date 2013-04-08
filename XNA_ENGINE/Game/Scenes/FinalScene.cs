﻿using System;
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
using XNA_ENGINE.Game.Helpers;
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
            ScrollWheelDown
        }

        private static ContentManager m_Content;

        // Controls
        GamePadState m_GamePadState;
        private static InputManager m_InputManager;

        private float m_ElapseTime;
        private int m_FrameCounter;
        private int m_Fps;
        private SpriteFont m_DebugFont;

        private const double CAMERAZOOMMIN = 0.4;
        private const double CAMERAZOOMMAX = 1.5;
        private const double ZOOMSTRENGTH = 0.001;
        private const double CAMERASTARTSCALE = 1.0;
        private double m_CameraScale = CAMERASTARTSCALE;
        private double m_CameraScaleTarget = CAMERASTARTSCALE;
        private Vector3 m_CameraTargetPos;

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
            //Input manager + inputs
            m_InputManager = new InputManager();

            InputAction leftClick = new InputAction((int)PlayerInput.LeftClick, TriggerState.Pressed);
            InputAction rightClick = new InputAction((int)PlayerInput.RightClick, TriggerState.Pressed);
            InputAction scrollWheelDown = new InputAction((int)PlayerInput.ScrollWheelDown, TriggerState.Down);

            leftClick.MouseButton = MouseButtons.LeftButton;
            rightClick.MouseButton = MouseButtons.RightButton;
            scrollWheelDown.MouseButton = MouseButtons.MiddleButton;
            
            m_InputManager.MapAction(leftClick);
            m_InputManager.MapAction(rightClick);
            m_InputManager.MapAction(scrollWheelDown);

            //Initialize the GridFieldManager
            GridFieldManager.GetInstance(this).Initialize();
         
            //Adjust the camera position
            SceneManager.RenderContext.Camera.Translate(800, 1000, 800);
            SceneManager.RenderContext.Camera.Rotate(-45, 30, 150);

            m_CameraTargetPos = SceneManager.RenderContext.Camera.LocalPosition;

            /*
            // ------------------------------------------
            // OPEN AND READ XML FILE
            // ------------------------------------------
            // the file must be available in the Debug (or release) folder
            System.IO.Stream stream = TitleContainer.OpenStream("tilemap.xml");

            XDocument doc = XDocument.Load(stream);

            m_Tiles = new List<Tile>();

            m_Tiles = (from tile in doc.Descendants("tile")
                       select new Tile()
                       {
                           position = new Vector2(Convert.ToInt32(tile.Element("positionX").Value), Convert.ToInt32(tile.Element("positionY").Value)),
                           type = Convert.ToString(tile.Element("type").Value),
                           settlement = Convert.ToString(tile.Element("settlement").Value)
                       }).ToList();

            // Test if the xml reader worked (and it does)
            System.Diagnostics.Debug.WriteLine("Type: " + m_Tiles.ElementAt(0).type);
            // ------------------------------------------
            // END READING XML FILE
            // ------------------------------------------
            */

            base.Initialize();
        }

        public override void Update(RenderContext renderContext)
        {
            // FPS
            m_ElapseTime += (float)renderContext.GameTime.ElapsedGameTime.TotalSeconds;
            m_FrameCounter++;

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

            base.Update(renderContext);
        }

        public override void Draw2D(RenderContext renderContext, bool drawBefore3D)
        {
            // Show FPS 2
            renderContext.SpriteBatch.DrawString(m_DebugFont, "FPS: " + m_Fps, new Vector2(10, 10), Color.White);

            // DrawGUI
            Menu.GetInstance().Draw(renderContext);

            // Show Selection
            renderContext.SpriteBatch.DrawString(m_DebugFont, "Selected: " + Menu.GetInstance().GetSelectedMode(), new Vector2(10, 30), Color.Black);

            base.Draw2D(renderContext, drawBefore3D);
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
            bool isMouseInScreen = false;
            if (IsMouseInScreen(renderContext)) isMouseInScreen = true;

            // Handle Keyboard Input
            KeyboardState keyboardState = renderContext.Input.CurrentKeyboardState;
            // Handle GamePad Input
            m_GamePadState = GamePad.GetState(PlayerIndex.One);

            //-------------------
            //CAMERA movement
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

            System.Diagnostics.Debug.WriteLine(forwardVecCam +"  " + rightVecCam);

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

            int mouseX = renderContext.Input.CurrentMouseState.X;
            int mouseY = renderContext.Input.CurrentMouseState.Y;

            //Mouse
            if (isMouseInScreen)
            {
                int offset = 5;

                Viewport vp = renderContext.GraphicsDevice.Viewport;

                if (mouseX < offset) m_CameraTargetPos += rightVecCam * scrollStrength;
                if (mouseY < offset) m_CameraTargetPos += -forwardVecCam * scrollStrength;

                if (mouseX > vp.Width - offset) m_CameraTargetPos += -rightVecCam * scrollStrength;
                if (mouseY > vp.Height - offset) m_CameraTargetPos += forwardVecCam * scrollStrength;
            }

            //Mouse wheel move
            if (m_InputManager.IsActionTriggered((int) PlayerInput.ScrollWheelDown))
            {
                m_CameraTargetPos += rightVecCam * (mouseX - renderContext.Input.OldMouseState.X) * (float)m_CameraScale * 1.33f; //magic numbers
                m_CameraTargetPos += -forwardVecCam * (mouseY - renderContext.Input.OldMouseState.Y) * (float)m_CameraScale * 1.8f; //magic number
            }

            //Move the actual camera with the vector
            renderContext.Camera.LocalPosition += (m_CameraTargetPos - renderContext.Camera.LocalPosition) / 5; //Change the value to fiddle with the speed of the smooth transition

            //Zoom in and out camera
            double mouseScrollDifference = (double) m_InputManager.CurrentMouseState.ScrollWheelValue -
                                           (double) m_InputManager.OldMouseState.ScrollWheelValue;

            double newCameraScaleTarget = m_CameraScaleTarget + mouseScrollDifference * ZOOMSTRENGTH * -1;
            if (newCameraScaleTarget < CAMERAZOOMMIN)
                newCameraScaleTarget = CAMERAZOOMMIN;
            if (newCameraScaleTarget > CAMERAZOOMMAX)
                newCameraScaleTarget = CAMERAZOOMMAX;
            m_CameraScaleTarget = newCameraScaleTarget;

            m_CameraScale += (m_CameraScaleTarget - m_CameraScale)/10; //Change the value to fiddle with the speed of the smooth transition

            renderContext.Camera.Projection = CalculateProjectionMatrixOrthographic(renderContext);
            //---------------------

            //Handle menu //If menu is hit don't do the grid test
            if (Menu.GetInstance().HandleInput(renderContext)) return;

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
    
        //Quaternion conversion helpers
        public static Vector3 GetForwardVectorOfQuaternion(Quaternion q) 
        {
            return new Vector3(2 * (q.X * q.Z + q.W * q.Y),
                            2 * (q.Y * q.X - q.W * q.X),
                            1 - 2 * (q.X * q.X + q.Y * q.Y));
        }
    }
}
