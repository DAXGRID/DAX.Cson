using System;
using System.Collections.Generic;
using System.IO;

namespace DAX.Cson.Internals
{
    class CallbackStream : Stream
    {
        public class CallbackStreamDataRequest : EventArgs
        {
            readonly CallbackStream _callbackStream;

            internal CallbackStreamDataRequest(CallbackStream callbackStream)
            {
                _callbackStream = callbackStream;
            }

            public void Write(byte[] buffer, int offset, int count)
            {
                _callbackStream.Write(buffer, offset, count);
            }

            public void Write(byte[] buffer)
            {
                _callbackStream.Write(buffer);
            }
        }

        readonly Action<CallbackStreamDataRequest> _requestData;
        readonly CallbackStreamDataRequest _dataRequest;
        readonly Queue<byte[]> _buffers = new Queue<byte[]>();
        readonly IDisposable _extraDisposable;

        int _currentBufferPosition;

        public CallbackStream(Action<CallbackStreamDataRequest> requestData, IDisposable extraDisposable)
        {
            _requestData = requestData ?? throw new ArgumentNullException(nameof(requestData));
            _extraDisposable = extraDisposable;
            _dataRequest = new CallbackStreamDataRequest(this);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            var copy = new byte[count];
            Buffer.BlockCopy(buffer, offset, copy, 0, count);
            _buffers.Enqueue(copy);
        }

        void Write(byte[] buffer)
        {
            _buffers.Enqueue(buffer);
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (_buffers.Count == 0)
            {
                _requestData(_dataRequest);

                // if a data request does not result in more data, then we're done
                if (_buffers.Count == 0)
                {
                    return 0;
                }
            }

            byte[] currentBuffer = _buffers.Peek();
            //if (!_buffers.TryPeek(out currentBuffer))
            //{
                
            //    throw new ApplicationException("_buffers.Count was > 0 before but there was no buffer in the queue!");
            //}

            var remainingOfCurrentBuffer = currentBuffer.Length - _currentBufferPosition;

            if (remainingOfCurrentBuffer <= count)
            {
                Buffer.BlockCopy(currentBuffer, _currentBufferPosition, buffer, offset, remainingOfCurrentBuffer);
                _buffers.Dequeue();
                _currentBufferPosition = 0;
                return remainingOfCurrentBuffer;
            }

            var bytesToReturnNow = count;
            Buffer.BlockCopy(currentBuffer, _currentBufferPosition, buffer, offset, bytesToReturnNow);
            _currentBufferPosition += bytesToReturnNow;
            return bytesToReturnNow;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            throw new NotSupportedException("This type of stream is not capable of seeking");
        }

        public override void SetLength(long value)
        {
            throw new NotSupportedException("This type of stream cannot have its length set");
        }

        public override void Flush()
        {
        }

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => true;

        public override long Length => -1;

        public override long Position
        {
            get { return -1; }
            set { throw new InvalidOperationException("Cannot set Position"); }
        }

        protected override void Dispose(bool disposing)
        {
            _extraDisposable?.Dispose();
            base.Dispose(disposing);
        }
    }
}