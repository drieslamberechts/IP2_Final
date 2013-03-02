using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XNA_ENGINE.Engine;
using XNA_ENGINE.Engine.Objects;
using XNA_ENGINE.Engine.Scenegraph;
using XNA_ENGINE.Game.Objects;


namespace XNA_ENGINE.Game.Objects.Concept2
{
    public class GridTile
    {
        public enum TileType { Normal, Inactive };

        private static GameSprite m_NormalTile;
        private static GameSprite m_InactiveTile;

        private Vector2 m_Position;

        TileType m_Type;

        public GridTile(TileType tileType, Vector2 positon)
        {
            m_Type = tileType;
            m_Position = positon;
        }

        public void Update()
        {

        }

        public void Initialize()
        {
            m_NormalTile = new GameSprite("OrangeTile");
            m_NormalTile.Translate(m_Position.X, m_Position.Y);
            m_InactiveTile = new GameSprite("GreyTile");
            m_InactiveTile.Translate(m_Position.X, m_Position.Y);

            switch (m_Type)
            {
                case TileType.Normal:
                    m_NormalTile.CanDraw = true;

                    break;
                case TileType.Inactive:
                    m_InactiveTile.CanDraw = true;
                    break;
            }
        }

        public GameSprite GetSprite(int id)
        {
            if (id == 0)
                return m_NormalTile;
            if (id == 1)
                return m_InactiveTile;

            return m_NormalTile;
        }
    }
}