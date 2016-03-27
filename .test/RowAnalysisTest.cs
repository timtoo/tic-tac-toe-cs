using System;

using NUnit.Framework;
using TicTacToeLib;

namespace TicTacToeTest
{
	[TestFixture]
	public class RowAnalysisTest
	{

		[Test]
		public void TestAnalysis() {
			int[] row = { 1, 1, 0 };

			RowAnalysis r = new RowAnalysis(1, 3);
			r.analyse(row,0,0);
			//Console.WriteLine(r.ToString());
			Assert.That(r.result[0].index, Is.EqualTo(0));
			Assert.That(r.result[0].suggestion, Is.EqualTo(2));
			Assert.That(r.result[0].rank, Is.EqualTo(1));
			Assert.That(r.result[0].position, Is.EqualTo(0));
			Assert.That(r.result[0].direction, Is.EqualTo(0));

			r = new RowAnalysis(2, 3);
			r.analyse(row,1,90);
			Console.WriteLine(r.ToString());
			Assert.That(r.result[0].index, Is.EqualTo(0));
			Assert.That(r.result[0].suggestion, Is.EqualTo(2));
			Assert.That(r.result[0].rank, Is.EqualTo(0.999f));
			Assert.That(r.result[0].position, Is.EqualTo(1));
			Assert.That(r.result[0].direction, Is.EqualTo(90));


		}


	}


}
