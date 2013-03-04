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

        public Army(ArmyType armyType)
        {
            m_Type = armyType;
        }

        public void Initialize()
        {
            switch (m_Type)
            {
                case ArmyType.Green:
                    m_ArmySprite = new GameSprite("GreenArmy");
                    break;
                case ArmyType.Red:
                    m_ArmySprite = new GameSprite("RedArmy");
                    break;
                case ArmyType.Blue:
                    m_ArmySprite = new GameSprite("BlueArmy");
                    break;
                case ArmyType.Yellow:
                    m_ArmySprite = new GameSprite("YellowArmy");
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
        }

        public GridTile GetActiveTile()
        {
            return m_ActiveGridTile;
        }
    }
}