using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using XNA_ENGINE.Engine.Objects;
using XNA_ENGINE.Engine.Scenegraph;
using XNA_ENGINE.Game.Objects;
using VampirePuzzle.Framework;

namespace XNA_ENGINE.Game.Scenes
{
    class GameSceneConcept1:GameScene
    {
        private GameSprite m_SmileySprite;

        public GameSceneConcept1() : base("GameSceneConcept1") { }

        public override void Initialize()
        {
            m_SmileySprite = new GameSprite("TestSmiley");
            m_SmileySprite.Translate(0, 0);
            AddSceneObject(m_SmileySprite);
        }


        // Test Dries

    }
}
