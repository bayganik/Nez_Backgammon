﻿using System;
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
        public int[] NumOfCheckersInPipe;

        public BKBoard()
        {
            NumOfCheckersInPipe = new int[26];
            InitCheckers();
        }
        public void InitCheckers()
        {
            //
            // Places all checkers at their initial positions.
            //
            NumOfCheckersInPipe[0] = -2;           //black Computer
            NumOfCheckersInPipe[11] = -5;          //black Computer
            NumOfCheckersInPipe[18] = -5;          //black Computer
            NumOfCheckersInPipe[16] = -3;          //black Computer

            NumOfCheckersInPipe[5] = 5;            //white
            NumOfCheckersInPipe[12] = 5;           //white
            NumOfCheckersInPipe[7] = 3;            //white
            NumOfCheckersInPipe[23] = 2;           //white

            //		NumOfCheckersInPipe[0] = 15;
            //
            //		NumOfCheckersInPipe[23] = -15;

            //		NumOfCheckersInPipe[0] = 13;
            //		NumOfCheckersInPipe[24] = 2;
            //		NumOfCheckersInPipe[5] = 0;
            //		NumOfCheckersInPipe[12] = 0;
            //		NumOfCheckersInPipe[11] = 0;
            //		
            //		NumOfCheckersInPipe[16] = 0;
            //		NumOfCheckersInPipe[7] = -3;
            //		NumOfCheckersInPipe[18] = -2;
            //		NumOfCheckersInPipe[19] = -2;
            //		NumOfCheckersInPipe[20] = -2;
            //		NumOfCheckersInPipe[21] = -2;
            //		NumOfCheckersInPipe[22] = -2;
            //		NumOfCheckersInPipe[23] = -2;
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
                if (IsValid(player, this.NumOfCheckersInPipe[to]))
                {
                    int r = SignOfNum(this.NumOfCheckersInPipe[from]); // the value of r can be -1/1/0

                    this.NumOfCheckersInPipe[from] -= r;
                    // check if the pipe contain opposite player's 1 checker in it
                    if (-r == this.NumOfCheckersInPipe[to])
                    {
                        // hit the blot and place into the bar
                        this.NumOfCheckersInPipe[player == 1 ? this.BarNumForPlayer(2) : this.BarNumForPlayer(1)] += NumOfCheckersInPipe[to];
                        this.NumOfCheckersInPipe[to] = 0;
                    }
                    this.NumOfCheckersInPipe[to] += r;
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
        /// <param name="NumOfCheckersInPipe"> </param>
        /// <param name="player">
        /// @return </param>
        public bool HasCheckerInBar(int player)
        {
            //
            // check position 24 or 25
            //
            return this.NumOfCheckersInPipe[BarNumForPlayer(player)] != 0;
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

        public int GetNumOfCheckersInPipe(int index)
        {
            return this.NumOfCheckersInPipe[index];
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
            for (int i = 0; i < NumOfCheckersInPipe.Length; i++)
            {
                if (NumOfCheckersInPipe[i] > 0 && playerID == 1)
                {
                    // count += this.NumOfCheckersInPipe[i];
                    count++;
                }
                else if (NumOfCheckersInPipe[i] < 0 && playerID == 2)
                {
                    // count += this.NumOfCheckersInPipe[i];
                    count++;
                }
            }
            return count;
        }

        public int CountNumOfCheckersInPipes(int playerID) // what would this method
        {
            // should return??
            int count = 0;
            for (int i = 0; i < NumOfCheckersInPipe.Length; i++)
            {
                if (NumOfCheckersInPipe[i] > 0 && playerID == 1)
                {
                    count += this.NumOfCheckersInPipe[i];
                }
                // count++;
                else if (NumOfCheckersInPipe[i] < 0 && playerID == 2)
                {
                    count += this.NumOfCheckersInPipe[i];
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
                if (this.IsPlayerOwnThePipe(player, this.NumOfCheckersInPipe[i]))
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
                if (NumOfCheckersInPipe[i] > 0)
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
            NumOfCheckersInPipe[pos]--;
            NumOfCheckersInPipe[this.BarNumForPlayer(1)]++;
        }

        public void RemoveChecker(int position)
        {
            if (NumOfCheckersInPipe[position] > 0)
                NumOfCheckersInPipe[position]--;
            else if (NumOfCheckersInPipe[position] < 0)
                NumOfCheckersInPipe[position]++;
        }
    }
}

