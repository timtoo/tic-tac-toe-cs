using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToeLib
{
	/// <summary>
	/// Row result struct. Holds result of a row analysis.
	/// </summary>
	public struct LineResult {
		public float rank; // 0 to 1
		public int index;  // position of sequence in line
		public int suggestion; // suggested empty space in line to take
		public int position; // position on board of first cell in line
		public int direction; // direction of line (0 = horizontal, 90 = vertical, 45 = diag. down, 215 = diag. up)

		public override string ToString() {
			return string.Format("<LineResult: rank={0} pos={4} dir={3} index={1} move={2}>", 
				rank, index, suggestion, direction, position);
		}
	}

	/// <summary>
	/// Row analysis. Analyses a tic-tac-toe like board row (array)
	/// </summary>
	class RowAnalysis {
		/* 
			 * 
			 * Find win condition (complete sequence) 100% rank
			 * Find blocks (when called against other player)
			 * Find sequences with no blocking pieces (higher score for less blanks)
			 * 
			 * 
			 * 
			 * score is precent of sequence 
			 * 
			 * return 
			 */

		// { score, sequence start, suggested empty cell }
		public List<LineResult> result {get;}
		private int player;
		private int row_analysed_count = 0;
		//private int[] row;
		private int seqlen;
		private bool sorted = true;

		/// <summary>
		/// Initializes a new instance of the <see cref="TicTacToe.RowAnalysis"/> class. 
		/// player number, row array, length of win sequence
		/// </summary>
		/// <param name="player">Player number to analyze for.</param>
		/// <param name="row">Row. Array of integers representing row on board.</param>
		/// <param name="seqlen">Length of sequence required to win</param>
		public RowAnalysis(int player, int seqlen) {
			this.player = player;
			this.seqlen = seqlen;
			this.result = new List<LineResult>();
		}

		/// <summary>
		/// Sort function to reverse sort the LineResult list.
		/// </summary>
		/// <param name="a">The a LineResult.</param>
		/// <param name="b">The y LineResult.</param>
		public static int _Sort(LineResult a, LineResult b) {
			// reverse sort based on rank
			if (b.rank < a.rank) return -1;
			if (a.rank > b.rank) return 1;
			return 0;
		}

		/// <summary>
		/// Sort this instance, highest rank first. Returns the RowResult list.
		/// </summary>
		public List<LineResult> Sort() {
			if(sorted == false) {
				result.Sort(_Sort);
				sorted = true;
			}
			return result;
		}

		/// <summary>
		/// Analyse a line array. Return list of results (which are also stored in the object).
		/// 
		/// Scoring:
		/// 
		/// </summary>
		public RowAnalysis analyse(int[] line, int position, int direction) {
			LineResult row_result;
			int sequences_possible = line.Length - seqlen + 1;

			sorted = false;
			row_analysed_count++;

			Console.WriteLine("sequences possible:" + sequences_possible);

			if (sequences_possible >= 1) {

				// loop over "first_cell" of each sequence possible in row.
				for (int first_cell = 0; first_cell < sequences_possible; first_cell++) {
					int seq_player_count = 0;
					int seq_other_count = 0;
					int seq_last_empty = -1;

					// loop over sequence and count cells owned by players & note empties
					for (int i=0; i < seqlen; i++) {
						if (line[first_cell+i] == player) {
							seq_player_count++;
						}
						else if (line[first_cell+i] == 0) {
							seq_last_empty = first_cell + i;
						}
						else {
							seq_other_count++;
						}
					}

					// create and populate RowResult object based on analysis of sequence
					row_result = new LineResult();
					row_result.index = first_cell;
					row_result.suggestion = seq_last_empty;
					row_result.rank = 0;
					row_result.position = position;
					row_result.direction = direction;


					if (seq_last_empty == -1) { // no available spaces
						row_result.rank = -1;
					}
					else if (seq_other_count == seqlen - 1) { // blocking is required or other wins
						row_result.suggestion = seq_last_empty;
						row_result.rank = 0.999f;					
					}
					else if (seq_other_count > 0) { // there is a blocker so this sequence is not valuable
						row_result.rank = (float)seq_other_count / seqlen / seqlen;
					}
					else if (seq_player_count == seqlen - 1) { // player will win
						row_result.rank = 1;
					}
					else if (seq_player_count > 0) { // percentage of player pieces in sequence
						row_result.rank = (float)seq_player_count / seqlen;
					}
					result.Add(row_result);
				}	
			}
			return this;
		}

		public LineResult best_suggestion() {
			return Sort()[0];
		}

		public string ToString(bool sorted) {
			StringBuilder sb = new StringBuilder();
			if(sorted)
				Sort();
			foreach (LineResult r in result) {
				sb.Append(r.ToString() + Environment.NewLine);
			}
			return sb.ToString();
		}

		public override string ToString() {
			return ToString(true);
		}

	}


}


