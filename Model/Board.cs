using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model {

	public class Board {
		public Cell[,] Cells { get; set; }
		private readonly int _size;

		public event EventHandler StateChanged;

		public bool Initialised { get; private set; } = false;

		public Board(int size) {
			Cells = new Cell[size, size];
			_size = size;
		}

		public Board(string FEN) {
			string[] lines = FEN.Remove(FEN.LastIndexOf('/')).Split('/');
			_size = lines.Length;
			Cells = new Cell[_size, _size];
			for (int y = 0; y < lines.Length; y++) {
				if (!lines[y].Contains('A')) {
					for (int x = 0; x < _size; x++)
						Cells[y, x] = new Cell(x, y);
				} else {
					string[] parts = lines[y].Split('A');
					int x = 0;
					foreach (string part in parts) {
						if (!part.Equals(string.Empty)) {
							int max = int.Parse(part) + x;
							for (; x < max; x++) {
								Cells[y, x] = new Cell(x, y);
							}
						}

						if (x <  _size) {
							Cells[y, x] = new Cell(x, y, true);
							x++;
						}
					}
				}
			}
			
			// Set the neighbours
			for (int y = 0; y < _size; y++) {
				for (int x = 0; x < _size; x++) {
					Cell cell = Cells[y, x];
					Cell[] neighbours = GetNeighbours(cell);
					cell.Neighbours = neighbours;
					cell.AliveNeighbours = neighbours.Count(c => c.Alive);
				}
			}

			Initialised = true;
		}
		public void Initialise() {
			// Fill the board
			for (int y = 0; y < _size; y++) {
				for (int x = 0; x < _size; x++) {
					Cells[y, x] = new Cell(x, y);
				}
			}

			// Set the neighbours
			for (int y = 0; y < _size; y++) {
				for (int x = 0; x < _size; x++) {
					Cell cell = Cells[y, x];
					Cell[] neighbours = GetNeighbours(cell);
					cell.Neighbours = neighbours;
					cell.AliveNeighbours = neighbours.Count(c => c.Alive);
				}
			}
			Initialised = true;
		}

		/// <summary>
		/// Gets all the neighbours of cells at the initialisation. So the algorithm for running the game algorithm isn't O(n2) but O(n)
		/// </summary>
		/// <param name="cell"></param>
		/// <returns></returns>
		private Cell[] GetNeighbours(Cell cell) {
			Cell[] returnValue = new Cell[8]; // Maximum 8 cells
			/* Neighbours are
			 * {
			 *		(X-1, Y-1), (X, Y-1), (X+1, Y-1),
			 *		(X-1, Y), (X+1, Y),
			 *		(X-1, Y+1), (X, Y+1), (X+1, Y+1)
			 * }
			*/
			int counter = 0;
			for (int y = cell.Y - 1; y <= cell.Y + 1; y++) {
				for (int x = cell.X - 1; x <= cell.X + 1; x++) {
					if (x >= _size || y >= _size || x < 0 || y < 0) continue;
					if (x == cell.X && y == cell.Y) continue; // Ignore the cell itself
					returnValue[counter] = Cells[y, x];
					counter++;
				}
			}
			return returnValue.Where(c => c != null).ToArray();
		}

		public void Randomise(int seed) {
			Random random = new Random(seed);
			for (int y = 0; y < _size; y++) {
				for (int x = 0; x < _size; x++) {
					int r = random.Next(0, _size + x);
					Cells[y, x].Alive = r < 10;
				}
			}
			
			for (int y = 0; y < _size; y++) {
				for (int x = 0; x < _size; x++) {
					Cell cell = Cells[y, x];
					cell.AliveNeighbours = cell.Neighbours.Count(c => c.Alive);
				}
			}
		}

		public Cell[] GetAliveCells() {
			List<Cell> output = new List<Cell>();
			for (int y = 0; y < _size; y++) {
				for (int x = 0; x < _size; x++) {
					Cell cell = Cells[y, x];
					if (cell.Alive)
						output.Add(cell);
				}
			}
			return output.ToArray();
		}

		/// <summary>
		/// Sets the next state of the board. Complexity: O(n2)
		/// </summary>
		/// <remarks>
		/// Average speeds per board size:
		/// <list type="bullet">
		/// <item>50x50 => 0.000286 seconds</item>
		/// <item>75x75 => 0.000623 seconds</item>
		/// <item>100x100 => 0.00112 seconds</item>
		/// <item>500x500 => 0.0305 seconds</item>
		/// <item>1000x1000 => 0.133244 seconds</item>
		/// </list>
		/// </remarks>
		public void NextState() {
			// TODO: Fix the next state because it goes from top to bottom, so a cell below should stay alive if the cell above has already been marked dead
			for (int y = 0; y < _size; y++) {
				for (int x = 0; x < _size; x++) {
					Cell cell = Cells[y, x];
					cell.Alive = cell.GetNextState();
				}
			}
			
			for (int y = 0; y < _size; y++) {
				for (int x = 0; x < _size; x++) {
					Cell cell = Cells[y, x];
					cell.AliveNeighbours = cell.Neighbours.Count(c => c.Alive);
				}
			}
			StateChanged?.Invoke(this, EventArgs.Empty);
		}

		private string GenerateFEN() {
			StringBuilder sb = new StringBuilder();
			for (int y = 0; y < _size; y++) {
				int count = 0;
				for (int x = 0; x < _size; x++) {
					if (Cells[y, x].Alive) {
						if (count != 0)
							sb.Append(count);
						sb.Append('A');
						count = 0;
					} else {
						count++;
					}
				}
				if (count != 0)
					sb.Append(count);
				sb.Append('/');
			}
			return sb.ToString();
		}

		public int GetSize() {
			return _size;
		}

		public override string ToString() {
			return GenerateFEN();
		}
	}
}