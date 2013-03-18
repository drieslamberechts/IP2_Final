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

using Microsoft.Xna.Framework.Media;
using XNA_ENGINE.Game.Scenes;

using XNA_ENGINE.Game.Objects.Concept2;

namespace XNA_ENGINE.Game.Scenes
{
    class FinalScene : GameScene
    {
        private GridTile m_TestTile;

        public FinalScene(ContentManager content)
            : base("FinalScene")
        {

        }

        public override void Initialize()
        {
            m_TestTile = new GridTile(this);


            //Set the camera a bit backwards
            SceneManager.RenderContext.Camera.Translate(300, 300, 300);
            SceneManager.RenderContext.Camera.Rotate(-40, 45, 0);

            base.Initialize();
        }

        public override void Update(Engine.RenderContext renderContext)
        {
        
            base.Update(renderContext);
        }

    }
}
