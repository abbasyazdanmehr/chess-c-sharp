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

            Player player1 = new Player("Player 1", PieceColor.WHITE);
            Player player2 = new Player("Player 2", PieceColor.BLACK);

            Player current = player1;

            int moveCounter = 0;

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
                System.Console.WriteLine(selectedCoordinate.x + ", " + selectedCoordinate.y);

                Piece selectedPiece = Board.GetPiece(selectedCoordinate);
                System.Console.WriteLine(selectedPiece);

                // List<Coordinate> possibles = PiecePossibleMoves(selectedPiece, selectedCoordinate);
                // InsertPossibles(possibles);
            }

            return 0;
        }

        public static void InsertPossibles(List<Coordinate> possibles)
        {
            foreach (var possible in possibles)
            {
                Board.InsertPiece(possible, Board.possiblePiece);
            }
        }

        public static List<Coordinate> PiecePossibleMoves(Piece piece, Coordinate coordinate)
        {
            // switch case is bullshit in syntax (but better at performance)
            if (piece.name == PieceName.PAWN)
            {
                return PAWNPassibleMoves(coordinate, piece.color);
            }
            // else if (PieceName == PieceName.KING)
            // {
            //     return KINGPassibleMoves(coordinate);
            // }
            // else if (PieceName == PieceName.QUEEN)
            // {
            //     return QUEENPassibleMoves(coordinate);
            // }
            // else if (PieceName == PieceName.KNIGHT)
            // {
            //     return KNIGHTPassibleMoves(coordinate);
            // }
            // else if (PieceName == PieceName.BISHOP)
            // {
            //     return BISHOPPassibleMoves(coordinate);
            // }
            // else if (PieceName == PieceName.ROOK)
            // {
            //     return ROOKPassibleMoves(coordinate);
            // }
            else
            {
                throw new Exception("Wrong Piece in PiecePossibleMoves");
            }
        }

        public static List<Coordinate> PAWNPassibleMoves(Coordinate coordinate, PieceColor color)
        {
            List<Coordinate> possibles = new List<Coordinate>();
            if (color == PieceColor.WHITE)
            {
                if (coordinate.y == 6)
                {
                    possibles.Add(new Coordinate(coordinate.x, coordinate.y - 2));
                }
                possibles.Add(new Coordinate(coordinate.x, coordinate.y - 1));
            }
            return possibles;
        }

        // public List<Coordinate> KINGPassibleMoves(Coordinate coordinate) { }

        // public List<Coordinate> QUEENPassibleMoves(Coordinate coordinate) { }

        // public List<Coordinate> KNIGHTPassibleMoves(Coordinate coordinate) { }

        // public List<Coordinate> BISHOPPassibleMoves(Coordinate coordinate) { }

        // public List<Coordinate> ROOKPassibleMoves(Coordinate coordinate) { }

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

        public static void PrintBoard()
        {
            System.Console.Write("+");
            System.Console.WriteLine(Stringer.StringRepeat("-----+", colsCount));

            for (int i = 0; i < rowsCount; i++)
            {
                System.Console.Write("|");
                System.Console.WriteLine(Stringer.StringRepeat("     |", colsCount));

                System.Console.Write("| ");
                for (int j = 0; j < colsCount; j++)
                {
                    System.Console.Write(board[(i * colsCount) + j].abbrivation + " | ");
                }
                System.Console.WriteLine();

                System.Console.Write("|");
                System.Console.WriteLine(Stringer.StringRepeat("     |", colsCount));

                System.Console.Write("+");
                System.Console.WriteLine(Stringer.StringRepeat("-----+", colsCount));
            }
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

        public override string ToString()
        {
            return x + ", " + y;
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
    }
}
