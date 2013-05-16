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
        protected GameModelGrid m_TileModel;
        protected List<GameModelGrid> m_PropList;

        protected const float GRIDWIDTH = 64;
        protected const float GRIDDEPTH = 64;
        protected const float GRIDHEIGHT = 32;
        protected const int YOFFSETMIN = 0;
        protected const int YOFFSETMAX = 15;

        protected int m_WoodCount = 0;

        public BasePrefab()
        {
            m_PropList = new List<GameModelGrid>();
        }

        public bool Open 
        {
            get { return m_bOpen; }
        }

        public GameModelGrid TileModel
        {
            get { return m_TileModel; }
        }

        public List<GameModelGrid> PropList
        {
            get { return m_PropList; }
        }

        public int WoodCount
        {
            get { return m_WoodCount; }
        }
    }
}
