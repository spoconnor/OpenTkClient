using System;
using Sean.Shared;

namespace OpenTkClient
{
	public static class Global
	{
		public const int CHUNK_SIZE = 32;
		public const int CHUNK_HEIGHT = 128;
		public const float Scale = 4.0f;

		public static Position LookingAt = new Position(2000, 70, 900);
	}
}

