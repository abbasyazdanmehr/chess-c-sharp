// packages
using System;
using System.Collections.Generic;

// my packages
using MyUtil;
using Chess;

class Program
{
    static void Main()
    {
        System.Console.WriteLine("Start App ...");
        Game.Start();
        System.Console.WriteLine("End App ...");
        System.Console.WriteLine("press any key to continue...");
        System.Console.ReadLine();
    }
}

namespace Chess
{
    public enum PieceName
    {
        KNIGHT,
        BISHOP,
        ROOK,
        QUEEN,
        PAWN,
        KING,
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

            List<Coordinate> possibles = new List<Coordinate>();

            // game loop
            while (true)
            {
                Board.PrintBoard();

                System.Console.Write(current + " Choose Piece Coordinate: ");
                string command1 = Console.ReadLine();

                if (command1.Equals("exit"))
                {
                    EndOfGame();
                    break;
                }

                Coordinate selectedCoordinate = Coordinate.FromString(command1);
                System.Console.WriteLine("selected coordinate: " + selectedCoordinate);

                Board.ClearPossibles(possibles);

                possibles = Board.PiecePossibleMoves(selectedCoordinate);
                Board.InsertPossibles(possibles);
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
                abbrivation = " . ";
            }
            else if (name == PieceName.POSSIBLE)
            {
                abbrivation = "%%%";
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

        public static Piece blankPiece = new Piece(PieceName.NONE, PieceColor.NONE);
        public static Piece possiblePiece = new Piece(PieceName.POSSIBLE, PieceColor.NONE);

        public static bool whiteKingFirstMove = false;
        public static bool blackKingFirstMove = false;
        public static bool whiteRookFirstMove = false;
        public static bool blackRookFirstMove = false;
        public static bool whiteKingCastel = false;
        public static bool blackKingCastel = false;

        public static bool InsertPiece(Coordinate coordinate, Piece piece)
        {
            board[(coordinate.y * colsCount) + coordinate.x] = piece;
            return true;
        }

        public static bool InsertPiece(int inX, int inY, Piece piece)
        {
            board[(inY * colsCount) + inX] = piece;
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

        public static void InsertPossibles(List<Coordinate> possibles)
        {
            foreach (var p in possibles)
            {
                Board.InsertPiece(p, Board.possiblePiece);
            }
        }

        public static void DoMove(int x1, int y1, int x2, int y2)
        {
            Piece piece = GetPiece(x1, y1);
            if (piece.abbrivation.Equals("W.K"))
            {
                whiteKingFirstMove = true;
            }
            else if (piece.abbrivation.Equals("B.K"))
            {
                blackKingFirstMove = true;
            }
            else if (piece.abbrivation.Equals("W.R"))
            {
                blackRookFirstMove = true;
            }
            else if (piece.abbrivation.Equals("B.R"))
            {
                blackRookFirstMove = true;
            }

            InsertPiece(new Coordinate(x1, y1), blankPiece);
            InsertPiece(new Coordinate(x2, y2), piece);
        }

        public static void ClearPossibles(List<Coordinate> possibles)
        {
            foreach (var possible in possibles)
            {
                InsertPiece(possible, blankPiece);
            }
            possibles.Clear();
        }

        public static bool IsBlank(Coordinate coordinate)
        {
            return GetPiece(coordinate) == blankPiece;
        }

        public static bool IsBlank(int inX, int inY)
        {
            return GetPiece(inX, inY) == blankPiece;
        }

        public static bool IsDestinationOk(Coordinate coordinate)
        {
            return IsCoordinateInBoard(coordinate) && IsBlank(coordinate);
        }

        public static bool IsDestinationOk(int inX, int inY)
        {
            return IsBlank(inX, inY) && IsCoordinateInBoard(inX, inY);
        }

        public static bool IsCheckOK(int inX, int inY)
        {
            Piece piece = GetPiece(inX, inY);
            InsertPiece(inX, inY, blankPiece);
            if (IsChecked(piece.color))
            {
                InsertPiece(inX, inY, piece);
                return false;
            }
            InsertPiece(inX, inY, piece);
            return true;
        }

        public static List<Coordinate> PiecePossibleMoves(Coordinate coordinate)
        {
            Piece piece = GetPiece(coordinate);

            if (piece.name == PieceName.KING)
            {
                if (piece.color == PieceColor.WHITE)
                {
                    return WhiteKingPossibleMoves(coordinate);
                }
                else if (piece.color == PieceColor.BLACK)
                {
                    return BlackKingPossibleMoves(coordinate);
                }
            }
            else
            {
                if (!IsCheckOK(coordinate.x, coordinate.y))
                    return new List<Coordinate>();

                if (piece.name == PieceName.KNIGHT)
                {
                    return KnightPossibleMoves(coordinate);
                }
                else if (piece.name == PieceName.BISHOP)
                {
                    return BishopPossibleMoves(coordinate);
                }
                else if (piece.name == PieceName.ROOK)
                {
                    return RookPossibleMoves(coordinate);
                }
                else if (piece.name == PieceName.QUEEN)
                {
                    return QueenPossibleMoves(coordinate);
                }
                if (piece.name == PieceName.PAWN)
                {
                    if (piece.color == PieceColor.WHITE)
                    {
                        return WhitePawnPossibleMoves(coordinate);
                    }
                    else if (piece.color == PieceColor.BLACK)
                    {
                        return BlackPawnPossibleMoves(coordinate);
                    }
                }
            }

            throw new Exception("$ AY: Wrong Piece in PiecePossibleMoves");
        }

        public static List<Coordinate> WhitePawnPossibleMoves(Coordinate coordinate)
        {
            List<Coordinate> possibles = new List<Coordinate>();
            System.Console.WriteLine(
                coordinate.y
                    + " $ "
                    + IsBlank(coordinate.x, coordinate.y - 1)
                    + " $ "
                    + IsBlank(coordinate.x, coordinate.y - 2)
            );
            if (
                coordinate.y == 6
                && IsBlank(coordinate.x, coordinate.y - 1)
                && IsBlank(coordinate.x, coordinate.y - 2)
            )
            {
                possibles.Add(new Coordinate(coordinate.x, coordinate.y - 2));
            }

            if (IsDestinationOk(coordinate.x, coordinate.y - 1))
            {
                possibles.Add(new Coordinate(coordinate.x, coordinate.y - 1));
            }

            return possibles;
        }

        public static List<Coordinate> BlackPawnPossibleMoves(Coordinate coordinate)
        {
            List<Coordinate> possibles = new List<Coordinate>();
            if (
                coordinate.y == 1
                && IsBlank(coordinate.x, coordinate.y + 1)
                && IsBlank(coordinate.x, coordinate.y + 2)
            )
            {
                possibles.Add(new Coordinate(coordinate.x, coordinate.y + 2));
            }
            else if (IsDestinationOk(coordinate.x, coordinate.y + 1))
            {
                possibles.Add(new Coordinate(coordinate.x, coordinate.y + 1));
            }

            return possibles;
        }

        public static List<Coordinate> KnightPossibleMoves(Coordinate coordinate)
        {
            List<Coordinate> possibles = new List<Coordinate>();
            Coordinate current = new Coordinate(0, 0);

            current.x = coordinate.x + 1;
            current.y = coordinate.y + 2;
            if (IsDestinationOk(current))
                possibles.Add(new Coordinate(current.x, current.y));

            current.x = coordinate.x + 2;
            current.y = coordinate.y + 1;
            if (IsDestinationOk(current))
                possibles.Add(new Coordinate(current.x, current.y));

            current.x = coordinate.x - 1;
            current.y = coordinate.y + 2;
            if (IsDestinationOk(current))
                possibles.Add(new Coordinate(current.x, current.y));

            current.x = coordinate.x - 2;
            current.y = coordinate.y + 1;
            if (IsDestinationOk(current))
                possibles.Add(new Coordinate(current.x, current.y));

            current.x = coordinate.x + 1;
            current.y = coordinate.y - 2;
            if (IsDestinationOk(current))
                possibles.Add(new Coordinate(current.x, current.y));

            current.x = coordinate.x + 2;
            current.y = coordinate.y - 1;
            if (IsDestinationOk(current))
                possibles.Add(new Coordinate(current.x, current.y));

            current.x = coordinate.x - 1;
            current.y = coordinate.y - 2;
            if (IsDestinationOk(current))
                possibles.Add(new Coordinate(current.x, current.y));

            current.x = coordinate.x - 2;
            current.y = coordinate.y - 1;
            if (IsDestinationOk(current))
                possibles.Add(new Coordinate(current.x, current.y));

            return possibles;
        }

        public static List<Coordinate> BishopPossibleMoves(Coordinate coordinate)
        {
            List<Coordinate> possibles = new List<Coordinate>();
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

                if (IsDestinationOk(current))
                    possibles.Add(new Coordinate(current.x, current.y));
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

                if (IsDestinationOk(current))
                    possibles.Add(new Coordinate(current.x, current.y));
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

                if (IsDestinationOk(current))
                    possibles.Add(new Coordinate(current.x, current.y));
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

                if (IsDestinationOk(current))
                {
                    possibles.Add(new Coordinate(current.x, current.y));
                }
            }

            return possibles;
        }

        public static List<Coordinate> RookPossibleMoves(Coordinate coordinate)
        {
            List<Coordinate> possibles = new List<Coordinate>();
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

                Piece currentPiece = GetPiece(current);

                if (currentPiece.abbrivation != blankPiece.abbrivation)
                {
                    break;
                }

                if (IsDestinationOk(current))
                {
                    possibles.Add(new Coordinate(current.x, current.y));
                }
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

                if (IsDestinationOk(current))
                {
                    possibles.Add(new Coordinate(current.x, current.y));
                }
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

                if (IsDestinationOk(current))
                {
                    possibles.Add(new Coordinate(current.x, current.y));
                }
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

                if (IsDestinationOk(current))
                {
                    possibles.Add(new Coordinate(current.x, current.y));
                }
            }

            return possibles;
        }

        public static List<Coordinate> QueenPossibleMoves(Coordinate coordinate)
        {
            List<Coordinate> possibles = new List<Coordinate>();

            possibles.AddRange(BishopPossibleMoves(coordinate));
            possibles.AddRange(RookPossibleMoves(coordinate));

            return possibles;
        }

        public static List<Coordinate> KingUsualPossibleMoves(Coordinate coordinate)
        {
            List<Coordinate> possibles = new List<Coordinate>();
            Coordinate current = new Coordinate(0, 0);

            current.x = coordinate.x + 1;
            current.y = coordinate.y;
            if (IsDestinationOk(current))
            {
                possibles.Add(new Coordinate(current.x, current.y));
            }

            current.x = coordinate.x + 1;
            current.y = coordinate.y + 1;
            if (IsDestinationOk(current))
            {
                possibles.Add(new Coordinate(current.x, current.y));
            }

            current.x = coordinate.x;
            current.y = coordinate.y + 1;
            if (IsDestinationOk(current))
            {
                possibles.Add(new Coordinate(current.x, current.y));
            }

            current.x = coordinate.x - 1;
            current.y = coordinate.y + 1;
            if (IsDestinationOk(current))
            {
                possibles.Add(new Coordinate(current.x, current.y));
            }

            current.x = coordinate.x - 1;
            current.y = coordinate.y;
            if (IsDestinationOk(current))
            {
                possibles.Add(new Coordinate(current.x, current.y));
            }

            current.x = coordinate.x - 1;
            current.y = coordinate.y - 1;
            if (IsDestinationOk(current))
            {
                possibles.Add(new Coordinate(current.x, current.y));
            }

            current.x = coordinate.x;
            current.y = coordinate.y - 1;
            if (IsDestinationOk(current))
            {
                possibles.Add(new Coordinate(current.x, current.y));
            }

            current.x = coordinate.x + 1;
            current.y = coordinate.y - 1;
            if (IsDestinationOk(current))
            {
                possibles.Add(new Coordinate(current.x, current.y));
            }

            return possibles;
        }

        public static List<Coordinate> WhiteKingPossibleMoves(Coordinate coordinate)
        {
            List<Coordinate> possibles = new List<Coordinate>();
            possibles.AddRange(KingUsualPossibleMoves(coordinate));

            // white castle
            possibles.AddRange(Castel());

            return possibles;
        }

        public static List<Coordinate> WhiteKingPossibleCastel()
        {
            List<Coordinate> possibles = new List<Coordinate>();
            if (!whiteKingCastel && !whiteKingFirstMove && !whiteRookFirstMove)
            {
                if (IsBlank(Coordinate.FromString("f1")) && IsBlank(Coordinate.FromString("g1")))
                {
                    possibles.Add(Coordinate.FromString("g1"));
                }
                else { }
            }

            return possibles;
        }

        public static List<Coordinate> BlackKingPossibleMoves(Coordinate coordinate)
        {
            List<Coordinate> possibles = new List<Coordinate>();
            possibles.AddRange(KingUsualPossibleMoves(coordinate));

            // Black castle


            return possibles;
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

        public void Copy(Coordinate coordinate1, Coordinate coordinate2)
        {
            coordinate1.x = coordinate2.x;
            coordinate1.y = coordinate2.y;
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

    public class Lister
    {
        public static void PrintCordinateList(List<Coordinate> list)
        {
            foreach (var l in list)
            {
                System.Console.Write(l + " $ ");
            }
            System.Console.WriteLine();
        }
    }
}
