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

    public enum InputFormat
    {
        TWOSTEPS,
        ONESTEP,
        NONE
    }

    public enum PieceColor
    {
        WHITE,
        BLACK,
        NONE
    }

    public enum MoveType
    {
        MOVEMENT,
        CAPTURE,
        SMALLCASTLE,
        BIGCASTLE,
        NONE
    }

    public class Game
    {
        public static List<Move> gameMoves = new List<Move>();

        public static int Start()
        {
            System.Console.WriteLine("Game Started...");
            Board.InitialBoard();
            // Board.InitialBoardTest();

            InputFormat inputFormat = InputFormat.ONESTEP;

            Player player1 = new Player("Player 1", PieceColor.WHITE);
            Player player2 = new Player("Player 2", PieceColor.BLACK);

            Player currentPlayer = player1;
            Player opponent = player2;

            int moveCounter = 0;

            List<Move> possibleMoves = new List<Move>();

            // game loop
            while (true)
            {
                if (moveCounter % 2 == 0)
                {
                    currentPlayer = player1;
                    opponent = player2;
                }
                else
                {
                    currentPlayer = player2;
                    opponent = player1;
                }

                Board.ClearPossibleMoves(possibleMoves);

                Board.PrintBoard();

                Coordinate startCoordinate;
                Coordinate destinationCoordinate;
                int moveIndex = -1;
                Move inputMove = Move.noMove;

                System.Console.WriteLine("example: (e2) or (e2 e4)");
                System.Console.Write("Move: ");
                string command = Console.ReadLine();

                if (command.Equals("resign"))
                    EndOfGame(currentPlayer);

                string[] inps = command.Trim().Split(' ');

                if (inps.Length == 1)
                {
                    inputFormat = InputFormat.TWOSTEPS;
                }
                else if (inps.Length == 2)
                {
                    inputFormat = InputFormat.ONESTEP;
                }
                else
                {
                    System.Console.WriteLine("not correct address count!");
                }

                if (inputFormat == InputFormat.TWOSTEPS)
                {
                    string command1 = inps[0];

                    startCoordinate = Coordinate.FromString(command1);
                    if (
                        startCoordinate != Board.notFoundCoordinate
                        && Board.GetPiece(startCoordinate).color == currentPlayer.color
                    )
                    {
                        possibleMoves = Board.PiecePossibleMoves(startCoordinate);
                        if (possibleMoves.Count == 0)
                            continue;
                    }
                    else
                    {
                        continue;
                    }

                    System.Console.WriteLine("Start Coordinate: " + startCoordinate);
                    System.Console.WriteLine("* You must choose your move destinaiton or resign!");

                    // showing possible moves
                    Board.InsertPossibleMoves(possibleMoves);

                    Board.PrintBoard();

                    // move is obligation after toching piece (touch-move rule in chess)
                    while (true)
                    {
                        System.Console.Write("Choose Destination Coordinate : ");
                        string command2 = Console.ReadLine();
                        destinationCoordinate = Coordinate.FromString(command2);

                        moveIndex = Coordinate.GetCoordinateInList(
                            destinationCoordinate,
                            possibleMoves
                        );

                        if (moveIndex != -1)
                            break;
                    }

                    inputMove = possibleMoves[moveIndex];
                }
                else if (inputFormat == InputFormat.ONESTEP)
                {
                    startCoordinate = Coordinate.FromString(inps[0]);
                    destinationCoordinate = Coordinate.FromString(inps[1]);

                    if (
                        startCoordinate != Board.notFoundCoordinate
                        && Board.GetPiece(startCoordinate).color == currentPlayer.color
                    )
                    {
                        possibleMoves = Board.PiecePossibleMoves(startCoordinate);
                        Move.PrintList(possibleMoves);
                        if (possibleMoves.Count == 0)
                            continue;

                        moveIndex = Coordinate.GetCoordinateInList(
                            destinationCoordinate,
                            possibleMoves
                        );

                        if (moveIndex != -1)
                            inputMove = possibleMoves[moveIndex];
                    }

                    if (moveIndex == -1)
                    {
                        continue;
                    }
                }
                else
                {
                    throw new Exception("$ AY: Wrong inputFormat!");
                }

                Board.ClearPossibleMoves(possibleMoves);

                Board.DoMove(inputMove);

                if (Board.IsMated(opponent.color))
                {
                    EndOfGame(currentPlayer);
                }

                moveCounter += 1;
            }
        }

        public static void EndOfGame(Player winner)
        {
            Board.PrintBoard();
            System.Console.WriteLine(
                winner.name + " with color " + winner.color + " won the Game!"
            );
            System.Console.WriteLine("Good Night Kids!");
            System.Environment.Exit(0);
        }
    }

    public class Player
    {
        public string name;
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

        public static Piece ChoosePiece(PieceColor color)
        {
            System.Console.WriteLine("1.Queen, 2.Rook, 3.Bishop, 4.Knight");
            Piece newPiece = Board.blankPiece;
            while (newPiece == Board.blankPiece)
            {
                System.Console.Write("Choose new piece by number: ");
                string command = Console.ReadLine();
                if (command == "1")
                {
                    newPiece = new Piece(PieceName.QUEEN, color);
                }
                else if (command == "2")
                {
                    newPiece = new Piece(PieceName.ROOK, color);
                }
                else if (command == "3")
                {
                    newPiece = new Piece(PieceName.BISHOP, color);
                }
                else if (command == "4")
                {
                    newPiece = new Piece(PieceName.KNIGHT, color);
                }
            }
            return newPiece;
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

        public static bool whiteKingFirstMoveDone = false;
        public static bool blackKingFirstMoveDone = false;

        public static bool H1FirstMoveDone = false;
        public static bool A1FirstMoveDone = false;
        public static bool H8FirstMoveDone = false;
        public static bool A8FirstMoveDone = false;

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

        public static void InsertPossibleMoves(List<Move> possibleMoves)
        {
            foreach (var p in possibleMoves)
                if (
                    p.type == MoveType.MOVEMENT
                    || p.type == MoveType.SMALLCASTLE
                    || p.type == MoveType.BIGCASTLE
                )
                    Board.InsertPiece(p.to, Board.possiblePiece);
        }

        public static void DoMove(Move move)
        {
            Piece piece = GetPiece(move.from);
            InsertPiece(move.from, blankPiece);
            InsertPiece(move.to, piece);

            if (piece.color == PieceColor.WHITE)
            {
                if (move.from.isEqual(Coordinate.FromString("h1")))
                    H1FirstMoveDone = true;
                else if (piece.abbrivation == "W.K")
                    whiteKingFirstMoveDone = true;
                else if (move.from.isEqual(Coordinate.FromString("a1")))
                    A1FirstMoveDone = true;
                else if (move.to.isEqual(Coordinate.FromString("h8")))
                    H8FirstMoveDone = true;
                else if (move.to.isEqual(Coordinate.FromString("a8")))
                    A8FirstMoveDone = true;

                if (move.type == MoveType.SMALLCASTLE)
                {
                    piece = GetPiece(Coordinate.FromString("h1"));
                    InsertPiece(Coordinate.FromString("h1"), blankPiece);
                    InsertPiece(Coordinate.FromString("f1"), piece);
                }
                else if (move.type == MoveType.BIGCASTLE)
                {
                    piece = GetPiece(Coordinate.FromString("a1"));
                    InsertPiece(Coordinate.FromString("a1"), blankPiece);
                    InsertPiece(Coordinate.FromString("d1"), piece);
                }
                else if (piece.name == PieceName.PAWN && move.to.y == 0)
                {
                    InsertPiece(move.to, blankPiece);
                    Piece newPiece = Piece.ChoosePiece(PieceColor.WHITE);
                    InsertPiece(move.to.x, move.to.y, newPiece);
                }
            }
            else if (piece.color == PieceColor.BLACK)
            {
                if (
                    move.from.isEqual(Coordinate.FromString("h8"))
                    || move.to.isEqual(Coordinate.FromString("h8"))
                )
                    H8FirstMoveDone = true;
                else if (
                    move.from.isEqual(Coordinate.FromString("a8"))
                    || move.to.isEqual(Coordinate.FromString("a8"))
                )
                    A8FirstMoveDone = true;
                else if (piece.abbrivation == "B.K")
                    blackKingFirstMoveDone = true;
                else if (move.to.isEqual(Coordinate.FromString("h1")))
                    H1FirstMoveDone = true;
                else if (move.to.isEqual(Coordinate.FromString("a1")))
                    A1FirstMoveDone = true;

                if (move.type == MoveType.SMALLCASTLE)
                {
                    piece = GetPiece(Coordinate.FromString("h8"));
                    InsertPiece(Coordinate.FromString("h8"), blankPiece);
                    InsertPiece(Coordinate.FromString("f8"), piece);
                }
                else if (move.type == MoveType.BIGCASTLE)
                {
                    piece = GetPiece(Coordinate.FromString("a8"));
                    InsertPiece(Coordinate.FromString("a8"), blankPiece);
                    InsertPiece(Coordinate.FromString("d8"), piece);
                }
                else if (piece.name == PieceName.PAWN && move.to.y == 7)
                {
                    InsertPiece(move.to, blankPiece);
                    Piece newPiece = Piece.ChoosePiece(PieceColor.BLACK);
                    InsertPiece(move.to.x, move.to.y, newPiece);
                }
            }
        }

        public static void ClearPossibleMoves(List<Move> possibleMoves)
        {
            foreach (var p in possibleMoves)
            {
                if (
                    p.type == MoveType.MOVEMENT
                    || p.type == MoveType.SMALLCASTLE
                    || p.type == MoveType.BIGCASTLE
                )
                    InsertPiece(p.to, blankPiece);
            }
            possibleMoves.Clear();
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

        public static bool IsOpponentPiece(int inX, int inY, PieceColor color)
        {
            return IsCoordinateInBoard(inX, inY)
                && !IsBlank(inX, inY)
                && GetPiece(inX, inY).color != color;
        }

        public static bool IsOpponentPiece(Coordinate coordinate, PieceColor color)
        {
            return IsOpponentPiece(coordinate.x, coordinate.y, color);
        }

        public static bool IsCheckOkAfterMove(Move move)
        {
            Piece fromPiece = GetPiece(move.from);
            Piece toPiece = GetPiece(move.to);
            InsertPiece(move.from, blankPiece);
            InsertPiece(move.to, fromPiece);

            if (IsChecked(fromPiece.color))
            {
                InsertPiece(move.to, toPiece);
                InsertPiece(move.from, fromPiece);
                return false;
            }

            InsertPiece(move.to, toPiece);
            InsertPiece(move.from, fromPiece);
            return true;
        }

        public static bool IsCheckOkForSmallCastle(PieceColor color)
        {
            string strColor;
            if (color == PieceColor.WHITE)
                strColor = "W";
            else
                strColor = "B";

            Coordinate kingCoordinate = GetCoordinates(strColor + ".K")[0];
            Piece king = GetPiece(kingCoordinate);

            InsertPiece(kingCoordinate, blankPiece);
            InsertPiece(kingCoordinate.x + 1, kingCoordinate.y, king);
            if (IsChecked(king.color))
            {
                InsertPiece(kingCoordinate.x + 1, kingCoordinate.y, blankPiece);
                InsertPiece(kingCoordinate, king);
                return false;
            }

            InsertPiece(kingCoordinate.x + 1, kingCoordinate.y, blankPiece);
            InsertPiece(kingCoordinate.x + 2, kingCoordinate.y, king);

            if (IsChecked(king.color))
            {
                InsertPiece(kingCoordinate.x + 2, kingCoordinate.y, blankPiece);
                InsertPiece(kingCoordinate, king);
                return false;
            }

            InsertPiece(kingCoordinate.x + 2, kingCoordinate.y, blankPiece);
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

            Coordinate kingCoordinate = GetCoordinates(strColor + ".K")[0];
            Piece king = GetPiece(kingCoordinate);

            InsertPiece(kingCoordinate, blankPiece);
            InsertPiece(kingCoordinate.x - 1, kingCoordinate.y, king);
            if (IsChecked(king.color))
            {
                InsertPiece(kingCoordinate.x - 1, kingCoordinate.y, blankPiece);
                InsertPiece(kingCoordinate, king);
                return false;
            }

            InsertPiece(kingCoordinate.x - 1, kingCoordinate.y, blankPiece);
            InsertPiece(kingCoordinate.x - 2, kingCoordinate.y, king);

            if (IsChecked(king.color))
            {
                InsertPiece(kingCoordinate.x - 2, kingCoordinate.y, blankPiece);
                InsertPiece(kingCoordinate, king);
                return false;
            }

            InsertPiece(kingCoordinate.x - 2, kingCoordinate.y, blankPiece);
            InsertPiece(kingCoordinate, king);

            return true;
        }

        public static List<Move> AllPossibleMoves(PieceColor color)
        {
            List<Move> possibleMoves = new List<Move>();
            string sColor = Stringer.strColor(color);

            List<Coordinate> coordinates = new List<Coordinate>();
            coordinates = GetCoordinates(sColor + ".N");
            foreach (var coordinate in coordinates)
            {
                possibleMoves.AddRange(PiecePossibleMoves(coordinate));
            }

            coordinates = GetCoordinates(sColor + ".B");
            foreach (var coordinate in coordinates)
            {
                possibleMoves.AddRange(PiecePossibleMoves(coordinate));
            }

            coordinates = GetCoordinates(sColor + ".R");
            foreach (var coordinate in coordinates)
            {
                possibleMoves.AddRange(PiecePossibleMoves(coordinate));
            }

            coordinates = GetCoordinates(sColor + ".Q");
            foreach (var coordinate in coordinates)
            {
                possibleMoves.AddRange(PiecePossibleMoves(coordinate));
            }

            coordinates = GetCoordinates(sColor + ".P");
            foreach (var coordinate in coordinates)
            {
                possibleMoves.AddRange(PiecePossibleMoves(coordinate));
            }

            coordinates = GetCoordinates(sColor + ".K");
            foreach (var coordinate in coordinates)
            {
                possibleMoves.AddRange(PiecePossibleMoves(coordinate));
            }

            return possibleMoves;
        }

        public static List<Move> PiecePossibleMoves(Coordinate coordinate)
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
                if (piece.name == PieceName.KNIGHT)
                {
                    return KnightPossibleMoves(coordinate, piece.color);
                }
                else if (piece.name == PieceName.BISHOP)
                {
                    return BishopPossibleMoves(coordinate, piece.color);
                }
                else if (piece.name == PieceName.ROOK)
                {
                    return RookPossibleMoves(coordinate, piece.color);
                }
                else if (piece.name == PieceName.QUEEN)
                {
                    return QueenPossibleMoves(coordinate, piece.color);
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

        public static List<Move> WhitePawnPossibleMoves(Coordinate coordinate)
        {
            List<Move> possibles = new List<Move>();

            // movements
            if (
                coordinate.y == 6
                && IsBlank(coordinate.x, coordinate.y - 1)
                && IsBlank(coordinate.x, coordinate.y - 2)
            )
            {
                Coordinate to = new Coordinate(coordinate.x, coordinate.y - 2);
                Move move = new Move(coordinate, to, MoveType.MOVEMENT);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(new Move(coordinate, to, MoveType.MOVEMENT));
            }

            if (IsDestinationOk(coordinate.x, coordinate.y - 1))
            {
                Coordinate to = new Coordinate(coordinate.x, coordinate.y - 1);
                Move move = new Move(coordinate, to, MoveType.MOVEMENT);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(new Move(coordinate, to, MoveType.MOVEMENT));
            }

            // captures
            if (IsOpponentPiece(coordinate.x - 1, coordinate.y - 1, PieceColor.WHITE))
            {
                Coordinate to = new Coordinate(coordinate.x - 1, coordinate.y - 1);
                Move move = new Move(coordinate, to, MoveType.CAPTURE);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(new Move(coordinate, to, MoveType.CAPTURE));
            }

            if (IsOpponentPiece(coordinate.x + 1, coordinate.y - 1, PieceColor.WHITE))
            {
                Coordinate to = new Coordinate(coordinate.x + 1, coordinate.y - 1);
                Move move = new Move(coordinate, to, MoveType.CAPTURE);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(new Move(coordinate, to, MoveType.CAPTURE));
            }

            return possibles;
        }

        public static List<Move> BlackPawnPossibleMoves(Coordinate coordinate)
        {
            List<Move> possibles = new List<Move>();

            // movements
            if (
                coordinate.y == 1
                && IsBlank(coordinate.x, coordinate.y + 1)
                && IsBlank(coordinate.x, coordinate.y + 2)
            )
            {
                Coordinate to = new Coordinate(coordinate.x, coordinate.y + 2);
                Move move = new Move(coordinate, to, MoveType.MOVEMENT);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(new Move(coordinate, to, MoveType.MOVEMENT));
            }

            if (IsDestinationOk(coordinate.x, coordinate.y + 1))
            {
                Coordinate to = new Coordinate(coordinate.x, coordinate.y + 1);
                Move move = new Move(coordinate, to, MoveType.MOVEMENT);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(new Move(coordinate, to, MoveType.MOVEMENT));
            }

            // captures
            if (IsOpponentPiece(coordinate.x - 1, coordinate.y + 1, PieceColor.BLACK))
            {
                Coordinate to = new Coordinate(coordinate.x - 1, coordinate.y + 1);
                Move move = new Move(coordinate, to, MoveType.CAPTURE);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(new Move(coordinate, to, MoveType.CAPTURE));
            }

            if (IsOpponentPiece(coordinate.x + 1, coordinate.y + 1, PieceColor.BLACK))
            {
                Coordinate to = new Coordinate(coordinate.x + 1, coordinate.y + 1);
                Move move = new Move(coordinate, to, MoveType.CAPTURE);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(new Move(coordinate, to, MoveType.CAPTURE));
            }

            return possibles;
        }

        public static List<Move> KnightPossibleMoves(Coordinate coordinate, PieceColor color)
        {
            List<Move> possibles = new List<Move>();
            Coordinate current = new Coordinate(0, 0);
            Coordinate to = new Coordinate(0, 0);

            current.x = coordinate.x + 1;
            current.y = coordinate.y + 2;
            if (IsDestinationOk(current))
            {
                Move move = new Move(coordinate, current.Copy(), MoveType.MOVEMENT);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(move);
            }
            else if (IsOpponentPiece(current, color))
            {
                Move move = new Move(coordinate, current.Copy(), MoveType.CAPTURE);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(move);
            }

            current.x = coordinate.x + 2;
            current.y = coordinate.y + 1;
            if (IsDestinationOk(current))
            {
                Move move = new Move(coordinate, current.Copy(), MoveType.MOVEMENT);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(move);
            }
            else if (IsOpponentPiece(current, color))
            {
                Move move = new Move(coordinate, current.Copy(), MoveType.CAPTURE);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(move);
            }

            current.x = coordinate.x - 1;
            current.y = coordinate.y + 2;
            if (IsDestinationOk(current))
            {
                Move move = new Move(coordinate, current.Copy(), MoveType.MOVEMENT);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(move);
            }
            else if (IsOpponentPiece(current, color))
            {
                Move move = new Move(coordinate, current.Copy(), MoveType.CAPTURE);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(move);
            }

            current.x = coordinate.x - 2;
            current.y = coordinate.y + 1;
            if (IsDestinationOk(current))
            {
                Move move = new Move(coordinate, current.Copy(), MoveType.MOVEMENT);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(move);
            }
            else if (IsOpponentPiece(current, color))
            {
                Move move = new Move(coordinate, current.Copy(), MoveType.CAPTURE);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(move);
            }

            current.x = coordinate.x + 1;
            current.y = coordinate.y - 2;
            if (IsDestinationOk(current))
            {
                Move move = new Move(coordinate, current.Copy(), MoveType.MOVEMENT);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(move);
            }
            else if (IsOpponentPiece(current, color))
            {
                Move move = new Move(coordinate, current.Copy(), MoveType.CAPTURE);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(move);
            }

            current.x = coordinate.x + 2;
            current.y = coordinate.y - 1;
            if (IsDestinationOk(current))
            {
                Move move = new Move(coordinate, current.Copy(), MoveType.MOVEMENT);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(move);
            }
            else if (IsOpponentPiece(current, color))
            {
                Move move = new Move(coordinate, current.Copy(), MoveType.CAPTURE);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(move);
            }

            current.x = coordinate.x - 1;
            current.y = coordinate.y - 2;
            if (IsDestinationOk(current))
            {
                Move move = new Move(coordinate, current.Copy(), MoveType.MOVEMENT);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(move);
            }
            else if (IsOpponentPiece(current, color))
            {
                Move move = new Move(coordinate, current.Copy(), MoveType.CAPTURE);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(move);
            }

            current.x = coordinate.x - 2;
            current.y = coordinate.y - 1;
            if (IsDestinationOk(current))
            {
                Move move = new Move(coordinate, current.Copy(), MoveType.MOVEMENT);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(move);
            }
            else if (IsOpponentPiece(current, color))
            {
                Move move = new Move(coordinate, current.Copy(), MoveType.CAPTURE);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(move);
            }

            return possibles;
        }

        public static List<Move> BishopPossibleMoves(Coordinate coordinate, PieceColor color)
        {
            List<Move> possibles = new List<Move>();
            Coordinate current = new Coordinate(0, 0);

            current.x = coordinate.x;
            current.y = coordinate.y;
            while (true)
            {
                current.x += 1;
                current.y += 1;
                if (!IsCoordinateInBoard(current.x, current.y))
                    break;

                Piece currentPiece = GetPiece(current);

                if (currentPiece.abbrivation != blankPiece.abbrivation)
                    break;

                if (IsDestinationOk(current))
                {
                    Move move = new Move(coordinate, current.Copy(), MoveType.MOVEMENT);
                    if (IsCheckOkAfterMove(move))
                        possibles.Add(move);
                }
            }

            if (IsOpponentPiece(current, color))
            {
                Move move = new Move(coordinate, current.Copy(), MoveType.CAPTURE);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(move);
            }

            current.x = coordinate.x;
            current.y = coordinate.y;
            while (true)
            {
                current.x -= 1;
                current.y += 1;
                if (!IsCoordinateInBoard(current.x, current.y))
                    break;

                Piece currentPiece = GetPiece(current);

                if (currentPiece.abbrivation != blankPiece.abbrivation)
                    break;

                if (IsDestinationOk(current))
                {
                    Move move = new Move(coordinate, current.Copy(), MoveType.MOVEMENT);
                    if (IsCheckOkAfterMove(move))
                        possibles.Add(move);
                }
            }

            if (IsOpponentPiece(current, color))
            {
                Move move = new Move(coordinate, current.Copy(), MoveType.CAPTURE);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(move);
            }

            current.x = coordinate.x;
            current.y = coordinate.y;
            while (true)
            {
                current.x += 1;
                current.y -= 1;
                if (!IsCoordinateInBoard(current.x, current.y))
                    break;

                Piece currentPiece = GetPiece(current);

                if (currentPiece.abbrivation != blankPiece.abbrivation)
                    break;

                if (IsDestinationOk(current))
                {
                    Move move = new Move(coordinate, current.Copy(), MoveType.MOVEMENT);
                    if (IsCheckOkAfterMove(move))
                        possibles.Add(move);
                }
            }

            if (IsOpponentPiece(current, color))
            {
                Move move = new Move(coordinate, current.Copy(), MoveType.CAPTURE);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(move);
            }

            current.x = coordinate.x;
            current.y = coordinate.y;
            while (true)
            {
                current.x -= 1;
                current.y -= 1;
                if (!IsCoordinateInBoard(current.x, current.y))
                    break;

                Piece currentPiece = GetPiece(current);

                if (currentPiece.abbrivation != blankPiece.abbrivation)
                    break;

                if (IsDestinationOk(current))
                {
                    Move move = new Move(coordinate, current.Copy(), MoveType.MOVEMENT);
                    if (IsCheckOkAfterMove(move))
                        possibles.Add(move);
                }
            }

            if (IsOpponentPiece(current, color))
            {
                Move move = new Move(coordinate, current.Copy(), MoveType.CAPTURE);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(move);
            }

            return possibles;
        }

        public static List<Move> RookPossibleMoves(Coordinate coordinate, PieceColor color)
        {
            List<Move> possibles = new List<Move>();
            Coordinate current = new Coordinate(0, 0);

            current.x = coordinate.x;
            current.y = coordinate.y;
            while (true)
            {
                current.x += 1;
                if (!IsCoordinateInBoard(current.x, current.y))
                    break;

                Piece currentPiece = GetPiece(current);

                if (currentPiece.abbrivation != blankPiece.abbrivation)
                    break;

                if (IsDestinationOk(current))
                {
                    Move move = new Move(coordinate, current.Copy(), MoveType.MOVEMENT);
                    if (IsCheckOkAfterMove(move))
                        possibles.Add(move);
                }
            }

            if (IsOpponentPiece(current, color))
            {
                Move move = new Move(coordinate, current.Copy(), MoveType.CAPTURE);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(move);
            }

            current.x = coordinate.x;
            current.y = coordinate.y;
            while (true)
            {
                current.y += 1;
                if (!IsCoordinateInBoard(current.x, current.y))
                    break;

                Piece currentPiece = GetPiece(current);

                if (currentPiece.abbrivation != blankPiece.abbrivation)
                    break;

                if (IsDestinationOk(current))
                {
                    Move move = new Move(coordinate, current.Copy(), MoveType.MOVEMENT);
                    if (IsCheckOkAfterMove(move))
                        possibles.Add(move);
                }
            }

            if (IsOpponentPiece(current, color))
            {
                Move move = new Move(coordinate, current.Copy(), MoveType.CAPTURE);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(move);
            }

            current.x = coordinate.x;
            current.y = coordinate.y;
            while (true)
            {
                current.x -= 1;
                if (!IsCoordinateInBoard(current.x, current.y))
                    break;

                Piece currentPiece = GetPiece(current);

                if (currentPiece.abbrivation != blankPiece.abbrivation)
                    break;

                if (IsDestinationOk(current))
                {
                    Move move = new Move(coordinate, current.Copy(), MoveType.MOVEMENT);
                    if (IsCheckOkAfterMove(move))
                        possibles.Add(move);
                }
            }

            if (IsOpponentPiece(current, color))
            {
                Move move = new Move(coordinate, current.Copy(), MoveType.CAPTURE);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(move);
            }

            current.x = coordinate.x;
            current.y = coordinate.y;
            while (true)
            {
                current.y -= 1;
                if (!IsCoordinateInBoard(current.x, current.y))
                    break;

                Piece currentPiece = GetPiece(current);

                if (currentPiece.abbrivation != blankPiece.abbrivation)
                    break;

                if (IsDestinationOk(current))
                {
                    Move move = new Move(coordinate, current.Copy(), MoveType.MOVEMENT);
                    if (IsCheckOkAfterMove(move))
                        possibles.Add(move);
                }
            }

            if (IsOpponentPiece(current, color))
            {
                Move move = new Move(coordinate, current.Copy(), MoveType.CAPTURE);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(move);
            }

            return possibles;
        }

        public static List<Move> QueenPossibleMoves(Coordinate coordinate, PieceColor color)
        {
            List<Move> possibles = new List<Move>();

            possibles.AddRange(BishopPossibleMoves(coordinate, color));
            possibles.AddRange(RookPossibleMoves(coordinate, color));

            return possibles;
        }

        public static List<Move> KingUsualPossibleMoves(Coordinate coordinate, PieceColor color)
        {
            List<Move> possibles = new List<Move>();
            Coordinate current = new Coordinate(0, 0);

            current.x = coordinate.x + 1;
            current.y = coordinate.y;
            if (IsDestinationOk(current))
            {
                Move move = new Move(coordinate, current.Copy(), MoveType.MOVEMENT);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(move);
            }
            else if (IsOpponentPiece(current, color))
            {
                Move move = new Move(coordinate, current.Copy(), MoveType.CAPTURE);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(move);
            }

            current.x = coordinate.x + 1;
            current.y = coordinate.y + 1;
            if (IsDestinationOk(current))
            {
                Move move = new Move(coordinate, current.Copy(), MoveType.MOVEMENT);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(move);
            }
            else if (IsOpponentPiece(current, color))
            {
                Move move = new Move(coordinate, current.Copy(), MoveType.CAPTURE);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(move);
            }

            current.x = coordinate.x;
            current.y = coordinate.y + 1;
            if (IsDestinationOk(current))
            {
                Move move = new Move(coordinate, current.Copy(), MoveType.MOVEMENT);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(move);
            }
            else if (IsOpponentPiece(current, color))
            {
                Move move = new Move(coordinate, current.Copy(), MoveType.CAPTURE);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(move);
            }

            current.x = coordinate.x - 1;
            current.y = coordinate.y + 1;
            if (IsDestinationOk(current))
            {
                Move move = new Move(coordinate, current.Copy(), MoveType.MOVEMENT);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(move);
            }
            else if (IsOpponentPiece(current, color))
            {
                Move move = new Move(coordinate, current.Copy(), MoveType.CAPTURE);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(move);
            }

            current.x = coordinate.x - 1;
            current.y = coordinate.y;
            if (IsDestinationOk(current))
            {
                Move move = new Move(coordinate, current.Copy(), MoveType.MOVEMENT);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(move);
            }
            else if (IsOpponentPiece(current, color))
            {
                Move move = new Move(coordinate, current.Copy(), MoveType.CAPTURE);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(move);
            }

            current.x = coordinate.x - 1;
            current.y = coordinate.y - 1;
            if (IsDestinationOk(current))
            {
                Move move = new Move(coordinate, current.Copy(), MoveType.MOVEMENT);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(move);
            }
            else if (IsOpponentPiece(current, color))
            {
                Move move = new Move(coordinate, current.Copy(), MoveType.CAPTURE);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(move);
            }

            current.x = coordinate.x;
            current.y = coordinate.y - 1;
            if (IsDestinationOk(current))
            {
                Move move = new Move(coordinate, current.Copy(), MoveType.MOVEMENT);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(move);
            }
            else if (IsOpponentPiece(current, color))
            {
                Move move = new Move(coordinate, current.Copy(), MoveType.CAPTURE);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(move);
            }

            current.x = coordinate.x + 1;
            current.y = coordinate.y - 1;
            if (IsDestinationOk(current))
            {
                Move move = new Move(coordinate, current.Copy(), MoveType.MOVEMENT);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(move);
            }
            else if (IsOpponentPiece(current, color))
            {
                Move move = new Move(coordinate, current.Copy(), MoveType.CAPTURE);
                if (IsCheckOkAfterMove(move))
                    possibles.Add(move);
            }

            return possibles;
        }

        public static List<Move> WhiteKingPossibleMoves(Coordinate coordinate)
        {
            List<Move> possibles = new List<Move>();

            possibles.AddRange(KingUsualPossibleMoves(coordinate, PieceColor.WHITE));

            possibles.AddRange(WhiteKingPossibleCastle());

            return possibles;
        }

        public static List<Move> WhiteKingPossibleCastle()
        {
            List<Move> possibles = new List<Move>();
            if (!whiteKingFirstMoveDone && !IsChecked(PieceColor.WHITE))
            {
                if (
                    IsBlank(Coordinate.FromString("f1"))
                    && IsBlank(Coordinate.FromString("g1"))
                    && IsCheckOkForSmallCastle(PieceColor.WHITE)
                    && !H1FirstMoveDone
                )
                    possibles.Add(
                        new Move(
                            Coordinate.FromString("e1"),
                            Coordinate.FromString("g1"),
                            MoveType.SMALLCASTLE
                        )
                    );

                if (
                    IsBlank(Coordinate.FromString("c1"))
                    && IsBlank(Coordinate.FromString("d1"))
                    && IsCheckOKForBigCastle(PieceColor.WHITE)
                    && !A1FirstMoveDone
                )
                    possibles.Add(
                        new Move(
                            Coordinate.FromString("e1"),
                            Coordinate.FromString("c1"),
                            MoveType.BIGCASTLE
                        )
                    );
            }

            return possibles;
        }

        public static List<Move> BlackKingPossibleMoves(Coordinate coordinate)
        {
            List<Move> possibles = new List<Move>();

            possibles.AddRange(KingUsualPossibleMoves(coordinate, PieceColor.BLACK));

            possibles.AddRange(BlackKingPossibleCastle());

            return possibles;
        }

        public static List<Move> BlackKingPossibleCastle()
        {
            List<Move> possibles = new List<Move>();
            if (!blackKingFirstMoveDone && !IsChecked(PieceColor.BLACK))
            {
                if (
                    IsBlank(Coordinate.FromString("f8"))
                    && IsBlank(Coordinate.FromString("g8"))
                    && IsCheckOkForSmallCastle(PieceColor.BLACK)
                    && !H8FirstMoveDone
                )
                    possibles.Add(
                        new Move(
                            Coordinate.FromString("e8"),
                            Coordinate.FromString("g8"),
                            MoveType.SMALLCASTLE
                        )
                    );

                if (
                    IsBlank(Coordinate.FromString("c8"))
                    && IsBlank(Coordinate.FromString("d8"))
                    && IsCheckOKForBigCastle(PieceColor.BLACK)
                    && !A8FirstMoveDone
                )
                    possibles.Add(
                        new Move(
                            Coordinate.FromString("e8"),
                            Coordinate.FromString("c8"),
                            MoveType.BIGCASTLE
                        )
                    );
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

            Coordinate kingCoordinate = GetCoordinates(friendColor + ".K")[0];

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

        public static bool IsMated(PieceColor color)
        {
            return AllPossibleMoves(color).Count == 0;
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

        public static List<Coordinate> GetCoordinates(string abbrivation)
        {
            List<Coordinate> coordinates = new List<Coordinate>();
            for (int i = 0; i < rowsCount; i++)
                for (int j = 0; j < colsCount; j++)
                    if (GetPiece(i, j).abbrivation == abbrivation)
                        coordinates.Add(new Coordinate(i, j));

            return coordinates;
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

        public static int GetCoordinateInList(Coordinate coordinate, List<Move> moves)
        {
            for (int i = 0; i < moves.Count; i++)
                if (moves[i].to.isEqual(coordinate))
                    return i;

            return -1;
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
        public MoveType type;

        public static Move noMove = new Move(
            Board.notFoundCoordinate,
            Board.notFoundCoordinate,
            MoveType.NONE
        );

        public Move(Coordinate from, Coordinate to, MoveType type)
        {
            this.from = from;
            this.to = to;
            this.type = type;
        }

        public static void PrintList(List<Move> moves)
        {
          for (int i = 0; i < moves.Count; i++)
          {
            Console.WriteLine(moves[i].ToString());
          }
        }

        public bool IsEqual(Move move)
        {
            return from == move.from && to == move.to && type == move.type;
        }

        public override string ToString()
        {
            return "Move: {from: " + from + ", to: " + to + ", type: " + type + "}";
        }
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

        public static string strColor(PieceColor color)
        {
            if (color == PieceColor.WHITE)
                return "W";
            else if (color == PieceColor.BLACK)
                return "B";
            else
                throw new Exception("$ AY: Bad Color");
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
