using System;
using Sean.Shared;

namespace OpenTkClient
{
	public static class Global
	{
		public const int CHUNK_SIZE = 32;
		public const int CHUNK_HEIGHT = 128;
		public const float Scale = 2.0f;
		public static Position LookingAt = new Position((64*32)+16, 30, (32*32)+16);
		public static Facing Direction;
		public static int MaxChunkLimit = (int)Math.Sqrt(int.MaxValue);
	}
}

