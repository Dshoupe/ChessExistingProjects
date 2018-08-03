using System.Web.Mvc;
using Kvam.Chess.Core;

namespace Kvam.Chess.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Chess()
        {
            return View();
        }

        public ActionResult Chess906()
        {
            return View();
        }

        public ActionResult CreateGame()
        {
            var game = Game.CreateGame(false);

            return Json(new { GameId = game.Id, PlayerId = game.WhitePlayerIdentifier, PiecePlaces = Game.PiecePlaces, Is906 = false },
                JsonRequestBehavior.AllowGet);
        }

        public ActionResult CreateGame906()
        {
            var game = Game.CreateGame(true);

            return Json(new { GameId = game.Id, PlayerId = game.WhitePlayerIdentifier, PiecePlaces = Game.PiecePlaces, Is906 = true },
                JsonRequestBehavior.AllowGet);
        }


        public ActionResult ListLegalMoves(int gameId, int playerId, string lastMove)
        {
            var moveWrapper = Game.ListLegalMoves(gameId, playerId, lastMove);

            return Json(moveWrapper, JsonRequestBehavior.AllowGet);
        }
    }
}
