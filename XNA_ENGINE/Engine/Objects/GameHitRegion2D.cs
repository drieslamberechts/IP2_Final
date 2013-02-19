using Microsoft.Xna.Framework;
using XNA_ENGINE.Engine.Helpers;
using XNA_ENGINE.Engine.Scenegraph;

namespace XNA_ENGINE.Engine.Objects
{
    public class GameHitRegion2D : GameObject2D
    {
        public enum HitRegionBorder
        {
            Top,
            Bottom,
            Left,
            Right,
            Undefined
        }

        public Color DrawColor { get; set; }
        private Rectangle _hitRect;

        public GameHitRegion2D()
        {
            _hitRect = Rectangle.Empty;
            DrawColor = Color.Red;
            CanDraw = false;
        }

        public GameHitRegion2D(int width, int height)
        {
            _hitRect = new Rectangle(0, 0, width, height);
            DrawColor = Color.Red;

            CanDraw = false;
        }

        public void SetDimensions(int width, int height)
        {
            _hitRect = new Rectangle(0, 0, width, height);
        }

        public bool HitTest(int x, int y)
        {
            return _hitRect.Contains(x, y);
        }

        public bool HitTest(GameHitRegion2D hitRegion)
        {
            return _hitRect.Intersects(hitRegion._hitRect);
        }

        public Rectangle Intersection(GameHitRegion2D hitRegion)
        {
            return Rectangle.Intersect(hitRegion._hitRect, _hitRect);
        }

        public override void Update(RenderContext renderContext)
        {
            base.Update(renderContext);

            _hitRect.Location = new Point((int)WorldPosition.X, (int)WorldPosition.Y);

            if (CanDraw) Debug2D.DrawRectangle(_hitRect, DrawColor);
        }

        public HitRegionBorder GetClosestVerticalBorder(GameHitRegion2D hitRegion)
        {
            var otherTopPoint = hitRegion._hitRect.Location.Y;
            var otherBottomPoint = otherTopPoint + hitRegion._hitRect.Height;

            var myTopPoint = _hitRect.Location.Y;
            var myBottomPoint = myTopPoint + _hitRect.Height;

            if (otherTopPoint < myTopPoint && myTopPoint < otherBottomPoint) return HitRegionBorder.Top;
            if (otherTopPoint < myBottomPoint && myBottomPoint < otherBottomPoint) return HitRegionBorder.Bottom;

            return HitRegionBorder.Undefined;
        }

        public HitRegionBorder GetClosestHorizontalBorder(GameHitRegion2D hitRegion)
        {
            var otherLeftPoint = hitRegion._hitRect.Location.X;
            var otherRightPoint = otherLeftPoint + hitRegion._hitRect.Width;

            var myLeftPoint = _hitRect.Location.X;
            var myRightPoint = myLeftPoint + _hitRect.Width;

            if (otherLeftPoint < myLeftPoint && myLeftPoint < otherRightPoint) return HitRegionBorder.Left;
            if (otherLeftPoint < myRightPoint && myRightPoint < otherRightPoint) return HitRegionBorder.Right;

            return HitRegionBorder.Undefined;
        }
    }
}
