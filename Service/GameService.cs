using TaskForTg.Models;
using TaskForTg.Repository;

namespace TaskForTg.Service
{
    public class GameService
    {
        private readonly DatabaseInMemory _databaseInMemory;
        public GameService(DatabaseInMemory databaseInMemory)
        {
            this._databaseInMemory = databaseInMemory;
        }
        public Game CreateGame(int w, int h, int mines)
        {
            return _databaseInMemory.CreateGame(w, h, mines);
        }

        public Game? GeyById(Guid id)
        {
            var game = _databaseInMemory.GetById(id);
            return game;
        }

        public Game Turn(GameTurnRequest request)
        {
            var game = _databaseInMemory.GetById(request.game_id);
            if (game == null) { throw new Exception("Отсутствует игра с таким кодом"); }
            game.ShowCell(request.row, request.col);
            return game;
            
        }

        

    }
}
