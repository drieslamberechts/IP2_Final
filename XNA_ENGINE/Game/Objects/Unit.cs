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
    public abstract class Unit : Placeable
    {
        protected const float GRIDHEIGHT = 32;

        protected List<GridTile> m_PathToFollow;

        protected GridTile m_TargetTile;
        protected GridTile m_CurrentTile;

        protected float m_PreviousDistanceToTile;

        protected float MOVEMENTSPEED = 0.5f; //seconds per tile

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(RenderContext renderContext)
        {
            if (m_PathToFollow != null && m_PathToFollow.Any())
                SetTargetTile(m_PathToFollow.ElementAt(0));

            //Do smooth movement
            //Check if the targetTile exists
            //Check if the targetTile and the Currenttile are the same
            if (m_TargetTile != null && m_CurrentTile != m_TargetTile)
            {
                //Values
                float deltaTime = renderContext.GameTime.ElapsedGameTime.Milliseconds / 1000.0f;
                Vector3 targetPos = m_TargetTile.Model.WorldPosition;
                Vector3 worldPos = m_Model.WorldPosition;
                Vector3 currentTilePos = m_CurrentTile.Model.WorldPosition;
                //Offset the soldier so it has the correct position
                targetPos.Y += GRIDHEIGHT;

                //Calculate the distance for 1 tile to the other
                Vector3 distanceVector = targetPos - currentTilePos;
                //Calculate the direction
                Vector3 directionVector = (targetPos - worldPos);
                directionVector.Normalize();
                distanceVector = distanceVector.Length() * directionVector;

                //Calculate a new vector without y value
                Vector3 directionVectorCalc = directionVector;
                directionVectorCalc.Y = 0;
                directionVectorCalc.Normalize();

                //Do the right rotation
                int add = 0;
                if (directionVectorCalc.X != 0) add = 270;
                m_Model.Rotate(0, (directionVectorCalc.X * 90 * -1) + (directionVectorCalc.Z * 90) + -90 + add, 0); //  90*dot - 90

                //If the model is in the proximity, stick it to the tile
                if (m_PreviousDistanceToTile < (targetPos - worldPos).Length())
                {
                    m_CurrentTile = m_TargetTile;
                    m_Model.Translate(targetPos);
                    m_PathToFollow.Remove(m_TargetTile);

                    m_PreviousDistanceToTile = 100000.0f;
                }
                else //else just move it towards it
                {
                    m_Model.Translate(worldPos + (distanceVector * (deltaTime / MOVEMENTSPEED)));
                    m_PreviousDistanceToTile = (targetPos - worldPos).Length();
                }
            }

            base.Update(renderContext);
        }

        public override void OnSelected()
        {
            base.OnSelected();
        }

        public override void OnPermanentSelected()
        {
            m_CurrentTile.IsInUse = true;
            base.OnPermanentSelected();
        }

        public GridTile CurrentTile
        {
            get { return m_CurrentTile; }
        }

        public GridTile TargetTile
        {
            get { return m_TargetTile; }
        }
    }
}
