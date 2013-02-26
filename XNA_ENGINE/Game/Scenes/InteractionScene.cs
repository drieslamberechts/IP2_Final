using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNA_ENGINE.Engine.Scenegraph;

namespace XNA_ENGINE.Game.Scenes
{
    class InteractionScene : GameScene
    {
        // Variables
        private ContentManager Content;

        // Methods
        public InteractionScene(ContentManager content)
            :base("InteractionScene")
        {
            Content = content;
        }
    }
}
