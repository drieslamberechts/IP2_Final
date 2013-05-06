using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using XNA_ENGINE.Game.Objects;

namespace XNA_ENGINE.Game.TilePrefabs
{
    class BasePrefab
    {
        protected bool m_bOpen = true;
        protected bool m_bShowTile = true;
        protected GameModelGrid m_TileModel;
        protected Texture2D m_Tiletexture;
        protected List<GameModelGrid> m_PropList;

        protected const float GRIDWIDTH = 64;
        protected const float GRIDDEPTH = 64;
        protected const float GRIDHEIGHT = 32;
        protected const int YOFFSETMIN = 0;
        protected const int YOFFSETMAX = 15;


        public BasePrefab()
        {
            m_PropList = new List<GameModelGrid>();
        }

        public bool Open 
        {
            get { return m_bOpen; }
        }

        public bool ShowTile
        {
            get { return m_bShowTile; }
        }

        public GameModelGrid TileModel
        {
            get { return m_TileModel; }
        }

        public Texture2D TileTexture
        {
            get { return m_Tiletexture; }
        }

        public List<GameModelGrid> PropList
        {
            get { return m_PropList; }
        }
    }
}
