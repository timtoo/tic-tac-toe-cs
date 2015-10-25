using System;

using NUnit.Framework;

namespace TicTacToe
{
	[TestFixture]
	public class TicTacToeTest
	{
		[Test]
		public void TestTicTacToe()
		{
			// test default object size
			TicTacToe t = new TicTacToe();
			Assert.That(3, Is.EqualTo(t.width));
			Assert.That(3, Is.EqualTo(t.height));
			Assert.That(1, Is.EqualTo(t.player_turn));

			t = new TicTacToe(5, 4);
			Assert.That(5, Is.EqualTo(t.width));
			Assert.That(4, Is.EqualTo(t.height));
			Assert.That(1, Is.EqualTo(t.player_turn));
		}

		[Test]
		public void TestPlace() {
			TicTacToe t = new TicTacToe();

			string s = t.ToString();
			Assert.AreEqual("...\n...\n...", s);
			Assert.AreEqual(1, t.player_turn);

			// test x,y place
			t.place(2,0); 
			Assert.AreEqual("..X\n...\n...", t.ToString());
			Assert.AreEqual(2, t.player_turn);

			// test x,y place
			t.place(3); 
			Assert.AreEqual("..X\nO..\n...", t.ToString());
			Assert.AreEqual(1, t.player_turn);

			// test x,y place
			t.place(8); 
			Assert.AreEqual("..X\nO..\n..X", t.ToString());
			Assert.AreEqual(2, t.player_turn);

		}

		[Test]
		public void TestParseBoard() {
			TicTacToe t;
			t = new TicTacToe(".........");
			Assert.AreEqual("...\n...\n...", t.ToString());
			Assert.AreEqual(3, t.width);
			Assert.AreEqual(3, t.height);
			Assert.AreEqual(0, t.turns);
			Assert.AreEqual(1, t.player_turn);


			t = new TicTacToe("..X.O.X..");
			Assert.AreEqual("..X\n.O.\nX..", t.ToString());
			Assert.AreEqual(3, t.width);
			Assert.AreEqual(3, t.height);
			Assert.AreEqual(3, t.turns);
			Assert.AreEqual(2, t.player_turn);

			t = new TicTacToe("..............X.");

			Assert.AreEqual("....\n....\n....\n..X.", t.ToString());
			Assert.AreEqual(4, t.width);
			Assert.AreEqual(4, t.height);
			Assert.AreEqual(1, t.turns);
			Assert.AreEqual(2, t.player_turn);

			t = new TicTacToe("....\n....\n....\n..X.\n....");
			Assert.AreEqual("....\n....\n....\n..X.\n....", t.ToString());
			Assert.AreEqual(4, t.width);
			Assert.AreEqual(5, t.height);
			Assert.AreEqual(1, t.turns);
			Assert.AreEqual(2, t.player_turn);
		}

		[Test]
		public void TestParseBoard2() {
			Assert.That(() => new TicTacToe("........"), Throws.TypeOf<ArgumentException>());
		}

		[Test]
		public void TestExtractHorizontal() {
			TicTacToe t = new TicTacToe("XOX\nOXO\nXXO");

			int[] tr1 = { 1, 2, 1 };
			CollectionAssert.AreEqual(tr1, t.extract_row(0));
			CollectionAssert.AreEqual(tr1, t.extract_row(1));
			CollectionAssert.AreEqual(tr1, t.extract_row(2));

			int[] tr2 = { 2, 1, 2 };
			CollectionAssert.AreEqual(tr2, t.extract_row(3));
			CollectionAssert.AreEqual(tr2, t.extract_row(4));
			CollectionAssert.AreEqual(tr2, t.extract_row(5));

			TicTacToe t2 = new TicTacToe("XOXO\nOXOX\nXXOO\nOOXX\nXOOX");
			int[] t2r1 = { 1, 1, 2, 2 };
			CollectionAssert.AreEqual(t2r1, t2.extract_row(9));

		}
			
		[Test]
		public void TestExtractRow2() {
			TicTacToe t = new TicTacToe("XOX\nOXO\nXXO");
			Assert.That(() => t.extract_row(9), Throws.TypeOf<IndexOutOfRangeException>());
		}

		[Test]
		public void TestExtractVertical() {
			TicTacToe t = new TicTacToe("XOX\nOXO\nXXO");

			int[] tr1 = { 1, 2, 1 };
			CollectionAssert.AreEqual(tr1, t.extract_column(0));
			CollectionAssert.AreEqual(tr1, t.extract_column(3));
			CollectionAssert.AreEqual(tr1, t.extract_column(6));

			int[] tr2 = { 2, 1, 1 };
			CollectionAssert.AreEqual(tr2, t.extract_column(1));
			CollectionAssert.AreEqual(tr2, t.extract_column(4));
			CollectionAssert.AreEqual(tr2, t.extract_column(7));

			TicTacToe t2 = new TicTacToe("XOXO\n" + 
										 "OXOX\n" + 
										 "XXOO\n" + 
										 "OOXX\n" + 
				 						 "XOOX");
			int[] t2r1 = { 2, 1, 1, 2, 2 };
			CollectionAssert.AreEqual(t2r1, t2.extract_column(9));

		}

		[Test]
		public void TestExtractColumn2() {
			TicTacToe t = new TicTacToe("XOX\nOXO\nXXO");
			Assert.That(() => t.extract_row(9), Throws.TypeOf<IndexOutOfRangeException>());
		}

		[Test]
		public void TestExtractDiagD() {
			TicTacToe t = new TicTacToe("XOX\nOXO\nXXO");

			// XOX
			// OXO
			// XXO

			int[] tr1 = { 1, 1, 2 };
			CollectionAssert.AreEqual(tr1, t.extract_diagonal_down(0));
			CollectionAssert.AreEqual(tr1, t.extract_diagonal_down(4));
			CollectionAssert.AreEqual(tr1, t.extract_diagonal_down(8));

			int[] tr2 = { 2, 2 };
			int[] ta2 = t.extract_diagonal_down(1);
			CollectionAssert.AreEqual(tr2, ta2);
			CollectionAssert.AreEqual(tr2, t.extract_diagonal_down(5));

			int[] tr3 = { 2, 1 };
			CollectionAssert.AreEqual(tr3, t.extract_diagonal_down(3));
			CollectionAssert.AreEqual(tr3, t.extract_diagonal_down(7));

			TicTacToe t2 = new TicTacToe(
				"XOXO\n" + 
				"OXOX\n" + 
				"XXOO\n" + 
				"OOXX\n" + 
				"XOOX");

			int[] t2r1 = { 2, 1, 1, 1 };
			CollectionAssert.AreEqual(t2r1, t2.extract_diagonal_down(9));

			int[] t2r2 = { 2, 2 };
			CollectionAssert.AreEqual(t2r2, t2.extract_diagonal_down(12));

			int[] t2r3 = { 2, 2, 2 };
			CollectionAssert.AreEqual(t2r3, t2.extract_diagonal_down(1));

			TicTacToe t3 = new TicTacToe(
				               "XOXOXO\n" +
				               "OXOXOX\n" +
				               "XXOOXO\n");

			int[] t3r1 = { 1, 1, 1 };
			CollectionAssert.AreEqual(t3r1, t3.extract_diagonal_down(9));

			int[] t3r2 = { 1 };
			CollectionAssert.AreEqual(t3r2, t3.extract_diagonal_down(12));

			int[] t3r3 = { 1, 1 };
			CollectionAssert.AreEqual(t3r3, t3.extract_diagonal_down(4));
		}

		[Test]
		public void TestExtractDiagU() {
			TicTacToe t = new TicTacToe("XOX\nOXO\nXXO");

			// XOX
			// OXO
			// XXO

			int[] tr1 = { 1, 1, 1 };
			CollectionAssert.AreEqual(tr1, t.extract_diagonal_up(6));
			CollectionAssert.AreEqual(tr1, t.extract_diagonal_up(4));
			CollectionAssert.AreEqual(tr1, t.extract_diagonal_up(2));

			int[] tr2 = { 1, 2 };
			int[] ta2 = t.extract_diagonal_up(7); // 1, 2
			CollectionAssert.AreEqual(tr2, ta2);
			CollectionAssert.AreEqual(tr2, t.extract_diagonal_up(5));

			int[] tr3 = { 2, 2 };
			CollectionAssert.AreEqual(tr3, t.extract_diagonal_up(3));
			CollectionAssert.AreEqual(tr3, t.extract_diagonal_up(1));

			TicTacToe t2 = new TicTacToe(
				"XOXO\n" + 
				"OXOX\n" + 
				"XXOO\n" + 
				"OOXX\n" + 
				"XOOX");

			int[] t2r1 = { 2, 1, 2, 2 };
			CollectionAssert.AreEqual(t2r1, t2.extract_diagonal_up(9));

			int[] t2r2 = { 2, 1 };
			CollectionAssert.AreEqual(t2r2, t2.extract_diagonal_up(18));

			int[] t2r3 = { 1, 1, 1 };
			CollectionAssert.AreEqual(t2r3, t2.extract_diagonal_up(8));

			TicTacToe t3 = new TicTacToe(
				"XOXOXO\n" +
				"OXOXOX\n" +
				"XXOOXO\n");

			int[] t3r1 = { 2, 1, 1 };
			CollectionAssert.AreEqual(t3r1, t3.extract_diagonal_up(9)); // 4, 1

			int[] t3r2 = { 1 };
			CollectionAssert.AreEqual(t3r2, t3.extract_diagonal_up(0));

			int[] t3r3 = { 1, 1 };
			CollectionAssert.AreEqual(t3r3, t3.extract_diagonal_up(16)); // 5, 2
		}

	}



}