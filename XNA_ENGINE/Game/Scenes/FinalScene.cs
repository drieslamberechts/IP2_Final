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

        List<Tile> m_Tiles;

        // Controls
        GamePadState gamePadState;
        private InputManager m_InputManager;
        private float elapseTime;
        private int frameCounter;
        private int FPS;
        private SpriteFont m_DebugFont;


        public FinalScene(ContentManager content)
            : base("FinalScene")
        {
            //CONTENT

            // FONT
            m_DebugFont = content.Load<SpriteFont>("Fonts/DebugFont");
        }

        public override void Initialize()
        {
            //Input manager + inputs
            m_InputManager = new InputManager();

            InputAction LeftClick = new InputAction((int)PlayerInput.LeftClick, TriggerState.Pressed);
            LeftClick.MouseButton = MouseButtons.LeftButton;
            m_InputManager.MapAction(LeftClick);

            //Initialize the GridFieldManager
            GridFieldManager.GetInstance(this).Initialize();
         
            //Adjust the camera position
            SceneManager.RenderContext.Camera.Translate(300, 300, 300);
            SceneManager.RenderContext.Camera.Rotate(-45, 30, 150);


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


            base.Initialize();
        }

        public override void Update(RenderContext renderContext)
        {
            // FPS
            elapseTime += (float)renderContext.GameTime.ElapsedGameTime.TotalSeconds;
            frameCounter++;

            if (elapseTime > 1)
            {
                FPS = frameCounter;
                frameCounter = 0;
                elapseTime = 0;
            }

            // Handle Input
            HandleInput(renderContext);

            GridFieldManager.GetInstance(this).Update(renderContext);

            base.Update(renderContext);
        }

        public override void Draw2D(RenderContext renderContext, bool drawBefore3D)
        {
            // Show FPS 2
            renderContext.SpriteBatch.DrawString(m_DebugFont, "FPS: " + FPS, new Vector2(10, 10), Color.White);

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

            // Handle Mouse Input
            Vector2 mPos = new Vector2(renderContext.Input.CurrentMouseState.X, renderContext.Input.CurrentMouseState.Y);

            //Raycast to grid
            if (m_InputManager.GetAction((int)PlayerInput.LeftClick).IsTriggered)
            {
                Viewport vp = renderContext.GraphicsDevice.Viewport;
                Vector3 pos1 = vp.Unproject(new Vector3(mPos.X, mPos.Y, 0),
                    renderContext.Camera.Projection,
                    renderContext.Camera.View,
                    renderContext.Camera.GetWorldMatrix());
                Vector3 pos2 = vp.Unproject(new Vector3(mPos.X, mPos.Y, 1),
                    renderContext.Camera.Projection,
                    renderContext.Camera.View,
                    renderContext.Camera.GetWorldMatrix());
                Vector3 dir = Vector3.Normalize(pos2 - pos1);
                //dir = -renderContext.Camera.View.Forward;

                System.Diagnostics.Debug.WriteLine("" + dir.ToString());

                Ray ray = new Ray(renderContext.Camera.WorldPosition, dir);

                GridFieldManager.GetInstance(this).HitTestField(ray);

               /* System.Diagnostics.Debug.WriteLine("" + dir.ToString() + 
                    renderContext.Camera.Projection.ToString() + 
                    renderContext.Camera.View.ToString() + 
                    renderContext.Camera.GetWorldMatrix().ToString());*/
            }

            // Handle GamePad Input
            gamePadState = GamePad.GetState(PlayerIndex.One);

            //GamePad
            if (gamePadState.IsConnected)
            {
                if (gamePadState.ThumbSticks.Left.Y > 0)
                    renderContext.Camera.LocalPosition += new Vector3(0, 0, -10 * gamePadState.ThumbSticks.Left.Y);
                if (gamePadState.ThumbSticks.Left.X < 0)
                    renderContext.Camera.LocalPosition += new Vector3(10 * gamePadState.ThumbSticks.Left.X, 0, 0);
                if (gamePadState.ThumbSticks.Left.Y < 0)
                    renderContext.Camera.LocalPosition += new Vector3(0, 0, -10 * gamePadState.ThumbSticks.Left.Y);
                if (gamePadState.ThumbSticks.Left.X > 0)
                    renderContext.Camera.LocalPosition += new Vector3(10 * gamePadState.ThumbSticks.Left.X, 0, 0);
            }

            // Handle Keyboard Input
            KeyboardState keyboardState = renderContext.Input.CurrentKeyboardState;

            if (keyboardState[Keys.Z] == KeyState.Down)
                renderContext.Camera.LocalPosition += new Vector3(0, 0, -10);
            if (keyboardState[Keys.Q] == KeyState.Down)
                renderContext.Camera.LocalPosition += new Vector3(-10, 0, 0);
            if (keyboardState[Keys.S] == KeyState.Down)
                renderContext.Camera.LocalPosition += new Vector3(0, 0, 10);
            if (keyboardState[Keys.D] == KeyState.Down)
                renderContext.Camera.LocalPosition += new Vector3(10, 0, 0);
        }
    }

    public class Tile
    {
        public Vector2 position { get; set; }
        public string type { get; set; }
        public string settlement { get; set; }
    }
}
