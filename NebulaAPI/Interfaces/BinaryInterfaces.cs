﻿using System;
using System.IO;

namespace NebulaAPI
{
    /// <summary>
    /// Provides access to BinaryWriter with LZ4 compression
    /// </summary>
    public interface IWriterProvider : IDisposable
    {
        BinaryWriter BinaryWriter { get; }
        byte[] CloseAndGetBytes();
    }

    /// <summary>
    /// Provides access to BinaryReader with LZ4 compression
    /// </summary>
    public interface IReaderProvider : IDisposable
    {
        BinaryReader BinaryReader { get; }
    }
}