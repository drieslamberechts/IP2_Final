using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using XNA_ENGINE.Engine.Objects;
using XNA_ENGINE.Engine.Scenegraph;

namespace XNA_ENGINE.Game.Scenes
{
    public class GameModelDemo:GameScene
    {
        private GameAnimatedModel _model;

        public GameModelDemo():base("GameModelDemo"){}

        public override void Initialize()
        {
            _model = new GameAnimatedModel("Vampire");
            AddSceneObject(_model);

            SceneManager.RenderContext.Camera.Translate(0,25,150);

            base.Initialize();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);

            _model.PlayAnimation("Run");
        }

        public override void Update(Engine.RenderContext renderContext)
        {
            _model.Rotate(0,45.0f*(float)renderContext.GameTime.TotalGameTime.TotalSeconds,0);

            base.Update(renderContext);
        }
    }
}
