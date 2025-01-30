namespace TaskForTg.Models
{
    public class Game
    {
        public Guid game_id { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int MinesCount { get; set; }
        public bool Completed { get; set; }
        public char[,] Field { get; set; }
        private bool[,] Mines { get; }
        private bool[,] Checked { get; }

        public Game(int w, int h, int mines)
        {
            Width = w;
            Height = h;
            MinesCount = mines;
            Completed = false;
            Field = new char[w, h];
            Checked  = new bool[w, h];
            Mines = new bool[w, h];
            game_id = Guid.NewGuid();
            FillInField();
            FillInMines();
        }
        private void FillInField()
        {
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    Field[i, j] = ' ';
                }
            }
        }
        private void FillInMines()
        {
            int mines_on_field = 0;
            var rnd = new Random();

            while (mines_on_field < MinesCount)
            {
                int x = rnd.Next(Width);
                int y = rnd.Next(Height);
                if (!Mines[x, y])
                {
                    Mines[x, y] = true;
                    mines_on_field++;
                }
            }
        }

        public void ShowCell(int row, int col)
        {
            if (this.Completed || this.Checked[row, col]) throw new InvalidOperationException("Недоступный ход");

            if (Mines[row, col])
            {
                this.Completed = true;
                ShowAllMines('X');
            }
            else
            {
                OpenCells(row, col);
                if (CheckWin())
                {
                    Completed = true;
                    ShowAllMines('M');
                }
            }
        }

        private void OpenCells(int row, int col)
        {
            var queue = new Queue<(int, int)>();
            queue.Enqueue((row, col));
            while (queue.Count > 0)
            {
                var (r, c) = queue.Dequeue();
                if (Checked[r, c]) continue; 
                Checked[r, c] = true;
                int mineCount = CountNearMines(r, c);
                Field[r, c] = mineCount == 0 ? '0' : (char)('0' + mineCount);
                if (mineCount == 0)
                {
                    foreach (var (nr, nc) in GetNeighbors(r, c))
                    {
                        if (!Checked[nr, nc]) queue.Enqueue((nr, nc));
                    }
                }
            }
        }

        private int CountNearMines(int row, int col) =>
            GetNeighbors(row, col).Count(n => Mines[n.row, n.col]);

        private List<(int row, int col)> GetNeighbors(int row, int col)
        {
            var neighbors = new List<(int, int)>();

            for (int dr = -1; dr <= 1; dr++)
            {
                for (int dc = -1; dc <= 1; dc++)
                {
                    if (dr == 0 && dc == 0) continue;
                    int newRow = row + dr;
                    int newCol = col + dc;
                    if (newRow >= 0 && newCol >= 0 && newRow < Height && newCol < Width) neighbors.Add((newRow, newCol));
                }
            }
            return neighbors;          
        }

        private bool CheckWin()
        {
            return (Checked.Cast<bool>().Count(r => r) == Width * Height - MinesCount);
        }

        private void ShowAllMines(char symbol)
        {
            for (int i = 0; i < Height; i++)
                for (int j = 0; j < Width; j++)
                    if (Mines[i, j]) Field[i, j] = symbol;
        }

    }
}
