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
        public enum Settlement { None, Green, Red, Yellow, Blue };

        private GameSprite m_NormalTile;
        private GameSprite m_InactiveTile;
        private GameSprite m_DummyTile1;
        private GameSprite m_DummyTile2;

        private GameSprite m_GreenSettlement;
        private GameSprite m_RedSettlement;
        private GameSprite m_YellowSettlement;
        private GameSprite m_BlueSettlement;

        private static GameSprite m_Selector;

        private bool m_IsSelected = false;

        private Vector2 m_Position;

        TileType m_Type;
        Settlement m_Settlement;

        public GridTile(TileType tileType, Vector2 positon)
        {
            m_Type = tileType;
            m_Settlement = Settlement.None;
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

            m_GreenSettlement = new GameSprite("GreenSettlement");
            m_RedSettlement = new GameSprite("RedSettlement");
            m_YellowSettlement = new GameSprite("YellowSettlement");
            m_BlueSettlement = new GameSprite("BlueSettlement");

            m_Selector =new GameSprite("Selector");

            UpdateTypes();
        }

        public void Update()
        {
            
        }

        public GameSprite GetSprite(int id)
        {
            if (id == 0)
                return m_NormalTile;
            if (id == 1)
                return m_InactiveTile;
            if (id == 2)
                return m_DummyTile1;
            if (id == 3)
                return m_DummyTile2;
            if (id == 4)
                return m_GreenSettlement;
            if (id == 5)
                return m_RedSettlement;
            if (id == 6)
                return m_YellowSettlement;
            if (id == 7)
                return m_BlueSettlement;

            return m_Selector;
        }

        public void SetTileType(TileType type)
        {
            m_Type = type;
            UpdateTypes();
        }

        public void SetSettlement(Settlement type)
        {
            m_Settlement = type;

            switch (type)
            {
                case Settlement.Green:
                    m_GreenSettlement.Translate(m_Position.X, m_Position.Y);
                    break;

                case Settlement.Red:
                    m_RedSettlement.Translate(m_Position.X, m_Position.Y);
                    break;

                case Settlement.Yellow:
                    m_YellowSettlement.Translate(m_Position.X, m_Position.Y);
                    break;

                case Settlement.Blue:
                    m_BlueSettlement.Translate(m_Position.X, m_Position.Y);
                    break;
            }

            UpdateTypes();
        }

        public void UpdateTypes()
        {
            m_NormalTile.CanDraw = false;
            m_InactiveTile.CanDraw = false;
            m_DummyTile1.CanDraw = false;
            m_DummyTile2.CanDraw = false;

            m_GreenSettlement.CanDraw = false;
            m_RedSettlement.CanDraw = false;
            m_YellowSettlement.CanDraw = false;
            m_BlueSettlement.CanDraw = false;

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

            switch (m_Settlement)
            {
                case Settlement.Green:
                    m_GreenSettlement.CanDraw = true;
                    break;

                case Settlement.Red:
                    m_RedSettlement.CanDraw = true;
                    break;

                case Settlement.Yellow:
                    m_YellowSettlement.CanDraw = true;
                    break;

                case Settlement.Blue:
                    m_BlueSettlement.CanDraw = true;
                    break;
            }
        }

        public Vector2 GetPosition()
        {
            return m_Position;
        }

        public void SetSelector(bool value)
        {
            if (value)
            {
                m_IsSelected = true;
                m_Selector.CanDraw = true;
                m_Selector.Translate(m_Position);
            }
            else
            {
                m_IsSelected = false;
                m_Selector.CanDraw = false;
            }
        }
    }
}