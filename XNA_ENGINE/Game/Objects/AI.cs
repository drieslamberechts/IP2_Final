using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

// -- INFO --
// - We should use a player list so the AI knows which player to control

// ---------------------------------------------------------
// -- USED BY AI AS WELL AS THE ACTUAL PLAYER --
// ---------------------------------------------------------

namespace XNA_ENGINE.Game.Objects
{
    class AI
    {
        public AI()
        {

        }

        // INITIALIZE
        public void Initialize()
        {
            // Create reference to Army
            // Create reference to worker
        }

        // SCOUT
        public void Scout()
        {
            // search for resources
            // return to tribe when resources found
        }

        // ATTACK
        public void Attack()
        {
            // attack nearby player
            // Get Current Tile
            // if enemy is 2 tiles away from army -> Attack (from worker -> Retreat)
        }

        // BUILD TILE
        public void BuildTile()
        {
            // with worker reference
        }

        // BUILD TRIBE
        public void BuildTribe()
        {
            // With worker reference
        }

        // SPLIT THE ARMY
        public void SplitArmy()
        {
            // Create 2 references from first Army Reference
        }


    }
}
