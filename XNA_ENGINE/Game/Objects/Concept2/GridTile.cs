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
        public enum TileType { Normal, Inactive, Dummy1, Dummy2 };

        private GameSprite m_NormalTile;
        private GameSprite m_InactiveTile;
        private GameSprite m_DummyTile1;
        private GameSprite m_DummyTile2;

        private Vector2 m_Position;

        TileType m_Type;

        public GridTile(TileType tileType, Vector2 positon)
        {
            m_Type = tileType;
            m_Position = positon;
        }

        public void Initialize()
        {
            m_NormalTile = new GameSprite("OrangeTile");
            m_NormalTile.Translate(m_Position.X, m_Position.Y);
            m_InactiveTile = new GameSprite("GreyTile");
            m_InactiveTile.Translate(m_Position.X, m_Position.Y);
            m_DummyTile1 = new GameSprite("RedTile");
            m_DummyTile1.Translate(m_Position.X, m_Position.Y);
            m_DummyTile2 = new GameSprite("GreenTile");
            m_DummyTile2.Translate(m_Position.X, m_Position.Y);

            UpdateTileType();
        }

        public void Update()
        {
            
        }

        public GameSprite GetSprite(TileType id)
        {
            if (id == TileType.Normal)
                return m_NormalTile;
            if (id == TileType.Inactive)
                return m_InactiveTile;
            if (id == TileType.Dummy1)
                return m_DummyTile1;
            
            return m_DummyTile2;
        }

        public void SetType(TileType type)
        {
            m_Type = type;
            UpdateTileType();
        }

        public void UpdateTileType()
        {
            m_NormalTile.CanDraw = false;
            m_InactiveTile.CanDraw = false;
            m_DummyTile1.CanDraw = false;
            m_DummyTile2.CanDraw = false;


            switch (m_Type)
            {
                case TileType.Normal:
                    m_NormalTile.CanDraw = true;
                    break;

                case TileType.Inactive:
                    m_InactiveTile.CanDraw = true;
                    break;

                case TileType.Dummy1:
                    m_DummyTile1.CanDraw = true;
                    break;

                case TileType.Dummy2:
                    m_DummyTile2.CanDraw = true;
                    break;
            }
        }

        public Vector2 GetPosition()
        {
            return m_Position;
        }
    }
}