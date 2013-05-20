using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
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
            ToggleTileType,
            GoBackToMainMenu,
            ToggleDebug
        }

        //DATAMEMBERS
        private static ContentManager m_Content;
        private static InputManager m_InputManager;

        // Handle GamePad Input
        private static GamePadState m_GamePadState;

        private SpriteFont m_DebugFont;

        private readonly Texture2D m_TexBackground;

        //CameraVariables
        private const double CAMERAZOOMMIN = 0.2;
        private const double CAMERAZOOMMAX = 0.7;
        private const double ZOOMSTRENGTH = 0.001;
        private const double CAMERASTARTSCALE = 0.7;
        private double m_CameraScale = CAMERASTARTSCALE;
        private double m_CameraScaleTarget = CAMERASTARTSCALE;
        private double m_CameraRotationOffset;
        private Vector3 m_CameraTargetPos;

        private float m_ElapseTime;
        private int m_FrameCounter;
        private int m_Fps;

        private const float PATROLINTERVAL = 2;
        private float m_ArmyPatrolTimer = 0;

        private bool m_DrawDebug = true;


        public PlayScene(ContentManager content, string map)
            : base("PlayScene")
        {
            //CONTENT
            m_Content = content;

            // FONT
            m_DebugFont = content.Load<SpriteFont>("Fonts/DebugFont");

            //TEXTURES
            m_TexBackground = content.Load<Texture2D>("Textures/Background");

            //Load the map into the gridfieldManager
            GridFieldManager.GetInstance().LoadMap(this, map);

        }

        public override void Initialize()
        {
            // Initialize player and AI
            var userplayer = new Player(Player.PlayerColor.Blue);
            var enemyPlayer = new Player(Player.PlayerColor.Red);
            GridFieldManager.GetInstance().AddPlayer(userplayer);
            GridFieldManager.GetInstance().AddPlayer(enemyPlayer);
            GridFieldManager.GetInstance().SetUserPlayer(userplayer);

            //INPUTS
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
            InputAction goBackToMainMenu = new InputAction((int)PlayerInput.GoBackToMainMenu,TriggerState.Pressed);
            InputAction toggleDebug = new InputAction((int)PlayerInput.ToggleDebug,TriggerState.Pressed);

            // Controller Specific
            /* none */

            leftClick.MouseButton = MouseButtons.LeftButton;
            leftClick.GamePadButton = Buttons.A;
            rightClick.MouseButton = MouseButtons.RightButton;
            rightClick.GamePadButton = Buttons.RightStick;
            leftHold.MouseButton = MouseButtons.LeftButton;
            scrollWheelDown.MouseButton = MouseButtons.MiddleButton;
            toggleCreativeMode.KeyButton = Keys.C;
            rotateClockwise.KeyButton = Keys.A;
            rotateCounterClockwise.KeyButton = Keys.E;
            toggleSelectionMode.KeyButton = Keys.V;
            toggleTileType.KeyButton = Keys.B;
            goBackToMainMenu.KeyButton = Keys.Escape;
            toggleDebug.KeyButton = Keys.N;

            m_InputManager.MapAction(leftClick);
            m_InputManager.MapAction(rightClick);
            m_InputManager.MapAction(leftHold);
            m_InputManager.MapAction(scrollWheelDown);
            m_InputManager.MapAction(toggleCreativeMode);
            m_InputManager.MapAction(rotateClockwise);
            m_InputManager.MapAction(rotateCounterClockwise);
            m_InputManager.MapAction(toggleSelectionMode);
            m_InputManager.MapAction(toggleTileType);
            m_InputManager.MapAction(goBackToMainMenu);
            m_InputManager.MapAction(toggleDebug);

            //Adjust the camera position
            SceneManager.RenderContext.Camera.Translate(800, 1000, 800);
            SceneManager.RenderContext.Camera.Rotate(-45, 30, 150);
            m_CameraTargetPos = SceneManager.RenderContext.Camera.LocalPosition;

            GridFieldManager.GetInstance().Initialize();

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

            //Update the gridfield manager
            GridFieldManager.GetInstance().Update(renderContext);

            // MOVE ENEMY ARMY
            m_ArmyPatrolTimer += renderContext.GameTime.ElapsedGameTime.Milliseconds/1000.0f;
            int spot = (int)((m_ArmyPatrolTimer % (4 * PATROLINTERVAL)) / PATROLINTERVAL);
            switch (spot)
            {
                case 0:
                    // move 1
                    GridFieldManager.GetInstance()
                                    .AiPlayer.GetOwnedList()
                                    .Find(x => x.GetOwner() == GridFieldManager.GetInstance().AiPlayer)
                                    .SetTargetTile(GridFieldManager.GetInstance().GridField[12, 12]);
                    break;
                case 1:
                    // move 2
                    GridFieldManager.GetInstance()
                                    .AiPlayer.GetOwnedList()
                                    .Find(x => x.GetOwner() == GridFieldManager.GetInstance().AiPlayer)
                                    .SetTargetTile(GridFieldManager.GetInstance().GridField[15, 12]);

                    break;
                case 2:
                    // move 3
                    GridFieldManager.GetInstance()
                                    .AiPlayer.GetOwnedList()
                                    .Find(x => x.GetOwner() == GridFieldManager.GetInstance().AiPlayer)
                                    .SetTargetTile(GridFieldManager.GetInstance().GridField[15, 15]);

                    break;
                case 3:
                    // move 4
                    GridFieldManager.GetInstance()
                                    .AiPlayer.GetOwnedList()
                                    .Find(x => x.GetOwner() == GridFieldManager.GetInstance().AiPlayer)
                                    .SetTargetTile(GridFieldManager.GetInstance().GridField[12, 15]);
                    break;

            }


            base.Update(renderContext);
        }

        public override void Draw2D(RenderContext renderContext, bool drawBefore3D)
        {
            
            //graphics.GraphicsDevice.
           // renderContext.GraphicsDevice.DepthStencilState.DepthBufferEnable = true;
            //renderContext.GraphicsDevice.DepthStencilState.DepthBufferWriteEnable = true;


            if (drawBefore3D == false) // draw after the 3D is drawn
            {
                // Show FPS 2
                // renderContext.SpriteBatch.DrawString(m_DebugFont, "FPS: " + m_Fps, new Vector2(10, 10), Color.White);

                // DrawGUI
                Menu.GetInstance().Draw(renderContext);

                if (m_DrawDebug)
                {
                    int relativeDrawPos = 100;
                    // Creative mode
                    renderContext.SpriteBatch.DrawString(m_DebugFont, "N: DEBUG: " + GridFieldManager.GetInstance().CreativeMode, new Vector2(10, relativeDrawPos), Color.Red);

                    // Creative mode
                    relativeDrawPos += 20;
                    renderContext.SpriteBatch.DrawString(m_DebugFont, "C: Creative mode: " + GridFieldManager.GetInstance().CreativeMode, new Vector2(10, relativeDrawPos), Color.Red);

                    // TileType
                    relativeDrawPos += 20;
                    renderContext.SpriteBatch.DrawString(m_DebugFont, "B: Tiletype to build: " + Menu.GetInstance().TileTypeSelected, new Vector2(10, relativeDrawPos), Color.Red);

                /*

                // Show Selection
                renderContext.SpriteBatch.DrawString(m_DebugFont, "Selected: " + Menu.GetInstance().GetSelectedMode(), new Vector2(10, 30), Color.White);

                                // Selection mode
                renderContext.SpriteBatch.DrawString(m_DebugFont, "V: Selection mode: " + GridFieldManager.GetInstance().SelectionModeMeth, new Vector2(10, 90), Color.White);


                // Show Camera position (DEBUG)
                renderContext.SpriteBatch.DrawString(m_DebugFont, "Camera Pos: " + m_CameraTargetPos,
                                                     new Vector2(10, 150), Color.White);
                 */
                }

            }
            else // Draw before the 3D is drawn
            {
                renderContext.SpriteBatch.Draw(m_TexBackground, new Vector2(0, 0), Color.White);
            }


            base.Draw2D(renderContext, drawBefore3D);
        }

        public override void Draw3D(RenderContext renderContext)
        {
            SceneManager.RenderContext.GraphicsDevice.BlendState = BlendState.AlphaBlend;
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

            //Creative Mode
            if (m_InputManager.IsActionTriggered((int)PlayerInput.ToggleCreativeMode))
                GridFieldManager.GetInstance().CreativeMode = !GridFieldManager.GetInstance().CreativeMode;

            //TileTypeSelected
            if (m_InputManager.IsActionTriggered((int)PlayerInput.ToggleSelectionMode))
                GridFieldManager.GetInstance().NextSelectionMode();

            //SelectionMode
            if (m_InputManager.IsActionTriggered((int)PlayerInput.ToggleTileType))
                Menu.GetInstance().NextTileType();

            //DebugMode
            if (m_InputManager.IsActionTriggered((int) PlayerInput.ToggleDebug))
                m_DrawDebug = !m_DrawDebug;

            if (m_InputManager.IsActionTriggered((int) PlayerInput.GoBackToMainMenu))
            {
                SceneManager.RemoveGameScene(this);
                SceneManager.SetActiveScene("MainMenuScene");
            }

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

            m_GamePadState = GamePad.GetState(PlayerIndex.One);

            //Gamepad
            if (m_GamePadState.IsConnected)
            {
                if (m_GamePadState.ThumbSticks.Left.Y > 0)
                    m_CameraTargetPos += -forwardVecCam * m_GamePadState.ThumbSticks.Left.Y * 5;
                if (m_GamePadState.ThumbSticks.Left.Y < 0)
                    m_CameraTargetPos += forwardVecCam * -m_GamePadState.ThumbSticks.Left.Y * 5;
                if (m_GamePadState.ThumbSticks.Left.X < 0)
                    m_CameraTargetPos += rightVecCam * -m_GamePadState.ThumbSticks.Left.X * 5;
                if (m_GamePadState.ThumbSticks.Left.X > 0)
                    m_CameraTargetPos += -rightVecCam * m_GamePadState.ThumbSticks.Left.X * 5;
            }


            //Keyboard
            float scrollStrength = 10 * (float)m_CameraScale;
            if (keyboardState[Keys.Z] == KeyState.Down)
                /*if (m_CameraTargetPos.Z > 850)*/
                m_CameraTargetPos += -forwardVecCam*scrollStrength;
            if (keyboardState[Keys.S] == KeyState.Down)
                m_CameraTargetPos += forwardVecCam*scrollStrength;
            if (keyboardState[Keys.Q] == KeyState.Down)
                /* if (m_CameraTargetPos.X > 620) */
                m_CameraTargetPos += rightVecCam*scrollStrength;
            if (keyboardState[Keys.D] == KeyState.Down)
                m_CameraTargetPos += -rightVecCam*scrollStrength;

            //Rotate
            float camSpeed = 0.5f;
            if (m_InputManager.IsActionTriggered((int)PlayerInput.RotateClockWise))
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

                if (mouseX < offset) m_CameraTargetPos += rightVecCam * scrollStrength;
                if (mouseY < offset) m_CameraTargetPos += -forwardVecCam * scrollStrength;

                if (mouseX > vp.Width - offset) m_CameraTargetPos += -rightVecCam * scrollStrength;
                if (mouseY > vp.Height - offset) m_CameraTargetPos += forwardVecCam * scrollStrength;
            }

            //Mouse wheel move
            if (m_InputManager.IsActionTriggered((int)PlayerInput.ScrollWheelDown))
            {
                m_CameraTargetPos += rightVecCam * (mouseX - renderContext.Input.OldMouseState.X) * (float)m_CameraScale *
                                     1.33f; //magic numbers
                m_CameraTargetPos += -forwardVecCam * (mouseY - renderContext.Input.OldMouseState.Y) * (float)m_CameraScale *
                                     1.8f; //magic number
            }

            //Move the actual camera with the vector
            renderContext.Camera.LocalPosition += (m_CameraTargetPos - renderContext.Camera.LocalPosition) / 5;
            //Change the value to fiddle with the speed of the smooth transition

            //Zoom in and out camera
            double mouseScrollDifference = (double)m_InputManager.CurrentMouseState.ScrollWheelValue -
                                           (double)m_InputManager.OldMouseState.ScrollWheelValue;

            double newCameraScaleTarget = m_CameraScaleTarget + mouseScrollDifference * ZOOMSTRENGTH * -1;
            if (newCameraScaleTarget < CAMERAZOOMMIN)
                newCameraScaleTarget = CAMERAZOOMMIN;
            if (newCameraScaleTarget > CAMERAZOOMMAX)
                newCameraScaleTarget = CAMERAZOOMMAX;
            m_CameraScaleTarget = newCameraScaleTarget;

            m_CameraScale += (m_CameraScaleTarget - m_CameraScale) / 10;
            //Change the value to fiddle with the speed of the smooth transition

            renderContext.Camera.Projection = CalculateProjectionMatrixOrthographic(renderContext);

            #endregion
            #endregion

            if (m_GamePadState.Buttons.RightShoulder != 0)
            {
                System.Console.WriteLine("Right shoulder pressed");
            }
            if (m_GamePadState.Buttons.LeftShoulder != 0)
            {
                System.Console.WriteLine("Left shoulder pressed");
            }

            // Work with X Y B to select meu options
            if (Menu.GetInstance().SubMenu == Menu.SubMenuSelected.VillagerMode)
            {
                if (m_GamePadState.IsButtonDown(Buttons.X))
                {
                    GridFieldManager.GetInstance().SelectionModeMeth = GridFieldManager.SelectionMode.select2x2;
                    Menu.GetInstance().SetSelectedMode(Menu.ModeSelected.BuildSettlement);
                }

                if (m_GamePadState.IsButtonDown(Buttons.Y))
                {
                    GridFieldManager.GetInstance().SelectionModeMeth = GridFieldManager.SelectionMode.select2x2;
                    Menu.GetInstance().SetSelectedMode(Menu.ModeSelected.BuildSchool);
                }
            }


            GridFieldManager.GetInstance().HandleInput(renderContext);
        }


        public Matrix CalculateProjectionMatrixOrthographic(RenderContext renderContext)
        {
            float aspectRatio = (float)renderContext.GraphicsDevice.Viewport.Width / (float)renderContext.GraphicsDevice.Viewport.Height;
            return Matrix.CreateOrthographic((720 * aspectRatio) * (float)m_CameraScale, 720 * (float)m_CameraScale, 0.1f, 10000f);
        }

        public static ContentManager GetContentManager()
        {
            return m_Content;
        }
        public static Ray CalculateCursorRay(RenderContext renderContext)
        {
            Matrix view = renderContext.Camera.View;
            Matrix projection = renderContext.Camera.Projection;

            int mouseX = renderContext.Input.CurrentMouseState.X;
            int mouseY = renderContext.Input.CurrentMouseState.Y;

            // Change selection if you're using a Controller
            if (m_GamePadState.IsConnected)
            {
                mouseX = renderContext.GraphicsDevice.Viewport.Width / 2;
                mouseY = renderContext.GraphicsDevice.Viewport.Height / 2;
            }

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
        public static InputManager GetInputManager()
        {
            return m_InputManager;
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
        public static Vector3 GetForwardVectorOfQuaternion(Quaternion q)
        {
            return new Vector3(2 * (q.X * q.Z + q.W * q.Y),
                            2 * (q.Y * q.X - q.W * q.X),
                            1 - 2 * (q.X * q.X + q.Y * q.Y));
        }
    }
}
