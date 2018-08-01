using System;
using System.Collections.Generic;
using System.Linq;

namespace Kvam.Chess.Core
{
    public class Game
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int WhitePlayerIdentifier { get; set; }
        public int BlackPlayerIdentifier { get; set; }

        public List<string> Moves { get; set; }

        public static Game CreateGame()
        {
        var game = new Game
                           {
                               Id = Guid.NewGuid().GetHashCode(),
                               WhitePlayerIdentifier = Guid.NewGuid().GetHashCode(),
                               BlackPlayerIdentifier = Guid.NewGuid().GetHashCode(),
                               Moves = new List<string>(),
                               Pieces = new Dictionary<string, string>
                                            {
{"a1", "wr" }, {"b1" ,"wn" }, {"c1", "wb" }, {"d1" ,"wq" }, {"e1", "wk" }, {"f1" ,"wb" }, {"g1", "wn" }, {"h1" ,"wr" },
{"a2", "wp" }, {"b2" ,"wp" }, {"c2", "wp" }, {"d2" ,"wp" }, {"e2", "wp" }, {"f2" ,"wp" }, {"g2", "wp" }, {"h2" ,"wp" },

{"a8", "br" }, {"b8" ,"bn" }, {"c8", "bb" }, {"d8" ,"bq" }, {"e8", "bk" }, {"f8" ,"bb" }, {"g8", "bn" }, {"h8" ,"br" },
{"a7", "bp" }, {"b7" ,"bp" }, {"c7", "bp" }, {"d7" ,"bp" }, {"e7", "bp" }, {"f7" ,"bp" }, {"g7", "bp" }, {"h7" ,"bp" }
                                            }
                           };

            if (Games.ContainsKey(game.Id))
            {
                throw new Exception("Game already exists.");
            }

            Games.Add(game.Id, game);
            return game;
        }

        private static readonly Dictionary<int, Game> Games = new Dictionary<int, Game>();

        public void RegisterMove(string p, bool removePiece = true)
        {
            if (p == "undefined")
                return;

            Moves.Add(p);

            var move = p.Split('-');
            Pieces[move[1]] = Pieces[move[0]];
            Pieces.Remove(move[0]);

            if (move.Length == 3)
            {
              Pieces[move[1]] = move[2];
            }
        }

        public Dictionary<string, string> Pieces { get; set; }

        public void UnregisterPreviousMove()
        {
            Moves.RemoveAt(Moves.Count - 1);
        }

        public static object ListLegalMoves(int gameId, int playerId, string lastMove)
        {
            if (!Games.ContainsKey(gameId))
            {
                throw new Exception("Game not found.");
            }
            var game = Games[gameId];

            string statusMessage = "";

            bool isInCheck = false;

            if (lastMove != null)
            {
                game.RegisterMove(lastMove);

                var v = new MoveValidator(game);
                v.Turn = v.Turn == "w" ? "b" : "w";
                var moves = v.CalculateValidMoves().ToList();

                var kings = game.Pieces.Where(x => x.Value.EndsWith("k"));
                isInCheck = kings.Aggregate(false, (current, king) => current || moves.Any(x => x.EndsWith(king.Key)));
            }
            if (isInCheck)
            {
                statusMessage = "Check!";
            }

            var validator = new MoveValidator(game);
            var tentativeMoves = validator.CalculateValidMoves().ToList();
            var verifiedMoves = new List<string>();
            foreach (var move in tentativeMoves)
            {
                var v = new MoveValidator(game);

                var moveBackup = game.Pieces.ToDictionary(x => x.Key, x => x.Value);

                game.RegisterMove(move);
                v.Turn = v.Turn == "w" ? "b" : "w";

                var moves = v.CalculateValidMoves().ToList();
                var kings = game.Pieces.Where(x => x.Value.EndsWith("k"));
                isInCheck = kings.Aggregate(false, (current, king) => current || moves.Any(x => x.EndsWith(king.Key)));

                game.UnregisterPreviousMove();
                game.Pieces = moveBackup;
                if (!isInCheck)
                {
                    verifiedMoves.Add(move);
                }
            }

            if (verifiedMoves.Count == 0)
            {
                statusMessage = isInCheck ? "Checkmate!" : "Draw: No more moves possible.";
            }

            //king versus king
            //king and bishop versus king
            //king and knight versus king
            //king and bishop versus king and bishop with the bishops on the same colour. (Any number of additional bishops of either color on the same color of square due to underpromotion do not affect the situation.)
            else if
                (
                    (game.Pieces.Count == 2) ||
                    (game.Pieces.Count == 3 && (game.Pieces.ContainsValue("wb") || game.Pieces.ContainsValue("bb"))) ||
                    (game.Pieces.Count == 3 && (game.Pieces.ContainsValue("wn") || game.Pieces.ContainsValue("bn"))) ||
                    (game.Pieces.Count == 4 && (game.Pieces.ContainsValue("wb") && game.Pieces.ContainsValue("bb")))
                )
            {
                statusMessage = "Draw: Checkmate impossible (insufficent material.)";
                verifiedMoves.Clear();
            }
            return new MoveWrapper {StatusMessage = statusMessage, Moves = verifiedMoves};
        }
    }
}
