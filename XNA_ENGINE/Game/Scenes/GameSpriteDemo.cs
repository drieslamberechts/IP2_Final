using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using XNA_ENGINE.Engine.Objects;
using XNA_ENGINE.Engine.Scenegraph;
using XNA_ENGINE.Engine;

namespace XNA_ENGINE.Game.Scenes
{
    public class GameSpriteDemo:GameScene
    {
        private GameSprite _heroSprite;
        private GameSprite _heroSprite2;

        public GameSpriteDemo():base("GameSpriteDemo")
        {
            
        }

        public override void Initialize()
        {
            _heroSprite = new GameSprite("Hero");
            _heroSprite.Translate(250,300);
            AddSceneObject(_heroSprite);

            _heroSprite2 = new GameSprite("Hero");
            _heroSprite2.Translate(450,300);
            _heroSprite2.Effect = SpriteEffects.FlipHorizontally;
            AddSceneObject(_heroSprite2);

            base.Initialize();
        }
    }
}
