using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Backgammon.Model
{
    public class Move
    {
        public int MoveTo { get; set; }
        public int DiceValue { get; set; }
        public Move (int _destination, int _diceValue)
        {
            MoveTo = _destination;
            DiceValue = _diceValue;
        }
    }
}
