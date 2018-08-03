using System;
using System.Collections.Generic;
using System.Linq;
using Kvam.Chess.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TestChess
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void PiecesCreatedInGame()
        {
            var game = Game.CreateGame(true);
            Assert.AreEqual(32, game.Pieces.Count);
            game = Game.CreateGame(false);
            Assert.AreEqual(32, game.Pieces.Count);
        }

        [TestMethod]
        public void RegularChessWhiteCheck()
        {
            var game = Game.CreateGame(false);
            List<string> list = game.Pieces.Values.ToList().GetRange(0, 16);
            List<string> expected = new List<string>() { "wr", "wn", "wb", "wq", "wk", "wb", "wn", "wr", "wp", "wp", "wp", "wp", "wp", "wp", "wp", "wp" };
            Assert.AreEqual(list.Count, expected.Count);
            for (int i = 0; i < list.Count; i++)
            {
                Assert.AreEqual(expected[i], list[i]);
            }
        }

        [TestMethod]
        public void RegularChessBlackCheck()
        {
            var game = Game.CreateGame(false);
            List<string> list = game.Pieces.Values.ToList().GetRange(16, 16);
            List<string> expected = new List<string>() { "br", "bn", "bb", "bq", "bk", "bb", "bn", "br", "bp", "bp", "bp", "bp", "bp", "bp", "bp", "bp" };
            Assert.AreEqual(list.Count, expected.Count);
            for (int i = 0; i < list.Count; i++)
            {
                Assert.AreEqual(list[i], expected[i]);
            }
        }

        [TestMethod]
        public void BlackAndWhiteAreEqual()
        {
            var game = Game.CreateGame(false);
            List<string> whiteList = game.Pieces.Values.ToList().GetRange(0, 16);
            List<string> blackList = game.Pieces.Values.ToList().GetRange(16, 16);
            Assert.AreEqual(whiteList.Count, blackList.Count);
            for (int i = 0; i < whiteList.Count; i++)
            {
                Assert.AreEqual(whiteList[i][1], blackList[i][1]);
            }
        }

        [TestMethod]
        public void Check960KingRookPlacementWhite()
        {
            var game = Game.CreateGame(true);
            List<string> whiteList = game.Pieces.Values.ToList().GetRange(0, 16);

            int king = whiteList.IndexOf("wk");
            var holder = whiteList.Where(x => x == "wr").ToList();
            int rook1 = whiteList.IndexOf(holder[0]);
            int rook2 = 0;
            for (int i = rook1 + 1; i < game.Pieces.Values.Count; i++)
            {
                if (game.Pieces.Values.ToList()[i] == "wr")
                {
                    rook2 = i;
                }
            }
            Assert.IsTrue(king > rook1 && king < rook2);
        }

        [TestMethod]
        public void Check960KingRookPlacementBlack()
        {
            var game = Game.CreateGame(true);
            List<string> blackList = game.Pieces.Values.ToList().GetRange(16, 16);

            int king = blackList.IndexOf("bk");
            var holder = blackList.Where(x => x == "br").ToList();
            int rook1 = blackList.IndexOf(holder[0]);
            int rook2 = 0;
            for (int i = rook1 + 1; i < game.Pieces.Values.Count; i++)
            {
                if (game.Pieces.Values.ToList()[i] == "br")
                {
                    rook2 = i;
                }
            }
            Assert.IsTrue(king > rook1 && king < rook2);
        }

        [TestMethod]
        public void CheckBishopPlacementWhite()
        {
            var game = Game.CreateGame(true);
            List<string> whiteList = game.Pieces.Values.ToList().GetRange(0, 16);

            int bishop1 = whiteList.IndexOf("wb");
            int bishop2 = 0;
            for (int i = bishop1 + 1; i < game.Pieces.Values.Count; i++)
            {
                if (game.Pieces.Values.ToList()[i] == "wb")
                {
                    bishop2 = i;
                }
            }
            Assert.IsTrue((bishop1 % 2 == 1 && bishop2 % 2 == 0) || (bishop2 % 2 == 1 && bishop1 % 2 == 0));
        }

        [TestMethod]
        public void CheckBishopPlacementBlack()
        {
            var game = Game.CreateGame(true);
            List<string> blackList = game.Pieces.Values.ToList().GetRange(16, 16);

            int bishop1 = blackList.IndexOf("bb");
            int bishop2 = 0;
            for (int i = bishop1 + 1; i < game.Pieces.Values.Count; i++)
            {
                if (game.Pieces.Values.ToList()[i] == "bb")
                {
                    bishop2 = i;
                }
            }
            Assert.IsTrue((bishop1 % 2 == 1 && bishop2 % 2 == 0) || (bishop2 % 2 == 1 && bishop1 % 2 == 0));
        }

        [TestMethod]
        public void ConfirmRandomness()
        {
            for (int i = 0; i < 1000; i++)
            {
                var game = Game.CreateGame(true);
                var game2 = Game.CreateGame(true);
                Assert.AreNotEqual(game.Pieces, game2.Pieces);
            }
        }

        [TestMethod]
        public void Confirm960()
        {
            List<Game> games = new List<Game>();
            games.Add(Game.CreateGame(true));
            while (games.Count <= 100)
            {
                var game = Game.CreateGame(true);
                List<string> white1 = game.Pieces.Values.ToList().GetRange(0, 7);
                bool isEqual = false;
                foreach (Game g in games)
                {
                    List<string> white2 = g.Pieces.Values.ToList().GetRange(0, 7);
                    if (white1.SequenceEqual(white2))
                    {
                        isEqual = true;
                        break;
                    }
                }
                if (!isEqual)
                {
                    games.Add(game);
                }
            } 
            Assert.AreEqual(960, games.Count);
        }
    }
}