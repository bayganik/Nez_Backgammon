using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nez;

namespace TestMiniMax.ECS.Components
{
    /*
     * initial board for Backgammon
     * negative numbers are computer
     * positive numbers are human player
     * 
     * position 24 = grave yard for human player checkers
     * position 25 = grave yard for computer player checkers
     * 
     *  12 13 14 15 16 17 18 19 20 21 22 23
     * |-----------------|------------------|
     * |05          -3   |-5             02 |
     * |                 |                  | 24
     * |                 |                  | 25
     * |-5          03   |05             -2 |
     * |-----------------|------------------|
     *  11 10  9  8  7  6  5  4  3  2  1  0
     */
    public class BoardComponent : Component
    {
        public int[] NumOfCheckers;
        public BoardComponent()
        {
            NumOfCheckers = new int[26];
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();
            //
            // Places all checkers at their initial positions.
            //
            NumOfCheckers[ 0] = -2;          //black Computer
            NumOfCheckers[11] = -5;          //black Computer
            NumOfCheckers[18] = -5;          //black Computer
            NumOfCheckers[16] = -3;          //black Computer

            NumOfCheckers[ 5] = 5;           //white
            NumOfCheckers[12] = 5;           //white
            NumOfCheckers[ 7] = 3;           //white
            NumOfCheckers[23] = 2;           //white
        }
    }
}
