﻿using System;
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
        private static Array<byte> worldMapHeight;
        private static Array<byte> worldMapBlocks;
        private static int chunkMidpoint = Global.CHUNK_SIZE / 2;

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
            var map = new Array<byte>(size);
            map.DeSerialize(msg.Data);
            if (msg.WorldMapResponse.MapRequestType == Sean.Shared.Comms.MapRequestType.HeightMap)
                worldMapHeight = map;
            else if (msg.WorldMapResponse.MapRequestType == Sean.Shared.Comms.MapRequestType.Terrain)
                worldMapBlocks = map;
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

        public static IEnumerable<Tuple<Position, Block.BlockType>> GetWorldMapBlocks(Facing direction)        {
            lock (_lock)
            {
                if (worldMapHeight != null && worldMapBlocks != null)
                {
                    // TODO - facing direction
                    var s = worldMapHeight.Size.scale;
                    for (int z = worldMapHeight.Size.minZ; z < worldMapHeight.Size.maxZ - s; z += s)
                    {
                        for (int x = worldMapHeight.Size.minX; x < worldMapHeight.Size.maxX - s; x += s)
                        {
                            //if (Math.Abs(z - Global.LookingAt.Z) <= 100 && Math.Abs(x - Global.LookingAt.X) <= 100)
                            yield return 
                                new Tuple<Position, Block.BlockType>(
                                    new Position(x+chunkMidpoint, worldMapHeight[x,z], z+chunkMidpoint), 
                                    (Block.BlockType)worldMapBlocks[x,z]);
                        }
                    }
                }
            }
        }
    }
}

