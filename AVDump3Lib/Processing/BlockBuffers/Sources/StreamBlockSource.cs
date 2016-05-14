﻿using System.IO;

namespace AVDump3Lib.BlockBuffers.Sources {
    public class StreamBlockSource : IBlockSource {
		private readonly Stream source;

		public StreamBlockSource(Stream source) { this.source = source; }
		public long Length => source.Length;
		public int Read(byte[] block) => source.Read(block, 0, block.Length);
	}
}