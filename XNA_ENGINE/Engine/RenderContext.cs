using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using XNA_ENGINE.Engine.Helpers;
using XNA_ENGINE.Game.Objects;

namespace XNA_ENGINE.Engine
{
    public class RenderContext
    {
        public SpriteBatch SpriteBatch { get; set; }
        public GraphicsDevice GraphicsDevice { get; set; }
        public GameTime GameTime { get; set; }
        public InputManager Input { get; set; }
        public BaseCamera Camera { get; set; }
    }

}
