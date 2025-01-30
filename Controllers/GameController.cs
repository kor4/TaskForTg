using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Mvc;
using TaskForTg.Models;
using TaskForTg.Service;

namespace TaskForTg.Controllers
{
    [ApiController]
    [Route("api/new")]
    public class GameController : ControllerBase
    {
        private readonly GameService _gameService;

        public GameController(GameService gameService)
        {
            _gameService = gameService;
        }

        [HttpPost]
        [ProducesResponseType(typeof(GameInfoResponse),StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public ActionResult<GameInfoResponse> CreateGame([FromBody] NewGameRequest request)
        {
            if (request.width * request.height <= request.mines_count)
            {
                return BadRequest(new ErrorResponse() { error = "Проищошла непредвиденная ошибка" });
            }

            var AddedGame = _gameService.CreateGame(request.width, request.height, request.mines_count);

            //формируем ответ  в виде списка , т.к. json не однозанчно работает с string/char[,]
            List<List<string>> list_field = new List<List<string>>();
            for (int i = 0; i < AddedGame.Height; i++)
            {
                List<string> ListY = new List<string>();

                for (int j = 0; j < AddedGame.Width; j++)
                {
                    ListY.Add(AddedGame.Field[i, j].ToString());
                }

                list_field.Add(ListY);
            }

            var response = new GameInfoResponse()
            {
                game_id = AddedGame.game_id,
                width = AddedGame.Width,
                height = AddedGame.Height,
                mines_count = AddedGame.MinesCount,
                completed = AddedGame.Completed,
                field = list_field,

            };
            return Ok(response);
        }
        
        
    }
}
