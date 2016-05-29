using AVDump3Lib.Processing.BlockBuffers;
using System.Security.Cryptography;
using System.Threading;

namespace AVDump3Lib.Processing.BlockConsumers {
	public class HashCalculator : BlockConsumer {
        public HashAlgorithm HashAlgorithm { get; }
        public HashCalculator(string name, IBlockStreamReader reader, HashAlgorithm hashAlgorithm) : base(name, reader) {
            HashAlgorithm = hashAlgorithm;
        }


        protected override void DoWork(CancellationToken ct) {
			HashAlgorithm.Initialize();

			int toRead;
            do {
                ct.ThrowIfCancellationRequested();
                HashAlgorithm.TransformBlock(Reader.GetBlock(out toRead), 0, toRead, null, 0);
            } while(Reader.Advance());

            HashAlgorithm.TransformFinalBlock(new byte[0], 0, 0);
        }
    }
}
