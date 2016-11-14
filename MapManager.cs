using System;
using Sean.Shared;
using System.Collections.Generic;

namespace OpenTkClient
{
    public static class MapManager
    {
        //private static Chunks chunks = new Chunks ();
		private static SortedList<ChunkCoords, Chunk> _chunksN = new SortedList<ChunkCoords, Chunk>();
		private static SortedList<ChunkCoords, Chunk> _chunksS = new SortedList<ChunkCoords, Chunk>();
		private static SortedList<ChunkCoords, Chunk> _chunksE = new SortedList<ChunkCoords, Chunk>();
		private static SortedList<ChunkCoords, Chunk> _chunksW = new SortedList<ChunkCoords, Chunk>();
        private static Array<int> worldMap;

        private static object _lock = new object ();

        public static void SetWorldMap(Sean.Shared.Comms.Message msg)
        {
            var size = new ArraySize()
            {
                scale = msg.WorldMapResponse.Scale,
                minX = msg.WorldMapResponse.MinPosition.X,
                minY = msg.WorldMapResponse.MinPosition.Y,
                minZ = msg.WorldMapResponse.MinPosition.Z,
                maxX = msg.WorldMapResponse.MaxPosition.X,
                maxY = msg.WorldMapResponse.MaxPosition.Y,
                maxZ = msg.WorldMapResponse.MaxPosition.Z,
            };
            worldMap = new Array<int>(size);
            worldMap.DeSerialize(msg.Data);
        }

        public static void AddChunk(Sean.Shared.Comms.Message msg)
        {
			lock (_lock) {
				var position = msg.Map.MinPosition;
				var coords = new ChunkCoords (position);
				var chunk = Sean.Shared.Chunk.Deserialize (coords, msg.Data);
				_chunksN.Add (coords, chunk);
				_chunksS.Add (new ChunkCoords(Global.MaxChunkLimit - position.X, Global.MaxChunkLimit - position.Z), chunk);
				_chunksE.Add (new ChunkCoords(Global.MaxChunkLimit - position.X, position.Z), chunk);
				_chunksW.Add (new ChunkCoords(position.X, Global.MaxChunkLimit - position.Z), chunk);
			}
        }

        public static void SetBlock(Position position, Block newBlock)
        {
            var coords = new ChunkCoords(position);
            _chunksN[coords].Blocks[position] = newBlock;
        }

		public static IEnumerable<Tuple<Position, Block.BlockType>> GetBlocks(Facing direction)
        {
			lock (_lock) {
				var list = _chunksN;
				switch (direction) {
				case Facing.North:
					list = _chunksN;
					break;
				case Facing.East:
					list = _chunksE;
					break;
				case Facing.South:
					list = _chunksS;
					break;
				case Facing.West:
					list = _chunksW;
					break;
				}
				foreach (var chunk in list) {
					foreach (var item in chunk.Value.GetVisibleIterator(direction)) {
						yield return item;
					}
				}
			}
        }

        public static IEnumerable<List<Position>> GetWorldMapBlocks(Facing direction)
        {
            lock (_lock)
            {
                if (worldMap != null)
                {
                    // TODO - facing direction
                    var s = worldMap.Size.scale;
                    for (int z = worldMap.Size.minZ; z < worldMap.Size.maxZ - s; z += s)
                    {
                        for (int x = worldMap.Size.minX; x < worldMap.Size.maxX - s; x += s)
                        {
                            yield return new List<Position>
                            {
                                new Position(x, worldMap[z,x], z),
                                //new Position(x+s, worldMap[z,x+s], z),
                                //new Position(x+s, worldMap[z+s,x+s], z+s),
                                //new Position(x, worldMap[z+s,x], z+s),
                            };
                        }
                    }
                }
            }
        }
    }
}

