using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNA_ENGINE.Engine;
using XNA_ENGINE.Engine.Scenegraph;


namespace XNA_ENGINE.Game
{
    class PauseScreen : GameScene
    {
        ContentManager Content;
        SpriteBatch spriteBatch;
        SpriteFont spritefont;

        Boolean m_bCanSwitchScene = true;

        public PauseScreen(ContentManager content)
            : base("PauseScreen")
        {
            Content = content;
            spritefont = Content.Load<SpriteFont>("Fonts/menufont");
        }

        public override void Update(RenderContext renderContext)
        {
            // CHECK FOR EXTRA PRESSED BUTTONS (PAUSE BUTTON, ...)
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            KeyboardState keyboardState = Keyboard.GetState(PlayerIndex.One);

            //GamePad
            if (gamePadState.IsConnected)
            {
                if (gamePadState.Buttons.Start == ButtonState.Pressed && m_bCanSwitchScene)
                {
                    SceneManager.SetActiveScene("GameSceneConcept1");
                    m_bCanSwitchScene = false;
                }

                if (gamePadState.Buttons.Start == ButtonState.Released && !m_bCanSwitchScene)
                    m_bCanSwitchScene = true;
            }

            //Keyboard
            if (keyboardState[Keys.P] == KeyState.Down && m_bCanSwitchScene)
            {
                SceneManager.SetActiveScene("GameSceneConcept1");
                m_bCanSwitchScene = false;
            }

            if (keyboardState[Keys.P] == KeyState.Up && !m_bCanSwitchScene)
                m_bCanSwitchScene = true;

            base.Update(renderContext);
        }

        public override void Draw2D(RenderContext renderContext, bool drawBefore3D)
        {
            spriteBatch = new SpriteBatch(renderContext.GraphicsDevice);

            spriteBatch.Begin();
                spriteBatch.DrawString(spritefont, "Test", new Vector2(10, 10), Color.Black);
            spriteBatch.End();
        }
    }
}
