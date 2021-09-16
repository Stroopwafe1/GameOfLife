using System.Linq;

namespace Model {

	public class Cell {
		public bool Alive { get; set; }
		public int X { get; set; }
		public int Y { get; set; }
		public Cell[] Neighbours { get; set; }
		public int AliveNeighbours { get; set; } = -1;

		public Cell(int x, int y, bool alive = false) {
			X = x;
			Y = y;
			Alive = alive;
		}

		public bool GetNextState() {
			if (AliveNeighbours == -1)
				AliveNeighbours = Neighbours.Count(c => c.Alive);

			// 4. Any dead cell with exactly three live neighbours becomes a live cell, as if by reproduction.
			if (!Alive)
				return AliveNeighbours == 3;

			// For any living cell, it survives if it has 2 or 3 neighbours. Any other situation, the cell dies (=> Next state = false)
			return AliveNeighbours == 2 || AliveNeighbours == 3;
		}

		public override string ToString() {
			return $"({X}, {Y}): " + (Alive ? "Alive" : "Dead");
		}
	}
}