using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

// -- INFO --
// - We should use a player list so the AI knows which player to control

// ---------------------------------------------------------
// -- USED BY AI AS WELL AS THE ACTUAL PLAYER --
// ---------------------------------------------------------

namespace XNA_ENGINE.Game.Objects
{
    public class AI
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
        public GridTile Move(GridTile currentTile, GridTile destinationTile)
        {
            Console.WriteLine("The army starts moving.");
            // Only move the selected army

            /*if (currentTile.Row < destinationTile.Row)
            {
            }*/

            return destinationTile;
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
        public void BuildGoblin()
        {
            // With worker reference
        }

        // BUILD TRIBE
        public void BuildVillager(Placeable placeable)
        {
            
        }

        // SPLIT THE ARMY
        public Army SplitArmy(Army army, Army newArmy)
        {
            // gebeurd nu hier
            //army.SplitArmy();

            newArmy.ArmySize = army.ArmySize;
            newArmy.SetTargetTile(army.GetTargetTile());
            
            Console.WriteLine("The selected army is split");

            return newArmy;
        }

        // MERGE THE ARMY
        public void Merge(Army firstArmy, Army secondArmy)
        {
            int firstArmyCount = firstArmy.ArmySize;
            secondArmy.ArmySize += firstArmyCount;

            // remove firstArmy

             if (Menu.GetInstance().m_Enable7)
            {
                Menu.GetInstance().m_Enable7 = false;
                Menu.GetInstance().m_Enable8 = true;
            }
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
