using System;
using System.Collections.Generic;

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

        public string currentState { get; protected set; }
        public List<int> indexRecord { get; protected set; }
        
        public Board()
        {
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

        public void PlaceSymbol(int index, char symbol)
        {
            char[] changeState = currentState.ToCharArray();            // set state to char array for checking
            changeState[index] = symbol;                                // change symbol
            UpdateCurrentState(new string(changeState));                // update state
            PushNewIndex(index);
        }

        public bool CheckForWin(string state, int index, char symbol)
        {
            // Check horizontal, vertical, and diagonal wins
            int horizonalEval = EvaluateHorizonal(state, index, symbol);
            int verticalEval = EvaluateVertical(state, index, symbol);
            int diagonalUpDownEval = EvaluateDiagonalUpDown(state, index, symbol);
            int diagonalDownUpEval = EvaluateDiagonalDownUp(state, index, symbol);

            if (horizonalEval == 4 || verticalEval == 4 || diagonalDownUpEval == 4 || diagonalUpDownEval == 4)      // if any evalutions returns a 4, return a winning state
            {
                return true;
            }

            return false;
        }

        protected int EvaluateHorizonal(string state, int index, char symbol)
        {
            bool before = true;
            bool after = true;
            int count = 1;
    
            for (int i = 1; i < 4; i++) 
            {
                if (index - (i * 6) < 0)                        // if next left check is out of index, do not check further
                {
                    before = false;
                }

                if (index + (i * 6) > MAX_INDEX)                // if next right check is out of index, do not check further
                {
                    after = false;
                }

                if (!before && !after)                          // if there are no connecting symbols on either side, break from check and continue
                {
                    return count;
                }

                if (before)                                     // begin left side checking
                {
                    if (state[index - (i * 6)] == symbol)       // if matching symbol found, increase current count
                    {
                        count++;
                    }
                    else                                        // if non matching symbol found, do not check further
                    {
                        before = false;
                    }
                }

                if (after)                                      // begin right side checking
                {
                    if (state[index + (i * 6)] == symbol)       // if matching symbol found, increase current count
                    {
                        count++;
                    }
                    else                                        // if non matching symbol found, do not check further
                    {
                        after = false;
                    }
                }
            }

            return count;                                       // if for some reason no offical count was returned, return the current count    
        }

        protected int EvaluateVertical(string state, int index, char symbol)
        {
            int BEFORE_CHECK = index - (index % 6);
            int AFTER_CHECK = index - (index % 6) + 5;

            bool before = true;
            bool after = true;
            int count = 1;

            for (int i = 1; i < 4; i++)
            {
                if (index - i < BEFORE_CHECK)                   // if next below check is out of index, do not check further
                {
                    before = false;
                }

                if (index + i > AFTER_CHECK)                    // if next above check is out of index, do not check further
                {        
                    after = false;
                }

                if (!before && !after)                          // if there are no connecting symbols on either side, break from check and continue
                {
                    return count;
                }

                if (before)                                     // begin left side checking
                {
                    if (state[index - i] == symbol)             // if matching symbol found, increase current count
                    {
                        count++;
                    }
                    else                                        // if non matching symbol found, do not check further
                    {
                        before = false;
                    }
                }
    
                if (after)                                      // begin right side checking
                {
                    if (state[index + i] == symbol)             // if matching symbol found, increase current count
                    {
                        count++;
                    }
                    else                                        // if non matching symbol found, do not check further
                    {
                        after = false;
                    }
                }
            }    

            return count;                                       // if for some reason no offical count was returned, return the current count
        }

        protected int EvaluateDiagonalUpDown(string state, int index, char symbol)
        {
            int BEFORE_CHECK = index - 25 + (5 * (index % 6));
            int AFTER_CHECK = index + (5 * (index % 6));

            bool before = true;
            bool after = true;
            int count = 1;

            for (int i = 1; i < 4; i++)
            {
                if (index - (5 * i) < 0 || index - (5 * i) < BEFORE_CHECK)              // if next upper left check is out of index, do not check further
                {
                    before = false;
                }

                if (index + (5 * i) > MAX_INDEX || index + (5 * i) > AFTER_CHECK)       // if next lower right check is out of index, do not check further
                {
                    after = false;
                }

                if (!before && !after)                                                  // if there are no connecting symbols on either side, break from check and continue
                {
                    return count;
                }

                if (before)                                                             // begin upper left side checking
                {
                    if (state[index - (5 * i)] == symbol)                               // if matching symbol found, increase current count
                    {
                        count++;
                    }
                    else                                                                // if non matching symbol found, do not check further
                    {
                        before = false;
                    }
                }

                if (after)                                                              // begin lower right side checking
                {
                    if (state[index + (5 * i)] == symbol)                               // if matching symbol found, increase current count
                    {
                        count++;
                    }
                    else                                                                // if non matching symbol found, do not check further
                    {
                        after = false;
                    }
                }
            }

            return count;                                                               // if for some reason no offical count was returned, return the current count
        }

        protected int EvaluateDiagonalDownUp(string state, int index, char symbol)
        {
            int BEFORE_CHECK = index - (7 * (index % 6));
            int AFTER_CHECK = index + 35 - (7 * (index % 6));

            bool before = true;
            bool after = true;
            int count = 1;

            for (int i = 1; i < 4; i++)
            {
                if (index - (7 * i) < 0 || index - (7 * i) < BEFORE_CHECK)              // if next lower left check is out of index, do not check further
                {
                    before = false;
                }

                if (index + (7 * i) > MAX_INDEX || index + (7 * i) > AFTER_CHECK)       // if next upper right check is out of index, do not check further
                {
                    after = false;
                }

                if (!before && !after)                                                  // if there are no connecting symbols on either side, break from check and continue
                {
                    return count;
                }

                if (before)                                                             // begin lower left side checking
                {
                    if (state[index - (7 * i)] == symbol)                               // if matching symbol found, increase current count
                    {
                        count++;
                    }
                    else                                                                // if non matching symbol found, do not check further
                    {
                        before = false;
                    }
                }

                if (after)                                                              // begin upper right side checking
                {
                    if (state[index + (7 * i)] == symbol)                               // if matching symbol found, increase current count
                    {
                        count++;
                    }               
                    else                                                                // if non matching symbol found, do not check further
                    {
                        after = false;
                    }
                }
            }

            return count;                                                               // if for some reason no offical count was returned, return the current count
        }

        public void Display()
        {
            char[] displayBoard = currentState.Replace('-', ' ').ToCharArray();

            Console.Clear();
            Console.WriteLine("");

            for (int i = ROWS - 1; i >= 0; i--)
            {
                for (int j = 0; j < COLUMNS; j++)
                {    
                    Console.Write($"|{displayBoard[i + (j * 6)]}");
                }
                Console.WriteLine("|");
            }

            for (int i = 1; i <= COLUMNS; i++)
            {
                Console.Write(" {0}", i);
            }

            Console.WriteLine("\n");
        }

        public bool IsFull()
        {
            if (currentState.Contains("-"))
            {
                return false;
            }

            return true;
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

        protected Player(char symbol)
        {
            Symbol = symbol;
        }

        public abstract int ChooseColumn(Board board);
    }

    public class HumanPlayer : Player
    {
        public HumanPlayer(char symbol) : base(symbol) { }

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
        public ComputerPlayer(char symbol) : base(symbol) { }

        public override int ChooseColumn(Board board)
        {
            TestableBoard testBoard = new TestableBoard(board.currentState, board.indexRecord);         // initialize a new test board to access test methods and avoid altering the actual game board

            List<move> moves = new List<move>();

            int depth = 7;                                                                              // ai search depth kept at 7 as program may crash with higher depths

            for (int i = 0; i < testBoard.getColumns() - 1; i++)                                        // search for all initial possible moves
            {
                if (testBoard.CheckMove(i) >= 0)
                {
                    moves.Add(new move(i, 0));
                }
            }

            int finalEval = Search(testBoard, moves, depth, -5, 5, 'O', true);                          // inital call to search function with current depth, range of [-5, 5], and with score setting true

            List<int> optimalColumns = new List<int>();

            for (int i = 0; i < moves.Count - 1; i++)                                                   // loop through all initial possible moves
            {
                if (testBoard.indexRecord.Count < 4 && (i == 0 || i == 1 || i == 5 || i == 6))          // if less than 4 moves have been played, dont play on the edges
                {
                    continue;
                }
                
                if (moves[i].Score == finalEval)
                {
                    optimalColumns.Add(moves[i].Index);
                }
            }

            if (optimalColumns.Count > 1)                                                               // if multiple positions were found with an equal score, pick a random one out of them
            {
                Random rand = new Random();
                int r = rand.Next(optimalColumns.Count);
                return optimalColumns[r];
            }

            return optimalColumns[0];                                                                   // if only one position was found, return that position
        }

        public int Search(TestableBoard board, List<move> moveList, int depth, int alpha, int beta, char currentPlayer, bool setScore)
        {
            /*
             
            This is the main method that handles the ai

            Implements a version that mixes Minimax and Negamax features with low level alpha-beta pruning

            -- params --

            board - holds the current test board
            
            moveList - holds the initial moves to check - setScore says whether or not those values are currently in scope to store
            
            depth - tells the search method the limit to which it recursively call itself
            
            alpha and beta - hold a range of values in which to search the current scope for. Simply put, if a searched value is outside of that scope, we 'prune' that branch of the search to reduce searched conditions
            
            current player - stores the symbol of the player who is being teseted

            setScore - tells search method if it should store score values to moveList

            */
            
            
            
            if (board.indexRecord.Count == board.getRows() * board.getColumns())        // if board is full return draw
            {
                if (setScore)
                {
                    moveList[board.indexRecord[board.indexRecord.Count - 1]].Score = 0;
                }

                return 0;
            }

            if (depth == 0)         // if the depth is at 0, return a static evaluation of the position
            {
                if (setScore)
                {
                    moveList[board.indexRecord[board.indexRecord.Count - 1]].Score = board.EvalCurrentState(board.currentState, board.indexRecord[board.indexRecord.Count - 1], currentPlayer);
                }

                return board.EvalCurrentState(board.currentState, board.indexRecord[board.indexRecord.Count - 1], currentPlayer);
            }

            for (int i = 0; i < board.getColumns() - 1; i++)        // before placing next value, check if any placements immediately return a win condition. If so, return win score
            {
                if (board.CheckMove(i) >= 0 && board.CheckForWin(board.currentState, board.CheckMove(i), currentPlayer))
                {
                    if (setScore)
                    {
                        moveList[i].Score = 4;
                    }

                    return 4;
                }
            }

            int maxEval = -5;                                                                                       // initialize maxEval to lowest possible score

            for (int i = 0; i < board.middleToOut.Length - 1; i++)                                                  // loop through idecies from inner to outer
            {
                int position = board.CheckMove(board.middleToOut[i]);                                               // check the current position if it is valid, if not do not play

                if (position >= 0)
                {
                    char nextPlayer = ' ';                                                                          // set value for next player
                    if (currentPlayer == 'O') { nextPlayer = 'X'; }
                    if (currentPlayer == 'X') { nextPlayer = 'O'; }

                    board.PlaceSymbol(position, currentPlayer);                                                     // place symbol in test board

                    int eval = -Search(board, new List<move>(), depth - 1, -beta, -alpha, nextPlayer, false);       // recusive call on search function with depth - 1, score range of [-beta, alpha], and no setting of move scores

                    board.RemoveSymbol(position);                                                                   // remove the current position in depth scope 

                    maxEval = Math.Max(maxEval, eval);                                                              // get the highest value between maxEval and searched eval

                    alpha = Math.Max(alpha, eval);                                                                  // decrease score range depedning on serached eval

                    if (setScore)
                    {
                        moveList[board.middleToOut[i]].Score = maxEval;
                    }

                    if (beta <= alpha) { break; }                                                                   // if found position lies out of score range, do not check further in loop
                }
            }

            return maxEval;
        }
    }

    public class Game
    {
        private Board board = new Board("------------------------------------------", new List<int>());
        private Player[] players = new Player[2];
        private int currentPlayerIndex = 0;

        public Game(bool playAgainstComputer)
        {
            players[0] = new HumanPlayer('X');
            players[1] = playAgainstComputer ? (Player)new ComputerPlayer('O') : new HumanPlayer('O');
        }

        public void Start()
        {
            bool gameEnded = false;
            while (!board.IsFull() && !gameEnded)
            {
                board.Display();
                Player currentPlayer = players[currentPlayerIndex];
                int validMove = -1;
                int columnChoice = -1;

                while (validMove < 0)
                {
                    columnChoice = currentPlayer.ChooseColumn(board);
                    validMove = board.CheckMove(columnChoice);

                    if (validMove >= 0)
                    {
                        board.PlaceSymbol(validMove, currentPlayer.Symbol);
                    }
                    else
                    {
                        Console.WriteLine("Invalid move, try again.");
                    }
                }

                gameEnded = board.CheckForWin(board.currentState, validMove, currentPlayer.Symbol);

                board.Display();
                if (gameEnded)
                {    
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
