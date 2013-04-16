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
        private bool m_AttackScene;

        public AI()
        {

        }

        // INITIALIZE
        public void Initialize()
        {
            // Create reference to Army

            m_AttackScene = false;
        }

        // SCOUT
        public void Scout()
        {
            // search for resources
            // return to tribe when resources found
        }

        // MOVE
        public void Move()
        {
            Console.WriteLine("The army starts moving.");
            // Only move the selected army
        }

        // ATTACK
        public void Attack()
        {
            // attack nearby player
            // Get Current Tile
            // if enemy is 2 tiles away from army -> Attack (from worker -> Retreat)
            Console.WriteLine("The attack Algorithm has started!");

            m_AttackScene = true;
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
        public void SplitArmy(Army army)
        {
            // Create 2 references from first Army Reference
            Console.WriteLine("The selected army is split");
        }

        // GETTERS AND SETTERS
        public bool GetAttack()
        {
            return m_AttackScene;
        }

        public void ResetAttack()
        {
            m_AttackScene = false;
        }


    }
}
