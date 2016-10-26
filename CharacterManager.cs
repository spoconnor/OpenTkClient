using Sean.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTkClient
{
    public static class CharacterManager
    {
        private static Dictionary<int, Position> _characters = new Dictionary<int, Position>();
        private static object _lock;
        public static void UpdateLocation(int id, Position position)
        {
            lock (_lock)
            {
                _characters[id] = position;
            }
        }

        public static IEnumerable<Tuple<Position, int>> GetCharacters(Facing direction)
        {
            lock (_lock)
            {
                var list = _characters;
                //switch (direction)
                //{
                //    case Facing.North:
                //        list = _chunksN;
                //        break;
                //    case Facing.East:
                //        list = _chunksE;
                //        break;
                //    case Facing.South:
                //        list = _chunksS;
                //        break;
                //    case Facing.West:
                //        list = _chunksW;
                //        break;
                //}
                foreach (var character in list)
                {
                    yield return new Tuple<Position, int>(character.Value, character.Key);
                }
            }
        }

    }
}
