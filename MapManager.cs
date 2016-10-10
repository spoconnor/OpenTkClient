using System;
using Sean.Shared;

namespace OpenTkClient
{
    public static class MapManager
    {
        private static Chunks chunks = new Chunks ();

        public static void AddChunk(Sean.Shared.Comms.Message msg)
        {
            var chunk = Sean.Shared.Chunk.Deserialize (msg.Data);
            var position = msg.Map.MinPosition;
            var coords = new ChunkCoords (ref position);
            chunks.Add (coords, chunk);
        }
    }
}

