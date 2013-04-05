using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using XNA_ENGINE;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
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



using GameModel = XNA_ENGINE.Engine.Objects.GameModel;

namespace XNA_ENGINE.Game.Scenes
{
    class FinalScene : GameScene
    {
        enum PlayerInput
        {
            LeftClick
        }

        private enum BuildSelection
        {
            BuildingAwesome,
            BuildingStupid,
            BuildingOutOfInspiration
        }

        private ContentManager m_Content;

        // Menu
        private Menu m_Menu;

        // Controls
        GamePadState m_GamePadState;
        private InputManager m_InputManager;
        private float m_ElapseTime;
        private int m_FrameCounter;
        private int m_Fps;
        private SpriteFont m_DebugFont;

        private BuildSelection m_BuildSelection;

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
            m_BuildSelection = BuildSelection.BuildingAwesome;

            //Input manager + inputs
            m_InputManager = new InputManager();

            InputAction leftClick = new InputAction((int)PlayerInput.LeftClick, TriggerState.Pressed);
            leftClick.MouseButton = MouseButtons.LeftButton;
            m_InputManager.MapAction(leftClick);

            //Initialize the GridFieldManager
            GridFieldManager.GetInstance(this).Initialize();
         
            //Adjust the camera position
            SceneManager.RenderContext.Camera.Translate(300, 300, 300);
            SceneManager.RenderContext.Camera.Rotate(-45, 30, 150);

            // Menu
            m_Menu = new Menu(m_Content, 15);

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
            m_Menu.Update(renderContext, m_InputManager);

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
            m_Menu.Draw(renderContext);

            // Show Selection
            renderContext.SpriteBatch.DrawString(m_DebugFont, "Selected: " + m_BuildSelection, new Vector2(10, 30), Color.Black);

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

            // Handle Keyboard Input
            KeyboardState keyboardState = renderContext.Input.CurrentKeyboardState;
            // Handle GamePad Input
            m_GamePadState = GamePad.GetState(PlayerIndex.One);


            // Handle Mouse Input
            var mPos = new Vector2(renderContext.Input.CurrentMouseState.X, renderContext.Input.CurrentMouseState.Y);

            //Raycast to grid
            GridTile hittedTile = null;
            if (m_InputManager.GetAction((int)PlayerInput.LeftClick).IsTriggered)
                hittedTile = GridFieldManager.GetInstance(this).HitTestField(CalculateCursorRay(renderContext));

            if (hittedTile != null)
            {
                hittedTile.Selected = true;
            }
            
            //Selection of what to build
            if (keyboardState[Keys.U] == KeyState.Down)
                m_BuildSelection = BuildSelection.BuildingAwesome;
            
            if (keyboardState[Keys.I] == KeyState.Down)
                m_BuildSelection = BuildSelection.BuildingStupid;
            
            if (keyboardState[Keys.O] == KeyState.Down)            
                m_BuildSelection = BuildSelection.BuildingOutOfInspiration;


            //CAMERA
            //Camera Vectors     
            Vector3 forwardVecCam = GetForwardVectorOfQuaternion(renderContext.Camera.LocalRotation);
            forwardVecCam.Y = 0;
            forwardVecCam.Normalize();
            Vector3 rightVecCam = GetRightVectorOfQuaternion(renderContext.Camera.LocalRotation);
            rightVecCam.Y = 0;
            rightVecCam.Normalize();

            
            //GamePad
            //THIS MIGHT NOT WORK DRIES, fidle around with the - and + of the vectors, also the right vector isn't totally right.
            if (m_GamePadState.IsConnected)
            {
                if (m_GamePadState.ThumbSticks.Left.Y > 0)
                    renderContext.Camera.LocalPosition += -forwardVecCam * m_GamePadState.ThumbSticks.Left.Y;
                if (m_GamePadState.ThumbSticks.Left.X < 0)
                    renderContext.Camera.LocalPosition += rightVecCam * m_GamePadState.ThumbSticks.Left.X;
                if (m_GamePadState.ThumbSticks.Left.Y < 0)
                    renderContext.Camera.LocalPosition += forwardVecCam * m_GamePadState.ThumbSticks.Left.Y;
                if (m_GamePadState.ThumbSticks.Left.X > 0)
                    renderContext.Camera.LocalPosition += -rightVecCam * m_GamePadState.ThumbSticks.Left.X;
            }



            if (keyboardState[Keys.Z] == KeyState.Down)
                renderContext.Camera.LocalPosition += -forwardVecCam * 10;
            if (keyboardState[Keys.Q] == KeyState.Down)
                renderContext.Camera.LocalPosition += rightVecCam * 10;
            if (keyboardState[Keys.S] == KeyState.Down)
                renderContext.Camera.LocalPosition += forwardVecCam * 10;
            if (keyboardState[Keys.D] == KeyState.Down)
                renderContext.Camera.LocalPosition += -rightVecCam * 10;
        }

        public Ray CalculateCursorRay(RenderContext renderContext)
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
        public Vector3 GetForwardVectorOfQuaternion(Quaternion q) 
        {
            return new Vector3(2 * (q.X * q.Z + q.W * q.Y),
                            2 * (q.Y * q.X - q.W * q.X),
                            1 - 2 * (q.X * q.X + q.Y * q.Y));
        }
        public Vector3 GetUpVectorOfQuaternion(Quaternion q) 
        {
            return new Vector3(2 * (q.X * q.Y - q.W * q.Z),
                            1 - 2 * (q.X * q.Z + q.Z * q.Z),
                            2 * (q.Y * q.Z + q.W * q.X));
        }
        public Vector3 GetRightVectorOfQuaternion(Quaternion q) 
        {
            return new Vector3(1 - 2 * (q.Y * q.Y + q.Z * q.Z),
                2 * (q.X * q.Y + q.W * q.Z),
                2 * (q.X * q.Z - q.W * q.Y));
        }
    }
}
