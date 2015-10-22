using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe
{
	public struct RowResult {
		public float rank;
		public int index;
		public int suggestion;
	}

	class TicTacToe

	{
		public static char[] player_mark = {'.', 'X', 'O'};

		private static Random random = new Random();

		private int[] board;
		public int width { get; private set; }
		public int height { get; private set; }

		public int player_count { get; private set; }
		public int player_turn { get; private set; }
		public int win_length { get; private set; }
		public int turns { get; private set; } // how many places have been played on the board
		private List<int> history = new List<int>();

		// -----------------------------------------------------------------------------------------

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

		// shortcut constructor for standard tictactoe
		public TicTacToe() : this(3, 3) {}

		// create a game from a string representation
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
			
		private void constructor_defaults() {
			// because mono here doens't support Auto-property defaults yet
			width = 0;
			height = 0;
			player_count = 2;
			player_turn = 1;
			win_length = 3;
		}

		// -----------------------------------------------------------------------------------------

		// Build ASCII representation of game
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

		public string Describe() {
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			sb.Append("Dimensions: " + width + "x" + height + "\n");
			sb.Append("Turns: " + turns + "(player " + player_turn + ")\n");
			sb.Append(ToString());
			return sb.ToString();
		}

		public List<int> GetHistory() {
			return history;
		}

		private void next_player() {
			player_turn++;
			if (player_turn > player_count) {
				player_turn = 1;
			}
		}
			
		// place piece on linear board (all placement should go through this method)
		public void place(int index) {
			board[index] = player_turn;
			turns++;
			next_player();
			history.Add(index);
		}

		// place piece for player given column & row (zero based)
		public void place(int column, int row) {
			place((row * width) + column);
		}

		// place next player in random available place
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

		// return array of entire row which contains position
		internal int[] extract_row(int index) {
			int[] row = new int[width];
			int row_start = (index / width) * width;
			for (int i = 0; i < width; i++) {
				row[i] = board[row_start + i];
			}
			return row;
		}

		// return array of entire row which contains position
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

		internal int analyze_row(int player, int[] row) {
			/* 
			 * 
			 * Find win condition (complete sequence) 100% score
			 * Find sequences with no blocking pieces (higher score for less blanks)
			 * 
			 * 
			 * 
			 * score is precent of sequence 
			 * 
			 * return 
			 */
			int player_count = 0;
			int player_seq_count = 0;
			int other_count = 0;
			int other_seq_count = 0;
			int none_count = 0;
			return -1;


		}

		public static void Main (string[] args)
		{
			TicTacToe t2 = new TicTacToe(
				"XOXO\n" + 
				"OXOX\n" + 
				"XXOO\n" + 
				"OOXX\n" + 
				"XOOX");

			int[] t2r1 = { 2, 1, 2, 2 };
			Console.WriteLine(t2.extract_diagonal_up(9));

			TicTacToe t = new TicTacToe("XOX\nOXO\nXXO");

			// XOX
			// OXO
			// XXO

			Console.WriteLine(t.extract_diagonal_up(3));

		}

	}

	class RowAnalysis {

		// { score, sequence start, suggested empty cell }
		private List<RowResult> sequences = new List<RowResult>();
		private int player;
		private int[] row;
		private int seqlen;

		// player number, row array, length of win sequence
		public RowAnalysis(int player, int[] row, int seqlen) {
			this.player = player;
			this.row = row;
			this.seqlen = seqlen;
		}

		private static int order(RowResult x, RowResult y) {
			// reverse sort based on rank
			if (y.rank > x.rank) return -1;
			if (x.rank < y.rank) return 1;
			return 0;
		}

		internal void analyse() {
			RowResult rr;
			int len = row.Length - seqlen + 1;

			Console.WriteLine("ll:" + len);

			if (len >= 1) {
				for (int b = 0; b < len; b++) {
					int seq_player_count = 0;
					int seq_other_count = 0;
					int seq_last_empty = -1;
					for (int i=0; i < seqlen; i++) {
						if (row[b+i] == player) {
							seq_player_count++;
						}
						else if (row[b+i] == 0) {
							seq_last_empty = b + i;
						}
						else {
							seq_other_count++;
						}
					}

					rr = new RowResult();
					rr.index = b;
					rr.suggestion = seq_last_empty;
					rr.rank = 0;
					if (seq_other_count > 0) { // there is a blocker so all is useless in this row?
						rr.rank = 0;
					}
					else if (seq_player_count == seqlen - 1) { // player will win
						rr.rank = 1;
					}
					else if (seq_player_count > 0) { // percentage of player pieces in sequence
						rr.rank = seq_player_count / seqlen;
					}
					sequences.Add(rr);
				}
			}

		}

		public RowResult best_suggestion() {
			sequences.Sort(order);				
			return sequences[0];
		}

		public override string ToString() {
			StringBuilder sb = new StringBuilder();
//			sequences.Sort((RowResult x, RowResult y) => x.rank > y.rank ? -1 : (x.rank < y.rank ? 1 : 0));	
			foreach (RowResult r in sequences) {
				sb.Append(string.Format("{0} {1} {1}\n", r.rank, r.index, r.suggestion));
			}
			return sb.ToString();
		}
	}


}

