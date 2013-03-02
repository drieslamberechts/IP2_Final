using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNA_ENGINE;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

using XNA_ENGINE.Engine;
using XNA_ENGINE.Engine.Scenegraph;
using XNA_ENGINE.Engine.Objects;

using XNA_ENGINE.Engine.Helpers;

using XNA_ENGINE.Game.Objects;

using IP2_Xna_Template.Objects;
using Microsoft.Xna.Framework.Media;
using XNA_ENGINE.Game.Scenes;

using XNA_ENGINE.Game.Objects.Concept2;

namespace XNA_ENGINE.Game
{
    public class GameSceneConcept2 : GameScene
    {
        enum PlayerInput
        {
            GoUp,
            GoDown,
            Shoot
        }

        private const int GRID_ROW_LENGTH = 30;
        private const int GRID_COLUMN_LENGTH = 30;
        private const int GRID_OFFSET = 25;
        private const int SCREEN_OFFSET_HORIZONTAL = 50;
        private const int SCREEN_OFFSET_VERTICAL = 50;


      //  private GridTile[][] m_GridField = new GridTile[GRID_ROW_LENGTH][];

        private List<List<GridTile>> m_GridField;

        private ContentManager Content;
        private InputManager m_InputManager;



        public GameSceneConcept2(ContentManager content)
            : base("GameSceneConcept2")
        {
            //CONTENT
            Content = content;

            //INPUT
            m_InputManager = new InputManager();

            InputAction GoUp = new InputAction((int)PlayerInput.GoUp, TriggerState.Down);
            GoUp.KeyButton = Keys.Z;
            GoUp.GamePadButton = Buttons.LeftThumbstickUp;

            InputAction GoDown = new InputAction((int)PlayerInput.GoDown, TriggerState.Down);
            GoDown.KeyButton = Keys.S;
            GoDown.GamePadButton = Buttons.LeftThumbstickDown;

            InputAction Shoot = new InputAction((int)PlayerInput.Shoot, TriggerState.Pressed);
            Shoot.KeyButton = Keys.Space;
            Shoot.GamePadButton = Buttons.RightTrigger;

            m_InputManager.MapAction(GoUp);
            m_InputManager.MapAction(GoDown);
            m_InputManager.MapAction(Shoot);

            m_GridField = new List<List<GridTile>>();

            //GENERATE GRIDFIELD
            for (int i = 0; i < GRID_ROW_LENGTH; i++)
            {
                List<GridTile> tempList = new List<GridTile>();
                for (int j = 0; j < GRID_COLUMN_LENGTH; j++)
                {
                    tempList.Add(new GridTile(GridTile.TileType.Normal, new Vector2((i * GRID_OFFSET) + SCREEN_OFFSET_HORIZONTAL, (j * GRID_OFFSET) + SCREEN_OFFSET_VERTICAL)));
                }
                m_GridField.Add(tempList);
            }
        }

        public override void Initialize()
        {
            // Add Interaction Scene for later use
            // -------------------------------------
            var interactionScene = new InteractionScene(Content);
            SceneManager.AddGameScene(interactionScene);
            //interactionScene.Initialize(20, 220);
            //SceneManager.SetActiveScene("InteractionScene");
            foreach (var gridTileList in m_GridField)
            {
                foreach (var gridTile in gridTileList)
                {
                    gridTile.Initialize();
                    gridTile.GetSprite(0).CanDraw = true;
                    gridTile.GetSprite(1).CanDraw = false;
                    AddSceneObject(gridTile.GetSprite(0));
                    AddSceneObject(gridTile.GetSprite(1));
                }
            }
            base.Initialize();
        }

        public override void LoadContent(ContentManager contentManager)
        {
            base.LoadContent(contentManager);
        }

        public override void Update(RenderContext renderContext)
        {
            //INPUT
            m_InputManager.Update();
            if (m_InputManager.GetAction((int)PlayerInput.GoUp).IsTriggered)
                System.Console.WriteLine("Up");

            if (m_InputManager.GetAction((int)PlayerInput.GoDown).IsTriggered)
                System.Console.WriteLine("Down");

            if (m_InputManager.GetAction((int)PlayerInput.Shoot).IsTriggered)
                System.Console.WriteLine("Shoot");

            foreach (var gridTileList in m_GridField)
            {
                foreach (var gridTile in gridTileList)
                {
                    gridTile.Update();
                }
            }

            base.Update(renderContext);
        }

        public override void Draw2D(RenderContext renderContext, bool drawBefore3D)
        {
            base.Draw2D(renderContext, drawBefore3D);
        }

        public override void Draw3D(RenderContext renderContext)
        {
            base.Draw3D(renderContext);
        }
    }

}
