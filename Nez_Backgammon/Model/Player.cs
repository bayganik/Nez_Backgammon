using System.Collections.Generic;


namespace Backgammon.Model
{

    public class Player
	{
		private List<Checker> checkers;
		private int playerNum;
		private string name;

		//private Game game;
		private bool hasCheckerInBar;
		private int score; // number of checkers gathered

		/// <summary>
		/// Player constructor.
		/// 
		/// Initializes the player checkers.
		/// </summary>
		public Player(int _playerNum, string _str)
		{
			this.playerNum = _playerNum;
			this.name = _str;
			this.hasCheckerInBar = false;
			this.score = 0;
		}

		public Checker GetChecker(int index)
		{
			return checkers[index];
		}

		//public JLabel Label
		//{
		//	get
		//	{
		//		return jlabel;
		//	}
		//}

		public string Name
		{
			get
			{
				return name;
			}
		}
		//public Game Game
		//{
		//	get
		//	{
		//		return this.game;
		//	}
		//}

		public int Num
		{
			get
			{
				return playerNum;
			}
		}

		public bool Graveyard
		{
			set
			{
				this.hasCheckerInBar = value;
			}
		}

		public bool HasGraveyard()
		{
			return this.hasCheckerInBar;
		}
	}

}