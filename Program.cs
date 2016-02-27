namespace MineSweeper
{
    using System;
    using System.Collections.Generic;
    using Minesweeper;

    public class Program
    {
        public static void Main(string[] arguments)
        {
            string inputLine = string.Empty;
            char[,] boardArea = CreatePlayingField();
            char[,] bombsArea = GenerateBombsBoard();
            int openedCellsCount = 0;

            const int MaxEmptyCellsCount = 35;

            bool isGameOver = false;
            bool boardIsToBeRendered = true;
            bool isGameWon = false;

            List<ScoreboardEntry> scoreBoard = new List<ScoreboardEntry>(6);

            int inputRow = 0;
            int inputCol = 0;

            do
            {
                if (boardIsToBeRendered)
                {
                    Console.WriteLine("Let's play Minesweeper. Try to find all the cells without mines." +
                    " Command 'top' shows the Scoreboard, 'restart' starts a new game, 'exit' quits the game. Good luck!");
                    WriteBoardOnScreen(boardArea);
                    boardIsToBeRendered = false;
                }

                Console.Write("Input row and column: ");

                inputLine = Console.ReadLine().Trim();

                if (inputLine.Length >= 3)
                {
                    if (int.TryParse(inputLine[0].ToString(), out inputRow)
                        && int.TryParse(inputLine[2].ToString(), out inputCol)
                        && inputRow <= boardArea.GetLength(0)
                        && inputCol <= boardArea.GetLength(1))
                    {
                        inputLine = "turn";
                    }
                }

                switch (inputLine)
                {
                    case "top":
                        WriteScoreboardOnScreen(scoreBoard);
                        break;
                    case "restart":
                        boardArea = CreatePlayingField();
                        bombsArea = GenerateBombsBoard();
                        WriteBoardOnScreen(boardArea);
                        isGameOver = false;
                        boardIsToBeRendered = false;
                        break;
                    case "exit":
                        Console.WriteLine("Bye, bye!");
                        break;
                    case "turn":
                        if (bombsArea[inputRow, inputCol] != '*')
                        {
                            if (bombsArea[inputRow, inputCol] == '-')
                            {
                                WriteNumberOfBombsInCell(boardArea, bombsArea, inputRow, inputCol);
                                openedCellsCount++;
                            }

                            if (MaxEmptyCellsCount == openedCellsCount)
                            {
                                isGameWon = true;
                            }
                            else
                            {
                                WriteBoardOnScreen(boardArea);
                            }
                        }
                        else
                        {
                            isGameOver = true;
                        }

                        break;
                    default:
                        Console.WriteLine("\nError: Invalid command\n");
                        break;
                }

                if (isGameOver)
                {
                    WriteBoardOnScreen(bombsArea);

                    Console.Write(
                        "\nYou died with {0} points. " + "Please input your nickname: ",
                        openedCellsCount);
                    string nickname = Console.ReadLine();

                    ScoreboardEntry currentWinnerEntry = new ScoreboardEntry(nickname, openedCellsCount);

                    if (scoreBoard.Count < 5)
                    {
                        scoreBoard.Add(currentWinnerEntry);
                    }
                    else
                    {
                        for (int i = 0; i < scoreBoard.Count; i++)
                        {
                            if (scoreBoard[i].Points < currentWinnerEntry.Points)
                            {
                                scoreBoard.Insert(i, currentWinnerEntry);
                                scoreBoard.RemoveAt(scoreBoard.Count - 1);
                                break;
                            }
                        }
                    }

                    scoreBoard.Sort((ScoreboardEntry firstEntry, ScoreboardEntry secondEntry) => secondEntry.Name.CompareTo(firstEntry.Name));
                    scoreBoard.Sort((ScoreboardEntry firstEntry, ScoreboardEntry secondEntry) => secondEntry.Points.CompareTo(firstEntry.Points));

                    WriteScoreboardOnScreen(scoreBoard);

                    boardArea = CreatePlayingField();
                    bombsArea = GenerateBombsBoard();
                    openedCellsCount = 0;
                    isGameOver = false;
                    boardIsToBeRendered = true;
                }

                if (isGameWon)
                {
                    Console.WriteLine("\nCongratulations! You opened 35 cells without getting hit.");
                    WriteBoardOnScreen(bombsArea);
                    Console.WriteLine("Please input nickname: ");
                    string nickname = Console.ReadLine();
                    ScoreboardEntry winnerEntry = new ScoreboardEntry(nickname, openedCellsCount);
                    scoreBoard.Add(winnerEntry);
                    WriteScoreboardOnScreen(scoreBoard);

                    boardArea = CreatePlayingField();
                    bombsArea = GenerateBombsBoard();
                    openedCellsCount = 0;
                    isGameWon = false;
                    boardIsToBeRendered = true;
                }
            }
            while (inputLine != "exit");

            Console.WriteLine("Made in Bulgaria");
            Console.WriteLine("Goodbye");
            Console.Read();
        }

        private static void WriteScoreboardOnScreen(List<ScoreboardEntry> scoreboard)
        {
            Console.WriteLine("\nScoreboard:");
            if (scoreboard.Count > 0)
            {
                for (int i = 0; i < scoreboard.Count; i++)
                {
                    Console.WriteLine(
                        "{0}. {1} --> {2} opened cells", 
                        i + 1, 
                        scoreboard[i].Name, 
                        scoreboard[i].Points);
                }

                Console.WriteLine();
            }
            else
            {
                Console.WriteLine("Scoreboard is empty!\n");
            }
        }

        private static void WriteNumberOfBombsInCell(char[,] boardArea, char[,] bombsArea, int row, int col)
        {
            char countOfBombs = CountBombsAroundCell(bombsArea, row, col);
            bombsArea[row, col] = countOfBombs;
            boardArea[row, col] = countOfBombs;
        }

        private static void WriteBoardOnScreen(char[,] board)
        {
            int maxRows = board.GetLength(0);
            int maxCols = board.GetLength(1);
            Console.WriteLine("\n    0 1 2 3 4 5 6 7 8 9");
            Console.WriteLine("   ---------------------");
            for (int currentRow = 0; currentRow < maxRows; currentRow++)
            {
                Console.Write("{0} | ", currentRow);
                for (int currentCol = 0; currentCol < maxCols; currentCol++)
                {
                    Console.Write(string.Format("{0} ", board[currentRow, currentCol]));
                }

                Console.Write("|");
                Console.WriteLine();
            }

            Console.WriteLine("   ---------------------\n");
        }

        private static char[,] CreatePlayingField()
        {
            int boardRows = 5;
            int boardColumns = 10;
            char[,] board = new char[boardRows, boardColumns];
            for (int i = 0; i < boardRows; i++)
            {
                for (int j = 0; j < boardColumns; j++)
                {
                    board[i, j] = '?';
                }
            }

            return board;
        }

        private static char[,] GenerateBombsBoard()
        {
            int maxRows = 5;
            int maxCols = 10;
            char[,] bombsBoard = new char[maxRows, maxCols];

            for (int row = 0; row < maxRows; row++)
            {
                for (int col = 0; col < maxCols; col++)
                {
                    bombsBoard[row, col] = '-';
                }
            }

            List<int> bombsList = new List<int>();
            while (bombsList.Count < 15)
            {
                Random random = new Random();
                int randomNumber = random.Next(50);
                if (!bombsList.Contains(randomNumber))
                {
                    bombsList.Add(randomNumber);
                }
            }

            foreach (int bombNumber in bombsList)
            {
                int targetRow = bombNumber / maxCols;
                int targetCol = bombNumber % maxCols;
                if (targetCol == 0 && bombNumber != 0)
                {
                    targetRow--;
                    targetCol = maxCols;
                }
                else
                {
                    targetCol++;
                }

                bombsBoard[targetRow, targetCol - 1] = '*';
            }

            return bombsBoard;
        }
        
        private static char CountBombsAroundCell(char[,] bombsArea, int cellRow, int cellCol)
        {
            int countOfBombs = 0;
            int rowsCount = bombsArea.GetLength(0);
            int colsCount = bombsArea.GetLength(1);

            if (cellRow - 1 >= 0)
            {
                if (bombsArea[cellRow - 1, cellCol] == '*')
                {
                    countOfBombs++;
                }
            }

            if (cellRow + 1 < rowsCount)
            {
                if (bombsArea[cellRow + 1, cellCol] == '*')
                {
                    countOfBombs++;
                }
            }

            if (cellCol - 1 >= 0)
            {
                if (bombsArea[cellRow, cellCol - 1] == '*')
                {
                    countOfBombs++;
                }
            }

            if (cellCol + 1 < colsCount)
            {
                if (bombsArea[cellRow, cellCol + 1] == '*')
                {
                    countOfBombs++;
                }
            }

            if ((cellRow - 1 >= 0) && (cellCol - 1 >= 0))
            {
                if (bombsArea[cellRow - 1, cellCol - 1] == '*')
                {
                    countOfBombs++;
                }
            }

            if ((cellRow - 1 >= 0) && (cellCol + 1 < colsCount))
            {
                if (bombsArea[cellRow - 1, cellCol + 1] == '*')
                {
                    countOfBombs++;
                }
            }

            if ((cellRow + 1 < rowsCount) && (cellCol - 1 >= 0))
            {
                if (bombsArea[cellRow + 1, cellCol - 1] == '*')
                {
                    countOfBombs++;
                }
            }

            if ((cellRow + 1 < rowsCount) && (cellCol + 1 < colsCount))
            {
                if (bombsArea[cellRow + 1, cellCol + 1] == '*')
                {
                    countOfBombs++;
                }
            }

            return char.Parse(countOfBombs.ToString());
        }
    }
}
