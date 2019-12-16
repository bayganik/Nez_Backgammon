using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nez_Backgammon.Models
{
    //public class ExpectiMinimax
    //{
    //    /*
    //     * Player = 1 = Human
    //     * Player = 2 = Computer
    //     */
    //    public static int MAX_DEPTH = 1; // 4 because chooseMove already builds
    //                                     // Max's children before Minimax
    //                                     // is called
    //    private int[][] dice = new int[][]
    //    {
    //        new int[] {1, 1},
    //        new int[] {1, 2},
    //        new int[] {1, 3},
    //        new int[] {1, 4},
    //        new int[] {1, 5},
    //        new int[] {1, 6},
    //        new int[] {2, 2},
    //        new int[] {2, 3},
    //        new int[] {2, 4},
    //        new int[] {2, 5},
    //        new int[] {2, 6},
    //        new int[] {3, 3},
    //        new int[] {3, 4},
    //        new int[] {3, 5},
    //        new int[] {3, 6},
    //        new int[] {4, 4},
    //        new int[] {4, 5},
    //        new int[] {4, 6},
    //        new int[] {5, 5},
    //        new int[] {5, 6},
    //        new int[] {6, 6}
    //    };          //used in chance play (depth is divisible by 2)
    //    public ExpectiMinimax()
    //    {

    //    }
    //    public GameState chooseMove(GameState gs)
    //    {
    //        GameState bestGS = Minimax(gs);
    //        return bestGS;
    //    }
    //    public GameState chooseMove(GameState gs, int diceNum)
    //    {
    //        GameState temp;
    //        List<GameState> list = gs.getChildren(2);
    //        double max_val = double.NegativeInfinity;
    //        double temp_val;

    //        GameState best = null;
    //        //
    //        // Looking for best GameState from the list 
    //        //
    //        foreach(GameState iter in list)
    //        {
    //            temp = iter;
    //            temp_val = Minimax(temp, 1, diceNum);                //depth of 1 does NOTHING
    //            if (temp_val > max_val)
    //            {
    //                best = temp;
    //                max_val = temp_val;
    //            }
    //        }
    //        return best;
    //    }
    //    private GameState Minimax(GameState gs)
    //    {
    //        GameState bestGS = new GameState();
    //        double maxval;
    //        double val;             //returned value

    //        maxval = double.NegativeInfinity;          //max node computer
    //        val = maxval;
    //        List<GameState> list = gs.getChildren(2);
    //        if (list.Count == 0)
    //        {
    //            //
    //            // There are no more moves for Black
    //            //
    //            return null;
    //        }
                

    //        //
    //        // we have children to look thru
    //        //
    //        foreach (GameState iter in list)
    //        {
    //            //val = Math.Max(val, Minimax(iter, 1, 0));
    //            val = Minimax(iter, 1, 0);                //depth of 1 does NOTHING
    //            if (val >= maxval )
    //            {
    //                maxval = val;
    //                bestGS = iter;
    //            }
    //        }
    //        return bestGS;

    //    }
    //    private double Minimax(GameState gs, int depth, int diceNum)
    //    {
    //        double val;             //returned value
    //        if (depth == MAX_DEPTH)
    //        {
    //            return gs.evaluate(2);
    //        }

    //        if (depth % 2 == 0)             // chance node (depth of 0,2,4,6,8,...)
    //        {
    //            float v = 0f;
    //            //
    //            // Go thru every dice roll possible
    //            //
    //            foreach (int[] diceRoll in dice)
    //            {
    //                //
    //                // Create new GameState with ALL possible dice rolls
    //                //
    //                v += (float)((dice[0] == dice[1] ? 1.0f / 36 : 1.0f / 18) * Minimax(new GameState(gs.board, diceRoll), depth + 1, diceNum));
    //            }
    //            return v;
    //        }
    //        else if (depth % 4 == 1)                    // min node human
    //        {
    //            val = double.PositiveInfinity;
    //            List<GameState> list = gs.getChildren(1);
    //            if (list.Count == 0)
    //                return Math.Min(val, Minimax(gs, depth + 1, diceNum));

    //            //
    //            // we have children to look thru
    //            //
    //            foreach (GameState iter in list)
    //            {
    //                val = Math.Min(val, Minimax(iter, depth + 1, diceNum));
    //            }
    //            return val;
    //        }
    //        else
    //        {
    //            val = double.NegativeInfinity;          //max node computer
    //            List<GameState> list = gs.getChildren(2);
    //            if (list.Count == 0)
    //                return Math.Max(val, Minimax(gs, depth + 1, diceNum));

    //            //
    //            // we have children to look thru
    //            //
    //            foreach (GameState iter in list)
    //            {
    //                val = Math.Max(val, Minimax(iter, depth + 1, diceNum));
    //            }
    //            return val;
    //        }
    //    }
    //}
}
