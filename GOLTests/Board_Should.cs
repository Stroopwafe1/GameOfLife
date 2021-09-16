using Model;
using NUnit.Framework;

namespace GOLTests {
	public class Tests {
		[SetUp]
		public void Setup() { }

		[Test]
		public void BoardConstructor_FromFEN_ShouldReturnSameFEN_Empty() {
			string FEN = "8/8/8/8/8/8/8/8/";
			Board board = new Board(FEN);
			string actual = board.ToString();
			
			Assert.AreEqual(FEN, actual);
		}
		
		[Test]
		public void BoardConstructor_FromFEN_ShouldReturnSameFEN_Filled() {
			string FEN = "4A3/1AA3A1/8/8/8/8/8/8/";
			Board board = new Board(FEN);
			string actual = board.ToString();

			string FEN2 = "7A/7A/7A/8/8/8/8/8/";
			Board board2 = new Board(FEN2);

			Assert.AreEqual(FEN, actual);
			Assert.AreEqual(FEN2, board2.ToString());
		}

		[Test]
		public void BoardNextState_AliveCell0Neighbours_CellShouldDie() {
			string FEN = "7A/8/8/8/8/8/8/8/";
			Board board = new Board(FEN);
			board.NextState();
			string expected = "8/8/8/8/8/8/8/8/";
			Assert.AreEqual(expected, board.ToString());
		}
		
		[Test]
		public void BoardNextState_AliveCell1Neighbour_CellShouldDie() {
			string FEN = "7A/7A/8/8/8/8/8/8/";
			Board board = new Board(FEN);
			board.NextState();
			string expected = "8/8/8/8/8/8/8/8/";
			Assert.AreEqual(expected, board.ToString());
		}
		
		[Test]
		public void BoardNextState_Cell2Neighbours_CellShouldSurvive() {
			string FEN = "7A/7A/7A/8/8/8/8/8/";
			Board board = new Board(FEN);
			board.NextState();
			string expected = "8/6AA/8/8/8/8/8/8/";
			Assert.AreEqual(expected, board.ToString());
		}
	}
}