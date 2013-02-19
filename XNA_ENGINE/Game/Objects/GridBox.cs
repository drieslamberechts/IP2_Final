using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using XNA_ENGINE.Engine;
using XNA_ENGINE.Engine.Objects;
using XNA_ENGINE.Engine.Scenegraph;

namespace XNA_ENGINE.Game.Objects
{
    public class GridBox : GameObject2D
    {
        public enum TypesOfBoxes { MovableBox, StaticBox };

        private static GameSprite m_MovableBoxSprite;
        private static GameSprite m_StaticBoxSprite;

        TypesOfBoxes m_Type;

        // public GameButton(bool isSpriteSheet = false) :
        //        base(assetFile)

        public GridBox(TypesOfBoxes boxType)
            : base()
        {
            m_Type = boxType;
        }

        /*TypesOfBoxes.MovableBox;
_assetFile = assetFile;
Color = Color.White;
Effect = SpriteEffects.None;

public override void Draw(RenderContext renderContext)
{
    if (CanDraw)
    {
        renderContext.SpriteBatch.Draw(_texture, WorldPosition,
            DrawRect, Color, MathHelper.ToRadians(WorldRotation),
            Vector2.Zero, WorldScale, Effect, Depth);
        base.Draw(renderContext);
    }
}*/
        public override void Initialize()
        {
            m_MovableBoxSprite = new GameSprite("MovableBox");
            m_MovableBoxSprite.Translate(LocalPosition.X, LocalPosition.Y);
            m_StaticBoxSprite = new GameSprite("StaticBox");
            m_StaticBoxSprite.Translate(LocalPosition.X, LocalPosition.Y);
        }

        public override void Draw(RenderContext renderContext)
        {
            if (CanDraw)
            {
                switch (m_Type)
                {

                    case TypesOfBoxes.MovableBox:
                        m_MovableBoxSprite.Draw(renderContext);
                        break;
                    case TypesOfBoxes.StaticBox:
                        m_StaticBoxSprite.Draw(renderContext);
                        break;
                    default:
                        break;
                }
                base.Draw(renderContext);
            }
        }
    }
}