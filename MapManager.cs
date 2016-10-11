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

        public static IEnumerable<Tuple<Block,Position>> GetBlocks()
        {
            foreach (var chunk in chunks.GetChunks())
            {
                for (var x = 0; x < CHUNK_SIZE; x++)
                {
                    for (var z = 0; z < CHUNK_SIZE; z++)
                    {
                        for (var y = 0; y < CHUNK_HEIGHT; y++)
                        {
                            var block = chunk.Blocks[x, y, z];
                            yield return new Tuple<Block, Position>(block, new Position(chunk.MinPosition.X + x, chunk.MinPosition.Y + y, chunk.MinPosition.Z + z));
                        }
                    }
                }
            }
        }
    }
}

