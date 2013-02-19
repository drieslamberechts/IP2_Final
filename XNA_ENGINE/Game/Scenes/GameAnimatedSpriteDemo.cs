using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XNA_ENGINE.Engine.Objects;
using XNA_ENGINE.Engine.Scenegraph;

namespace XNA_ENGINE.Game.Scenes
{
    public class GameAnimatedSpriteDemo:GameScene
    {
        private GameAnimatedSprite _animatedHeroSprite;

        public GameAnimatedSpriteDemo():base("GameAnimatedSpriteDemo"){}

        public override void Initialize()
        {
            _animatedHeroSprite = new GameAnimatedSprite("Hero_SpriteSheet",8,100, new Point(64,78));
            AddSceneObject(_animatedHeroSprite);

            _animatedHeroSprite.PlayAnimation(true);

            base.Initialize();
        }
    }
}
