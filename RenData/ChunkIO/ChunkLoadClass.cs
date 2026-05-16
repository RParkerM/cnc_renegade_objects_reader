using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace RenData.ChunkIO;
public sealed class ChunkLoadClass : IDisposable
{
    private readonly BinaryReader _file;
    private int _stackIndex;
    private readonly uint[] _positionStack = new uint[256];
    private readonly ChunkHeader[] headerStack = new ChunkHeader[256];
    private bool inMicroChunk;
    private uint microChunkPosition;
    public MicroChunkHeader McHeader { get; private set; }
    public ChunkHeader ChunkHeader => headerStack.LastOrDefault();

    public ChunkLoadClass(Stream file)
    {
        _file = new(file);
        _stackIndex = 0;
        Array.Clear(headerStack, 0, headerStack.Length);
        Array.Clear(_positionStack, 0, _positionStack.Length);
        inMicroChunk = false;
        microChunkPosition = 0;
        McHeader = new MicroChunkHeader();
    }

    public bool Open_Chunk()
    {
        if (_stackIndex <= 0 || _positionStack[_stackIndex - 1] != (headerStack[_stackIndex - 1].ChunkSize & 0x7FFFFFFF))
        {
            try
            {
                headerStack[_stackIndex] = ReadChunkHeader();
                _positionStack[_stackIndex] = 0;
                _stackIndex++;
                return true;
            }
            catch (Exception e)
            {
                Trace.WriteLine(e);
            }
        }
        return false;
    }
    public bool Peek_Next_Chunk(ref uint id, ref uint length)
    {
        ChunkHeader h = new();
        if ((_stackIndex <= 0) || (_positionStack[_stackIndex - 1] != (headerStack[_stackIndex - 1].ChunkSize & 0x7FFFFFFF)))
        {
            if (ReadChunkHeader(ref h) == 8)
            {
                _file.BaseStream.Seek(-8, SeekOrigin.Current);
                length = h.ChunkSize;
                id = h.ChunkType & 0x7FFFFFFF;
                return true;
            }
        }
        return false;
    }

    public uint Cur_Chunk_ID => headerStack[_stackIndex - 1].ChunkType;
    public uint Cur_Chunk_Length => headerStack[_stackIndex - 1].ChunkSize & 0x7FFFFFFF;
    public int Cur_Chunk_Depth => _stackIndex;
    public bool Contains_Chunks => (headerStack[_stackIndex - 1].ChunkSize & 0x80000000) > 0;

    private int ReadChunkHeader(ref ChunkHeader h)
    {
        h.ChunkType = _file.ReadUInt32();
        h.ChunkSize = _file.ReadUInt32();
        return 8;
    }

    public bool Close_Chunk()
    {
        uint headerSize = headerStack[_stackIndex - 1].ChunkSize & 0x7FFFFFFF;
        if (_positionStack[_stackIndex - 1] < headerSize)
        {
            _file.BaseStream.Seek((headerSize - _positionStack[_stackIndex - 1]), SeekOrigin.Current);
        }
        _stackIndex--;
        if (_stackIndex > 0)
        {
            _positionStack[_stackIndex - 1] += headerSize;
            _positionStack[_stackIndex - 1] += 8;
        }
        return true;
    }

    public bool Open_Micro_Chunk()
    {
        try
        {
            MicroChunkHeader microChunkHeader = default;

            var bytesRead = Read(ref microChunkHeader);

            if (bytesRead == 0)
            {
                return false;
            }
            McHeader = microChunkHeader;
            microChunkPosition = 0;
            inMicroChunk = true;
            return true;
        }
        catch (Exception e)
        {
            Trace.WriteLine(e);
        }
        return false;
    }

    public bool Close_Micro_Chunk()
    {
        inMicroChunk = false;
        if (microChunkPosition < McHeader.ChunkSize)
        {
            _file.BaseStream.Seek((McHeader.ChunkSize - microChunkPosition), SeekOrigin.Current);
            if (_stackIndex > 0)
            {
                _positionStack[_stackIndex - 1] += McHeader.ChunkSize - microChunkPosition;
            }
        }
        return true;
    }

    public ulong Cur_Micro_Chunk_ID => McHeader.ChunkType;
    public ulong Cur_Micro_Chunk_Length => McHeader.ChunkSize;

    public long Seek(uint numBytes)
    {
        Debug.Assert(_file.BaseStream.CanRead);
        if ((_positionStack[_stackIndex - 1] + numBytes) <= (headerStack[_stackIndex - 1].ChunkSize & 0x7FFFFFFF))
        {
            if ((!inMicroChunk) || ((microChunkPosition + numBytes) <= McHeader.ChunkSize))
            {
                var pos = _file.BaseStream.Position;
                var seek = _file.BaseStream.Seek(numBytes, SeekOrigin.Current);
                if ((seek - pos) == (int)numBytes)
                {
                    _positionStack[_stackIndex - 1] += numBytes;
                    if (inMicroChunk)
                    {
                        microChunkPosition += numBytes;
                    }
                    return numBytes;
                }
            }
        }
        return 0;
    }

    public long Read(ref string str)
    {
        try
        {
            UInt16 length = 0;

            long result = Read(ref length);

            byte[] stringBytes = new byte[length];
            var span = stringBytes.AsSpan();

            result += Read(span);

            Debug.Assert(result == sizeof(UInt16) + length);

            str = Encoding.UTF8.GetString(stringBytes);

            return result;
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex);
            return 0;
        }
    }

    public bool EndOfChunk => _stackIndex == 0 || _positionStack[_stackIndex - 1] >= (headerStack[_stackIndex - 1].ChunkSize & 0x7FFFFFFF);

    public long Read(Span<byte> span)
    {
        uint numBytes = (uint)span.Length;
        Debug.Assert(_file.BaseStream.CanRead);
        if ((_positionStack[_stackIndex - 1] + numBytes) <= (headerStack[_stackIndex - 1].ChunkSize & 0x7FFFFFFF))
        {
            if ((!inMicroChunk) || ((microChunkPosition + numBytes) <= McHeader.ChunkSize))
            {
                var read = _file.BaseStream.Read(span);
                if (read == (int)numBytes)
                {
                    _positionStack[_stackIndex - 1] += numBytes;
                    if (inMicroChunk)
                    {
                        microChunkPosition += numBytes;
                    }
                    return numBytes;
                }
            }
        }
        return 0;
    }

    //public long Read<T>(ref T t) where T : struct
    //{
    //    Span<byte> span = MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref t, 1));
    //    return Read(span);
    //}
    public long Read<T>(ref T t) where T : unmanaged
    {
        Span<byte> span = MemoryMarshal.AsBytes(MemoryMarshal.CreateSpan(ref t, 1));
        return Read(span);
    }

    public T Read<T>() where T : unmanaged
    {
        var t = default(T);
        var bytesRead = Read(ref t);
        var expectedSize = Marshal.SizeOf<T>();
        if (typeof(T) == typeof(bool))
            expectedSize = 1;
        if (bytesRead != expectedSize)
        {
            throw new Exception($"Error reading structure. Expected Size: {expectedSize}, Actual: {bytesRead}");
        }
        return t;
    }

    public void ReadMicroChunkWWString(out string result)
    {
        result = ReadMicroChunkWWString();
    }

    public string ReadMicroChunkWWString()
    {
        try
        {
            var str = string.Empty;

            var length = checked((int)Cur_Micro_Chunk_Length);

            Span<byte> span = stackalloc byte[length];

            var result = Read(span);

            Debug.Assert(result == length);

            str = Encoding.UTF8.GetString(span);

            return str;

        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex);
            return string.Empty;
        }
    }

    public string ReadWWString()
    {
        try
        {
            var str = string.Empty;

            var length = Cur_Chunk_Length;

            byte[] stringBytes = new byte[length];
            var span = stringBytes.AsSpan();

            long result = Read(span);

            Debug.Assert(result == length);

            str = Encoding.UTF8.GetString(stringBytes);

            return str;

        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex);
            return string.Empty;
        }
    }

    public string ReadString()
    {
        var str = string.Empty;
        Read(ref str);
        return str;
    }

    public string ReadWideString()
    {
        try
        {
            UInt32 length = 0;

            long result = Read(ref length);

            byte[] stringBytes = new byte[length * 2];
            var span = stringBytes.AsSpan();

            result += Read(span);

            Debug.Assert(result == sizeof(UInt32) + (length * 2));

            string str = Encoding.Unicode.GetString(stringBytes);

            return str;
        }
        catch (Exception ex)
        {
            Trace.WriteLine(ex);
            return string.Empty;
        }
    }


    private ChunkHeader ReadChunkHeader()
    {
        ChunkHeader header = new()
        {
            ChunkType = _file.ReadUInt32(),
            ChunkSize = _file.ReadUInt32()
        };

        //Console.WriteLine(header);

        return header;
    }

    public void Dispose()
    {
        _file.Dispose();
    }
}