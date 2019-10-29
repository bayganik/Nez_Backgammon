using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Backgammon.Model
{
    public class Checker
    {
        public int Player { get; set; }
        public Checker(int _player)
        {
            //
            // Player 0 = Human
            // Player 1 = CPU
            //
            Player = _player;
        }
        public List<Move> getPossibleMoves()
        {
            List<Move> moves = new List<Move>();
            int op = (this.Owner == 1 ? -1 : 1);
            // get current dice & board obj
            Dice dice = ((StartGame)SwingUtilities.getRoot(this)).Game.Dice;
            Board board = ((BoardAnimation)this.Parent).CurrentBoard;
            int pos, diceToUse = 0;
            if (board.hasCheckerInBar(1))
            {
                if (this.Point != board.barNumForPl(1))
                {
                    return moves;
                }
            }
            if (dice.RollDouble)
            {
                int counter = 1;
                diceToUse = dice.totalusedDices();
                while (diceToUse != 4)
                {
                    pos = (this.Point + (((counter++)) * op * dice.getIndividualDiceValue(diceToUse)));
                    if (board.inBounds(pos))
                    {
                        if (board.isValid(this.Owner, board.getNumOfCheckersInPipe(pos)))
                        {
                            moves.Add(new Move(pos, dice.getIndividualDiceValue(diceToUse)));
                            diceToUse++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else
            {
                for (diceToUse = 0; diceToUse < 2; diceToUse++)
                {
                    if (dice.isusedDice(diceToUse))
                    {
                        continue;
                    }
                    pos = (this.Point + (op * dice.getIndividualDiceValue(diceToUse)));
                    if (board.inBounds(pos))
                    {
                        if (board.isValid(this.Owner, board.getNumOfCheckersInPipe(pos)))
                        {
                            moves.Add(new Move(pos, dice.getIndividualDiceValue(diceToUse)));
                        }
                    }

                }
                // bujhlam na
                if (moves.size() >= 1 && dice.totalusedDices() == 2)
                {
                    pos = (this.Point + (op * (dice.getIndividualDiceValue(0) + dice.getIndividualDiceValue(1))));
                    if (board.inBounds(pos))
                    {
                        if (board.isValid(this.Owner, board.getNumOfCheckersInPipe(pos)))
                        {
                            moves.add(new Move(pos, dice.getIndividualDiceValue(diceToUse)));
                        }
                    }
                }
            }
            return moves;
        }


        private void resetPos()
        {
            setLocation(panelX, panelY);
        }

    }
}
