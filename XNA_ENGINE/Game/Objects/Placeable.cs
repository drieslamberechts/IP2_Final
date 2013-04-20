using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XNA_ENGINE.Engine;

//----------------
//This class will be used as an abstract class for placeable things on the grid
//----------------

namespace XNA_ENGINE.Game.Objects
{
    abstract class Placeable
    {
        public enum PlaceableType
        {
            Settlement,
            Army,

            enumSize
        }

        protected bool m_Static = false;
        protected PlaceableType m_PlaceableType;
        protected GameModelGrid m_Model;

        public virtual void Update(RenderContext renderContext)
        {
            
        }

        public virtual void OnHit()
        {

        }

        public bool Static 
        {
            get { return m_Static; }
            set { m_Static = value; }
        }

        public PlaceableType PlaceableTypeMeth
        {
            get { return m_PlaceableType; }
            set { m_PlaceableType = value; }
        }
        
        public GameModelGrid Model
        {
            get { return m_Model; }
            set { m_Model = value; }
        }
    }
}
