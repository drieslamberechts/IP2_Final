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

using XNA_ENGINE.Game.Objects.Concept2;
using GameModel = XNA_ENGINE.Engine.Objects.GameModel;

namespace XNA_ENGINE.Game.Scenes
{
    class FinalScene : GameScene
    {
        Model myModel;
        List<Tile> m_Tiles;

        // Load XML file
        private Stream m_XmlFile;

        public FinalScene(ContentManager content)
            : base("FinalScene")
        {
            //CONTENT
            myModel = content.Load<Model>("ship2");
        }

        public override void Initialize()
        {
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
            base.Update(renderContext);
        }

        public override void Draw2D(RenderContext renderContext, bool drawBefore3D)
        {
            base.Draw2D(renderContext, drawBefore3D);
        }

        public override void Draw3D(RenderContext renderContext)
        {
            Matrix[] modelTransforms = new Matrix[myModel.Bones.Count];

            //draw model
            Matrix worldMatrix = Matrix.CreateScale(0.01f, 0.01f, 0.01f);
            myModel.CopyAbsoluteBoneTransformsTo(modelTransforms);

            foreach (ModelMesh mesh in myModel.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.World = modelTransforms[mesh.ParentBone.Index] * worldMatrix;
                    effect.View = renderContext.Camera.View;
                    effect.Projection = renderContext.Camera.Projection;
                }
                mesh.Draw();
            }

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
