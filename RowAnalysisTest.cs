using System;

using NUnit.Framework;

namespace TicTacToe
{
	[TestFixture]
	public class RowAnalysisTest
	{

		[Test]
		public void TestAnalysis() {
			int[] row = { 1, 1, 0 };
			RowAnalysis r = new RowAnalysis(1, row, 3);
			r.analyse();
			//Console.WriteLine(r.ToString());
			Assert.That(r.result[0].index, Is.EqualTo(0));
			Assert.That(r.result[0].suggestion, Is.EqualTo(2));
			Assert.That(r.result[0].rank, Is.EqualTo(1));

			r = new RowAnalysis(2, row, 3);
			r.analyse();
			Console.WriteLine(r.ToString());
			Assert.That(r.result[0].index, Is.EqualTo(0));
			Assert.That(r.result[0].suggestion, Is.EqualTo(2));
			Assert.That(r.result[0].rank, Is.EqualTo(0.999f));


		}


	}


}
