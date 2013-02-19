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
    public class FiddleDemoScene:GameScene
    {
        //Create Box
        private GameSprite m_Box,m_Hero;
        private GameButton m_TestButton;
       // private GameH
       // private GridBox m_TestGridBox;
 
        //Create a list of all points
        private List<Vector2> m_PosList = new List<Vector2>();

        //Define the field dimensions
        private int m_FieldWidth = 10;
        private int m_FieldHeight = 10;
        private int m_FieldTotal;
        private Vector2 m_StartFieldPos = new Vector2(20, 20);

        private int m_BoxWidth = 32;
        private int m_BoxHeight = 32;

       // private GridBox m_TestGridBox;

        public FiddleDemoScene() : base("FiddleDemoScene") { }

        public override void Initialize()
        {
            m_FieldTotal = m_FieldWidth * m_FieldHeight;

            m_Box = new GameSprite("MovableBox");
            m_Box.Translate(250, 300);
            AddSceneObject(m_Box);

          /*  m_Hero = new GameSprite("HeroBox");
            m_Hero.Translate(100, 200);
            AddSceneObject(m_Hero);*/

            m_TestButton = new GameButton("StaticBox");
            m_TestButton.Translate(100, 100);
            AddSceneObject(m_TestButton);

            m_TestButton.OnClick += new Action(BoxClicked);


           /* m_TestGridBox = new GridBox(GridBox.TypesOfBoxes.MovableBox);
            m_TestGridBox.Translate(100, 100);
            AddSceneObject(m_TestGridBox);
            */

           // m_PosList.ex
            //m_PosList.Add(651);

            for (int i = 0; i < m_FieldHeight; ++i)
            {
                for (int j = 0; j < m_FieldWidth; ++j)
                {
                    m_PosList.Add(new Vector2(m_StartFieldPos.X + (j * m_BoxWidth), m_StartFieldPos.Y + (i * m_BoxHeight)));
                }
            }

            base.Initialize();
        }

        public override void Update(Engine.RenderContext renderContext)
        {
            int mill = renderContext.GameTime.ElapsedGameTime.Milliseconds;
            double deltaTime = (double)mill / 1000.0;
            double scale = Math.Sin((double)renderContext.GameTime.TotalGameTime.Milliseconds);

          //  m_Box.Scale((float)(scale), (float)(scale));
            //m_Box.Scale(
            
            //if (m_TestButton.OnEnter)
            //{

            //}

            base.Update(renderContext);
        }

        public void BoxClicked()
        {
            Console.WriteLine("Box clicked");
        }
    }
}
