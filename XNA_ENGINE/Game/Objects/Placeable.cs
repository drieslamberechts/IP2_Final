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
    public abstract class Placeable
    {
        public enum PlaceableType
        {
            Settlement,
            School,
            Shrine,
            Flag,
            Villager,
            Army,
            Sjaman,

            enumSize
        }

        protected bool m_Static = true;
        protected PlaceableType m_PlaceableType;
        protected GameModelGrid m_Model;
        protected GridTile m_LinkedTile;
        protected Player m_Owner;

        public virtual void Update(RenderContext renderContext)
        {
            
        }

        public virtual bool OnSelected()
        {
            if (!m_LinkedTile.Selected) return false;

            return true;
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

        public GridTile LinkedTile
        {
            get { return m_LinkedTile; }
            set { m_LinkedTile = value; }
        }

        public virtual void QueueVillager(int amount = 1)
        {

        }

        public virtual void QueueSoldier(int amount = 1)
        {

        }

        public virtual void SetTargetTile(GridTile targetTile)
        {
            
        }
        public virtual GridTile GetTargetTile()
        {
            return null;
        }

        public void SetOwner(Player owner)
        {
            m_Owner = owner;
        }
    }
}
