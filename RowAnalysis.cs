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
		private int[] row;
		private int seqlen;

		// player number, row array, length of win sequence
		public RowAnalysis(int player, int[] row, int seqlen) {
			this.player = player;
			this.row = row;
			this.seqlen = seqlen;
			this.result = new List<RowResult>();
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

					if (seq_other_count == seqlen - 1) { // blocking is required or other wins
						rr.suggestion = seq_last_empty;
						rr.rank = 0.999f;					
					}
					else if (seq_other_count > 0) { // there is a blocker so all is useless in this row?
						rr.rank = 0;
					}
					else if (seq_player_count == seqlen - 1) { // player will win
						rr.rank = 1;
					}
					else if (seq_player_count > 0) { // percentage of player pieces in sequence
						rr.rank = seq_player_count / seqlen;
					}
					result.Add(rr);
				}
			}

		}

		public RowResult best_suggestion() {
			result.Sort(order);				
			return result[0];
		}

		public override string ToString() {
			result.Sort(order);
			StringBuilder sb = new StringBuilder();
			foreach (RowResult r in result) {
				sb.Append(string.Format("<RowResult: rank={0} index={1} move={2}>{3}", r.rank, r.index, r.suggestion, Environment.NewLine));
			}
			return sb.ToString();
		}
	}


}


