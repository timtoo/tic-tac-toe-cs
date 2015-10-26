using System;
using System.Collections.Generic;
using System.Text;

namespace TicTacToe
{
	/// <summary>
	/// Row result struct. Holds result of a row analysis.
	/// </summary>
	public struct RowResult {
		public float rank; // 0 to 1
		public int index;  // position on board
		public int suggestion; // suggested empty board space to take
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
		public List<RowResult> result {get;}
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
			this.result = new List<RowResult>();
		}

		public static int order(RowResult x, RowResult y) {
			// reverse sort based on rank
			if (y.rank > x.rank) return -1;
			if (x.rank < y.rank) return 1;
			return 0;
		}

		/// <summary>
		/// Sort this instance, highest rank first. Returns the RowResult list.
		/// </summary>
		public List<RowResult> Sort() {
			if(sorted == false) {
				result.Sort(order);
				sorted = true;
			}
			return result;
		}

		/// <summary>
		/// Analyse this instance. Return list of results (which are also stored in the object).
		/// </summary>
		public List<RowResult> analyse(int[] row) {
			RowResult row_result;
			int sequences_possible = row.Length - seqlen + 1;

			sorted = false;
			row_analysed_count++;

			Console.WriteLine("sp:" + sequences_possible);

			if (sequences_possible >= 1) {
				for (int first_cell = 0; first_cell < sequences_possible; first_cell++) {
					int seq_player_count = 0;
					int seq_other_count = 0;
					int seq_last_empty = -1;
					for (int i=0; i < seqlen; i++) {
						if (row[first_cell+i] == player) {
							seq_player_count++;
						}
						else if (row[first_cell+i] == 0) {
							seq_last_empty = first_cell + i;
						}
						else {
							seq_other_count++;
						}
					}

					row_result = new RowResult();
					row_result.index = first_cell;
					row_result.suggestion = seq_last_empty;
					row_result.rank = 0;

					if (seq_other_count == seqlen - 1) { // blocking is required or other wins
						row_result.suggestion = seq_last_empty;
						row_result.rank = 0.999f;					
					}
					else if (seq_other_count > 0) { // there is a blocker so all is useless in this row?
						row_result.rank = 0;
					}
					else if (seq_player_count == seqlen - 1) { // player will win
						row_result.rank = 1;
					}
					else if (seq_player_count > 0) { // percentage of player pieces in sequence
						row_result.rank = seq_player_count / seqlen;
					}
					result.Add(row_result);
				}
			
			}

			return result;

		}

		public RowResult best_suggestion() {
			return Sort()[0];
		}

		public override string ToString() {
			StringBuilder sb = new StringBuilder();
			foreach (RowResult r in Sort()) {
				sb.Append(string.Format("<RowResult: rank={0} index={1} move={2}>{3}", r.rank, r.index, r.suggestion, Environment.NewLine));
			}
			return sb.ToString();
		}
	}


}


