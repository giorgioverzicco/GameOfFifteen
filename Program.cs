const byte BOARD_SIZE = 4;
const byte CELLS_COUNT = BOARD_SIZE * BOARD_SIZE;

int[] gameBoard = GetShuffledNumbersInRange(CELLS_COUNT);

/* TEST GAME BOARD
int[] gameBoard =
{
    1, 2, 3, 4,
    5, 6, 7, 8,
    9, 10, 11, 12,
    13, 14, 0, 15
};
*/

bool hasWon = RunGame();
Console.WriteLine(hasWon ? "\nCongratulazioni, hai vinto!" : "\nMi dispiace che ti sia arreso... riprovaci!");

bool RunGame()
{
    bool hasWon = false;
    
    do 
    {
        PrintGameBoard(gameBoard);

        int numberToMove = GetNumberToMove();
        if (numberToMove == -1)
        {
            break;
        }

        bool canMove = MoveNumber(gameBoard, numberToMove);
        if (!canMove)
        {
            Console.WriteLine($"Non puoi spostare {numberToMove}...");
            continue;
        }
        
        SwapCells(gameBoard, numberToMove);

        if (IsSolved())
        {
            hasWon = true;
            PrintGameBoard(gameBoard);
        }
    } while (!hasWon);

    return hasWon;
}

void SwapCells(int[] gameBoard, int numberToMove)
{
    int emptyCellIdx = Array.IndexOf(gameBoard, 0);
    int cellToMoveIdx = Array.IndexOf(gameBoard, numberToMove);
    
    (gameBoard[emptyCellIdx], gameBoard[cellToMoveIdx]) = (gameBoard[cellToMoveIdx], gameBoard[emptyCellIdx]);
}

bool MoveNumber(int[] gameBoard, int numberToMove)
{
    int emptyCellIdx = Array.IndexOf(gameBoard, 0);
    int cellToMoveIdx = Array.IndexOf(gameBoard, numberToMove);

    return 
        cellToMoveIdx != -1
        && IsValidMove(cellToMoveIdx, emptyCellIdx);
}

bool IsValidMove(int cellToMoveIdx, int emptyCellIdx)
{
    switch (cellToMoveIdx % BOARD_SIZE)
    {
        // cell to move is on the most left spot
        case 0:
            if (cellToMoveIdx + BOARD_SIZE == emptyCellIdx
                || cellToMoveIdx - BOARD_SIZE == emptyCellIdx
                || cellToMoveIdx + 1 == emptyCellIdx)
            {
                return true;
            }
            break;
        
        // cell to move is on the most right spot
        case >= BOARD_SIZE - 1:
            if (cellToMoveIdx + BOARD_SIZE == emptyCellIdx
                || cellToMoveIdx - BOARD_SIZE == emptyCellIdx
                || cellToMoveIdx - 1 == emptyCellIdx)
            {
                return true;
            }
            break;
        
        // cell to move is on any middle spot
        default:
            if (cellToMoveIdx + BOARD_SIZE == emptyCellIdx
                || cellToMoveIdx - BOARD_SIZE == emptyCellIdx
                || cellToMoveIdx + 1 == emptyCellIdx
                || cellToMoveIdx - 1 == emptyCellIdx)
            {
                return true;
            }
            break;
    }

    return false;
}

int GetNumberToMove()
{
    int numberToMove;
    
    do
    {
        Console.WriteLine("\nInserisci il numero che vuoi spostare (-1 per arrenderti):");
        Console.Write("> ");
        numberToMove = Convert.ToInt32(Console.ReadLine());
    } while (numberToMove == 0);

    return numberToMove;
}

void PrintGameBoard(int[] gameBoard)
{
    for (int i = 0; i < gameBoard.Length; i++)
    {
        if (i % 4 == 0)
        {
            Console.WriteLine();
        }
    
        Console.Write(" ");
        Console.Write(gameBoard[i] == 0 ? "  " : $"{gameBoard[i],2}");
        Console.Write(" |");
    }
    
    Console.WriteLine("\n");
}

bool IsSolved()
{
    return
        Enumerable.Range(0, CELLS_COUNT - 2)
            .All(i => gameBoard[i] == i + 1);
}

bool IsSolvable(int[] gameBoard)
{
    bool isEvenSized = BOARD_SIZE % 2 == 0;
    bool parity = true;
    
    for (int prev = 0; prev < CELLS_COUNT - 2; prev++) {
        for (int next = prev + 1; next < CELLS_COUNT - 1; next++) {
            if (gameBoard[next] < gameBoard[prev])
            {
                parity = !parity;
            }
        }
    }

    if (isEvenSized
        && (gameBoard[gameBoard.Length - 1] / BOARD_SIZE) % 2 == 0)
    {
        parity = !parity;
    }
    
    return parity;
}

int[] GetShuffledNumbersInRange(byte cellsCount)
{
    int[] board = 
        Enumerable.Range(0, cellsCount)
            .OrderBy(_ => new Random().Next())
            .ToArray();

    if (!IsSolvable(board))
    {
        (board[0], board[1]) = (board[1], board[0]);
    }

    return board;
}