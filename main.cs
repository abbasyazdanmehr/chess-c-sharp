using System;
using System.Collections.Generic;

using MyUtil;
using Chess;

namespace App
{
    class Program
    {
        static void Main()
        {
            System.Console.WriteLine("Start App ...");
            Game.Start();
            System.Console.WriteLine("End App ...");
            System.Console.ReadLine();
        }
    }
}

namespace Chess
{
    public enum PieceName
    {
        PAWN,
        KING,
        QUEEN,
        KNIGHT,
        BISHOP,
        ROOK,
        NONE,
        POSSIBLE
    }

    public enum PieceColor
    {
        WHITE,
        BLACK,
        NONE
    }

    public class Game
    {
        public static int Start()
        {
            System.Console.WriteLine("Game Started ...");
            Board.InitialBoard();
            // Board.InitialBoardTest();

            Player player1 = new Player("Player 1", PieceColor.WHITE);
            Player player2 = new Player("Player 2", PieceColor.BLACK);

            Player current = player1;

            int moveCounter = 0;

            // game loop
            while (true)
            {
                Board.PrintBoard();
                System.Console.WriteLine(Board.IsChecked(PieceColor.WHITE));

                Board.ClearPossibles();

                System.Console.Write(current + " Choose Piece Coordinate: ");
                string command1 = Console.ReadLine();

                if (command1.Equals("exit"))
                {
                    EndOfGame();
                    break;
                }

                Coordinate selectedCoordinate = Coordinate.FromString(command1);
                System.Console.WriteLine(selectedCoordinate.x + ", " + selectedCoordinate.y);

                Piece selectedPiece = Board.GetPiece(selectedCoordinate);
                System.Console.WriteLine(selectedPiece);

                Board.PiecePossibleMoves(selectedPiece, selectedCoordinate);
                Board.InsertPossibles();
            }

            return 0;
        }

        public static void EndOfGame()
        {
            System.Console.WriteLine("GoodBy!");
        }
    }

    class Player
    {
        string name;
        PieceColor color;
        int score;

        public Player(string inName, PieceColor inColor)
        {
            name = inName;
            color = inColor;
            score = 0;
        }

        public override string ToString()
        {
            return name;
        }
    }

    public class Piece
    {
        static public int generatedPieces = 0;
        public int id;
        public PieceName name;
        public PieceColor color;
        public string abbrivation;

        public Piece(PieceName inName, PieceColor inColor)
        {
            name = inName;
            color = inColor;
            id = generatedPieces;
            generatedPieces += 1;
            SetAbbrivation();
        }

        private void SetAbbrivation()
        {
            if (name == PieceName.PAWN)
            {
                abbrivation = "P";
            }
            else if (name == PieceName.KING)
            {
                abbrivation = "K";
            }
            else if (name == PieceName.QUEEN)
            {
                abbrivation = "Q";
            }
            else if (name == PieceName.KNIGHT)
            {
                abbrivation = "N";
            }
            else if (name == PieceName.BISHOP)
            {
                abbrivation = "B";
            }
            else if (name == PieceName.ROOK)
            {
                abbrivation = "R";
            }
            else if (name == PieceName.NONE)
            {
                abbrivation = " O ";
            }
            else if (name == PieceName.POSSIBLE)
            {
                abbrivation = " % ";
            }

            if (color == PieceColor.WHITE)
            {
                abbrivation = "W." + abbrivation;
            }
            else if (color == PieceColor.BLACK)
            {
                abbrivation = "B." + abbrivation;
            }
        }

        public override string ToString()
        {
            return abbrivation;
        }
    }

    public class Board
    {
        public static Piece[] board = new Piece[64];
        public static int rowsCount = 8;
        public static int colsCount = 8;

        public static Coordinate notFoundCoordinate = new Coordinate(-1, -1);

        static List<Coordinate> possibles = new List<Coordinate>();

        public static Piece blankPiece = new Piece(PieceName.NONE, PieceColor.NONE);
        public static Piece possiblePiece = new Piece(PieceName.POSSIBLE, PieceColor.NONE);

        public static bool InsertPiece(Coordinate coordinate, Piece piece)
        {
            board[(coordinate.y * colsCount) + coordinate.x] = piece;
            return true;
        }

        public static Piece GetPiece(Coordinate coordinate)
        {
            return board[(coordinate.y * colsCount) + coordinate.x];
        }

        public static Piece GetPiece(int inX, int inY)
        {
            return board[(inY * colsCount) + inX];
        }

        public static bool InitialBoard()
        {
            // set black pieces
            // first row
            InsertPiece(new Coordinate(0, 0), new Piece(PieceName.ROOK, PieceColor.BLACK));
            InsertPiece(new Coordinate(1, 0), new Piece(PieceName.KNIGHT, PieceColor.BLACK));
            InsertPiece(new Coordinate(2, 0), new Piece(PieceName.BISHOP, PieceColor.BLACK));
            InsertPiece(new Coordinate(3, 0), new Piece(PieceName.QUEEN, PieceColor.BLACK));
            InsertPiece(new Coordinate(4, 0), new Piece(PieceName.KING, PieceColor.BLACK));
            InsertPiece(new Coordinate(5, 0), new Piece(PieceName.BISHOP, PieceColor.BLACK));
            InsertPiece(new Coordinate(6, 0), new Piece(PieceName.KNIGHT, PieceColor.BLACK));
            InsertPiece(new Coordinate(7, 0), new Piece(PieceName.ROOK, PieceColor.BLACK));

            // second row
            for (int i = 0; i < colsCount; i++)
            {
                InsertPiece(new Coordinate(i, 1), new Piece(PieceName.PAWN, PieceColor.BLACK));
            }

            // set blanks squares
            for (int i = 2; i < 6; i++)
            {
                for (int j = 0; j < colsCount; j++)
                {
                    InsertPiece(new Coordinate(j, i), blankPiece);
                }
            }

            // set whilte pieces
            // second row
            for (int i = 0; i < colsCount; i++)
            {
                InsertPiece(new Coordinate(i, 6), new Piece(PieceName.PAWN, PieceColor.WHITE));
            }

            // first row
            InsertPiece(new Coordinate(0, 7), new Piece(PieceName.ROOK, PieceColor.WHITE));
            InsertPiece(new Coordinate(1, 7), new Piece(PieceName.KNIGHT, PieceColor.WHITE));
            InsertPiece(new Coordinate(2, 7), new Piece(PieceName.BISHOP, PieceColor.WHITE));
            InsertPiece(new Coordinate(3, 7), new Piece(PieceName.QUEEN, PieceColor.WHITE));
            InsertPiece(new Coordinate(4, 7), new Piece(PieceName.KING, PieceColor.WHITE));
            InsertPiece(new Coordinate(5, 7), new Piece(PieceName.BISHOP, PieceColor.WHITE));
            InsertPiece(new Coordinate(6, 7), new Piece(PieceName.KNIGHT, PieceColor.WHITE));
            InsertPiece(new Coordinate(7, 7), new Piece(PieceName.ROOK, PieceColor.WHITE));

            return true;
        }

        public static void InitialBoardTest()
        {
            // set blanks squares
            for (int i = 0; i < rowsCount; i++)
            {
                for (int j = 0; j < colsCount; j++)
                {
                    InsertPiece(new Coordinate(j, i), blankPiece);
                }
            }
            InsertPiece(new Coordinate(3, 7), new Piece(PieceName.KING, PieceColor.WHITE));
            InsertPiece(new Coordinate(3, 2), new Piece(PieceName.QUEEN, PieceColor.BLACK));
            InsertPiece(new Coordinate(3, 3), new Piece(PieceName.KNIGHT, PieceColor.BLACK));
        }

        public static void PrintBoard()
        {
            System.Console.Write("  +");
            System.Console.WriteLine(Stringer.StringRepeat("-----+", colsCount));

            for (int i = 0; i < rowsCount; i++)
            {
                System.Console.Write("  |");
                System.Console.WriteLine(Stringer.StringRepeat("     |", colsCount));

                System.Console.Write((8 - i) + " | ");
                for (int j = 0; j < colsCount; j++)
                {
                    System.Console.Write(board[(i * colsCount) + j].abbrivation + " | ");
                }
                System.Console.WriteLine();

                System.Console.Write("  |");
                System.Console.WriteLine(Stringer.StringRepeat("     |", colsCount));

                System.Console.Write("  +");
                System.Console.WriteLine(Stringer.StringRepeat("-----+", colsCount));
            }

            System.Console.Write("     ");
            for (int i = 0; i < colsCount; i++)
            {
                System.Console.Write((char)(i + 65) + "     ");
            }

            System.Console.WriteLine();
        }

        public static void InsertPossibles()
        {
            foreach (var possible in possibles)
            {
                Board.InsertPiece(possible, Board.possiblePiece);
            }
        }

        public static void DoMove(int x1, int y1, int x2, int y2)
        {
            Piece piece = GetPiece(x1, y1);
            InsertPiece(new Coordinate(x1, y1), blankPiece);
            InsertPiece(new Coordinate(x2, y2), piece);
        }

        public static void ClearPossibles()
        {
            foreach (var possible in possibles)
            {
                InsertPiece(possible, blankPiece);
            }
            possibles.Clear();
        }

        public static void AddPossible(int x1, int y1, int x2, int y2)
        {
            if (IsPossibleMove(x1, y1, x2, y2))
                possibles.Add(new Coordinate(x2, y2));
        }

        public static bool IsCheckOK(int x1, int y1, int x2, int y2)
        {
            DoMove(x1, y1, x2, y2);
            if (IsChecked(GetPiece(x2, y2).color))
            {
                DoMove(x2, y2, x1, y1);
                return false;
            }
            DoMove(x2, y2, x1, y1);
            return true;
        }

        public static bool PiecePossibleMoves(Coordinate coordinate)
        {
            // switch case is bullshit in syntax (but better at performance)
            Piece piece = GetPiece(coordinate);
            if (piece.name == PieceName.KNIGHT)
            {
                KnightPossibleMoves(coordinate, piece.color);
                return true;
            }
            else if (piece.name == PieceName.BISHOP)
            {
                BishopPossibleMoves(coordinate, piece.color);
                return true;
            }
            else if (piece.name == PieceName.ROOK)
            {
                RookPossibleMoves(coordinate);
                return true;
            }
            else if (piece.name == PieceName.QUEEN)
            {
                QueenPossibleMoves(coordinate);
                return true;
            }

            if (piece.color == PieceColor.WHITE)
            {
                if (piece.name == PieceName.PAWN)
                {
                    WhitePawnPossibleMoves(coordinate);
                }
                else if (piece.name == PieceName.KING)
                {
                    WhiteKingPossibleMoves(coordinate);
                }
            }
            else if (piece.color == PieceColor.BLACK)
            {
                if (piece.name == PieceName.PAWN)
                {
                    BlackPawnPossibleMoves(coordinate);
                }
                else if (piece.name == PieceName.KING)
                {
                    BlackKingPossibleMoves(coordinate);
                }
            }
            else
            {
                throw new Exception("$ AY: Wrong Piece in PiecePossibleMoves");
            }

            return true;
        }

        public static void WhitePawnPossibleMoves(Coordinate coordinate)
        {
            if (coordinate.y == 6 && Board.GetPiece(coordinate.x, coordinate.y - 1) == blankPiece)
            {
                AddPossible(coordinate.x, coordinate.y, coordinate.x, coordinate.y - 2);
            }
            AddPossible(coordinate.x, coordinate.y, coordinate.x, coordinate.y - 1);
        }

        public static void BlackPawnPossibleMoves(Coordinate coordinate)
        {
            if (coordinate.y == 1 && Board.GetPiece(coordinate.x, coordinate.y + 1) == blankPiece)
            {
                AddPossible(coordinate.x, coordinate.y, coordinate.x, coordinate.y + 2);
            }
            AddPossible(coordinate.x, coordinate.y, coordinate.x, coordinate.y + 1);
        }

        public static void KnightPossibleMoves(Coordinate coordinate, PieceColor color)
        {
            Coordinate current = new Coordinate(0, 0);

            current.x = coordinate.x + 1;
            current.y = coordinate.y + 2;
            AddPossible(coordinate.x, coordinate.y, current.x, current.y);

            current.x = coordinate.x + 2;
            current.y = coordinate.y + 1;
            AddPossible(coordinate.x, coordinate.y, current.x, current.y);

            current.x = coordinate.x - 1;
            current.y = coordinate.y + 2;
            AddPossible(coordinate.x, coordinate.y, current.x, current.y);

            current.x = coordinate.x - 2;
            current.y = coordinate.y + 1;
            AddPossible(coordinate.x, coordinate.y, current.x, current.y);

            current.x = coordinate.x + 1;
            current.y = coordinate.y - 2;
            AddPossible(coordinate.x, coordinate.y, current.x, current.y);

            current.x = coordinate.x + 2;
            current.y = coordinate.y - 1;
            AddPossible(coordinate.x, coordinate.y, current.x, current.y);

            current.x = coordinate.x - 1;
            current.y = coordinate.y - 2;
            AddPossible(coordinate.x, coordinate.y, current.x, current.y);

            current.x = coordinate.x - 2;
            current.y = coordinate.y - 1;
            AddPossible(coordinate.x, coordinate.y, current.x, current.y);
        }

        public static void BishopPossibleMoves(Coordinate coordinate, PieceColor color)
        {
            Coordinate current = new Coordinate(0, 0);

            current.x = coordinate.x;
            current.y = coordinate.y;
            while (true)
            {
                current.x += 1;
                current.y += 1;
                if (!IsCoordinateInBoard(current.x, current.y))
                {
                    break;
                }

                Piece currentPiece = GetPiece(current);

                if (currentPiece.abbrivation != blankPiece.abbrivation)
                {
                    break;
                }

                AddPossible(coordinate.x, coordinate.y, current.x, current.y);
            }

            current.x = coordinate.x;
            current.y = coordinate.y;
            while (true)
            {
                current.x -= 1;
                current.y += 1;
                if (!IsCoordinateInBoard(current.x, current.y))
                {
                    break;
                }

                Piece currentPiece = GetPiece(current);

                if (currentPiece.abbrivation != blankPiece.abbrivation)
                {
                    break;
                }

                AddPossible(coordinate.x, coordinate.y, current.x, current.y);
            }

            current.x = coordinate.x;
            current.y = coordinate.y;
            while (true)
            {
                current.x += 1;
                current.y -= 1;
                if (!IsCoordinateInBoard(current.x, current.y))
                {
                    break;
                }

                Piece currentPiece = GetPiece(current);

                if (currentPiece.abbrivation != blankPiece.abbrivation)
                {
                    break;
                }

                AddPossible(coordinate.x, coordinate.y, current.x, current.y);
            }

            current.x = coordinate.x;
            current.y = coordinate.y;
            while (true)
            {
                current.x -= 1;
                current.y -= 1;
                if (!IsCoordinateInBoard(current.x, current.y))
                {
                    break;
                }

                Piece currentPiece = GetPiece(current);

                if (currentPiece.abbrivation != blankPiece.abbrivation)
                {
                    break;
                }

                AddPossible(coordinate.x, coordinate.y, current.x, current.y);
            }
        }

        public static void RookPossibleMoves(Coordinate coordinate)
        {
            Coordinate current = new Coordinate(0, 0);

            current.x = coordinate.x;
            current.y = coordinate.y;
            while (true)
            {
                current.x += 1;
                if (!IsCoordinateInBoard(current.x, current.y))
                {
                    break;
                }

                if (currentPiece.abbrivation != blankPiece.abbrivation)
                {
                    break;
                }

                AddPossible(coordinate.x, coordinate.y, current.x, current.y);
            }

            current.x = coordinate.x;
            current.y = coordinate.y;
            while (true)
            {
                current.y += 1;
                if (!IsCoordinateInBoard(current.x, current.y))
                {
                    break;
                }

                Piece currentPiece = GetPiece(current);

                if (currentPiece.abbrivation != blankPiece.abbrivation)
                {
                    break;
                }

                AddPossible(coordinate.x, coordinate.y, current.x, current.y);
            }

            current.x = coordinate.x;
            current.y = coordinate.y;
            while (true)
            {
                current.x -= 1;
                if (!IsCoordinateInBoard(current.x, current.y))
                {
                    break;
                }

                Piece currentPiece = GetPiece(current);

                if (currentPiece.abbrivation != blankPiece.abbrivation)
                {
                    break;
                }

                AddPossible(coordinate.x, coordinate.y, current.x, current.y);
            }

            current.x = coordinate.x;
            current.y = coordinate.y;
            while (true)
            {
                current.y -= 1;
                if (!IsCoordinateInBoard(current.x, current.y))
                {
                    break;
                }

                Piece currentPiece = GetPiece(current);

                if (currentPiece.abbrivation != blankPiece.abbrivation)
                {
                    break;
                }

                AddPossible(coordinate.x, coordinate.y, current.x, current.y);
            }
        }

        public static void QueenPossibleMoves(Coordinate coordinate)
        {
            BishopPossibleMoves(coordinate, PieceColor.WHITE);
            RookPossibleMoves(coordinate);
        }

        public static void KingUsualPossibleMoves(Coordinate coordinate)
        {
            Coordinate current = new Coordinate(0, 0);

            current.x = coordinate.x + 1;
            current.y = coordinate.y;
            AddPossible(coordinate.x, coordinate.y, current.x, current.y);

            current.x = coordinate.x + 1;
            current.y = coordinate.y + 1;
            AddPossible(coordinate.x, coordinate.y, current.x, current.y);

            current.x = coordinate.x;
            current.y = coordinate.y + 1;
            AddPossible(coordinate.x, coordinate.y, current.x, current.y);

            current.x = coordinate.x - 1;
            current.y = coordinate.y + 1;
            AddPossible(coordinate.x, coordinate.y, current.x, current.y);

            current.x = coordinate.x - 1;
            current.y = coordinate.y;
            AddPossible(coordinate.x, coordinate.y, current.x, current.y);

            current.x = coordinate.x - 1;
            current.y = coordinate.y - 1;
            AddPossible(coordinate.x, coordinate.y, current.x, current.y);

            current.x = coordinate.x;
            current.y = coordinate.y - 1;
            AddPossible(coordinate.x, coordinate.y, current.x, current.y);

            current.x = coordinate.x + 1;
            current.y = coordinate.y - 1;
            AddPossible(coordinate.x, coordinate.y, current.x, current.y);
        }

        public static void WhiteKingPossibleMoves(Coordinate coordinate)
        {
            KingUsualPossibleMoves(coordinate);

            // white castle
        }

        public static void BlackKingPossibleMoves(Coordinate coordinate)
        {
            KingUsualPossibleMoves(coordinate);

            // Black castle
        }

        public static bool IsPossibleMove(int x1, int y1, int x2, int y2)
        {
            return IsCoordinateInBoard(x2, y2)
                && IsEmptySquare(x2, y2)
                && IsCheckOK(x1, y1, x2, y2);
        }

        public static bool IsEmptySquare(int inX, int inY)
        {
            return GetPiece(inX, inY) == blankPiece;
        }

        public static bool IsChecked(PieceColor color)
        {
            string friendColor;
            string enemyColor;
            if (color == PieceColor.WHITE)
            {
                friendColor = "W";
                enemyColor = "B";
            }
            else
            {
                friendColor = "B";
                enemyColor = "W";
            }

            Coordinate kingCoordinate = GetCoordinate(friendColor + ".K");

            Coordinate current = kingCoordinate.Copy();

            // check pawns
            current.x = kingCoordinate.x + 1;
            current.y = kingCoordinate.y + 1;
            if (
                IsCoordinateInBoard(current)
                && GetPiece(current).abbrivation.Equals(enemyColor + ".P")
            )
                return true;

            current.x = kingCoordinate.x - 1;
            current.y = kingCoordinate.y + 1;
            if (
                IsCoordinateInBoard(current)
                && GetPiece(current).abbrivation.Equals(enemyColor + ".P")
            )
                return true;

            // check knights shape
            current.x = kingCoordinate.x + 1;
            current.y = kingCoordinate.y + 2;
            if (
                IsCoordinateInBoard(current)
                && GetPiece(current).abbrivation.Equals(enemyColor + ".N")
            )
            {
                return true;
            }

            current.x = kingCoordinate.x + 2;
            current.y = kingCoordinate.y + 1;
            if (
                IsCoordinateInBoard(current)
                && GetPiece(current).abbrivation.Equals(enemyColor + ".N")
            )
            {
                return true;
            }

            current.x = kingCoordinate.x - 1;
            current.y = kingCoordinate.y + 2;
            if (
                IsCoordinateInBoard(current)
                && GetPiece(current).abbrivation.Equals(enemyColor + ".N")
            )
            {
                return true;
            }

            current.x = kingCoordinate.x - 2;
            current.y = kingCoordinate.y + 1;
            if (
                IsCoordinateInBoard(current)
                && GetPiece(current).abbrivation.Equals(enemyColor + ".N")
            )
            {
                return true;
            }

            current.x = kingCoordinate.x + 1;
            current.y = kingCoordinate.y - 2;
            if (
                IsCoordinateInBoard(current)
                && GetPiece(current).abbrivation.Equals(enemyColor + ".N")
            )
            {
                return true;
            }

            current.x = kingCoordinate.x + 2;
            current.y = kingCoordinate.y - 1;
            if (
                IsCoordinateInBoard(current)
                && GetPiece(current).abbrivation.Equals(enemyColor + ".N")
            )
            {
                return true;
            }

            current.x = kingCoordinate.x - 1;
            current.y = kingCoordinate.y - 2;
            if (
                IsCoordinateInBoard(current)
                && GetPiece(current).abbrivation.Equals(enemyColor + ".N")
            )
            {
                return true;
            }

            current.x = kingCoordinate.x - 2;
            current.y = kingCoordinate.y - 1;
            if (
                IsCoordinateInBoard(current)
                && GetPiece(current).abbrivation.Equals(enemyColor + ".N")
            )
            {
                return true;
            }

            // check diagonals
            current.x = kingCoordinate.x;
            current.y = kingCoordinate.y;
            while (true)
            {
                current.x += 1;
                current.y += 1;
                if (!IsCoordinateInBoard(current.x, current.y))
                {
                    break;
                }

                Piece currentPiece = GetPiece(current);

                if (
                    currentPiece.abbrivation == enemyColor + ".Q"
                    || currentPiece.abbrivation == enemyColor + ".B"
                )
                {
                    return true;
                }

                if (currentPiece.abbrivation != blankPiece.abbrivation)
                {
                    break;
                }
            }

            current.x = kingCoordinate.x;
            current.y = kingCoordinate.y;
            while (true)
            {
                current.x -= 1;
                current.y += 1;
                if (!IsCoordinateInBoard(current.x, current.y))
                {
                    break;
                }

                Piece currentPiece = GetPiece(current);

                if (
                    currentPiece.abbrivation == enemyColor + ".Q"
                    || currentPiece.abbrivation == enemyColor + ".B"
                )
                {
                    return true;
                }

                if (currentPiece.abbrivation != blankPiece.abbrivation)
                {
                    break;
                }
            }

            current.x = kingCoordinate.x;
            current.y = kingCoordinate.y;
            while (true)
            {
                current.x += 1;
                current.y -= 1;
                if (!IsCoordinateInBoard(current.x, current.y))
                {
                    break;
                }

                Piece currentPiece = GetPiece(current);

                if (
                    currentPiece.abbrivation == enemyColor + ".Q"
                    || currentPiece.abbrivation == enemyColor + ".B"
                )
                {
                    return true;
                }

                if (currentPiece.abbrivation != blankPiece.abbrivation)
                {
                    break;
                }
            }

            current.x = kingCoordinate.x;
            current.y = kingCoordinate.y;
            while (true)
            {
                current.x -= 1;
                current.y -= 1;
                if (!IsCoordinateInBoard(current.x, current.y))
                {
                    break;
                }

                Piece currentPiece = GetPiece(current);

                if (
                    currentPiece.abbrivation == enemyColor + ".Q"
                    || currentPiece.abbrivation == enemyColor + ".B"
                )
                {
                    return true;
                }

                if (currentPiece.abbrivation != blankPiece.abbrivation)
                {
                    break;
                }
            }

            // check row and column
            current.x = kingCoordinate.x;
            current.y = kingCoordinate.y;
            while (true)
            {
                current.x += 1;
                if (!IsCoordinateInBoard(current.x, current.y))
                {
                    break;
                }

                Piece currentPiece = GetPiece(current);

                if (
                    currentPiece.abbrivation == enemyColor + ".Q"
                    || currentPiece.abbrivation == enemyColor + ".R"
                )
                {
                    return true;
                }

                if (currentPiece.abbrivation != blankPiece.abbrivation)
                {
                    break;
                }
            }

            current.x = kingCoordinate.x;
            current.y = kingCoordinate.y;
            while (true)
            {
                current.y += 1;
                if (!IsCoordinateInBoard(current.x, current.y))
                {
                    break;
                }

                Piece currentPiece = GetPiece(current);

                if (
                    currentPiece.abbrivation == enemyColor + ".Q"
                    || currentPiece.abbrivation == enemyColor + ".R"
                )
                {
                    return true;
                }

                if (currentPiece.abbrivation != blankPiece.abbrivation)
                {
                    break;
                }
            }

            current.x = kingCoordinate.x;
            current.y = kingCoordinate.y;
            while (true)
            {
                current.x -= 1;
                if (!IsCoordinateInBoard(current.x, current.y))
                {
                    break;
                }

                Piece currentPiece = GetPiece(current);

                if (
                    currentPiece.abbrivation == enemyColor + ".Q"
                    || currentPiece.abbrivation == enemyColor + ".R"
                )
                {
                    return true;
                }

                if (currentPiece.abbrivation != blankPiece.abbrivation)
                {
                    break;
                }
            }

            current.x = kingCoordinate.x;
            current.y = kingCoordinate.y;
            while (true)
            {
                current.y -= 1;
                if (!IsCoordinateInBoard(current.x, current.y))
                {
                    break;
                }

                Piece currentPiece = GetPiece(current);

                if (
                    currentPiece.abbrivation == enemyColor + ".Q"
                    || currentPiece.abbrivation == enemyColor + ".R"
                )
                {
                    return true;
                }

                if (currentPiece.abbrivation != blankPiece.abbrivation)
                {
                    break;
                }
            }

            return false;
        }

        public static bool IsCoordinateInBoard(int inX, int inY)
        {
            if (inX < 0 || inY < 0 || inX >= colsCount || inY >= rowsCount)
            {
                return false;
            }
            return true;
        }

        public static bool IsCoordinateInBoard(Coordinate coordinate)
        {
            int inX = coordinate.x;
            int inY = coordinate.y;
            if (inX < 0 || inY < 0 || inX >= colsCount || inY >= rowsCount)
            {
                return false;
            }
            return true;
        }

        public static Coordinate GetCoordinate(string abbrivation)
        {
            for (int i = 0; i < rowsCount; i++)
            {
                for (int j = 0; j < colsCount; j++)
                {
                    if (GetPiece(i, j).abbrivation == abbrivation)
                    {
                        return new Coordinate(i, j);
                    }
                }
            }
            return notFoundCoordinate;
        }
    }

    public class Coordinate
    {
        public int x = 0;
        public int y = 0;

        public Coordinate(int inX, int inY)
        {
            x = inX;
            y = inY;
        }

        public static Coordinate FromString(string inp)
        {
            Coordinate coordinate = new Coordinate(0, 0);
            coordinate.x = (int)inp.Substring(0, 1).ToUpper().ToCharArray()[0] - 65;
            coordinate.y = Board.rowsCount - int.Parse(inp.Substring(1, 1));
            return coordinate;
        }

        public Coordinate Copy()
        {
            return new Coordinate(x, y);
        }

        public override string ToString()
        {
            return x + ", " + y;
        }
    }

    public class Move
    {
        public Coordinate from;
        public Coordinate to;
    }
}

namespace MyUtil
{
    public class Stringer
    {
        public static string StringRepeat(string inp, int times)
        {
            string outp = "";
            for (int i = 0; i < times; i++)
            {
                outp += inp;
            }
            return outp;
        }
    }
}
