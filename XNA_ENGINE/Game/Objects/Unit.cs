using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        protected bool m_CalculateNewPath = false;
        protected GridTile m_TargetPathfinding;

        protected float MOVEMENTSPEED = 0.5f; //seconds per tile

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(RenderContext renderContext)
        {
            if (m_PathToFollow != null && m_PathToFollow.Any())
            {
                SetTargetTile(m_PathToFollow.ElementAt(0));

                //if( )
                ////Set the correct vector
                //Vector3 targetPos = m_TargetTile.Model.WorldPosition;
                //Vector3 worldPos = m_Model.WorldPosition;
                //Vector3 currentTilePos = m_CurrentTile.Model.WorldPosition;
                //targetPos.Y += GRIDHEIGHT;

                ////Calculate the distance for 1 tile to the other
                //Vector3 distanceVector = targetPos - currentTilePos;
                ////Calculate the direction
                //Vector3 directionVector = targetPos - worldPos;
                //directionVector.Normalize();
                //distanceVector = distanceVector.Length() * directionVector;

                ////Calculate a new vector without y value
                //Vector3 directionVectorCalc = directionVector;
                //directionVectorCalc.Y = 0;
                //directionVectorCalc.Normalize();

                //m_PathFindingDirection = directionVectorCalc;
            }
            else
                SetTargetTile(m_CurrentTile);

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
                Vector3 directionVector = targetPos - worldPos;
                directionVector.Normalize();
                distanceVector = distanceVector.Length() * directionVector;

                //Calculate a new vector without y value
                Vector3 directionVectorCalc = directionVector;
                directionVectorCalc.Y = 0;
                directionVectorCalc.Normalize();

                //Do the right rotation
                var pathFindingDirection = targetPos - currentTilePos;
                pathFindingDirection.Y = 0;
                pathFindingDirection.Normalize();

                int add = 0;
                if (pathFindingDirection.Z == 1)
                    add = 180;
                m_Model.Rotate(0, MathHelper.ToDegrees(UnsignedAngleBetweenTwoV3(new Vector3(1, 0, 0), pathFindingDirection)) + 90 + add, 0);

                //If the model is in the proximity, stick it to the tile
                if (m_PreviousDistanceToTile < (targetPos - worldPos).Length())
                {
                    m_CurrentTile = m_TargetTile;
                    //if (m_CurrentTile == m_PathToFollow.First()) m_Model.Translate(targetPos);
                    if (m_PathToFollow != null) m_PathToFollow.Remove(m_TargetTile);

                    

                    m_PreviousDistanceToTile = 100000.0f;
                }
                else //else just move it towards it
                {
                    m_Model.Translate(worldPos + (distanceVector * (deltaTime / MOVEMENTSPEED)));
                    m_PreviousDistanceToTile = (targetPos - worldPos).Length();
                }
            }
            else if (m_TargetTile != null && m_CurrentTile == m_TargetTile)
            {
                Vector3 targetPos = m_TargetTile.Model.WorldPosition;

                //Offset the soldier so it has the correct position
                targetPos.Y += GRIDHEIGHT;

                m_Model.Translate(targetPos);
            }

            if (m_CalculateNewPath && m_CurrentTile == m_TargetTile)
            {
                m_PathToFollow = GridFieldManager.GetInstance().CalculatePath(m_CurrentTile, m_TargetPathfinding, m_PlaceableType);
                m_CalculateNewPath = false;
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

        public override void GoToTile(GridTile targetTile)
        {
            m_CalculateNewPath = true;
            m_TargetPathfinding = targetTile;
        }

        public float UnsignedAngleBetweenTwoV3(Vector3 v1, Vector3 v2)
        {
            v1.Normalize();
            v2.Normalize();
            return (float)Math.Acos(Vector3.Dot(v1, v2));
        }
    }
}
