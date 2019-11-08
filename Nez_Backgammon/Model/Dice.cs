namespace Backgammon.Model
{
	public class Dice
	{
		private int[] diceValues; // up to 4
		private bool[] usedDices; // shows if and which dice are usedDices(used)
										// during this turn

		public Dice()
		{
			diceValues = new int[4];
			usedDices = new bool[4];
		}

		/// <summary>
		/// Returns the sum of the dice.
		/// 
		/// </summary>
		public int Total
		{
			get
			{
				int total = 0;
				foreach (int b in diceValues)
				{
					total += b;
				}
				return total;
			}
		}

		public int[] DiceResults
		{
			get
			{
				return diceValues;
			}
		}

		public int getIndividualDiceValue(int i)
		{
			return diceValues[i];
		}

		public int FirstNotUsedDicesNum
		{
			get
			{
				for (int b = 0; b < 4; b++)
				{
					if (!usedDices[b])
					{
						return b;
					}
				}
				return -1;
			}
		}

		public int totalusedDices()
		{
			int total = 0;
			for (int b = 0; b < 4; b++)
			{
				if (usedDices[b])
				{
					total++;
				}
			}
			return total;
		}

		public bool RollDouble
		{
			get
			{
				return diceValues[0] == diceValues[1];
			}
		}

		public bool isusedDice(int i)
		{
			return usedDices[i];
		}

		public bool diceAvailable(int value)
		{
			if (!this.RollDouble)
			{
				if (this.diceValues[0] == value || this.diceValues[1] == value)
				{
					return !usedDices[diceValues[0] == value ? 0 : 1];
				}
				else if (this.diceValues[0] + this.diceValues[1] == value)
				{
					return this.totalusedDices() == 2;
				}
				return false;
			}
			else // bujhlam na ekhon bujhsi n
			{
				int c = (4 - (value / diceValues[0]));
				return this.totalusedDices() <= c;
			}
		}

		public void useAllDices()
		{
			for (int b = 0; b < 4; b++)
			{
				usedDices[b] = true;
			}
		}

		/// <summary>
		/// Checks whether the player made all their moves.
		/// 
		/// </summary>
		public bool AllDicesUsed
		{
			get
			{
				for (int i = 0; i < 4; i++)
				{
					if (!usedDices[i])
					{
						return false;
					}
				}
				return true;
			}
		}

		public void useDicesWithARoll(int roll) // bujhlam na ekhon bujhsi
		{
			if (roll == this.Total) // check for multimove and consume all dice
			{
				useAllDices();
			}
			else if (this.RollDouble) // consume the number of dice that were
			{
											// used to perform the move
				int temp = FirstNotUsedDicesNum;
				for (int i = temp; i < temp + (roll / diceValues[0]); i++)
				{
					usedDices[i] = true;
				}
			}
			else
			{
				for (int i = FirstNotUsedDicesNum; i < 2; i++)
				{
					if (diceValues[i] == roll)
					{
						usedDices[i] = true;
					}
				}
			}
		}

		/// <summary>
		/// Consume dice with number bigger than argument t
		/// </summary>
		/// <param name="t"> </param>
		/// <returns> true if an unusedDices die was found and usedDices,false
		///         otherwise </returns>
		public bool useBiggerThan(int value)
		{
			for (int i = 0; i < 4; i++)
			{
				if (diceValues[i] > value && !usedDices[i])
				{
					usedDices[i] = true;
					return true;
				}
			}
			return false;
		}
		public bool useBiggerThanCheckPass(int value)
		{
			for (int i = 0; i < 4; i++)
			{
				if (diceValues[i] > value && !usedDices[i])
				{

					return true;
				}
			}
			return false;
		}

		public void useIndividualDice(int i)
		{
			usedDices[i] = true;
		}

		/// <summary>
		/// Reset the dice.
		/// </summary>
		public void resetAllDices()
		{
			for (int i = 0; i < 4; i++)
			{
				usedDices[i] = false;
			}
		}

		public void setDummyDice(int value1, int value2)
		{
			resetAllDices();
			if (value1 == value2)
			{
				for (int i = 0; i < 4; i++)
				{
					diceValues[i] = value1;
				}
			}
			else
			{
				diceValues[0] = value1;
				diceValues[1] = value2;
				diceValues[2] = 0;
				diceValues[3] = 0;
			}
		}
	}

}