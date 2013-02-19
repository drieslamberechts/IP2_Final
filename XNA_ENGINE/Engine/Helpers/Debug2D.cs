using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace XNA_ENGINE.Engine.Helpers
{
    public static class Debug2D
    {
        private struct DebugLine
        {
            public Vector2 Point1;
            public Vector2 Point2;
            public Color Color;
        }

        private static Texture2D _pixel;
        private static List<DebugLine> _lines = new List<DebugLine>();


        public static void LoadContent(ContentManager contentManager)
        {
#if DEBUG
            if (_pixel == null)
                _pixel = contentManager.Load<Texture2D>("WhitePixel");
#endif
        }

        public static void DrawLine(Vector2 point1, Vector2 point2, Color color)
        {
#if DEBUG
            _lines.Add(new DebugLine { Point1 = point1, Point2 = point2, Color = color });
#endif
        }

        public static void DrawRectangle(Rectangle rect, Color color)
        {
            var point1 = new Vector2(rect.Location.X, rect.Location.Y);
            var point2 = point1 + new Vector2(rect.Width, 0);
            var point3 = point1 + new Vector2(rect.Width, rect.Height);
            var point4 = point1 + new Vector2(0, rect.Height);

            DrawLine(point1, point2, color);
            DrawLine(point2, point3, color);
            DrawLine(point3, point4, color);
            DrawLine(point4, point1, color);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
#if DEBUG
            foreach (var line in _lines)
            {
                var distance = Vector2.Distance(line.Point1, line.Point2);
                var angle = (float)Math.Atan2((line.Point2.Y - line.Point1.Y), (line.Point2.X - line.Point1.X));

                spriteBatch.Draw(_pixel, line.Point1, null, line.Color, angle, Vector2.Zero, new Vector2(distance, 1), SpriteEffects.None, 1.0f);
            }

            _lines.Clear();
#endif
        }
    }
}
