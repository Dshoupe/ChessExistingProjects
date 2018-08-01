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

        public ActionResult CreateGame()
        {
            var game = Game.CreateGame();

            return Json(new { GameId = game.Id, PlayerId = game.WhitePlayerIdentifier },
                JsonRequestBehavior.AllowGet);
        }


        public ActionResult ListLegalMoves(int gameId, int playerId, string lastMove)
        {
            var moveWrapper = Game.ListLegalMoves(gameId, playerId, lastMove);


            return Json(moveWrapper,
                        JsonRequestBehavior.AllowGet);
        }
    }
}
