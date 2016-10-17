using System;
using Sean.Shared;
using System.Collections.Generic;

namespace OpenTkClient
{
    public static class MapManager
    {
        private static Chunks chunks = new Chunks ();

        public static void SetWorldMap(Sean.Shared.Comms.Message msg)
        {
            /*
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
            Array<int> map = new Array<int>(size);
            map.DeSerialize(msg.Data);
            */
        }

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
				foreach (var item in chunk.GetVisibleIterator())
				{
					yield return item;
                }
            }
        }
    }
}

