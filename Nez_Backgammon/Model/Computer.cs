using System;
using System.Collections.Generic;

namespace Backgammon.Model
{

	public class Computer : Player
	{
		private Expectiminimax emm;

		public Computer(int pn) : base(pn, "Computer")
		{
			emm = new Expectiminimax();
		}

		public void play()
		{
			GameState init = new GameState(this.Game.CurrentBoard, this.Game.Dice);
			GameState res = emm.chooseMove(init);
			if (res == null) // no moves to play
			{
				this.Game.Dice.useAllDices();
				return;
			}
			calcMoves(this.Game.CurrentBoard.NumOfCheckersInPipe, res.board, this.Game.Dice);
			this.Game.Dice.useAllDices();
			Console.WriteLine(res); // print the result of the algorithm in the
										// console
		}

		/// <summary>
		/// Find which moves took place at init GameState that produced res GameState
		/// Perform those moves
		/// </summary>
		/// <param name="init">
		///            initial GameState </param>
		/// <param name="init">
		///            result GameState </param>
		private void calcMoves(int[] init, int[] dest, Dice dice)
		{
			List<int> from = new List<int>();
			List<int> to = new List<int>();
			if (init[25] != dest[25])
			{
				int counter = dest[25] - init[25];
				while (counter > 0)
				{
					from.Add(25);
					counter--;
				}
			}
			calcFromTo(init, dest, from, to);
			calcMoves(dice, from, to, dest);
			if (to.Count > 0)
			{
				foreach (int i in to)
				{
					if (init[i] > 0) // Player was hit during a multiple move
					{
						this.Game.BoardAnimation.CurrentBoard.sendToBar(i);
					}

				}
			}
			if (from.Count > 0) // gathering move for Computer
			{
				foreach (int i in from)
				{
					this.Game.BoardAnimation.removeChecker(i);
				}
			}
		}

		private static void calcFromTo(int[] init, int[] dest, List<int> diff, List<int> to)
		{
			int counter = 0;
			for (int i = 0; i < 24; i++)
			{
				if (init[i] < dest[i])
				{
					counter = dest[i] - init[i];
					while (counter > 0)
					{
						diff.Add(i);
						counter--;
					}
				}
				else if (init[i] > dest[i])
				{
					if (Math.Abs(dest[i]) != init[i])
					{
						counter = init[i] - dest[i];
					}
					else
					{
						counter = 1;
					}
					while (counter > 0)
					{
						to.Add(i);
						counter--;
					}
				}
			}
		}

		public void calcMoves(Dice dice, List<int> from, List<int> to, int[] dest)
		{
			List<int> temp = new List<int>();
			temp.AddRange(from);
			int maxDie = 0, minDie = 0;
			if (dice.getIndividualDiceValue(0) > dice.getIndividualDiceValue(1))
			{
				maxDie = dice.getIndividualDiceValue(0);
				minDie = dice.getIndividualDiceValue(1);
			}
			else
			{
				maxDie = dice.getIndividualDiceValue(1);
				minDie = dice.getIndividualDiceValue(0);
			}
			for (int i = 0; i < temp.Count; i++)
			{
				int f = temp[i];
				if (f == 25)
				{
					f = -1;
				}
				if (!dice.RollDouble)
				{
					if (to.Contains(f + minDie) && dest[f + minDie] != 0)
					{
						to.RemoveAt(Convert.ToInt32(f + minDie));
						this.Game.BoardAnimation.animateMove(f == -1 ? 25 : f, f + minDie);
						from.RemoveAt(Convert.ToInt32(temp[i]));
					}
					else if (to.Contains(f + maxDie) && dest[f + maxDie] != 0)
					{
						to.RemoveAt(Convert.ToInt32(f + maxDie));
						this.Game.BoardAnimation.animateMove(f == -1 ? 25 : f, f + maxDie);
						from.RemoveAt(Convert.ToInt32(temp[i]));

					}
					else if (to.Contains(f + maxDie + minDie))
					{
						to.RemoveAt(Convert.ToInt32(f + maxDie + minDie));
						this.Game.BoardAnimation.animateMove(f == -1 ? 25 : f, f + maxDie + minDie);
						from.RemoveAt(Convert.ToInt32(temp[i]));
					}
				}
				else
				{
					if (to.Contains(f + minDie) && dest[f + minDie] != 0)
					{
						to.RemoveAt(Convert.ToInt32(f + minDie));
						this.Game.BoardAnimation.animateMove(f == -1 ? 25 : f, f + minDie);
						from.RemoveAt(Convert.ToInt32(temp[i]));
					}
					else if (to.Contains(f + (2 * minDie)) && dest[f + 2 * minDie] != 0)
					{
						to.RemoveAt(Convert.ToInt32(f + (2 * minDie)));
						this.Game.BoardAnimation.animateMove(f == -1 ? 25 : f, f + (2 * minDie));
						from.RemoveAt(Convert.ToInt32(temp[i]));
					}
					else if (to.Contains(f + (3 * minDie)) && dest[f + 3 * minDie] != 0)
					{
						to.RemoveAt(Convert.ToInt32(f + (3 * minDie)));
						this.Game.BoardAnimation.animateMove(f == -1 ? 25 : f, f + (3 * minDie));
						from.RemoveAt(Convert.ToInt32(temp[i]));
					}
					else if (to.Contains(f + (4 * minDie)))
					{
						to.RemoveAt(Convert.ToInt32(f + (4 * minDie)));
						this.Game.BoardAnimation.animateMove(f == -1 ? 25 : f, f + (4 * minDie));
						from.RemoveAt(Convert.ToInt32(temp[i]));
					}
				}
			}
		}

	}

}