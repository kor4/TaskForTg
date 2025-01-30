using System.Runtime.InteropServices;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using TaskForTg.Models;
using TaskForTg.Service;

namespace TaskForTg.Controllers
{
    [ApiController]
    [Route("api/turn")]
    public class TurnController : Controller
    {
        private readonly GameService _gameService;

        public TurnController(GameService gameService)
        {
            _gameService = gameService;
        }

        [HttpPost]
        public ActionResult<GameInfoResponse> Turn(GameTurnRequest turn)
        {

            var game = _gameService.Turn(turn);

            // перенести в маппер по-хорошему
            List<List<string>> list_field = new List<List<string>>();
            for (int i = 0; i < game.Height; i++)
            {
                List<string> ListY = new List<string>();

                for (int j = 0; j < game.Width; j++)
                {
                    ListY.Add(game.Field[i, j].ToString());
                }

                list_field.Add(ListY);
            }
            var response = new GameInfoResponse()
            {
                game_id = game.game_id,
                width = game.Width,
                height = game.Height,
                mines_count = game.MinesCount,
                completed = game.Completed,
                field = list_field,

            };
            return Ok(response);


            
        }
    }
}
