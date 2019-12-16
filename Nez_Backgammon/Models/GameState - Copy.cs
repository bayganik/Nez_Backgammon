using System;
using System.Collections.Generic;
//using Dice = Backgammon.model.Dice;
//using Board = Backgammon.Board;

namespace Nez_Backgammon.Models
{
    public class GameStateOrig
    {
        internal int[] board;
        internal int[] dice;
        private double score;

        public GameStateOrig()
        {
            board = new int[26];
            dice = new int[4];
            this.setDice(3, 4);
            for (int i = 0; i < board.Length; i++)
            {
                if (i == 0)
                {
                    board[i] = 2;
                }
                if (i == 1)
                {
                    board[i] = 2;
                }
                if (i == 2)
                {
                    board[i] = 2;
                }
                if (i == 3)
                {
                    board[i] = 2;
                }
                if (i == 4)
                {
                    board[i] = 2;
                }
                if (i == 5)
                {
                    board[i] = 2;
                }
                if (i == 10)
                {
                    board[i] = -2;
                }
                // if(i == 0)
                // board[i] = -2;
                // else if(i == 5 || i == 12)
                // board[i] = 5;
                // else if(i == 11 || i == 18)
                // board[i] = -5;
                // else if(i == 16)
                // board[i] = -3;
                // else if(i == 7)
                // board[i] = 3;
                // else if(i == 23)
                // board[i] = 2;
            }
        }

        public GameStateOrig(int[] _board)
        {
            this.board = (int[])_board.Clone();
        }

        public GameStateOrig(int[] _board, int[] _dice)
        {
            if (this.board == null)
                this.board = new int[_board.Length];
            //
            // Clone the board
            //
            //for (int i = 0; i < _board.Length; i++)
            //    board[i] = _board[i];
            board = (int[])_board.Clone();
            //
            // All the used numbers to to end
            //
            //Array.Reverse(_dice);
            //dice = new int[_dice.Length];
            //dice = (int[])_dice.Clone();
            if (_dice[0] < _dice[1])
            {
                this.setDice(_dice[0], _dice[1]);
            }
            else
            {
                this.setDice(_dice[1], _dice[0]);
            }
        }

        //public GameState(BKBoard currentBoard, Dice dice)
        public GameStateOrig(BKBoard currentBoard)
        {
            this.board = new int[26];
            int[] temp = new int[4]; // dice.DiceResults;
            if (temp[0] < temp[1])
            {
                this.setDice(temp[0], temp[1]);
            }
            else
            {
                this.setDice(temp[1], temp[0]);
            }
            // Fill the int board using the actual positions of checkers in the
            // Board b.
            for (sbyte i = 0; i < 26; i++)
            {
                board[i] = currentBoard.GetBoardLocation(i);
            }
        }

        public void setDice(int i, int j)
        {
            this.dice = new int[4];
            this.dice[0] = i;
            this.dice[1] = j;
            if (i == j)
            {
                dice[2] = dice[3] = i;
            }
            else
            {
                dice[2] = dice[3] = 0;
            }
        }

        public double evaluate(int player)
        { // Huristic
            double eval = 0;

            int pipeContainingMoreThanOneBlkChecker = 0; // how many pipe contain
                                                         // (red checkers more
                                                         // than 1) negetive
                                                         // value
            int pipeContainingMoreThanOneWhiteChecker = 0; // how many pipe contain
                                                           // (white checkers more
                                                           // than 1) positive
                                                           // value

            int totalValueOfBlkCheckers = 0; // sum of total number of red
                                             // checkers
            int totalValueOfWhiteCheckers = 0; // sum of total number of white
                                               // checkers

            int distanceForBlkCheckers = 0; // distance (max distance : red checker)
                                            // if white checker in graveYard then 25
            int distanceForWhiteChekers = 0; // distance (max distance : white
                                             // checker) if white checker in
                                             // graveYard then 24

            int tmp = 0;
            int whiteGY = 24;
            int blackGY = 25;

            for (int b = 0; b < 24; b++)
            {
                if (this.board[b] == 0) // no checker in (board)pipe
                    continue;

                if (this.board[b] > 0)              //human player
                {
                    if (this.board[b] > 1)
                        pipeContainingMoreThanOneWhiteChecker++;

                    if (this.board[whiteGY] == 0)
                        tmp = b;
                    else
                        tmp = whiteGY;

                    if (tmp > distanceForWhiteChekers)
                    {
                        distanceForWhiteChekers = tmp;
                    }
                    totalValueOfWhiteCheckers += this.board[b];
                }
                else
                {                                   
                    if (this.board[b] < -1)         //computer player
                        pipeContainingMoreThanOneBlkChecker++;

                    if (this.board[blackGY] == 0)
                        tmp = whiteGY - b;
                    else
                        tmp = blackGY;

                    if (tmp > distanceForBlkCheckers)
                    {
                        distanceForBlkCheckers = tmp;
                    }
                    totalValueOfBlkCheckers += this.board[b];
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
        public List<GameState> getChildren(int player)
        {
            int diceLeft = (this.dice[0] == this.dice[1] ? 4 : 2);
            diceLeft = 0;
            //
            // how many real dice move do we have
            //
            for (int i=0; i < dice.Length; i++)
            {
                if (dice[i] > 0)
                    diceLeft += 1;
            }

            if (!this.hasGraveYard(this.board, player))
            {
                //
                // We are here when, there is Nothing in graveyard for BLACK
                //
                List<GameState> children = new List<GameState>(30);
                this.getChildren(children, player, 0, diceLeft - 1, this.board);
                this.setDice(dice[1], dice[0]); // swap dice
                this.getChildren(children, player, 0, diceLeft - 1, this.board);
                if (children.Count != 0)
                {
                    return children;
                }
                else // cannot find children,will check for partial moves only
                {
                    if (diceLeft == 2)
                    {
                        diceLeft--;
                        this.getChildren(children, player, 0, diceLeft - 1, this.board);
                        this.setDice(dice[1], dice[0]);
                        this.getChildren(children, player, 0, diceLeft - 1, this.board);
                        return children;
                    }
                    else // doubles
                    {
                        while (diceLeft > 0)
                        {
                            this.getChildren(children, player, 0, diceLeft - 1, this.board);
                            this.setDice(dice[1], dice[0]);
                            this.getChildren(children, player, 0, diceLeft - 1, this.board);
                            if (children.Count > 0)
                            {
                                return children;
                            }
                            diceLeft--;
                        }
                    }
                }
                return children;
            }
            else
            {
                //znznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznzn
                //  Black has checkers in graveyard
                //znznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznzn
                List<GameState> children = new List<GameState>(20);
                int[][] noGy = null;
                if (Math.Abs(board[this.graveYard(player)]) >= 2 || diceLeft == 4)
                {
                    noGy = new int[1][]; // at most two possible boards when moving
                }
                // out of graveyard
                else
                {
                    noGy = new int[2][];
                }
                int op = player == 1 ? -1 : 1;
                int moveLoc = 0;
                if (diceLeft == 4) // doubles
                {
                    noGy[0] = (int[])this.board.Clone();
                    while (this.hasGraveYard(noGy[0], player) && diceLeft != 0)
                    {
                        moveLoc = this.countFrom(player) + (op * this.dice[--diceLeft]);
                        if (this.isValid(player, this.board[moveLoc]))
                        {
                            this.move(player, noGy[0], this.graveYard(player), moveLoc);
                        }
                        else // can't place a checker from graveyard,return
                        {
                            return children;
                        }
                    }
                    this.getChildren(children, player, 0, diceLeft - 1, noGy[0]);
                    return children;
                }
                else
                {
                    if (Math.Abs(board[this.graveYard(player)]) < 2)
                    {
                        moveLoc = this.countFrom(player) + (op * this.dice[0]);
                        if (this.isValid(player, this.board[moveLoc]))
                        {
                            noGy[0] = (int[])this.board.Clone();
                            this.move(player, noGy[0], this.graveYard(player), moveLoc);
                            diceLeft--;
                            this.setDice(dice[1], dice[0]);
                            this.getChildren(children, player, 0, diceLeft - 1, noGy[0]);
                            this.setDice(dice[1], dice[0]);
                            diceLeft++;
                        }
                        moveLoc = this.countFrom(player) + (op * this.dice[1]);
                        if (this.isValid(player, this.board[moveLoc]))
                        {
                            diceLeft--;
                            noGy[1] = (int[])this.board.Clone();
                            this.move(player, noGy[1], this.graveYard(player), moveLoc);
                            this.getChildren(children, player, 0, diceLeft - 1, noGy[1]);
                        }
                        return children;
                    }
                    else
                    {
                        noGy[0] = (int[])this.board.Clone();
                        moveLoc = this.countFrom(player) + (op * this.dice[0]);
                        if (this.isValid(player, noGy[0][moveLoc]))
                        {
                            this.move(player, noGy[0], this.graveYard(player), moveLoc);
                        }
                        moveLoc = this.countFrom(player) + (op * this.dice[1]);
                        if (this.isValid(player, noGy[0][moveLoc]))
                        {
                            this.move(player, noGy[0], this.graveYard(player), moveLoc);
                        }
                        children.Add(this);
                        return children;
                    }

                }
            }
        }
        /// 
        /// <param name="children">
        ///            : the ArrayList<State> will be populated by legal states </param>
        /// <param name="player">
        ///            : player for whom getChildren is called </param>
        /// <param name="startLoc">
        ///            : search of possible move will start from startLoc </param>
        /// <param name="diceLeft">
        ///            : number of unconsumed dice </param>
        /// <param name="board">
        ///            : representation of the current board </param>
        //JAVA TO C# CONVERTER TODO TASK: The following line could not be converted:
        public void getChildren(List<GameState> children, int player, int startLoc, int diceLeft, int[] board)
        {
            if (diceLeft == -1) // no more dice left,add the new state
            {
                children.Add(new GameState(board));
                return;
            }
            // find the direction of movement
            int op = player == 1 ? -1 : 1;
            for (int i = startLoc; i < 24; i++)
            {
                // copy the board
                // int[] tempBoard = this.deepCopy(board);
                int[] tempBoard = (int[])this.board.Clone();
                //
                // if player is the owner of board[i] group then we might find a
                // move from there
                //
                if (this.playerHas(player, board[i]))
                {
                    int moveLoc = i + (op * this.dice[diceLeft]);

                    if (this.canGather(player, tempBoard, i, diceLeft))
                    {
                        this.gather(player, tempBoard, i);
                        this.getChildren(children, player, i, diceLeft - 1, tempBoard);
                    }
                    else if (this.inBounds(moveLoc)) // if destination is inside
                    {
                        //
                        // normal moves come here
                        //

                        // bounds
                        // if destination is a valid location
                        if (this.isValid(player, board[moveLoc]))
                        {
                            // perform the move at the tempBoard
                            this.move(player, tempBoard, i, moveLoc);
                            // recursive call of getChildren to find other possible
                            // moves using tempBoard
                            this.getChildren(children, player, i, diceLeft - 1, tempBoard);
                        }
                    }
                }
            }
        }

        // Return the position to start counting from when moving out of graveyard.
        public int countFrom(int player)
        {
            return player == 1 ? 24 : -1;
        }

        public bool isValid(int player, int cgroup)
        {
            if (player == 1)
            {
                if (cgroup >= 0 || cgroup == -1)
                {
                    return true;
                }
            }
            else
            {
                if (cgroup <= 0 || cgroup == 1)
                {
                    return true;
                }
            }
            return false;
        }

        private int signum(int _num)
        {
            int sig = 0;
            switch (_num)
            {
                case int n when (n > 0):  
                    sig = 1;
                    break;
                case int n when (n == 0):    
                    sig = 0;
                    break;
                case int n when (n < 0):    
                    sig = -1;
                    break;
            }

            return sig;
        }

        public void move(int player, int[] board, int from, int to)
        {
            int r = signum(board[from]);
            board[from] -= r;
            if (-r == board[to])
            {
                board[player == 1 ? this.graveYard(2) : this.graveYard(1)] += board[to];
                board[to] = 0;
            }
            board[to] += r;
        }

        // Removes the top checker of "from" from the board.
        public void gather(int player, int[] board, int from)
        {
            if (player == 1)
            {
                board[from] -= 1;
            }
            else
            {
                board[from] += 1;
            }
        }

        // Return true if player has a graveyard
        public bool hasGraveYard(int[] board, int player)
        {
            return board[graveYard(player)] != 0;
        }

        // Returns whether player "owns" this checkergroup or not.
        public bool playerHas(int player, int cgroup)
        {
            if (player == 1)
            {
                return cgroup > 0;
            }
            else
            {
                return cgroup < 0;
            }
        }

        public int[] deepCopy(int[] arg)
        {
            int[] tmp = new int[arg.Length];
            for (int i = 0; i < arg.Length; i++)
                tmp[i] = arg[i];

            return tmp;
            //return Arrays.CopyOf(arg, arg.Length);
        }

        public void setScore(double score)
        {
            this.score = score;
        }

        public double Score
        {
            get
            {
                return this.score;
            }
        }

        public bool Terminal
        {
            get
            {
                return false;
            }
        }

        public bool PlayerWins()
        {
            if (this.count(1, this.board) == 0)
            {
                return true;
            }
            return false;
        }

        public bool CPUWins()
        {
            if (this.count(2, this.board) == 0)
            {
                return true;
            }
            return false;
        }

        // Returns whether the position is inside board bounds or not(excluding
        // graveyards).
        public bool inBounds(int pos)
        {
            return pos >= 0 && pos < 24;
        }

        public int graveYard(int player)
        {
            return player == 1 ? 24 : 25;
        }

        // Returns the amount of checkers owned by the given player.
        public int count(int player, int[] board)
        {
            int count = 0;
            for (int i = 0; i < board.Length; i++)
            {
                if (this.playerHas(player, board[i]))
                {
                    count += Math.Abs(board[i]);
                }
            }
            return count;
        }


        // Returns whether given player can gather with given arguments.
        //JAVA TO C# CONVERTER TODO TASK: The following line could not be converted:
        public bool canGather(int player, int[] board, int from, int dienum)
        {
            int c = this.count(player, board);
            if (c > 0)
            {
                int start = player == 1 ? 0 : 18;
                int homecount = 0;
                for (int i = 0; i < 6; i++)
                {
                    if (this.playerHas(player, board[start + i]))
                    {
                        homecount += Math.Abs(board[start + i]);
                    }
                }
                if (homecount == c)
                {
                    int finish = player == 1 ? -1 : 24;
                    int sign = player == 1 ? -1 : 1;
                    if (from + sign * this.dice[dienum] == finish)
                    {
                        return true;
                    }
                    else
                    {
                        // Check if checker on previous group exists.
                        // If it does we can't gather, we can only
                        // move the previous checkers.
                        int lastInHome = player == 1 ? 5 : 18;
                        if (player == 1)
                        {
                            from++;
                        }
                        else
                        {
                            from--;
                        }
                        if (player == 1)
                        {
                            while (from <= lastInHome)
                            {
                                if (this.playerHas(player, board[from]))
                                {
                                    return false;
                                }
                                if (player == 1)
                                {
                                    from++;
                                }
                                else
                                {
                                    from--;
                                }
                            }
                        }
                        else
                        {
                            while (from >= lastInHome)
                            {
                                if (this.playerHas(player, board[from]))
                                {
                                    return false;
                                }
                                if (player == 1)
                                {
                                    from++;
                                }
                                else
                                {
                                    from--;
                                }
                            }
                        }
                    }
                    return true;
                }
            }
            return false;
        }

        public static string print(int[] board2)
        {
            string str = "\r\n=========================================\r\n";
            str += "[12]\t[13]\t[14]\t[15]\t[16]\t[17]\t[18]\t[19]\t[20]\t[21]\t[22]\t[23]\r\n";
            sbyte b = 12;
            for (; b < 24; b++)
            {
                if (board2[b] == 0)
                {
                    str += "empty\t";
                }
                else
                {
                    str += board2[b] + "\t";
                }
            }
            b = 11;
            str += "\r\n\r\n";
            for (; b >= 0; b--)
            {
                if (board2[b] == 0)
                {
                    str += "empty\t";
                }
                else
                {
                    str += board2[b] + "\t";
                }
            }
            str += "\r\n[11]\t[10]\t[9]\t[8]\t[7]\t[6]\t[5]\t[4]\t[3]\t[2]\t[1]\t[0]\r\nGraveyards\r\n";
            str += "[24]\t[25]\r\n";
            str += ((board2[24] == 0) ? "empty" : board2[24].ToString()) + "\t"
                    + ((board2[25] == 0 ? "empty" : board2[25].ToString())) + "\r\n";
            return str;
        }

        public string ToString()
        {
            string str = print(this.board);
            if (dice != null)
            {
                str += "================dice" + dice[0] + " " + dice[1] + "=========================\n";
            }
            return str;
        }
    }
}
