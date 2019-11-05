using System;
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
    public class BKBoard
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
        public int[] BoardLocation;
        public BKBoard()
        {
            BoardLocation = new int[28];
            //
            // initialize the game board used for finding legal moves
            //
            for (int i = 0; i < 28; i++)
                BoardLocation[i] = 0;
            //
            // Places all checkers at their initial positions.
            //
            BoardLocation[0] = -2;              //black Computer
            BoardLocation[11] = -5;             //black Computer
            BoardLocation[18] = -5;             //black Computer
            BoardLocation[16] = -3;             //black Computer
            //BoardLocation[0] = -1;              //black Computer
            //BoardLocation[11] = -1;             //black Computer
            //BoardLocation[18] = -1;             //black Computer
            //BoardLocation[16] = -1;             //black Computer

            BoardLocation[5] = 5;               //white
            BoardLocation[12] = 5;              //white
            BoardLocation[7] = 3;               //white
            BoardLocation[23] = 2;              //white
        }
        //
        // Test Location 24 is white, 25 is black
        //
        public bool HasCheckersInGraveYard(int _location)
        {
            if (Math.Abs(BoardLocation[_location]) > 0)
                return true;

            return false;
        }
        //
        // Test location 0 - 5 for white
        //
        public bool CanWhiteCollectCheckers()
        {
            int totalCheckers = 0;
            for (int i = 0; i < 6; i++)
            {
                if (BoardLocation[i] > 0)
                {
                    totalCheckers += BoardLocation[i];
                }
            }

            if (totalCheckers == 15)
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
                if (BoardLocation[i] < 0)
                {
                    totalCheckers += Math.Abs(BoardLocation[i]);
                }
            }

            if (totalCheckers == 15)
                return true;

            return false;
        }
        public int[] GetWhiteLegalMoves(int[] _dice, int _fromLoc)
        {
            int[] legalMoves;

            if (_dice[0] == _dice[1])
                legalMoves = new int[4];            //4 legal moves at most
            else
                legalMoves = new int[2];            // 2 legal moves
            //
            // if white is coming from GraveYard
            //
            if (_fromLoc == 24)
            {
                for (int i = 0; i < _dice.Count(); i++)
                {
                    int landloc = _fromLoc - _dice[i];
                    legalMoves[i] = 0;
                    switch (BoardLocation[landloc])
                    {
                        case -1:
                            legalMoves[i] = landloc * -1;       //single black checker
                            break;
                        case int n when (n >= 0):               //empty or whit checkers
                            legalMoves[i] = landloc;
                            break;
                    }
                }

                return legalMoves;
            }
            //
            // Can white collect checkers
            //
            if (CanWhiteCollectCheckers())
            {
                for (int i = 0; i < _dice.Count(); i++)
                {
                    int landloc = _fromLoc - _dice[i];
                    legalMoves[i] = 0;
                    switch (BoardLocation[landloc])
                    {
                        case int n when (n < 0):
                            legalMoves[i] = 26;                 //white collection stack
                            break;
                        case int n when (n >= 0):               //empty or whit checkers
                            legalMoves[i] = landloc;
                            break;
                    }
                }

                return legalMoves;
            }
            //
            // Normal white checker move
            //
            for (int i = 0; i < _dice.Count(); i++)
            {
                int landloc = _fromLoc - _dice[i];
                if (landloc >= 0)
                {
                    legalMoves[i] = 0;
                    switch (BoardLocation[landloc])
                    {
                        case -1:
                            legalMoves[i] = landloc * -1;       //single black checker
                            break;
                        case int n when (n >= 0):               //empty or whit checkers
                            legalMoves[i] = landloc;
                            break;
                    }
                }

            }
            return legalMoves;
        }
    }
}
