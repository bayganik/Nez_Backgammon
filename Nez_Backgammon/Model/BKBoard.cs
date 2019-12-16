using System;
using System.Collections.Generic;


namespace Backgammon.Model
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
    public class BKBoard
    {
        public int[] BoardLocation;

        public BKBoard()
        {
            BoardLocation = new int[26];
            InitCheckers();
        }
        public void InitCheckers()
        {
            //
            // Places all checkers at their initial positions.
            //
            BoardLocation[0] = -2;           //black Computer
            BoardLocation[11] = -5;          //black Computer
            BoardLocation[18] = -5;          //black Computer
            BoardLocation[16] = -3;          //black Computer

            BoardLocation[5] = 5;            //white
            BoardLocation[12] = 5;           //white
            BoardLocation[7] = 3;            //white
            BoardLocation[23] = 2;           //white

            //		BoardLocation[0] = 15;
            //
            //		BoardLocation[23] = -15;

            //		BoardLocation[0] = 13;
            //		BoardLocation[24] = 2;
            //		BoardLocation[5] = 0;
            //		BoardLocation[12] = 0;
            //		BoardLocation[11] = 0;
            //		
            //		BoardLocation[16] = 0;
            //		BoardLocation[7] = -3;
            //		BoardLocation[18] = -2;
            //		BoardLocation[19] = -2;
            //		BoardLocation[20] = -2;
            //		BoardLocation[21] = -2;
            //		BoardLocation[22] = -2;
            //		BoardLocation[23] = -2;
        } 

        public bool IsValid(int player, int containValue)
        {
            if (player == 1)
            {
                if (containValue >= 0 || containValue == -1)
                {
                    return true;
                }
            }
            else
            {
                if (containValue <= 0 || containValue == 1)
                {
                    return true;
                }
            }
            return false;
        } 


        public bool InBounds(int pos)
        {
            //
            // Returns whether the position is inside board bounds or not(excluding
            // graveyards).
            //
            return pos >= 0 && pos < 24;
        }
        //
        // give location of hit bar (player 1 = 24, player 2 = 25)
        //
        public int BarNumForPlayer(int player)
        {
            return (player == 1 ? 24 : 25);
        }

        /// <summary>
        /// Moves a checker using from/to values
        /// </summary>
        /// <param name="player">
        ///            the number of the player that will make the move </param>
        /// <param name="from">
        ///            the source of the move </param>
        /// <param name="to">
        ///            the destination of the move </param>
        /// <returns> true if move was successfull and false otherwise </returns>
        public bool Move(int player, int from, int to)
        {
            int diff = (from - to);
            // check if the direction of movement is correct
            if (diff < 0 && player == 1)
            {
                return false;
            }
            else if (diff > 0 && player == 2 && from != 25)
            {
                return false;
            }
            if (InBounds(to))
            {
                if (IsValid(player, this.BoardLocation[to]))
                {
                    int r = SignOfNum(this.BoardLocation[from]); // the value of r can be -1/1/0

                    this.BoardLocation[from] -= r;
                    // check if the pipe contain opposite player's 1 checker in it
                    if (-r == this.BoardLocation[to])
                    {
                        // hit the blot and place into the bar
                        this.BoardLocation[player == 1 ? this.BarNumForPlayer(2) : this.BarNumForPlayer(1)] += BoardLocation[to];
                        this.BoardLocation[to] = 0;
                    }
                    this.BoardLocation[to] += r;
                    return true;
                }
            }
            return false;
        } // end of move
        private int SignOfNum(int num)
        {
            //
            // return -1 if a negative
            // return +1 if a positive
            // retirn  0 if a zero
            //
            if (num == 0)
                return 0;
            if (num > 0)
                return 1;
            else
                return -1;
        }
        /// <summary>
        /// Checks if player has graveyard
        /// </summary>
        /// <param name="BoardLocation"> </param>
        /// <param name="player">
        /// @return </param>
        public bool HasCheckerInBar(int player)
        {
            //
            // check position 24 or 25
            //
            return this.BoardLocation[BarNumForPlayer(player)] != 0;
        } // end of hasGyard

        // Returns whether player "owns" this checker or not.
        public bool IsPlayerOwnThePipe(int player, int containValue)
        {
            if (player == 1)
            {
                return containValue > 0;            //player
            }
            else
            {
                return containValue < 0;            //computer
            }
        }

        public int GetBoardLocation(int index)
        {
            return this.BoardLocation[index];
        }

        /// <summary>
        /// Counts and returns checkers number
        /// </summary>
        /// <param name="playerID"> </param>
        public int CountNumOfPipesContainCheckers(int playerID) // what would this
                                                                        // method should
                                                                        // return??
        {
            int count = 0;
            for (int i = 0; i < BoardLocation.Length; i++)
            {
                if (BoardLocation[i] > 0 && playerID == 1)
                {
                    // count += this.BoardLocation[i];
                    count++;
                }
                else if (BoardLocation[i] < 0 && playerID == 2)
                {
                    // count += this.BoardLocation[i];
                    count++;
                }
            }
            return count;
        }

        public int CountBoardLocations(int playerID) // what would this method
        {
            // should return??
            int count = 0;
            for (int i = 0; i < BoardLocation.Length; i++)
            {
                if (BoardLocation[i] > 0 && playerID == 1)
                {
                    count += this.BoardLocation[i];
                }
                // count++;
                else if (BoardLocation[i] < 0 && playerID == 2)
                {
                    count += this.BoardLocation[i];
                }
                // count++;
            }

            return count;
        }

        // Returns the amount of checkers inside player's home.
        public int homeCount(int player)
        {
            int count = 0;
            for (int i = 0; i < 6; i++) // white's home
            {
                if (this.IsPlayerOwnThePipe(player, this.BoardLocation[i]))
                {
                    count++;
                }
            }
            // System.out.println("Player : " +player + "Home Count :" +count);
            return count;
        }

        /// <summary>
        /// Checks if player has checker inside his home before given point argument
        /// </summary>
        /// <param name="point">
        /// @return </param>
        private bool HasPrev(int point)
        {
            for (int i = point + 1; i < 6; i++)
            {
                if (BoardLocation[i] > 0)
                {
                    return true;
                }
            }
            return false;
        }

        // Return the position to start counting from when moving out of graveyard.
        // THIS METHOD HAVE NEVER BEEN USED
        // public int countFrom(int player)
        // {
        // return player == 1 ? 24 : -1;
        // }

        //public bool canGather(int pos, Dice dice)
        public bool canGather(int pos)
        {
            // check if there are cheackers out of the HOME
            if (homeCount(1) < CountNumOfPipesContainCheckers(1))
            {
                return false;
            }
            else
            {
                int i = 0;
                while (i < 4)
                {
                    if (dice.getIndividualDiceValue(i) - 1 == pos && !dice.isusedDice(i))
                    {
                        dice.useIndividualDice(i);
                        return true;
                    }
                    i++;
                }
                if (!HasPrev(pos) && dice.useBiggerThan(pos))
                {
                    return true;
                }
                return false;
            }
        }
        //public bool CanGatherCheckingPass(int pos, Dice dice)
        public bool CanGatherCheckingPass(int pos)
        {
            
            // check if there are cheackers out of the HOME
            if (homeCount(1) < CountNumOfPipesContainCheckers(1))
            {
                return false;
            }
            return true;
        }
        public void SendToBar(int pos)
        {
            //
            // Sents the checker at player's point to graveyard
            //
            BoardLocation[pos]--;
            BoardLocation[this.BarNumForPlayer(1)]++;
        }

        public void RemoveChecker(int position)
        {
            if (BoardLocation[position] > 0)
                BoardLocation[position]--;
            else if (BoardLocation[position] < 0)
                BoardLocation[position]++;
        }
    }
}

