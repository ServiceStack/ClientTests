using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientTest.UWP
{
    public class DebugTextWriter : StreamWriter
    {
        public DebugTextWriter()
            : base(new DebugOutStream(), Encoding.Unicode, 1024)
        {
            AutoFlush = true;
        }

        class DebugOutStream : Stream
        {
            public override void Write(byte[] buffer, int offset, int count)
            {
                Debug.Write(Encoding.Unicode.GetString(buffer, offset, count));
            }

            public override bool CanRead => false;
            public override bool CanSeek => false;
            public override bool CanWrite => true;
            public override void Flush() => Debug.Flush();
            public override long Length => throw new InvalidOperationException();
            public override int Read(byte[] buffer, int offset, int count) => throw new InvalidOperationException();
            public override long Seek(long offset, SeekOrigin origin) => throw new InvalidOperationException();
            public override void SetLength(long value) => throw new InvalidOperationException();
            public override long Position
            {
                get => throw new InvalidOperationException();
                set => throw new InvalidOperationException();
            }
        };
    }
}
