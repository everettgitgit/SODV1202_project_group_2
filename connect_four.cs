using System;

namespace ConnectFourGame
{
    public class move
    {
    public int Index { get; set; }
    public int Score { get; set; }

    public move(int index, int score)
    {
        Index = index;
        Score = score;
    }
}
    
    public class Board
    {
        public const int Rows = 6;
        public const int Columns = 7;
        private string[,] grid = new string[Rows, Columns];

        public string currentState { get; protected set; }
        public List<int> indexRecord { get; protected set; }
        
        public Board()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    grid[i, j] = " ";
                }
            }

            currentState = startState;
            indexRecord = startRecord;
        }

        protected void UpdateCurrentState(string State)
        {
            currentState = State;
        }

        protected void PushNewIndex(int index)
        {
            indexRecord.Add(index);
        }

        public override string ToString()
        {
            return currentState;
        }

        public int CheckMove(int column)
        {
            if (column < 0 || column >= COLUMNS)
            {
                return -1; // Column out of bounds
            }

            int columnIndex = column * 6;
            string columnCheck = currentState.Substring(columnIndex, 6);
    
            if (columnCheck.Contains("-"))
            {
                return columnIndex + columnCheck.IndexOf('-');
            }

            return -1;
        }

        public bool PlaceSymbol(int column, string symbol)
        {
            if (column < 0 || column >= Columns)
            {
                return false; // Column out of bounds
            }

            for (int i = Rows - 1; i >= 0; i--)
            {
                if (grid[i, column] == " ")
                {
                    grid[i, column] = symbol;
                    return true;
                }
            }

            return false; // Column is full
        }

        public bool CheckForWin(string symbol)
        {
            // Check horizontal, vertical, and diagonal wins
            return CheckHorizontalWin(symbol) || CheckVerticalWin(symbol) || CheckDiagonalWin(symbol);
        }

        private bool CheckHorizontalWin(string symbol)
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns - 3; j++)
                {
                    if (grid[i, j] == symbol && grid[i, j + 1] == symbol && grid[i, j + 2] == symbol && grid[i, j + 3] == symbol)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool CheckVerticalWin(string symbol)
        {
            for (int i = 0; i < Rows - 3; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    if (grid[i, j] == symbol && grid[i + 1, j] == symbol && grid[i + 2, j] == symbol && grid[i + 3, j] == symbol)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool CheckDiagonalWin(string symbol)
        {
            // Check diagonals
            for (int i = 3; i < Rows; i++)
            {
                for (int j = 0; j < Columns - 3; j++)
                {
                    if (grid[i, j] == symbol && grid[i - 1, j + 1] == symbol && grid[i - 2, j + 2] == symbol && grid[i - 3, j + 3] == symbol)
                    {
                        return true;
                    }
                }
            }

            for (int i = 3; i < Rows; i++)
            {
                for (int j = 3; j < Columns; j++)
                {
                    if (grid[i, j] == symbol && grid[i - 1, j - 1] == symbol && grid[i - 2, j - 2] == symbol && grid[i - 3, j - 3] == symbol)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void Display()
        {
            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    Console.Write($"|{grid[i, j]}");
                }
                Console.WriteLine("|");
            }
            Console.WriteLine(new string('-', Columns * 2));
        }

        public bool IsFull()
        {
            for (int j = 0; j < Columns; j++)
            {
                if (grid[0, j] == " ")
                {
                    return false;
                }
            }
            return true;
        }

        public bool CanPlaceSymbol(int column)
        {
            return grid[0, column] == " ";
        }

        public void RemoveSymbol(int column)
        {
            for (int i = 0; i < Rows; i++)
            {
                if (grid[i, column] != " ")
                {
                    grid[i, column] = " ";
                    break;
                }
            }
        }
    }

    public class TestableBoard : Board
    {
        public int[] middleToOut { get; private set; }

        public TestableBoard(string state, List<int> indexRecord) : base(state, indexRecord) 
        {
            middleToOut = new int[] { 2, 3, 1, 4, 0, 5 };
        }
        
        public int getRows()
        {
            return ROWS;
        }

        public int getColumns()
        {
            return COLUMNS;
        }
        
        private void RemoveIndex(int index)
        {
            indexRecord.Remove(index);
        }
    
        public void RemoveSymbol(int index)
        {
            char[] changeState = currentState.ToCharArray();
            changeState[index] = '-';

            UpdateCurrentState(new string(changeState));
            RemoveIndex(index);
        }

        public int EvalCurrentState(string state, int index, char symbol)
        {
            int horizonalEval = EvaluateHorizonal(state, index, symbol);
            int verticalEval = EvaluateVertical(state, index, symbol);
            int diagonalUpDownEval = EvaluateDiagonalUpDown(state, index, symbol);
            int diagonalDownUpEval = EvaluateDiagonalDownUp(state, index, symbol);

            if (horizonalEval == 1 && verticalEval == 1 && diagonalUpDownEval == 1 && diagonalDownUpEval == 1)
            {
                return 0;
            }

            int result = 0;
            result = Math.Max(horizonalEval, verticalEval);
            result = Math.Max(result, diagonalUpDownEval);
            result = Math.Max(result, diagonalDownUpEval);

            return result;
        }
    }

    public abstract class Player
    {
        public string Symbol { get; protected set; }

        protected Player(string symbol)
        {
            Symbol = symbol;
        }

        public abstract int ChooseColumn(Board board);
    }

    public class HumanPlayer : Player
    {
        public HumanPlayer(string symbol) : base(symbol) { }

        public override int ChooseColumn(Board board)
        {
            Console.Write($"Player {Symbol}, choose a column (1-{Board.Columns}): ");
            if (int.TryParse(Console.ReadLine(), out int column))
            {
                return column - 1; // Adjust for zero-based indexing
            }
            else
            {
                return -1; // Invalid input
            }
        }
    }

    public class ComputerPlayer : Player
    {
        public ComputerPlayer(string symbol) : base(symbol) { }

        public override int ChooseColumn(Board board)
        {
            // Implement AI Logic here
            // random column
            Random rnd = new Random();
            int col;
            do
            {
                col = rnd.Next(Board.Columns);
            } while (!board.CanPlaceSymbol(col));
            return col;
        }
    }

    public class Game
    {
        private Board board = new Board();
        private Player[] players = new Player[2];
        private int currentPlayerIndex = 0;

        public Game(bool playAgainstComputer)
        {
            players[0] = new HumanPlayer("X");
            players[1] = playAgainstComputer ? (Player)new ComputerPlayer("O") : new HumanPlayer("O");
        }

        public void Start()
        {
            bool gameEnded = false;
            while (!board.IsFull() && !gameEnded)
            {
                board.Display();
                Player currentPlayer = players[currentPlayerIndex];
                bool validMove = false;
                int columnChoice = -1;

                while (!validMove)
                {
                    columnChoice = currentPlayer.ChooseColumn(board);
                    validMove = board.PlaceSymbol(columnChoice, currentPlayer.Symbol);
                    if (!validMove)
                    {
                        Console.WriteLine("Invalid move, try again.");
                    }
                }

                gameEnded = board.CheckForWin(currentPlayer.Symbol);

                if (gameEnded)
                {
                    board.Display();
                    Console.WriteLine($"Player {currentPlayer.Symbol} wins!");
                }
                else if (board.IsFull())
                {
                    Console.WriteLine("The game is a draw.");
                    break;
                }

                currentPlayerIndex = (currentPlayerIndex + 1) % 2;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            bool playAgain = true;

            while (playAgain)
            {
                Console.Clear();
                Console.WriteLine("Welcome to Connect Four!");
                Console.WriteLine("Play against the computer? (yes/no)");
                string input = Console.ReadLine().Trim().ToLower();
                bool playAgainstComputer = input.StartsWith("y");

                Game game = new Game(playAgainstComputer);
                game.Start();

                // Ask if the player wants to play again
                Console.WriteLine("Do you want to play again? (yes/no)");
                string playAgainInput = Console.ReadLine().Trim().ToLower();
                playAgain = playAgainInput.StartsWith("y");
            }
        }
    }
}
