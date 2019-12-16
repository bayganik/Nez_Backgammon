using System;
using System.Linq;
using System.Collections.Generic;
//using Dice = Backgammon.model.Dice;
//using Board = Backgammon.Board;

namespace Nez_Backgammon.Models
{
    public class GameState
    {
        //internal int[] board;
        //internal int[] dice;
        //List<GameState> children;
        //List<int[]> ChildBoards;                    //intermediate children
        //List<int[]> TerminalBoards;                 //these will be evaluated
        public BGBoard OrigBoard;
        //public int DiceUsed { get; set; }
        //private double score;


        public GameState(BGBoard _board)
        {
            OrigBoard = _board;
        }

        public double evaluate(int[] board)
        {
            //
            // Huristic
            //
            int player = 2;
            double eval = 0;

            int pipeContainingMoreThanOneBlkChecker = 0; // how many pipe contain
                                                         // (black checkers more
                                                         // than 1) negetive
                                                         // value
            int pipeContainingMoreThanOneWhiteChecker = 0; // how many pipe contain
                                                           // (white checkers more
                                                           // than 1) positive
                                                           // value

            int totalValueOfBlkCheckers = 0;            // sum of total number of black
                                                        // checkers

            int totalValueOfWhiteCheckers = 0;          // sum of total number of white
                                                        // checkers

            int distanceForBlkCheckers = 0;             // distance (max distance : black checker)
                                                        // if black checker in graveYard then 25

            int distanceForWhiteChekers = 0;            // distance (max distance : white checker) 
                                                        // if white checker in graveYard then 24

            int tmp = 0;
            int whiteGY = 24;
            int blackGY = 25;

            for (int b = 0; b < 24; b++)
            {
                if (board[b] == 0) // no checker in (board)pipe
                    continue;

                if (board[b] > 0)              //human player
                {
                    if (board[b] > 1)
                        pipeContainingMoreThanOneWhiteChecker++;

                    if (board[whiteGY] == 0)
                        tmp = b;
                    else
                        tmp = whiteGY;

                    if (tmp > distanceForWhiteChekers)
                    {
                        distanceForWhiteChekers = tmp;
                    }
                    totalValueOfWhiteCheckers += board[b];
                }
                else
                {
                    if (board[b] < -1)         //computer player
                        pipeContainingMoreThanOneBlkChecker++;

                    if (board[blackGY] == 0)
                        tmp = 24 - b;                       //black graveyard checker is 24 points away from cast off
                    else
                        tmp = blackGY;

                    if (tmp > distanceForBlkCheckers)
                    {
                        distanceForBlkCheckers = tmp;
                    }

                    totalValueOfBlkCheckers += board[b];
                }
            }

            if (player == 1)
            {
                //human
                eval = (-0.025 * (pipeContainingMoreThanOneWhiteChecker - pipeContainingMoreThanOneBlkChecker))
                     - (0.02525 * (Math.Abs(board[this.graveYard(2)]) - board[this.graveYard(1)]))
                     + 0.4 * totalValueOfWhiteCheckers
                     + 0.0125 * distanceForWhiteChekers;
            }
            else
            {
                // computer
                eval = (0.025 * (pipeContainingMoreThanOneBlkChecker - pipeContainingMoreThanOneWhiteChecker))
                    + (0.02525 * (board[this.graveYard(1)] - Math.Abs(board[this.graveYard(2)])))
                    - 0.4 * totalValueOfBlkCheckers
                    - 0.0125 * distanceForBlkCheckers;

                // --------------------------------------------------

                //eval = (0.025 * (pipeContainingMoreThanOneBlkChecker - pipeContainingMoreThanOneWhiteChecker))
                //        + (0.02525 * (board[this.graveYard(1)] - Math.Abs(board[this.graveYard(2)])))
                //        - 0.4 * (Math.Abs(totalValueOfBlkCheckers) - totalValueOfWhiteCheckers)
                //        - 0.0125 * (distanceForBlkCheckers - distanceForWhiteChekers);
            }
            return eval;
        }
        public int[] Evaluate(List<int[]> _allTerminalNodes)
        {
            //
            // Count pips
            //
            //
            // we have terminal nodes to look thru
            //
            int[] bestGS = new int[1];
            double val = 0;
            double maxval = -1000;
            foreach (int[] iter in _allTerminalNodes)
            {
                //this.board = (int[])iter.Clone();
                //val = Math.Max(val, Minimax(iter, 1, 0));
                val = this.evaluate(iter);
                if (val >= maxval)
                {
                    maxval = val;
                    bestGS = (int[])iter.Clone();
                }
            }
            return bestGS;
        }

        public int[] GetCPUMove(int[] _dice)
        {
            //
            // if there are no moves available, the the original is returned
            //
            int[] result = OrigBoard.DispBoard;
            int[] tmpBoard;
            List<int[]> termNode = new List<int[]>();
            //
            // intermediate nodes for first 2 dice
            //
            List<int[]> Nodes_Dice_0_Level1 = new List<int[]>();
            List<int[]> Nodes_Dice_1_Level1 = new List<int[]>();
            //
            // If doubles, then 2 more intermediate nodes needed
            //
            List<int[]> Nodes_Dice_0_Level2 = new List<int[]>();
            List<int[]> Nodes_Dice_0_Level3 = new List<int[]>();
            //
            // Terminal nodes are the final plays (they get evaluated)
            //
            List<int[]> TermNodes = new List<int[]>();

            if (_dice.Length == 1)
            {
                //znznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznzn
                // Only one dice to calculate, All the nodes are terminal nodes
                //znznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznzn

                TermNodes = GetChildNodes(OrigBoard.DispBoard, _dice[0]);
            }
            else
            {
                if (_dice[0] == _dice[1])
                {
                    //znznznznznznznznznznznznznznznzn
                    // Doubles?
                    //znznznznznznznznznznznznznznznzn

                    TermNodes = new List<int[]>();
                    tmpBoard = (int[])OrigBoard.DispBoard.Clone();

                    Nodes_Dice_0_Level1 = GetChildNodes(tmpBoard, _dice[0]);           //1st branch

                    foreach (int[] tmp in Nodes_Dice_0_Level1)
                    {
                        termNode = GetChildNodes(tmp, _dice[0]);
                        if (termNode.Count == 0)
                            TermNodes.Add(tmp);     //previous level nodes were terminal nodes

                        Nodes_Dice_0_Level2 = GetChildNodes(tmp, _dice[0]);            //2nd branch
                    }

                    foreach (int[] tmp in Nodes_Dice_0_Level2)
                    {
                        termNode = GetChildNodes(tmp, _dice[0]);
                        if (termNode.Count == 0)
                            TermNodes.Add(tmp);     //previous level nodes were terminal nodes

                        Nodes_Dice_0_Level3 = GetChildNodes(tmp, _dice[0]);            //3rd branch
                    }
                    foreach (int[] tmp in Nodes_Dice_0_Level3)
                    {
                        termNode = GetChildNodes(tmp, _dice[0]);
                        if (termNode.Count == 0)
                            TermNodes.Add(tmp);     //previous level nodes were terminal nodes

                        foreach (int[] termNode1 in GetChildNodes(tmp, _dice[0]))
                            TermNodes.Add(termNode1);                                //terminal branch
      
                    }
                }
                else
                {
                    //znznznznznznznznznznznznznznznzn
                    // Normal 2 dice play
                    //znznznznznznznznznznznznznznznzn

                    TermNodes = new List<int[]>();
                    tmpBoard = (int[])OrigBoard.DispBoard.Clone();

                    Nodes_Dice_0_Level1 = GetChildNodes(tmpBoard, _dice[0]);          //1st branch (dice 0)

                    foreach(int[] tmp in Nodes_Dice_0_Level1)
                    {
                        //
                        // We MUST have a terminal node, if none found, then previous node is terminal node
                        //
                        termNode = GetChildNodes(tmp, _dice[1]);
                        if (termNode.Count == 0)
                            TermNodes.Add(tmp);     //previous level nodes were terminal nodes
                        else
                        {
                            foreach (int[] tmnode in GetChildNodes(tmp, _dice[1]))
                            {
                                TermNodes.Add(tmnode);                         //find terminal nodes with dice 1
                            }
                        }
                    }

                    tmpBoard = (int[])OrigBoard.DispBoard.Clone();
                    Nodes_Dice_1_Level1 = GetChildNodes(tmpBoard, _dice[1]);          //2nd branch (dice 1)
                    foreach (int[] tmp in Nodes_Dice_1_Level1)
                    {
                        //
                        // We MUST have a terminal node, if none found, then previous node is terminal node
                        //
                        termNode = GetChildNodes(tmp, _dice[0]);
                        if (termNode.Count == 0)
                            TermNodes.Add(tmp);     //previous level nodes were terminal nodes
                        else
                        {
                            foreach (int[] tmnode in GetChildNodes(tmp, _dice[0]))
                            {
                                TermNodes.Add(tmnode);                         //find terminal nodes with dice 0
                            }
                        }
                    }

                }

            }
            //
            // If there are no Terminal Nodes, then CPU has no play
            //
            if (TermNodes.Count <= 0)
                return OrigBoard.DispBoard;
            else
                return Evaluate(TermNodes);

        }
        private List<int[]> GetChildNodes(int[] tmpboard, int diceValue)
        {
            //
            // Get all possible moves for ONE dice number
            //
            int[] origBoard = (int[])tmpboard.Clone();         //save the original board
            int[] board = (int[])tmpboard.Clone();
            List<int[]> result = new List<int[]>();
            //int player = 2;                 
            for (int i = 0; i < 24; i++)
            {
                if (this.PlayerHas(board[i]))
                {
                    int moveLoc = i +  diceValue;
                    //
                    // Can CPU cast off the board?
                    //
                    if (CPUcanGather(board, i, diceValue))
                    {
                        board[i] += 1;                  //remove a checker (CPU has negative numbers)
                        if (board[27] < 0)
                            board[27] += -1;
                        else
                            board[27] = -1;

                        result.Add(board);
                        
                        board = (int[])origBoard.Clone();
                        //break;                      //no need to look thru other locations
                    }
                    else if (this.InBounds(moveLoc)) 
                    {
                        //
                        // if destination is a valid location, make the move
                        //
                        if (IsValid(board[moveLoc]))
                        {
                            //
                            // perform the move at the tempBoard
                            //
                            board = MakeMove(board, i, moveLoc);
                            result.Add(board);
                            //
                            // Get the original board again
                            //
                            board = (int[])origBoard.Clone();
                        }
                    }
                }
            }

            return result;
        }
        // Return the position to start counting from when moving out of graveyard.
        //public int countFrom(int player)
        //{
        //    return player == 1 ? 24 : -1;
        //}

        public bool IsValid(int stack)
        {
            if (stack <= 0 || stack == 1)
                return true;

            return false;
        }

        //private int signum(int _num)
        //{
        //    int sig = 0;
        //    switch (_num)
        //    {
        //        case int n when (n > 0):
        //            sig = 1;
        //            break;
        //        case int n when (n == 0):
        //            sig = 0;
        //            break;
        //        case int n when (n < 0):
        //            sig = -1;
        //            break;
        //    }

        //    return sig;
        //}
        public int[] MakeMove(int[] board, int from, int to)
        {
            int r = -1;
            board[from] -= r;
            //
            // single white checker gets hit
            //
            if (board[to] == 1)     
            {
                board[24] += 1;     //add one to White graveyard
                board[to] = 0;
            }
            board[to] += r;
            return board;
        }

        // Removes the top checker of "from" from the board.
        //public void gather(int player, int[] board, int from)
        //{
        //    if (player == 1)
        //    {
        //        board[from] -= 1;
        //    }
        //    else
        //    {
        //        board[from] += 1;
        //    }
        //}

        //// Return true if player has a graveyard
        //public bool hasGraveYard(int[] board, int player)
        //{
        //    return board[graveYard(player)] != 0;
        //}


        public bool PlayerHas(int stack)
        {
            //
            // Returns whether player "owns" this checkergroup or not.
            //
            return stack < 0;

        }
        public bool InBounds(int pos)
        {
            //
            // Returns whether the position is inside board bounds or not(excluding graveyards).
            //
            return pos >= 0 && pos < 24;
        }

        public int graveYard(int player)
        {
            return player == 1 ? 24 : 25;
        }


        public int Count(int[] board)
        {
            //
            // Returns the amount of checkers owned by CPU player.
            //
            int cnt = 0;
            for (int i = 0; i < board.Length; i++)
            {
                if (board[i] < 0)
                {
                    cnt += Math.Abs(board[i]);
                }
            }
            return cnt;
        }

        public bool CPUcanGather(int[] board, int from, int diceValue)
        {
            //
            // count of all checkers at home + cast off
            //
            int totalCheckers = 0;
            for (int i = 18; i < 24; i++)
            {
                if (board[i] < 0)
                {
                    totalCheckers += Math.Abs(board[i]);
                }
            }
            totalCheckers += Math.Abs(board[27]);         //what has already been collected

            if (totalCheckers == 15)
            {
                if (from + diceValue >= 24)
                    return true;                    //yes, this checker will cast off
            }
            return false;
        }

    }
}
