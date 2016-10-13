using System;
using Sean.Shared;
using System.Collections.Generic;

namespace OpenTkClient
{
    public static class MapManager
    {
        private static Chunks chunks = new Chunks ();
        public const int CHUNK_SIZE = 32; // TODO - move
        public const int CHUNK_HEIGHT = 128; // TODO - move

        public static void AddChunk(Sean.Shared.Comms.Message msg)
        {
            var position = msg.Map.MinPosition;
            var coords = new ChunkCoords (ref position);
            var chunk = Sean.Shared.Chunk.Deserialize (coords, msg.Data);
            chunks.Add (coords, chunk);
        }

        public static IEnumerable<Tuple<Position, Block.BlockType>> GetBlocks()
        {
            foreach (var chunk in chunks.GetChunks())
            {
				foreach (var item in chunk.Blocks.GetVisibleIterator())
				{
					yield return item;
                }
            }
        }
    }
}

