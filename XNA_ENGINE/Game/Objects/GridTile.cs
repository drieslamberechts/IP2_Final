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


namespace XNA_ENGINE.Game.Objects
{
    public class GridTile
    {
        private GameModel m_TileModel;
        private int m_Row, m_Column;

        private static float GRIDWIDTH = 65;
        private static float GRIDDEPTH = 65;
        //  private static float GRIDHEIGHT = 32;

        private string m_TileType;
        private string m_TileSettlement;

        private bool m_Selected { get; set; }

        public GridTile(GameScene pGameScene, int row, int column)
        {
            m_Row = row;
            m_Column = column;

            m_TileModel = new GameModel("Models/tile_Template");
            m_TileModel.Translate(new Vector3(GRIDWIDTH*m_Row, 0, GRIDDEPTH*m_Column));
            pGameScene.AddSceneObject(m_TileModel);

            m_Selected = false;

            m_TileModel.CreateBoundingBox(GRIDWIDTH, 32, GRIDDEPTH,new Vector3(0,16,0));
            m_TileModel.DrawBoundingBox = true;
        }

        public void Initialize()
        {
            m_TileType = "normal";
        }

        public void Update(Engine.RenderContext renderContext)
        {
            if (m_Selected)
                m_TileModel.Rotate(0, 45.0f*(float) renderContext.GameTime.TotalGameTime.TotalSeconds, 0);
        }

        public void SetTileType(string type)
        {
            if (m_TileModel.HitTest(ray))
            {
                m_Selected = true;
                System.Diagnostics.Debug.WriteLine(m_Row.ToString() + "," + m_Column.ToString() );

                return true;
            }
            m_TileType = type;
        }

        public string GetTileType()
        {
            return m_TileType;
        }

            return false;
        {
            m_TileSettlement = type;
        }

        public string GetTileSettlement()
        {
            return m_TileSettlement;
        }
    }
}