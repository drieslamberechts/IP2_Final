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
    public class Army
    {
        public enum ArmyType
        {
            Green,
            Red,
            Blue,
            Yellow
        };

        private GameSprite m_ArmySprite;

        private Vector2 m_Position;
        private GridTile m_ActiveGridTile;
        private ArmyType m_Type;

        private int m_ArmySize;

        private bool m_Active = true;

        private GridTile.TileType m_BonusTile;
        private GridTile.TileType m_NegativeTile;

        private GridTile.TileType m_ActiveTileType;


        public Army(ArmyType armyType)
        {
            m_Type = armyType;
        }

        public void Initialize()
        {
            m_ActiveTileType = GridTile.TileType.Normal;

            switch (m_Type)
            {
                case ArmyType.Green:
                    m_ArmySprite = new GameSprite("GreenArmy");
                    m_ArmySize = 4;
                    m_BonusTile = GridTile.TileType.Green;
                    m_NegativeTile = GridTile.TileType.Red;
                    break;
                case ArmyType.Red:
                    m_ArmySprite = new GameSprite("RedArmy");
                    m_ArmySize = 3;
                    m_BonusTile = GridTile.TileType.Red;
                    m_NegativeTile = GridTile.TileType.Green;
                    break;
                case ArmyType.Blue:
                    m_ArmySprite = new GameSprite("BlueArmy");
                    m_ArmySize = 5;
                    m_BonusTile = GridTile.TileType.Red;
                    m_NegativeTile = GridTile.TileType.Green;
                    break;
                case ArmyType.Yellow:
                    m_ArmySprite = new GameSprite("YellowArmy");
                    m_ArmySize = 2;
                    m_BonusTile = GridTile.TileType.Red;
                    m_NegativeTile = GridTile.TileType.Green;
                    break;
            }
        }

        public void Update()
        {

        }

        public GameSprite GetSprite()
        {
            return m_ArmySprite;
        }

        public Vector2 GetPosition()
        {
            return m_Position;
        }

        public void SetTile(GridTile tile)
        {
            m_ActiveGridTile = tile;
            m_Position = tile.GetPosition();
            m_ArmySprite.Translate(m_Position);
            m_ArmySprite.CanDraw = true;
        }

        public GridTile GetActiveTile()
        {
            return m_ActiveGridTile;
        }

        public int GetArmySize()
        {
            return m_ArmySize;
        }

        public void AddArmySize(int size)
        {
            m_ArmySize += size;
        }

        public void SetActive(bool value)
        {
            m_Active = value;
            m_ArmySprite.CanDraw = value;
        }

        public bool GetActive()
        {
            return m_Active;
        }

        public GridTile.TileType GetBonusTile()
        {
            return m_BonusTile;
        }

        public GridTile.TileType GetNegativeTile()
        {
            return m_NegativeTile;
        }
    }
}