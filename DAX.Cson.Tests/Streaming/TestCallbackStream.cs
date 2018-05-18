using System;
using System.Collections.Generic;
using System.Diagnostics;
using DAX.Cson.Internals;
using NUnit.Framework;

namespace DAX.Cson.Tests.Streaming
{
    [TestFixture]
    public class TestCallbackStream : FixtureBase
    {
        const int OneMegabyte = 1024*1024;

        [TestCase(10, 4)]
        [TestCase(1024, 40)]
        [TestCase(1024, 400)]
        [TestCase(1024, 90000)]
        [TestCase(65536, 4096)]
        [TestCase(OneMegabyte, 1024)]
        [TestCase(OneMegabyte, 2048)]
        [TestCase(OneMegabyte, 4096)]
        [TestCase(OneMegabyte, 8192)]
        [TestCase(OneMegabyte, 32768)]
        [TestCase(OneMegabyte, 65536)]
        public void CanReliablyRoundtripLotsOfBytes(int bufferSize, int chunkSize)
        {
            var stopwatch = Stopwatch.StartNew();

            var randomData = GetRandomData(bufferSize);
            var stream = GetCallbackStream(randomData, chunkSize);
            var roundtrippedData = GetDataOutOfStream(stream);

            Assert.That(roundtrippedData.Length, Is.EqualTo(randomData.Length));

            for (var index = 0; index < randomData.Length; index++)
            {
                var b1 = randomData[index];
                var b2 = roundtrippedData[index];

                Assert.That(b1, Is.EqualTo(b2));
            }

            Console.WriteLine($"Roundtrip time: {stopwatch.ElapsedMilliseconds} ms");
        }

        static byte[] GetDataOutOfStream(CallbackStream stream)
        {
            var readBytes = new List<byte>();
            var tempBuffer = new byte[1024];

            while (true)
            {
                var bytesRead = stream.Read(tempBuffer, 0, 1024);

                if (bytesRead == 0) break;

                for (var index = 0; index < bytesRead; index++)
                {
                    readBytes.Add(tempBuffer[index]);
                }
            }

            return readBytes.ToArray();
        }

        static CallbackStream GetCallbackStream(byte[] randomData, int chunkSize)
        {
            var position = 0;
            var bufferSize = chunkSize;

            var stream = new CallbackStream(request =>
            {
                var count = Math.Min(bufferSize, randomData.Length - position);
                if (count == 0) return;
                var buffer = new byte[count];
                Buffer.BlockCopy(randomData, position, buffer, 0, count);
                position += count;
                request.Write(buffer, 0, buffer.Length);
            }, null);
            return stream;
        }

        static byte[] GetRandomData(int length)
        {
            var bufferino = new byte[length];
            var random = new Random(DateTime.Now.GetHashCode());
            random.NextBytes(bufferino);
            return bufferino;
        }
    }
}