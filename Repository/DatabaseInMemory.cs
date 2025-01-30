using TaskForTg.Models;

namespace TaskForTg.Repository
{
    public class DatabaseInMemory
    {
        private readonly List<Game> _games = new List<Game>();
        public Game CreateGame(int w, int h, int count)

        {
            var new_game = new Game(w, h, count);
          
            _games.Add(new_game);
            return new_game;

        }
        public Game? GetById(Guid game_id)
        {
            return _games.FirstOrDefault(g=>g.game_id == game_id);
        }
    }
}
