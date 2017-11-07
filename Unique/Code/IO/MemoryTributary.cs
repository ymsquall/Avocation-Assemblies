
/********************************************************************
created:    2014-03-31
author:     lixianmin

http://www.codeproject.com/Articles/348590/A-replacement-for-MemoryStream

Copyright (C) - All Rights Reserved
*********************************************************************/
#pragma warning disable 0219 // warning CS0219: The variable `d' is assigned but its value is never used
using System;
using System.Collections;
using System.IO;
using System.Runtime.InteropServices;

namespace Unique.IO
{
    /// <summary>
    /// MemoryTributary is a re-implementation of MemoryStream that uses a dynamic list of byte arrays as a backing store, instead of a single byte array, the allocation
    /// of which will fail for relatively small streams as it requires contiguous memory.
    /// </summary>
    public class MemoryTributary : Stream       /* http://msdn.microsoft.com/en-us/library/system.io.stream.aspx */
    {
        public MemoryTributary()
        {
            Position = 0;
        }

        public MemoryTributary (byte[] source)
        {
            this.Write(source, 0, source.Length);
            Position = 0;
        }

        /* length is ignored because capacity has no meaning unless we implement an artifical limit */
        public MemoryTributary (int length)
        {
            SetLength(length);
            Position = length;
            byte[] d = _GetBlock();   //access block to prompt the allocation of memory
            Position = 0;
        }

        /* Use these properties to gain access to the appropriate block of memory for the current Position */

        /// <summary>
        /// The block of memory currently addressed by Position
        /// </summary>
        private byte[] _GetBlock ()
        {
            while (_blocks.Count <= _GetBlockID())
            {
                _blocks.Add(new byte[_blockSize]);
            }
            
            return _blocks[(int)_GetBlockID()] as byte[];
        }

        /// <summary>
        /// The id of the block currently addressed by Position
        /// </summary>
        private long _GetBlockID ()
        {
            return Position / _blockSize;
        }

        /// <summary>
        /// The offset of the byte currently addressed by Position, into the block that contains it
        /// </summary>
        private long _GetBlockOffset ()
        {
            return Position % _blockSize;
        }

        public override void Flush ()
        {
        }

        public override int Read (byte[] buffer, int offset, int count)
        {
            long lcount = (long)count;

            if (lcount < 0)
            {
                throw new ArgumentOutOfRangeException("count", lcount, "Number of bytes to copy cannot be negative.");
            }

            long remaining = (_length - Position);

            if (lcount > remaining)
            {
                lcount = remaining;
            }

            if (buffer == null)
            {
                throw new ArgumentNullException("buffer", "Buffer cannot be null.");
            }
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset",offset,"Destination offset cannot be negative.");
            }

            int read = 0;
            long copysize = 0;
            do
	        {
                copysize = Math.Min(lcount, (_blockSize - _GetBlockOffset()));
                Buffer.BlockCopy(_GetBlock(), (int)_GetBlockOffset(), buffer, offset, (int)copysize);
                lcount -= copysize;
                offset += (int)copysize;

                read += (int)copysize;
                Position += copysize;

	        } while (lcount > 0);

            return read;
               
        }

        public override long Seek (long offset, SeekOrigin origin)
        {
            switch (origin)
            {
                case SeekOrigin.Begin:
                    Position = offset;
                    break;
                case SeekOrigin.Current:
                    Position += offset;
                    break;
                case SeekOrigin.End:
                    Position = Length - offset;
                    break;
            }
            return Position;
        }

        public override void SetLength (long value)
        {
            _length = value;
        }

        public override void Write (byte[] buffer, int offset, int count)
        {
            long initialPosition = Position;
            int copysize;
            try
            {
                do
                {
                    copysize = Math.Min(count, (int)(_blockSize - _GetBlockOffset()));

                    EnsureCapacity(Position + copysize);

                    Buffer.BlockCopy(buffer, (int)offset, _GetBlock(), (int)_GetBlockOffset(), copysize);
                    count -= copysize;
                    offset += copysize;

                    Position += copysize;

                } while (count > 0);
            }
            catch (Exception e)
            {
                Position = initialPosition;
                throw e;
            }
        }

        public override int ReadByte ()
        {
            if (Position >= _length)
            {
                return -1;
            }

            byte b = _GetBlock()[_GetBlockOffset()];
            ++Position;

            return b;
        }

        public override void WriteByte (byte value)
        {
            EnsureCapacity(Position + 1);
            _GetBlock()[_GetBlockOffset()] = value;
            Position++;
        }

        protected void EnsureCapacity (long intended_length)
        {
            if (intended_length > _length)
            {
                _length = (intended_length);
            }
        }

        /* http://msdn.microsoft.com/en-us/library/fs2xkftw.aspx */
        protected override void Dispose (bool disposing)
        {
            /* We do not currently use unmanaged resources */
            base.Dispose(disposing);
        }

        /// <summary>
        /// Returns the entire content of the stream as a byte array. This is not safe because the call to new byte[] may 
        /// fail if the stream is large enough. Where possible use methods which operate on streams directly instead.
        /// </summary>
        /// <returns>A byte[] containing the current data in the stream</returns>
        public byte[] ToArray ()
        {
            long firstposition = Position;
            Position = 0;
            byte[] destination = new byte[Length];
            Read(destination, 0, (int)Length);
            Position = firstposition;
            return destination;
        }

        /// <summary>
        /// Reads length bytes from source into the this instance at the current position.
        /// </summary>
        /// <param name="source">The stream containing the data to copy</param>
        /// <param name="length">The number of bytes to copy</param>
        public void ReadFrom (Stream source, long length)
        {
            byte[] buffer = new byte[_bufferLength];
            int read;
            do
            {
                read = source.Read(buffer, 0, (int)Math.Min(_bufferLength, length));
                length -= read;
                this.Write(buffer, 0, read);

            } while (length > 0);
        }

        /// <summary>
        /// Writes the entire stream into destination, regardless of Position, which remains unchanged.
        /// </summary>
        /// <param name="destination">The stream to write the content of this stream to</param>
        public void WriteTo (Stream destination)
        {
            var initialPosition = Position;
            Position = 0;

            var bytes = new byte[_bufferLength];
            int dataRead;
            while ((dataRead = Read(bytes, 0, bytes.Length)) > 0)
            {
                destination.Write(bytes, 0, dataRead);
            }

            Position = initialPosition;
        }

        public override bool CanRead  { get { return true; } }
        
        public override bool CanSeek  { get { return true; } }
        
        public override bool CanWrite { get { return true; } }
        
        public override long Length   { get { return _length; } }
        
        public override long Position { get; set; }
        
        private long _length = 0;
        
        private readonly ArrayList _blocks = new ArrayList();

        private const int _blockSize    = 65536;
        private const int _bufferLength = 4096;
    }
}
