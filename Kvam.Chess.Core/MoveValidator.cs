using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Kvam.Chess.Core
{
    public class MoveValidator
    {
        public Dictionary<string, string> Pieces { get; set; }
        public string Turn { get; set; }
        public string[,] Board { get; set; }
        public List<string> Moves { get; set; }

        public MoveValidator(Game game, string turn = null)
        {
            Pieces = game.Pieces;
            if (turn == null)
            {
                Turn = game.Moves.Count() % 2 == 0 ? "w" : "b";
            }
            else
            {
                Turn = turn;
            }
            Moves = game.Moves;

            Board = TransformToArray(Pieces);


        }

        public IEnumerable<string> CalculateValidMoves()
        {
            Board = TransformToArray(Pieces);

            IEnumerable<Move> pawnMoves = CalculatePawnMoves(),
                              knightMoves = CalculateKnightMoves(),
                              rookMoves = CalculateRookMoves(),
                              bishopMoves = CalculateBishopMoves(),
                              queenMoves = CalculateQueenMoves(),
                              kingMoves = CalculateKingMoves();

            var allMoves = new HashSet<Move>(pawnMoves
                                               .Union(knightMoves)
                                               .Union(rookMoves)
                                               .Union(bishopMoves)
                                               .Union(queenMoves)
                                               .Union(kingMoves));

            return allMoves.Select(m =>
                                    string.Format("{0}-{1}",
                                                  ConvertFromXyToChessCoordinate(m.X1, m.Y1),
                                                  ConvertFromXyToChessCoordinate(m.X2, m.Y2)));
        }

        private IEnumerable<Move> CalculateKingMoves()
        {
            var knights = Pieces.Where(x => x.Value.StartsWith(Turn) && x.Value.EndsWith("k"))
                                .Select(x => ConvertFromChessCoordinateToXy(x.Key)).ToList();

            var moves =
                knights.Select(x => new Move(x.X, x.Y, x.X - 1, x.Y - 1)).Union(
                knights.Select(x => new Move(x.X, x.Y, x.X - 1, x.Y))).Union(
                knights.Select(x => new Move(x.X, x.Y, x.X - 1, x.Y + 1))).Union(
                knights.Select(x => new Move(x.X, x.Y, x.X, x.Y - 1))).Union(
                knights.Select(x => new Move(x.X, x.Y, x.X, x.Y + 1))).Union(
                knights.Select(x => new Move(x.X, x.Y, x.X + 1, x.Y - 1))).Union(
                knights.Select(x => new Move(x.X, x.Y, x.X + 1, x.Y))).Union(
                knights.Select(x => new Move(x.X, x.Y, x.X + 1, x.Y + 1)))
                .Where(x => x.X2 >= 0 && x.X2 <= 7 && x.Y2 >= 0 && x.Y2 <= 7)
                .Where(x => Board[x.X2, x.Y2] == null || Board[x.X2, x.Y2].StartsWith(Opponent)).ToList();

            foreach (var p in new[] { "w", "b" })
            {
                var col = p == "w" ? 0 : 7;
                if (Turn == p && Moves.Count(x => x.StartsWith("e" + (col + 1))) == 0)
                {
                    if (Board[1, col] == null && Board[2, col] == null && Board[3, col] == null && Board[0, col] == Turn + "r" && Moves.Count(x => x.StartsWith("a" + (col + 1))) == 0)
                    {
                        moves.Add(new Move(4, col, 2, col));
                    }
                    if (Board[5, col] == null && Board[6, col] == null && Board[7, col] == Turn + "r" && Moves.Count(x => x.StartsWith("h" + (col + 1))) == 0)
                    {
                        moves.Add(new Move(4, col, 6, col));
                    }
                }
            }

            return moves;
        }

        private IEnumerable<Move> CalculateQueenMoves()
        {
            var rookMoves = CalculateRookMoves(pieceName: "q");
            var bishopMoves = CalculateBishopMoves(pieceName: "q");
            return rookMoves.Union(bishopMoves);
        }

        private IEnumerable<Move> CalculateBishopMoves(string pieceName = "b")
        {
            var bishops = Pieces.Where(x => x.Value.StartsWith(Turn) && x.Value.EndsWith(pieceName))
                                .Select(x => ConvertFromChessCoordinateToXy(x.Key)).ToList();

            var moves = new List<Move>();
            foreach (var bishop in bishops)
            {
                foreach (var xDirection in new[] { 1, -1 })
                {
                    foreach (var yDirection in new[] { 1, -1 })
                    {
                        var x = xDirection;
                        var y = yDirection;

                        Move move;

                        while (IsInside((move = new Move(bishop.X, bishop.Y, bishop.X + x, bishop.Y + y))))
                        {
                            if (Board[move.X2, move.Y2] == null)
                            {
                                moves.Add(move);
                            }
                            else
                            {
                                if (Board[move.X2, move.Y2].StartsWith(Opponent))
                                {
                                    moves.Add(move);
                                }
                                break;
                            }
                            x += xDirection;
                            y += yDirection;

                        }
                    }
                }
            }

            return moves;
        }

        private IEnumerable<Move> CalculateRookMoves(string pieceName = "r")
        {
            var allowedToAttack = Opponent;

            var rooks = Pieces.Where(x => x.Value.StartsWith(Turn) && x.Value.EndsWith(pieceName))
                              .Select(x => ConvertFromChessCoordinateToXy(x.Key)).ToList();

            var moves = new List<Move>();
            foreach (var rook in rooks)
            {
                foreach (var direction in new[] { 1, -1 })
                {
                    for (var x = rook.X + direction; x <= 7 && x >= 0; x += direction)
                    {
                        if (Board[x, rook.Y] == null)
                        {
                            moves.Add(new Move(rook.X, rook.Y, x, rook.Y));
                        }
                        else
                        {
                            if (Board[x, rook.Y].StartsWith(allowedToAttack))
                            {
                                moves.Add(new Move(rook.X, rook.Y, x, rook.Y));
                            }
                            break;
                        }
                    }

                    for (var y = rook.Y + direction; y <= 7 && y >= 0; y += direction)
                    {
                        if (Board[rook.X, y] == null)
                        {
                            moves.Add(new Move(rook.X, rook.Y, rook.X, y));
                        }
                        else
                        {
                            if (Board[rook.X, y].StartsWith(allowedToAttack))
                            {
                                moves.Add(new Move(rook.X, rook.Y, rook.X, y));
                            }
                            break;
                        }
                    }
                }
            }

            return moves;
        }

        private IEnumerable<Move> CalculateKnightMoves()
        {
            var knights = Pieces.Where(x => x.Value.StartsWith(Turn) && x.Value.EndsWith("n"))
                                .Select(x => ConvertFromChessCoordinateToXy(x.Key)).ToList();

            var moves =
                knights.Select(x => new Move(x.X, x.Y, x.X + 1, x.Y + 2)).Union(
                knights.Select(x => new Move(x.X, x.Y, x.X + 1, x.Y - 2))).Union(
                knights.Select(x => new Move(x.X, x.Y, x.X - 1, x.Y + 2))).Union(
                knights.Select(x => new Move(x.X, x.Y, x.X - 1, x.Y - 2))).Union(
                knights.Select(x => new Move(x.X, x.Y, x.X + 2, x.Y + 1))).Union(
                knights.Select(x => new Move(x.X, x.Y, x.X + 2, x.Y - 1))).Union(
                knights.Select(x => new Move(x.X, x.Y, x.X - 2, x.Y + 1))).Union(
                knights.Select(x => new Move(x.X, x.Y, x.X - 2, x.Y - 1)))
                  .Where(IsInside)
                  .Where(x => Board[x.X2, x.Y2] == null || Board[x.X2, x.Y2].StartsWith(Opponent));

            return moves;
        }

        private static bool IsInside(Move m)
        {
            return m.X2 >= 0 && m.X2 <= 7 && m.Y2 >= 0 && m.Y2 <= 7;
        }

        private IEnumerable<Move> CalculatePawnMoves()
        {
            var direction = Turn == "w" ? 1 : -1;

            var pawns = Pieces.Where(x => x.Value.StartsWith(Turn) && x.Value.EndsWith("p"))
                               .Select(x => ConvertFromChessCoordinateToXy(x.Key)).ToList();

            Func<int, bool> isInStartPosition = yPosition =>
                                                            {
                                                                if (Turn == "w" && yPosition == 1) return true;
                                                                return Turn == "b" && yPosition == 6;
                                                            };

            var moves = new List<Move>();
            foreach (var piece in pawns)
            {
                for (var i = 1; i <= 2; ++i)
                {
                    var move = new Move(piece.X, piece.Y, piece.X, piece.Y + i * direction);

                    if (!IsInside(move) || Board[move.X2, move.Y2] != null) break;

                    moves.Add(move);

                    if (!isInStartPosition(piece.Y)) break;

                }
            }

            foreach (var pos in new[] { -1, 1 })
            {
                var captureMoves = pawns.Select(p => new Move(p.X, p.Y, p.X + pos, p.Y + direction))
                    .Where(IsInside)
                    .Where(p => Board[p.X2, p.Y2] != null && Board[p.X2, p.Y2].StartsWith(Opponent));

                moves.AddRange(captureMoves);
            }

            return moves;
        }

        private string Opponent
        {
            get { return Turn == "w" ? "b" : "w"; }
        }

        private static Position ConvertFromChessCoordinateToXy(string piece)
        {
            int x = piece[0] - 97,
                y = int.Parse(piece[1].ToString(CultureInfo.InvariantCulture)) - 1;

            return new Position { X = x, Y = y };
        }

        private static string ConvertFromXyToChessCoordinate(int x, int y)
        {
            char c = (char)(x + 97),
                 d = (char)(y + '1');

            return string.Concat(c, d);
        }
        private static string[,] TransformToArray(Dictionary<string, string> pieces)
        {
            var board = new string[8, 8];
            foreach (var piece in pieces)
            {
                var position = ConvertFromChessCoordinateToXy(piece.Key);
                board[position.X, position.Y] = piece.Value;
            }
            return board;
        }
    }

    internal class Position
    {
        public int X { get; set; }

        public int Y { get; set; }
    }
}
