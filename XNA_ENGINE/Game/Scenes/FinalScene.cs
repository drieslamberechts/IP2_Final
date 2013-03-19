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
            ClickTile,
            RightClickTile
        }

        List<Tile> m_Tiles;

        // Controls
        GamePadState gamePadState;

        public FinalScene(ContentManager content)
            : base("FinalScene")
        {
            //CONTENT
        }

        public override void Initialize()
        {
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
            Viewport vp = renderContext.GraphicsDevice.Viewport;
            Vector3 pos1 = vp.Unproject(new Vector3(renderContext.Input.CurrentMouseState.X, renderContext.Input.CurrentMouseState.Y, 0),
                renderContext.Camera.Projection,
                renderContext.Camera.View,
                renderContext.Camera.GetWorldMatrix());
            Vector3 pos2 = vp.Unproject(new Vector3(renderContext.Input.CurrentMouseState.X, renderContext.Input.CurrentMouseState.Y, 1),
                renderContext.Camera.Projection,
                renderContext.Camera.View,
                renderContext.Camera.GetWorldMatrix());
            Vector3 dir = Vector3.Normalize(pos2 - pos1);


            System.Diagnostics.Debug.WriteLine("" + dir.ToString());



            // Handle Input
            HandleInput(renderContext);

            GridFieldManager.GetInstance(this).Update(renderContext);


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

        private void HandleInput(RenderContext renderContext)
        {
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

            // Handle Mouse Input
            Vector2 mousePos = new Vector2(renderContext.Input.CurrentMouseState.X,renderContext.Input.CurrentMouseState.Y);
        }
    }

    public class Tile
    {
        public Vector2 position { get; set; }
        public string type { get; set; }
        public string settlement { get; set; }
    }
}
