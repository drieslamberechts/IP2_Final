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
        List<Tile> m_Tiles; 


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
            SceneManager.RenderContext.Camera.Rotate(-45, 45, 150);


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
            System.Diagnostics.Debug.WriteLine("Count: " + m_Tiles.ElementAt(0).type);
            // ------------------------------------------
            // END READING XML FILE
            // ------------------------------------------


            base.Initialize();
        }

        public override void Update(RenderContext renderContext)
        {
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
    }

    public class Tile
    {
        public Vector2 position { get; set; }
        public string type { get; set; }
        public string settlement { get; set; }
    }
}
