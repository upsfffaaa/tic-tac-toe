using System;
using System.Windows;
using System.Windows.Controls;
namespace TicTacToe
{
	public partial class MainWindow : Window
	{
		TicTacToeGame BoardData;
		Button[,] BoardView;
		public MainWindow()
		{
			InitializeComponent();
			BoardData = new TicTacToeGame();
			CreateGameTable();
			UpdateTable();

			if (BoardData.Turn == 1)
				RobotPlay();
		}
		private void CreateGameTable()
		{
			BoardView = new Button[3, 3];
			BoardView[0, 0] = Place00;
			BoardView[0, 1] = Place01;
			BoardView[0, 2] = Place02;
			BoardView[1, 0] = Place10;
			BoardView[1, 1] = Place11;
			BoardView[1, 2] = Place12;
			BoardView[2, 0] = Place20;
			BoardView[2, 1] = Place21;
			BoardView[2, 2] = Place22;
		}
		private void UpdateTable()
		{
			if (BoardData.Turn == 1)
				WhoIsNow.Text = "first move - O";
			else WhoIsNow.Text ="first move - X";
			for (int i = 0; i < BoardView.GetLength(0); i++)
				for (int j = 0; j < BoardView.GetLength(1); j++)
				{
					if (BoardData.GameTable[i, j] == 1)
					{
						BoardView[i, j].Content = "O";
						BoardView[i, j].IsEnabled = false;
					}
					else if (BoardData.GameTable[i, j] == -1)
					{
						BoardView[i, j].Content = "X";
						BoardView[i, j].IsEnabled = false;
					}
					else
					{
						BoardView[i, j].Content = "";
						BoardView[i, j].IsEnabled = true;
					}
				}
		}
		private void One_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Button Selected = sender as Button;
				int i = -1, j = -1;
				bool found = false;
				for (i = 0; i < BoardView.GetLength(0) && !found; i++)
					for (j = 0; j < BoardView.GetLength(1) && !found; j++)
						if (Selected.Name == BoardView[i, j].Name) found = true;
				i--; j--;
				BoardData.Play(BoardData.Turn, i, j);
				UpdateTable();

				RobotPlay();
			}
			catch (Exception ex)
			{
				if (ex.Message == "we have winner")
					SomeoneWon();
				StatusAndErrors.Text = ex.Message;
			}
		}
		private void RobotPlay()
		{
			try
			{
				BoardData.DoTurnRobot();
				UpdateTable();
			}
			catch (Exception ex)
			{
				StatusAndErrors.Text = ex.Message;
				if (ex.Message == "we have winner")
				{ SomeoneWon(); return; }
				if (BoardData.Steps < 9)
					RobotPlay();
			}
		}
		private void SomeoneWon()
		{
			UpdateTable();
			WinnerLineView.X1 = BoardData.WinLine.X1;
			WinnerLineView.X2 = BoardData.WinLine.X2;
			WinnerLineView.Y1 = BoardData.WinLine.Y1;
			WinnerLineView.Y2 = BoardData.WinLine.Y2;
			WinnerLineView.Visibility = Visibility.Visible;
			for (int i = 0; i < BoardView.GetLength(0); i++)
				for (int j = 0; j < BoardView.GetLength(1); j++)
					BoardView[i, j].IsEnabled = false;
		}
		private void GameOverButton_Click(object sender, RoutedEventArgs e)
		{
			this.BoardData = BoardData.GameOver();
			WinnerLineView.Visibility = Visibility.Collapsed;
			UpdateTable();
		}
	}
}

