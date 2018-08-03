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
        public static string PiecePlaces { get; set; } = "";
        public static Game CreateGame(bool is906)
        {
            PiecePlaces = "";
            int rook1Pos = 0;
            int rook2Pos = 0;
            int bishop1Pos = 0;
            int bishop2Pos = 0;
            int knight1Pos = 0;
            int knight2Pos = 0;
            int kingPos = 0;
            int queenPos = 0;
            Random rand = new Random();
            List<int> positions = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7 };
            kingPos = rand.Next(1, 6);
            positions.Remove(kingPos);
            if (kingPos == 1)
            {
                rook1Pos = 0;
                positions.Remove(0);
                rook2Pos = positions[rand.Next(0, positions.Count - 1)];
                positions.Remove(rook2Pos);
            }
            else
            {
                rook1Pos = positions[rand.Next(0, positions.Count - 1)];
                while (rook1Pos > kingPos)
                {
                    rook1Pos = positions[rand.Next(0, positions.Count - 1)];
                }
                positions.Remove(rook1Pos);

                rook2Pos = positions[rand.Next(0, positions.Count - 1)];
                while (rook2Pos < kingPos)
                {
                    rook2Pos = positions[rand.Next(0, positions.Count - 1)];
                }
                positions.Remove(rook2Pos);
            }

            bool blackSpot = false;
            bishop1Pos = positions[rand.Next(0, positions.Count - 1)];
            positions.Remove(bishop1Pos);
            if (bishop1Pos % 2 != 0)
            {
                blackSpot = true;
            }

            if (blackSpot)
            {
                bishop2Pos = positions[rand.Next(0, positions.Count - 1)];
                while (bishop2Pos % 2 != 0)
                {
                    bishop2Pos = positions[rand.Next(0, positions.Count - 1)];
                }
            }
            else
            {
                bishop2Pos = positions[rand.Next(0, positions.Count - 1)];
                while (bishop2Pos % 2 == 0)
                {
                    bishop2Pos = positions[rand.Next(0, positions.Count - 1)];
                }
            }
            positions.Remove(bishop2Pos);

            knight1Pos = positions[rand.Next(0, positions.Count - 1)];
            positions.Remove(knight1Pos);
            knight2Pos = positions[rand.Next(0, positions.Count - 1)];
            positions.Remove(knight2Pos);
            queenPos = positions[0];
            positions.Clear();

            List<string> all = new List<string>() { };
            switch (kingPos)
            {
                case 1:
                    all.Add("b1-wk");
                    break;
                case 2:
                    all.Add("c1-wk");
                    break;
                case 3:
                    all.Add("d1-wk");
                    break;
                case 4:
                    all.Add("e1-wk");
                    break;
                case 5:
                    all.Add("f1-wk");
                    break;
                case 6:
                    all.Add("g1-wk");
                    break;
            }
            switch (queenPos)
            {
                case 0:
                    all.Add("a1-wq");
                    break;
                case 1:
                    all.Add("b1-wq");
                    break;
                case 2:
                    all.Add("c1-wq");
                    break;
                case 3:
                    all.Add("d1-wq");
                    break;
                case 4:
                    all.Add("e1-wq");
                    break;
                case 5:
                    all.Add("f1-wq");
                    break;
                case 6:
                    all.Add("g1-wq");
                    break;
                case 7:
                    all.Add("h1-wq");
                    break;
            }
            switch (rook1Pos)
            {
                case 0:
                    all.Add("a1-wr");
                    break;
                case 1:
                    all.Add("b1-wr");
                    break;
                case 2:
                    all.Add("c1-wr");
                    break;
                case 3:
                    all.Add("d1-wr");
                    break;
                case 4:
                    all.Add("e1-wr");
                    break;
                case 5:
                    all.Add("f1-wr");
                    break;
                case 6:
                    all.Add("g1-wr");
                    break;
                case 7:
                    all.Add("h1-wr");
                    break;
            }
            switch (rook2Pos)
            {
                case 0:
                    all.Add("a1-wr");
                    break;
                case 1:
                    all.Add("b1-wr");
                    break;
                case 2:
                    all.Add("c1-wr");
                    break;
                case 3:
                    all.Add("d1-wr");
                    break;
                case 4:
                    all.Add("e1-wr");
                    break;
                case 5:
                    all.Add("f1-wr");
                    break;
                case 6:
                    all.Add("g1-wr");
                    break;
                case 7:
                    all.Add("h1-wr");
                    break;
            }
            switch (bishop1Pos)
            {
                case 0:
                    all.Add("a1-wb");
                    break;
                case 1:
                    all.Add("b1-wb");
                    break;
                case 2:
                    all.Add("c1-wb");
                    break;
                case 3:
                    all.Add("d1-wb");
                    break;
                case 4:
                    all.Add("e1-wb");
                    break;
                case 5:
                    all.Add("f1-wb");
                    break;
                case 6:
                    all.Add("g1-wb");
                    break;
                case 7:
                    all.Add("h1-wb");
                    break;
            }
            switch (bishop2Pos)
            {
                case 0:
                    all.Add("a1-wb");
                    break;
                case 1:
                    all.Add("b1-wb");
                    break;
                case 2:
                    all.Add("c1-wb");
                    break;
                case 3:
                    all.Add("d1-wb");
                    break;
                case 4:
                    all.Add("e1-wb");
                    break;
                case 5:
                    all.Add("f1-wb");
                    break;
                case 6:
                    all.Add("g1-wb");
                    break;
                case 7:
                    all.Add("h1-wb");
                    break;
            }
            switch (knight1Pos)
            {
                case 0:
                    all.Add("a1-wn");
                    break;
                case 1:
                    all.Add("b1-wn");
                    break;
                case 2:
                    all.Add("c1-wn");
                    break;
                case 3:
                    all.Add("d1-wn");
                    break;
                case 4:
                    all.Add("e1-wn");
                    break;
                case 5:
                    all.Add("f1-wn");
                    break;
                case 6:
                    all.Add("g1-wn");
                    break;
                case 7:
                    all.Add("h1-wn");
                    break;
            }
            switch (knight2Pos)
            {
                case 0:
                    all.Add("a1-wn");
                    break;
                case 1:
                    all.Add("b1-wn");
                    break;
                case 2:
                    all.Add("c1-wn");
                    break;
                case 3:
                    all.Add("d1-wn");
                    break;
                case 4:
                    all.Add("e1-wn");
                    break;
                case 5:
                    all.Add("f1-wn");
                    break;
                case 6:
                    all.Add("g1-wn");
                    break;
                case 7:
                    all.Add("h1-wn");
                    break;
            }

            all = all.OrderBy(x => x).ToList();

            var game = new Game();
            if (is906)
            {
                game = new Game
                {
                    Id = Guid.NewGuid().GetHashCode(),
                    WhitePlayerIdentifier = Guid.NewGuid().GetHashCode(),
                    BlackPlayerIdentifier = Guid.NewGuid().GetHashCode(),
                    Moves = new List<string>(),
                    Pieces = new Dictionary<string, string>
                                            {
{"a1", $"w{all[0][4]}" }, {"b1" , $"w{all[1][4]}" }, {"c1", $"w{all[2][4]}" }, {"d1" , $"w{all[3][4]}" }, {"e1", $"w{all[4][4]}" }, {"f1" , $"w{all[5][4]}" }, {"g1", $"w{all[6][4]}" }, {"h1" , $"w{all[7][4]}" },
{"a2", "wp" }, {"b2" ,"wp" }, {"c2", "wp" }, {"d2" ,"wp" }, {"e2", "wp" }, {"f2" ,"wp" }, {"g2", "wp" }, {"h2" ,"wp" },

{"a8", $"b{all[0][4]}" }, {"b8" , $"b{all[1][4]}" }, {"c8", $"b{all[2][4]}" }, {"d8" , $"b{all[3][4]}" }, {"e8", $"b{all[4][4]}" }, {"f8" , $"b{all[5][4]}" }, {"g8", $"b{all[6][4]}" }, {"h8" , $"b{all[7][4]}" },
{"a7", "bp" }, {"b7" ,"bp" }, {"c7", "bp" }, {"d7" ,"bp" }, {"e7", "bp" }, {"f7" ,"bp" }, {"g7", "bp" }, {"h7" ,"bp" }
                                            }
                };
            }
            else
            {
                game = new Game
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
            }

            if (Games.ContainsKey(game.Id))
            {
                throw new Exception("Game already exists.");
            }

            Games.Add(game.Id, game);
            foreach (string item in all)
            {
                PiecePlaces += $"{item[4]}-";
            }
            PiecePlaces.TrimEnd('-');
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
            return new MoveWrapper { StatusMessage = statusMessage, Moves = verifiedMoves };
        }
    }
}
