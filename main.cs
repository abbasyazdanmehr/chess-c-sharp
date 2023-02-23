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

            Player currentPlayer = player1;

            int moveCounter = 0;

            List<Coordinate> possibles = new List<Coordinate>();

            // game loop
            while (true)
            {
                if (moveCounter % 2 == 0)
                {
                    currentPlayer = player1;
                }
                else
                {
                    currentPlayer = player2;
                }

                Board.ClearPossibles(possibles);

                Board.PrintBoard();

                Coordinate startCoordinate;
                while (true)
                {
                    System.Console.Write("Choose Start Coordinate: ");
                    string command1 = Console.ReadLine();

                    if (command1.Equals("exit"))
                    {
                        EndOfGame();
                    }

                    startCoordinate = Coordinate.FromString(command1);

                    if (
                        startCoordinate != (Board.notFoundCoordinate)
                        && Board.GetPiece(startCoordinate).color == currentPlayer.color
                    )
                    {
                        possibles = Board.PiecePossibleMoves(startCoordinate);
                        if (possibles.Count != 0)
                            break;
                    }
                }
                System.Console.WriteLine("start coordinate: " + startCoordinate);

                // showing possible moves
                Board.InsertPossibles(possibles);

                Board.PrintBoard();

                // move is obligation after toching piece (touch-move rule in chess)
                Coordinate destinationCoordinate;
                while (true)
                {
                    System.Console.Write("Choose Destination Coordinate : ");
                    string command2 = Console.ReadLine();
                    destinationCoordinate = Coordinate.FromString(command2);

                    Coordinate coordinateInPossibles = Coordinate.GetCoordinateInList(
                        destinationCoordinate,
                        possibles
                    );

                    if (coordinateInPossibles != Board.notFoundCoordinate)
                    {
                        break;
                    }
                }

                Board.ClearPossibles(possibles);

                Board.DoMove(startCoordinate, destinationCoordinate);

                moveCounter += 1;
            }
        }

        public static void EndOfGame()
        {
            System.Console.WriteLine("GoodBy!");
            System.Environment.Exit(0);
        }
    }

    class Player
    {
        string name;
        public PieceColor color;

        public Player(string inName, PieceColor inColor)
        {
            name = inName;
            color = inColor;
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
            InsertPiece(0, 0, new Piece(PieceName.ROOK, PieceColor.BLACK));
            InsertPiece(1, 0, new Piece(PieceName.KNIGHT, PieceColor.BLACK));
            InsertPiece(2, 0, new Piece(PieceName.BISHOP, PieceColor.BLACK));
            InsertPiece(3, 0, new Piece(PieceName.QUEEN, PieceColor.BLACK));
            InsertPiece(4, 0, new Piece(PieceName.KING, PieceColor.BLACK));
            InsertPiece(5, 0, new Piece(PieceName.BISHOP, PieceColor.BLACK));
            InsertPiece(6, 0, new Piece(PieceName.KNIGHT, PieceColor.BLACK));
            InsertPiece(7, 0, new Piece(PieceName.ROOK, PieceColor.BLACK));

            // second row
            for (int i = 0; i < colsCount; i++)
            {
                InsertPiece(i, 1, new Piece(PieceName.PAWN, PieceColor.BLACK));
            }

            // set blanks squares
            for (int i = 2; i < 6; i++)
            {
                for (int j = 0; j < colsCount; j++)
                {
                    InsertPiece(j, i, blankPiece);
                }
            }

            // set whilte pieces
            // second row
            for (int i = 0; i < colsCount; i++)
            {
                InsertPiece(i, 6, new Piece(PieceName.PAWN, PieceColor.WHITE));
            }

            // first row
            InsertPiece(0, 7, new Piece(PieceName.ROOK, PieceColor.WHITE));
            InsertPiece(1, 7, new Piece(PieceName.KNIGHT, PieceColor.WHITE));
            InsertPiece(2, 7, new Piece(PieceName.BISHOP, PieceColor.WHITE));
            InsertPiece(3, 7, new Piece(PieceName.QUEEN, PieceColor.WHITE));
            InsertPiece(4, 7, new Piece(PieceName.KING, PieceColor.WHITE));
            InsertPiece(5, 7, new Piece(PieceName.BISHOP, PieceColor.WHITE));
            InsertPiece(6, 7, new Piece(PieceName.KNIGHT, PieceColor.WHITE));
            InsertPiece(7, 7, new Piece(PieceName.ROOK, PieceColor.WHITE));

            return true;
        }

        public static void InitialBoardTest()
        {
            // set blanks squares
            for (int i = 0; i < rowsCount; i++)
            {
                for (int j = 0; j < colsCount; j++)
                {
                    InsertPiece(j, i, blankPiece);
                }
            }
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

        public static void DoMove(Coordinate coordinate1, Coordinate coordinate2)
        {
            int x1 = coordinate1.x;
            int y1 = coordinate1.y;
            int x2 = coordinate2.x;
            int y2 = coordinate2.y;

            Piece piece = GetPiece(x1, y1);

            InsertPiece(new Coordinate(x1, y1), blankPiece);
            InsertPiece(new Coordinate(x2, y2), piece);

            // ToDo: these conditions execute in every move :/
            if (!whiteKingFirstMove && piece.abbrivation.Equals("W.K"))
            {
                whiteKingFirstMove = true;
                if (coordinate2.isEqual(Coordinate.FromString("g1")))
                {
                    DoMove(Coordinate.FromString("h1"), Coordinate.FromString("f1"));
                }
                else if (coordinate2.isEqual(Coordinate.FromString("c1")))
                {
                    DoMove(Coordinate.FromString("a1"), Coordinate.FromString("d1"));
                }
            }
            else if (!blackKingFirstMove && piece.abbrivation.Equals("B.K"))
            {
                blackKingFirstMove = true;
                if (coordinate2.isEqual(Coordinate.FromString("g8")))
                {
                    DoMove(Coordinate.FromString("h8"), Coordinate.FromString("f8"));
                }
                else if (coordinate2.isEqual(Coordinate.FromString("c8")))
                {
                    DoMove(Coordinate.FromString("a8"), Coordinate.FromString("d8"));
                }
            }
            else if (piece.abbrivation.Equals("W.R"))
            {
                blackRookFirstMove = true;
            }
            else if (piece.abbrivation.Equals("B.R"))
            {
                blackRookFirstMove = true;
            }
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

        public static bool IsColorBlank(Coordinate coordinate, PieceColor color)
        {
            return (GetPiece(coordinate) == blankPiece || GetPiece(coordinate).color == color);
        }

        public static bool IsColorBlank(int inX, int inY, PieceColor color)
        {
            return (GetPiece(inX, inY) == blankPiece || GetPiece(inX, inY).color == color);
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

        public static bool IsCheckOKForSmallCastle(PieceColor color)
        {
            string strColor;
            if (color == PieceColor.WHITE)
                strColor = "W";
            else
                strColor = "B";

            Coordinate kingCoordinate = GetCoordinate(strColor + ".K");
            Piece king = GetPiece(kingCoordinate);

            InsertPiece(kingCoordinate, blankPiece);
            InsertPiece(kingCoordinate.x + 1, kingCoordinate.y, king);
            if (IsChecked(king.color))
            {
                InsertPiece(kingCoordinate, king);
                return false;
            }

            InsertPiece(kingCoordinate.x + 1, kingCoordinate.y, blankPiece);
            InsertPiece(kingCoordinate.x + 2, kingCoordinate.y, king);

            if (IsChecked(king.color))
            {
                InsertPiece(kingCoordinate, king);
                return false;
            }

            InsertPiece(kingCoordinate, king);
            return true;
        }

        public static bool IsCheckOKForBigCastle(PieceColor color)
        {
            string strColor;
            if (color == PieceColor.WHITE)
                strColor = "W";
            else
                strColor = "B";

            Coordinate kingCoordinate = GetCoordinate(strColor + ".K");
            Piece king = GetPiece(kingCoordinate);

            InsertPiece(kingCoordinate, blankPiece);
            InsertPiece(kingCoordinate.x - 1, kingCoordinate.y, king);
            if (IsChecked(king.color))
            {
                InsertPiece(kingCoordinate, king);
                return false;
            }

            InsertPiece(kingCoordinate.x - 1, kingCoordinate.y, blankPiece);
            InsertPiece(kingCoordinate.x - 2, kingCoordinate.y, king);

            if (IsChecked(king.color))
            {
                InsertPiece(kingCoordinate, king);
                return false;
            }

            InsertPiece(kingCoordinate, king);
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

            if (IsDestinationOk(coordinate.x, coordinate.y + 1))
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
            possibles.AddRange(WhiteKingPossibleCastle());

            return possibles;
        }

        public static List<Coordinate> WhiteKingPossibleCastle()
        {
            List<Coordinate> possibles = new List<Coordinate>();
            if (!whiteKingFirstMove && !whiteRookFirstMove && !IsChecked(PieceColor.WHITE))
            {
                // System.Console.WriteLine(!whiteKingFirstMove + " $ " + !whiteRookFirstMove + " $ " + IsChecked(PieceColor.WHITE));
                if (
                    IsBlank(Coordinate.FromString("f1"))
                    && IsBlank(Coordinate.FromString("g1"))
                    && IsCheckOKForSmallCastle(PieceColor.WHITE)
                )
                {
                    possibles.Add(Coordinate.FromString("g1"));
                }

                if (
                    IsBlank(Coordinate.FromString("c1"))
                    && IsBlank(Coordinate.FromString("d1"))
                    && IsCheckOKForBigCastle(PieceColor.WHITE)
                )
                {
                    possibles.Add(Coordinate.FromString("c1"));
                }
            }

            return possibles;
        }

        public static List<Coordinate> BlackKingPossibleMoves(Coordinate coordinate)
        {
            List<Coordinate> possibles = new List<Coordinate>();
            possibles.AddRange(KingUsualPossibleMoves(coordinate));

            // Black castle
            possibles.AddRange(BlackKingPossibleCastle());

            return possibles;
        }

        public static List<Coordinate> BlackKingPossibleCastle()
        {
            List<Coordinate> possibles = new List<Coordinate>();
            if (!blackKingFirstMove && !blackRookFirstMove && !IsChecked(PieceColor.BLACK))
            {
                if (
                    IsBlank(Coordinate.FromString("f8"))
                    && IsBlank(Coordinate.FromString("g8"))
                    && IsCheckOKForSmallCastle(PieceColor.BLACK)
                )
                {
                    possibles.Add(Coordinate.FromString("g8"));
                }

                if (
                    IsBlank(Coordinate.FromString("c8"))
                    && IsBlank(Coordinate.FromString("d8"))
                    && IsCheckOKForBigCastle(PieceColor.BLACK)
                )
                {
                    possibles.Add(Coordinate.FromString("c8"));
                }
            }

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
            if (inp.Length != 2)
            {
                return Board.notFoundCoordinate;
            }

            if (
                (int)inp.Substring(0, 1).ToUpper().ToCharArray()[0] < (int)'A'
                || (int)inp.Substring(0, 1).ToUpper().ToCharArray()[0] > (int)'H'
                || (int)inp.Substring(1, 1).ToUpper().ToCharArray()[0] < (int)'1'
                || (int)inp.Substring(1, 1).ToUpper().ToCharArray()[0] > (int)'8'
            )
            {
                return Board.notFoundCoordinate;
            }

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

        public static Coordinate GetCoordinateInList(
            Coordinate coordinate,
            List<Coordinate> coordinates
        )
        {
            foreach (var c in coordinates)
            {
                if (c.isEqual(coordinate))
                {
                    return c;
                }
            }
            return Board.notFoundCoordinate;
        }

        public bool isEqual(Coordinate coordinate)
        {
            return x == coordinate.x && y == coordinate.y;
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
