using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace TicTacToe
{
	public class TicTacToeGame
	{
		private static List<TicTacToeGame> OldGames = new List<TicTacToeGame>();
		private int[,] gameTable;
		private int turn;
		private int steps;
		private Line winLine;
		public int[,] GameTable
		{
			get { return gameTable; }
			private set { gameTable = value; }
		}
		public int Turn
		{
			get { return turn; }
			private set { turn = value; }
		}
		public int Steps
		{
			get { return steps; }
			private set { steps = value; }
		}
		public Line WinLine
		{
			get { return winLine; }
			private set { winLine = value; }
		}
		Random rand;
		public TicTacToeGame()
		{
			Steps = 0;
			GameTable = new int[3, 3];
			for (int i = 0; i < GameTable.GetLength(0); i++)
				for (int j = 0; j < GameTable.GetLength(1); j++)
					GameTable[i, j] = 0;
			rand = new Random();
			if (rand.Next(2) == 1)
				Turn = 1;
			else Turn = -1;
		}
		private void ChangeTurn()
		{
			Turn *= -1;
		}
		public bool Play(int player, int i, int j)
		{
			GameTable[i, j] = player; 
			steps++;
			Line Winner = CheckWinner();

			if (Winner != null)
			{
				WinLine = Winner;
				throw new Exception("we have winner");
			}

			ChangeTurn(); 
			return true;
		}
		public TicTacToeGame GameOver()
		{
			OldGames.Add(this);
			return new TicTacToeGame();
		}
		public void DoTurnRobot()
		{
			int[] OptionToWin = freeCells(Turn);
			if (OptionToWin != null)
			{ Play(Turn, OptionToWin[0], OptionToWin[1]); return; }
			int[] OptionToBlock = freeCells(Turn * -1);
			if (OptionToBlock != null)
			{ Play(Turn, OptionToBlock[0], OptionToBlock[1]); return; }
			if (steps == 0) 
			{
				RandomOX();

				return;
			}
			if (steps == 1) 
			{
				if (gameTable[1, 1] != 0)
				{
					RandomOX();
					return;
				}
				if (gameTable[0, 0] != 0 || gameTable[2, 2] != 0 || gameTable[2, 0] != 0 || gameTable[0, 2] != 0)
				{
					Play(turn, 1, 1);
					return;
				}
			}
			Play(turn, rand.Next(2), rand.Next(2));
		}
		private bool RandomOX()
		{
			int a = 0, b = 0;
			if (rand.Next(2) == 1)
				a = 2;
			if (rand.Next(2) == 1)
				b = 2;
			return Play(Turn, a, b);
		}
		private int[] freeCells(int xo)
		{
			for (int i = 0; i < GameTable.GetLength(0); i++)
			{
				if (GameTable[i, 0] == xo && GameTable[i, 1] == xo && GameTable[i, 2] == 0)
					return new int[] { i, 2 };
				else if (GameTable[i, 1] == xo && GameTable[i, 2] == xo && GameTable[i, 0] == 0)
					return new int[] { i, 0 };
				else if (GameTable[i, 0] == xo && GameTable[i, 2] == xo && GameTable[i, 1] == 0)
					return new int[] { i, 1 };
			}
n:			for (int i = 0; i < GameTable.GetLength(1); i++)
			{
				if (GameTable[0, i] == xo && GameTable[1, i] == xo && GameTable[2, i] == 0)
					return new int[] { 2, i };
				else if (GameTable[1, i] == xo && GameTable[2, i] == xo && GameTable[0, i] == 0)
					return new int[] { 0, i };
				else if (GameTable[0, i] == xo && GameTable[2, i] == xo && GameTable[1, i] == 0)
					return new int[] { 1, i };
			}
			if (GameTable[0, 0] == xo && GameTable[2, 2] == xo && GameTable[1, 1] == 0)
				return new int[] { 1, 1 };
			else if (GameTable[1, 1] == xo && GameTable[2, 2] == xo && GameTable[0, 0] == 0)
				return new int[] { 0, 0 };
			else if (GameTable[0, 0] == xo && GameTable[1, 1] == xo && GameTable[2, 2] == 0)
				return new int[] { 2, 2 };

			if (GameTable[2, 0] == xo && GameTable[0, 2] == xo && GameTable[1, 1] == 0)
				return new int[] { 1, 1 };
			else if (GameTable[1, 1] == xo && GameTable[0, 2] == xo && GameTable[2, 0] == 0)
				return new int[] { 2, 0 };
			else if (GameTable[2, 0] == xo && GameTable[1, 1] == xo && GameTable[0, 2] == 0)
				return new int[] { 0, 2 };
			return null;
		}
		private Line CheckWinner()
		{
			for (int i = 0; i < GameTable.GetLength(0); i++)
			{
				if (GameTable[i, 0] == Turn && GameTable[i, 1] == Turn && GameTable[i, 2] == Turn)
					return new Line()
					{
						X1 = 0,
						Y1 = 40 + 80 * i,
						X2 = 240,
						Y2 = 40 + 80 * i
					};
			}
			for (int i = 0; i < GameTable.GetLength(1); i++)
			{
				if (GameTable[0, i] == Turn && GameTable[1, i] == Turn && GameTable[2, i] == Turn)
					return new Line()
					{
						X1 = 40 + 80 * i,
						Y1 = 0,
						X2 = 40 + 80 * i,
						Y2 = 240
					};
			}
			if (GameTable[0, 0] == Turn && GameTable[2, 2] == Turn && GameTable[1, 1] == Turn)
				return new Line()
				{
					X1 = 0,
					Y1 = 0,
					X2 = 240,
					Y2 = 240
				};
			if (GameTable[2, 0] == Turn && GameTable[0, 2] == Turn && GameTable[1, 1] == Turn)
				return new Line()
				{
					X1 = 0,
					Y1 = 240,
					X2 = 240,
					Y2 = 0
				};
			return null;
		}
	}
}
