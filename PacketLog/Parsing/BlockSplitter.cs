﻿// Copyright (c) Gothos
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.IO;

namespace Tera.PacketLog
{
    internal class BlockSplitter
    {
        private readonly MemoryStream _buffer = new MemoryStream();
        public event Action<byte[]> BlockFinished;

        protected virtual void OnBlockFinished(byte[] block)
        {
            var handler = BlockFinished;
            handler?.Invoke(block);
        }

        private static void RemoveFront(MemoryStream stream, int count)
        {
            Array.Copy(stream.GetBuffer(), count, stream.GetBuffer(), 0, stream.Length - count);
            stream.SetLength(stream.Length - count);
        }

        private static byte[] PopBlock(MemoryStream stream)
        {
            if (stream.Length < 2)
                return null;
            var buffer = stream.GetBuffer();
            var blockSize = buffer[0] | buffer[1] << 8;
            if (stream.Length < blockSize || blockSize < 2)
                return null;
            var block = new byte[blockSize];
            Array.Copy(buffer, 2, block, 0, blockSize - 2);
            RemoveFront(stream, blockSize);
            return block;
        }

        public byte[] PopBlock()
        {
            var block = PopBlock(_buffer);
            if (block != null)
            {
                OnBlockFinished(block);
            }
            return block;
        }

        public void PopAllBlocks()
        {
            while (PopBlock() != null)
            {
            }
        }

        public void Data(byte[] data)
        {
            _buffer.Write(data, 0, data.Length);
        }
    }
}