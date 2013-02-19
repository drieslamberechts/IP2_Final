using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using XNA_ENGINE.Engine.Objects;
using XNA_ENGINE.Engine.Scenegraph;

namespace XNA_ENGINE.Game.Scenes
{
    public class GoingWildDemo:GameScene
    {
        private GameAnimatedSprite _animatedHeroSprite;
        private const int HOR_ACCELERATION = 300;
        private const int HOR_MAX_SPEED = 200;

        private const int VER_ACCELERATION = 1000;

        private const int GROUND_LEVEL = 300;
        private const int JUMP_SPEED = 400;

        private int _direction = 1;
        private Vector2 _velocity = Vector2.Zero;

        public GoingWildDemo():base("GoingWildDemo")
        {}

        public override void Initialize()
        {
            _animatedHeroSprite = new GameAnimatedSprite("Hero_SpriteSheet", 8, 100, new Point(64, 78));
            _animatedHeroSprite.Translate(200, GROUND_LEVEL);
            AddSceneObject(_animatedHeroSprite);

            _animatedHeroSprite.PlayAnimation(true);

            base.Initialize();
        }

        public override void Update(Engine.RenderContext renderContext)
        {
            KeyboardState currKeyboardState = Keyboard.GetState();

            if (currKeyboardState.IsKeyDown(Keys.Right))
            {
                _direction = 1;
                _animatedHeroSprite.PlayAnimation(true);
                _animatedHeroSprite.Effect = SpriteEffects.None;
                _velocity.X += (float)(HOR_ACCELERATION * renderContext.GameTime.ElapsedGameTime.TotalSeconds);
            }
            else if (currKeyboardState.IsKeyDown(Keys.Left))
            {
                _direction = -1;
                _animatedHeroSprite.PlayAnimation(true);
                _animatedHeroSprite.Effect = SpriteEffects.FlipHorizontally;
                _velocity.X -= (float)(HOR_ACCELERATION * renderContext.GameTime.ElapsedGameTime.TotalSeconds);
            }
            else
            {
                _velocity.X -= _direction * (float)(HOR_ACCELERATION * renderContext.GameTime.ElapsedGameTime.TotalSeconds);

                if (_direction > 0 && _velocity.X < 0) _velocity.X = 0;
                else if (_direction < 0 && _velocity.X > 0) _velocity.X = 0;

                _animatedHeroSprite.StopAnimation();
            }

            _velocity.X = MathHelper.Clamp(_velocity.X, -HOR_MAX_SPEED, HOR_MAX_SPEED);


            if (currKeyboardState.IsKeyDown(Keys.Space) && _animatedHeroSprite.LocalPosition.Y == GROUND_LEVEL)
            {
                _velocity.Y -= VER_ACCELERATION / 2.0f;
            }
            else _velocity.Y += VER_ACCELERATION * (float)renderContext.GameTime.ElapsedGameTime.TotalSeconds;

            if (_animatedHeroSprite.LocalPosition.Y > GROUND_LEVEL)
            {
                _velocity.Y = 0;
                _animatedHeroSprite.LocalPosition = new Vector2(_animatedHeroSprite.LocalPosition.X, GROUND_LEVEL);
            }

            var pos = _animatedHeroSprite.LocalPosition;
            pos += _velocity * (float)renderContext.GameTime.ElapsedGameTime.TotalSeconds;
            _animatedHeroSprite.Translate(pos);

            base.Update(renderContext);
        }
    }
}
