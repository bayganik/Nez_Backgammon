﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nez;

namespace Nez_Backgammon
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
     * |                 |                  | 25 (black graveyard)
     * |                 |                  | 24 (white graveyard)
     * |-5          03   |05             -2 |
     * |-----------------|------------------|
     *  11 10  9  8  7  6  5  4  3  2  1  0
     */
    public class BGBoard
    {
        /*
         * Game legal move positions 0 - 23
         * White graveyard = 24 (white hit checkers go)
         * Black graveyard = 25 (black hit checkers go)
         * White collector = 26 (white checkers collected)
         * Black collector = 27 (black checkers collected)
         * 
         * Negative numbers are black checkers (CPU)
         * Positive numbers are white checkers (human)
         * Zero indicates an open location (black or white)
         */
        public int[] DispBoard;
        public BGBoard()
        {
            DispBoard = new int[28];
            //
            // initialize the game board used for finding legal moves
            //
            for (int i = 0; i < 28; i++)
                DispBoard[i] = 0;
            //
            // Places all checkers at their initial positions.
            //
            DispBoard[0] = -2;              //black Computer
            DispBoard[11] = -5;             //black Computer
            DispBoard[18] = -5;             //black Computer
            DispBoard[16] = -3;             //black Computer
            //DispBoard[25] = -1;             //black graveyard
            //
            // put single black checkers so we can hit
            //
            //DispBoard[0] = -1;              //black Computer
            //DispBoard[11] = -1;             //black Computer
            //DispBoard[18] = -1;             //black Computer
            //DispBoard[16] = -1;             //black Computer

            DispBoard[5] = 5;               //white
            DispBoard[12] = 5;              //white
            DispBoard[7] = 3;               //white
            DispBoard[23] = 2;              //white
            //
            // put all white checkers at home so we can test collection
            //
            //DispBoard[1] = 5;               //white
            //DispBoard[2] = 5;              //white
            //DispBoard[3] = 3;               //white
            //DispBoard[5] = 2;              //white

        }
        //
        // Test Location 24 is white, 25 is black
        //
        public bool HasCheckersInGraveYard(int _location)
        {
            if (Math.Abs(DispBoard[_location]) > 0)
                return true;

            return false;
        }
        public int GetDispBoard(int index)
        {
            return this.DispBoard[index];
        }
        //
        // Test location 0 - 5 for white
        //
        public bool CanWhiteCollectCheckers()
        {
            int totalCheckers = 0;
            for (int i = 0; i < 6; i++)
            {
                if (DispBoard[i] > 0)
                {
                    totalCheckers += DispBoard[i];
                }
            }
            totalCheckers += DispBoard[26];         //what has already been collected
            if (totalCheckers == 15)
                return true;

            return false;
        }
        public bool WhiteWinsGame()
        {
            if (DispBoard[26] == 15)
                return true;
            return false;
        }
        public bool BlackWinsGame()
        {
            if (Math.Abs(DispBoard[27]) == 15)
                return true;
            return false;
        }
        //
        // Test location 18 - 23 for black
        //
        public bool CanBlackCollectCheckers()
        {
            int totalCheckers = 0;
            for (int i = 18; i < 24; i++)
            {
                if (DispBoard[i] < 0)
                {
                    totalCheckers += Math.Abs(DispBoard[i]);
                }
            }
            totalCheckers += Math.Abs(DispBoard[27]);         //what has already been collected
            if (totalCheckers == 15)
                return true;

            return false;
        }
        //
        // This is called during white checker drag operations
        // So, we know the _fromLoc of the checkers 
        //
        public Dictionary<int, int> GetWhiteLegalMoves(int[] _dice, int _fromLoc)
        {
            Dictionary<int,int> legalMoves;
            //
            // How many moves can the checker have
            //
            //if (_dice[0] == _dice[1])
            //    legalMoves = new int[4];            //4 legal moves for doubles
            //else
            //    legalMoves = new int[2];            // 2 legal moves
            //
            legalMoves = new Dictionary<int, int>();

            //znznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznzn
            // if white is coming from GraveYard
            //znznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznzn
            if (_fromLoc == 24)
            {
                for (int i = 0; i < _dice.Count(); i++)
                {
                    if (_dice[i] <= 0)                          //dice roll is used
                        continue;
                    //
                    // landloc will be 18 - 23
                    //
                    int landloc = _fromLoc - _dice[i];
                    //legalMoves[i] = 0;
                    switch (DispBoard[landloc])
                    {
                        case -1:                                //single black checker
                            legalMoves.Add(i, landloc * -1);
                            //legalMoves[i] = landloc * -1;       
                            break;
                        case int n when (n >= 0):               //empty or whit checkers
                            legalMoves.Add(i, landloc);
                            //legalMoves[i] = landloc;
                            break;
                    }
                }
                
                return legalMoves;
            }
            
            //znznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznzn
            // Can white collect checkers
            //znznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznzn
            if (CanWhiteCollectCheckers())
            {
                for (int i = 0; i < _dice.Count(); i++)
                {
                    if (_dice[i] <= 0)                          //dice roll is used
                        continue;

                    int landloc = _fromLoc - _dice[i];
                    //legalMoves[i] = 0;
                    switch (landloc)
                    {
                        case int n when (n < 0):                //white collection stack
                            legalMoves.Add(i, 26);
                            //legalMoves[i] = 26;                 
                            break;
                        case int n when (n >= 0):               //cannot collect, must play
                            if (DispBoard[landloc] == -1)   //single black checker
                                legalMoves.Add(i, landloc);

                            if (DispBoard[landloc] >= 0)   //empty or white checkers
                                legalMoves.Add(i, landloc);

                            //legalMoves[i] = landloc;
                            break;
                    }
                }

                return legalMoves;
            }
            
            //znznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznzn
            // Normal white checker move
            //znznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznznzn
            for (int i = 0; i < _dice.Count(); i++)
            {
                if (_dice[i] <= 0)                          //dice roll is used
                    continue;

                int landloc = _fromLoc - _dice[i];
                if (landloc >= 0)
                {
                    //legalMoves[i] = 0;
                    switch (DispBoard[landloc])
                    {
                        case -1:                                //single black checker
                            legalMoves.Add(i, landloc * -1);
                            //legalMoves[i] = landloc * -1;       
                            break;
                        case int n when (n >= 0):               //empty or whit checkers
                            legalMoves.Add(i, landloc);
                            //legalMoves[i] = landloc;
                            break;
                    }
                }

            }
            return legalMoves;
        }
    }
}
