using IP2_Xna_Engine;
using IP2_Xna_Engine.Scenegraph;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IP2_Xna_Template.Scenes
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
            spritefont = Content.Load<SpriteFont>("menufont");
        }

        public override void Update(RenderContext renderContext)
        {
            // CHECK FOR EXTRA PRESSED BUTTONS (PAUSE BUTTON, ...)
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            if (gamePadState.IsConnected)
            {
                if (gamePadState.Buttons.Start == ButtonState.Pressed && m_bCanSwitchScene)
                {
                    SceneManager.SetActiveScene("MainScene");
                    m_bCanSwitchScene = false;
                }

                if (gamePadState.Buttons.Start == ButtonState.Released && !m_bCanSwitchScene)
                    m_bCanSwitchScene = true;
            }

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
