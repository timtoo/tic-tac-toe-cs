using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe
{
	public struct Direction {
		public static int horizontal = 0;
		public static int vertical = 90;
		public static int diagonal_up = 215;
		public static int diagonal_down = 45;
	}

	class TicTacToe
	{
		public static char[] player_mark = {'.', 'X', 'O'};

		private static Random random = new Random();

		private int[] board;
		public int width { get; private set; }
		public int height { get; private set; }
		//public int height { get; private set; }

		public int player_count { get; private set; }
		public int player_turn { get; private set; }
		public int win_length { get; private set; }
		public int turns { get; private set; } // how many places have been played on the board
		private List<int> history = new List<int>();

		// -----------------------------------------------------------------------------------------

		/// <summary>
		/// Initializes a new instance of the <see cref="TicTacToe.TicTacToe"/> class (tic-tac-toe like board).
		/// </summary>
		/// <param name="width">Width.</param>
		/// <param name="height">Height.</param>
		public TicTacToe(int width, int height) {
			constructor_defaults();

			this.width = width;
			this.height = height;

			// initialize new board with zeros
			board = new int[width * height];
			for (var i=0; i<board.Length; i++){
				board[i] = 0;	
			}

		}

		/// <summary>
		/// Shortcut to initialize a 3x3 new instance of the <see cref="TicTacToe.TicTacToe"/> class.
		/// </summary>
		public TicTacToe() : this(3, 3) {}

		/// <summary>
		/// Parse tic-tac-toe like game from string and initializes a new instance of the <see cref="TicTacToe.TicTacToe"/> class.
		/// The string should contain lines of X and O or . for an unused cell, with newlines at the end of each row. 
		/// </summary>
		/// <param name="s">S. The string containing the game.</param>
		public TicTacToe(string s) {
			/* Parse a string representing a game board, where . represents
			 * an empty space, and X is player 1, and O is player 2. 
			 * The (square) board can be a simple string, or there can be
			 * line breaks to start a new row.
			 */

			constructor_defaults();

			string pm = new string(player_mark);
			// match digits or characgers in the player_mark array
			System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex("[^0123456789" + pm + "]+");

			s = s.Trim();

			// if newline character found, assume that is the width of the grid
			int sep = s.IndexOfAny(new char[]{'\n', '\r'});

			// if newline not found, see if length of string is square
			if (sep == -1) {
				double tmp = Math.Sqrt(s.Length);
				sep = (int)tmp;
				if (sep != tmp) {
					sep = -1;
				}
			}

			// convert string into board array if it seems reasonable
			if (sep != -1) {
				s = re.Replace(s, ""); // get rid of unrecognized characters
				board = new int[s.Length];
				for (int i=0; i<s.Length; i++) {
					board[i] = pm.IndexOf(s[i]);
					if (board[i] != 0) {
						turns++;
						next_player();
					}
				}
				width = sep;
				height = s.Length / width;
			}
			if (sep == -1 || width * height != board.Length) {
				throw new ArgumentException("Can not parse tic-tac-toe string");
			}
		}

		/// <summary>
		/// Constructor defaults. Until mono supports Auto-property defaults.
		/// </summary>
		private void constructor_defaults() {
			width = 0;
			height = 0;
			player_count = 2;
			player_turn = 1;
			win_length = 3;
		}

		// -----------------------------------------------------------------------------------------

		/// <summary>
		/// Build ASCII representation of game. 
		/// </summary>
		/// <returns>A <see cref="System.String"/> that represents the current <see cref="TicTacToe.TicTacToe"/>.</returns>
		public override string ToString() {
			//return "" + player_mark[board[0]];
			StringBuilder sb = new StringBuilder();
			int w = 0;
			int h = 0;
			for (int i=0; i<board.Length; i++) {
				//Console.WriteLine(board.Length + " " + player_mark.Length);
				//Console.WriteLine(i + " " + board[i] + " " + player_mark[board[i]]);
				sb.Append(player_mark[board[i]]);
				if (++w % width == 0 && h < height -1) {
					sb.Append("\n");
					h++;
				} 
			}
			return sb.ToString();
		}

		/// <summary>
		/// Describe the details of this instance. Includes ToString() drawing of board. 
		/// </summary>
		public string Describe() {
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append("Dimensions: " + width + "x" + height + "\n");
			sb.Append("Turns: " + turns + "(player " + player_turn + ")\n");
			sb.Append(ToString());
			return sb.ToString();
		}

		/// <summary>
		/// Gets the history.
		/// </summary>
		/// <returns>The history.</returns>
		public List<int> GetHistory() {
			return history;
		}

		/// <summary>
		/// Sets the player_turn to the next player.
		/// </summary>
		private void next_player() {
			player_turn++;
			if (player_turn > player_count) {
				player_turn = 1;
			}
		}
			
		/// <summary>
		/// Claim board location for current player (all placement, or turns, should go through this method). 
		/// The number of turns in the game is incremented, and the player_turn moves to next player.
		/// </summary>
		/// <param name="index">Index. The location on the board array</param>
		public void place(int index) {
			board[index] = player_turn;
			turns++;
			next_player();
			history.Add(index);
		}

		/// <summary>
		/// claim column/row position for current player (figures out index and calls place(index))
		/// </summary>
		/// <param name="column">Column.</param>
		/// <param name="row">Row.</param>
		public void place(int column, int row) {
			place((row * width) + column);
		}
			
		/// <summary>
		/// Take turn for current player claiming an empty cell entirely at random
		/// </summary>
		public void place_random() {
			if (turns < board.Length) {
				while (true) {
					int index = random.Next(board.Length);
					if (board[index] == 0) {
						place(index);
						break;
					}
				}
			}
		}

		// semi-intelligent.. will win, will block
		public void place_with_analyse() {
			int[] row = { 1, 1, 0 };
			RowAnalysis r = new RowAnalysis(1, 3);
			//r.analyse(row);
			Console.WriteLine(r.ToString());

		}

		/// <summary>
		/// Analyses the whole board. Runs unsorted list of scores for each line for each position.
		/// </summary>
		/// <returns>Line Analysis object with all lines analysed in the *unsorted* results property.</returns>
		/// <param name="player">Player number. The perspective of the analysis</param>
		internal RowAnalysis analyse_board(int player) {
			RowAnalysis r = new RowAnalysis(player, win_length);
			Console.WriteLine("height: {0} width: {1}", height, width);

			// check all horizontal rows & diagonals where appropriate
			for (int i=0; i < height; i++) {
				r.analyse(extract_row(i*width), i * width, Direction.horizontal);
				Console.WriteLine("R" + i.ToString());

				if (i <= height - win_length) {
					Console.WriteLine("DD:");
					r.analyse(extract_diagonal_down(i*width), i*width, Direction.diagonal_down);
				}

				if (i >= win_length-1) {
					Console.WriteLine("DU:");
					r.analyse(extract_diagonal_up(i*width), i*width, Direction.diagonal_up);
				}
			}

			// check vertical rows & diagonal where appropriate
			for (int i=0; i < width; i++) {
				r.analyse(extract_column(i), i, Direction.vertical);
				Console.WriteLine("C" + i.ToString());

				if (i>0 && i <= width - win_length) {
					Console.WriteLine("DB:");
					r.analyse(extract_diagonal_down(i), i, Direction.diagonal_down);
					r.analyse(extract_diagonal_up((height-1) * width + i), (height-1) * width + i, Direction.diagonal_up);
				}

			}

			//r.Sort();

			float best_rank = r.result[0].rank;
			int same_rank_count = 1;

			// check for equal ranks, and if so, random pick one
			for (int i = 1; i < r.result.Capacity; i++) {
				if(r.result[i].rank == best_rank) {
					same_rank_count++;
				} else
					break;				
			}

			return r;
				
		}


		/// <summary>
		/// Iterable to extracts all line sequences (horizontal, vertical and two diagonals) for given position.
		/// </summary>
		/// <returns>Each row.</returns>
		/// <param name="index">The index of the board position to extract all lines for.</param>
		internal IEnumerable<int[]> interate_lines(int index) {
			yield return extract_row(index);
			yield return extract_column(index);
			yield return extract_diagonal_down(index);
			yield return extract_diagonal_up(index);
		}

		/// <summary>
		/// Extracts the horizontal line the given cell passes through.
		/// </summary>
		/// <returns>The row as Array</returns>
		/// <param name="index">Index.</param>
		internal int[] extract_row(int index) {
			int[] row = new int[width];
			int row_start = (index / width) * width;
			for (int i = 0; i < width; i++) {
				row[i] = board[row_start + i];
			}
			return row;
		}

		/// <summary>
		/// Extracts the vertical line the given cell passes through
		/// </summary>
		/// <returns>The column.</returns>
		/// <param name="index">Index.</param>
		internal int[] extract_column(int index) {
			int[] col = new int[height];
			int column = index % width;
			for (int i = 0; i < height; i++) {
				col[i] = board[(i * width) + column];
			}
			return col;
		}

		// return array of entire down diagonal which contains position
		internal int[] extract_diagonal_down(int index) {
			int[] row;
			int row_num = index / width; // row number selected cell is in
			int col_num = index % width;  // column of selected cell
			int start_row = 0;           // assume we start at the top for now
			int start_col = col_num - row_num; // how many columns do we need to move left to get to top 

			// if we went beyond first column moving to top row, so we need to move down and reset to 1st column
			if (start_col < 0) {
				start_row = -start_col;
				start_col = 0;
			}

			// Figure out array result size
			if (start_row == 0) {
				// lesser of height, or width minus start column
				row = new int[Math.Min(height, width - start_col)];
			}
			else {
				// lesser of width, or height - start row
				row = new int[Math.Min(width, height - start_row)];
			}

			for (int i=0; i < row.Length; i++) {
				row[i] = board[((start_row + i) * width) + (start_col + i)];
			}

			return row;
		}

		// return array of entire down diagonal which contains position
		internal int[] extract_diagonal_up(int index) {
			int[] row;
			int row_num = index / width; // row number selected cell is in
			int col_num = index % width;  // column of selected cell
			int start_col = 0;           // assume we start at the left for now
			int start_row = row_num + col_num; // how many columns do we need to move left to get to top 

			// if we went beyond first column moving to top row, so we need to move down and reset to 1st column
			if (start_row > height - 1 ) {
				start_col = start_row - (height - 1);
				start_row = height - 1;
			}

			// Figure out array result size
			if (start_row == height - 1) {
				// lesser of height, or width minus start column
				row = new int[Math.Min(height, width - start_col)];
			}
			else {
				// lesser of width, or height (from start_row+1)
				row = new int[Math.Min(width, start_row + 1)];
			}

			for (int i=0; i < row.Length; i++) {
				row[i] = board[((start_row - i) * width) + (start_col + i)];
			}

			return row;
		}
			
		public static void Mainx (string[] args)
		{
			int[] row = { 1, 1, 0 };
			RowAnalysis r = new RowAnalysis(1, 3);
			//r.analyse(row);
			Console.WriteLine(r.ToString());
		}

	}


}

