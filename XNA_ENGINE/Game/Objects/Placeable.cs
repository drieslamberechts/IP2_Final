using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XNA_ENGINE.Engine;
using XNA_ENGINE.Game.Managers;
using XNA_ENGINE.Game.Scenes;

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
            RallyPoint,
            Villager,
            Army,
            Shaman,

            enumSize
        }

        protected PlaceableType m_PlaceableType;
        protected GameModelGrid m_Model;
        protected GameModelGrid m_Rallypoint;
        protected List<GridTile> m_LinkedTileList;
        protected Player m_Owner;

        public virtual void Initialize()
        {
            //Load a rallyPointFlag
            m_Rallypoint = new GameModelGrid("Models/prop_Flag");
            m_Rallypoint.CanDraw = true;
            m_Rallypoint.LoadContent(PlayScene.GetContentManager());
            m_Rallypoint.DiffuseColor = new Vector3(0.0f, 0.8f, 0.0f);
            GridFieldManager.GetInstance().GameScene.AddSceneObject(m_Rallypoint);
            m_Rallypoint.LocalPosition += new Vector3(0, 300, 0);
        }

        public virtual void Update(RenderContext renderContext)
        {
            if (Model.Selected)
                OnSelected();

            if (Model.PermanentSelected)
                OnPermanentSelected();
            else
                m_Rallypoint.CanDraw = false;
        }

        public virtual void OnSelected()
        {
            if (m_LinkedTileList != null)
                foreach (var gridTile in m_LinkedTileList)
                    gridTile.Model.Selected = true;
        }

        public virtual void OnPermanentSelected()
        {
            if (m_LinkedTileList != null)
                foreach (var gridTile in m_LinkedTileList)
                    gridTile.Model.PermanentSelected = true;
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

        public List<GridTile> LinkedTiles
        {
            get { return m_LinkedTileList; }
            set { m_LinkedTileList = value; }
        }



        public virtual void QueueVillager(int amount = 1)
        {

        }

        public virtual void QueueSoldier(int amount = 1)
        {

        }

        public virtual bool SetTargetTile(GridTile targetTile)
        {
            return false;
        }

        public virtual GridTile GetTargetTile()
        {
            return null;
        }

        public void SetOwner(Player owner)
        {
            m_Owner = owner;
        }

        public Player GetOwner()
        {
            return m_Owner;
        }

        public bool HitTest(Ray ray)
        {
            if (m_Model.HitTest(ray))
                return true;

            return false;
        }
    }
}
