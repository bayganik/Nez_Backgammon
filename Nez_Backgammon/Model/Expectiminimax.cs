using System;
using System.Collections.Generic;

namespace Backgammon.Model
{
	public class Expectiminimax
	{
		public static int MAX_DEPTH = 1; // 4 because chooseMove already builds
											// Max's children before expectiminimax
											// is called
		private int[][] dice = new int[][]
		{
			new int[] {1, 1},
			new int[] {1, 2},
			new int[] {1, 3},
			new int[] {1, 4},
			new int[] {1, 5},
			new int[] {1, 6},
			new int[] {2, 2},
			new int[] {2, 3},
			new int[] {2, 4},
			new int[] {2, 5},
			new int[] {2, 6},
			new int[] {3, 3},
			new int[] {3, 4},
			new int[] {3, 5},
			new int[] {3, 6},
			new int[] {4, 4},
			new int[] {4, 5},
			new int[] {4, 6},
			new int[] {5, 5},
			new int[] {5, 6},
			new int[] {6, 6}
		};

		public GameState chooseMove(GameState gs)
		{
			GameState temp;
			List<GameState> list = gs.getChildren(2);
			double max_val = double.NegativeInfinity;
			double temp_val;
			GameState best = null;
//JAVA TO C# CONVERTER WARNING: Unlike Java's ListIterator, enumerators in .NET do not allow altering the collection:
			for (IEnumerator<GameState> iter = list.GetEnumerator(); iter.MoveNext();)
			{
				temp = iter.Current;
				temp_val = expectiminimax(temp, 0);
				if (temp_val > max_val)
				{
					best = temp;
					max_val = temp_val;
				}
			}
			return best;
		}

		private double expectiminimax(GameState gs, int depth)
		{

			if (depth == MAX_DEPTH)
			{
				return gs.evaluate(2);
			}

			if (depth % 2 == 0) // chance node
			{
				float v = 0f;
				foreach (int[] diceRoll in dice)
				{

					v += (float)((dice[0] == dice[1] ? 1.0f / 36 : 1.0f / 18) * expectiminimax(new GameState(gs.board, diceRoll), depth + 1));
				}
				return v;
			}
			else if (depth % 4 == 1) // min node
			{
				double v = double.PositiveInfinity;
				List<GameState> list = gs.getChildren(1);
				if (list.Count == 0)
				{
					return Math.Min(v, expectiminimax(gs, depth + 1));
				}
//JAVA TO C# CONVERTER WARNING: Unlike Java's ListIterator, enumerators in .NET do not allow altering the collection:
				for (IEnumerator<GameState> iter = list.GetEnumerator(); iter.MoveNext();)
				{
					v = Math.Min(v, expectiminimax(iter.Current, depth + 1));
				}
				return v;
			}
			else
			{
				double v = double.NegativeInfinity;
				List<GameState> list = gs.getChildren(2);
				if (list.Count == 0)
				{
					return Math.Max(v, expectiminimax(gs, depth + 1));
				}
//JAVA TO C# CONVERTER WARNING: Unlike Java's ListIterator, enumerators in .NET do not allow altering the collection:
				for (IEnumerator<GameState> iter = list.GetEnumerator(); iter.MoveNext();)
				{
					v = Math.Max(v, expectiminimax(iter.Current, depth + 1));
				}
				return v;
			}
		}
	}

}