using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using XNA_ENGINE.Engine;
using XNA_ENGINE.Engine.Objects;

namespace VampirePuzzle.Framework
{
    public class GameButton : GameSprite
    {
        public event Action OnClick;
        public event Action OnEnter;
        public event Action OnLeave;

        private bool _isSpriteSheet;
        private bool _isHovered;

        private Rectangle? _normalRect, _hoverRect, _pressedRect;
        private GameHitRegion2D _hitRegion;

        public GameButton(string assetFile, bool isSpriteSheet = false) :
            base(assetFile)
        {
            _isSpriteSheet = isSpriteSheet;

            _hitRegion = new GameHitRegion2D();
            AddChild(_hitRegion);
        }

        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);

            //Set Dimensions after the button texture is loaded, otherwise we can't extract the width and height
            if (_isSpriteSheet)
            {
                _hitRegion.SetDimensions((int)Width, (int)(Height / 3.0f));

                //Only use these Rectangle properties when the button texture is a spritesheet
                _normalRect = new Rectangle(0, 0, (int)Width, (int)(Height / 3.0f));
                _hoverRect = new Rectangle(0, (int)(Height / 3.0f), (int)Width, (int)(Height / 3.0f));
                _pressedRect = new Rectangle(0, (int)(Height / 3.0f) * 2, (int)Width, (int)(Height / 3.0f));

                DrawRect = _normalRect;
            }
            else _hitRegion.SetDimensions((int)Width, (int)Height);
        }

        public override void Update(RenderContext renderContext)
        {
            base.Update(renderContext);

            var currMouseX = renderContext.Input.CurrentMouseState.X;
            var currMouseY = renderContext.Input.CurrentMouseState.Y;

            if (!_isHovered)
            {
                if (_hitRegion.HitTest(currMouseX, currMouseY))
                {
                    _isHovered = true;

                    if (OnEnter != null) OnEnter();
                    DrawRect = _hoverRect;
                }
            }
            else
            {
                var newMouseButtonState = renderContext.Input.CurrentMouseState.LeftButton;
                var oldMouseButtonState = renderContext.Input.OldMouseState.LeftButton;
                DrawRect = _hoverRect;

                if (!_hitRegion.HitTest(currMouseX, currMouseY))
                {
                    _isHovered = false;

                    if (OnLeave != null) OnLeave();
                    DrawRect = _normalRect;
                }
                else if (newMouseButtonState == ButtonState.Pressed)
                {
                    DrawRect = _pressedRect;
                }
                else if (newMouseButtonState == ButtonState.Released && oldMouseButtonState == ButtonState.Pressed)
                {
                    if (OnClick != null) OnClick();
                    DrawRect = _normalRect;
                }
            }
        }
    }
}
