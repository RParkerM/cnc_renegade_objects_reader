using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace RenData.ChunkIO;

public class ChunkSaveClass : IDisposable
{
    public ChunkSaveClass(Stream file)
    {
        if (!file.CanWrite)
        {
            throw new InvalidOperationException("Stream must be writable.");
        }
        if (!file.CanSeek)
        {
            throw new InvalidOperationException("Stream must be seekable.");
        }

        File = new BinaryWriter(file);
        StackIndex = 0;
        PositionStack = new uint[256];
        HeaderStack = new ChunkHeader[256];
        InMicroChunk = false;
        MicroChunkPosition = 0;
        MCHeader.ChunkType = 0;
    }

    /*
     * Fields
     */
    private readonly BinaryWriter File;
    private int StackIndex;
    private readonly uint[] PositionStack;
    private readonly ChunkHeader[] HeaderStack;
    private bool InMicroChunk;
    private int MicroChunkPosition;
    private MicroChunkHeader MCHeader;

    /*
     * Functions
     */
    public bool Begin_Chunk(uint id)
    {
        ChunkHeader chunkh;
        chunkh.ChunkSize = 0;
        chunkh.ChunkType = 0;
        if (StackIndex > 0)
        {
            HeaderStack[StackIndex - 1].ChunkSize |= 0x80000000;
        }
        chunkh.ChunkType = id;
        chunkh.ChunkSize &= 0x80000000;
        PositionStack[StackIndex] = (uint)File.BaseStream.Seek(0, SeekOrigin.Current);
        HeaderStack[StackIndex].ChunkType = chunkh.ChunkType;
        HeaderStack[StackIndex].ChunkSize = chunkh.ChunkSize;
        StackIndex++;
        if (File.WriteAs(chunkh) == 8)
        {
            return true;
        }
        return false;
    }

    public bool End_Chunk()
    {
        ChunkHeader chunkh;
        int temp = (int)File.BaseStream.Seek(0, SeekOrigin.Current);
        StackIndex--;
        chunkh.ChunkType = HeaderStack[StackIndex].ChunkType;
        chunkh.ChunkSize = HeaderStack[StackIndex].ChunkSize;
        File.BaseStream.Seek(PositionStack[StackIndex], SeekOrigin.Begin);
        if (File.WriteAs(chunkh) == 8)
        {
            if (StackIndex != 0 && StackIndex < 256)
            {
                uint temp2 = (HeaderStack[StackIndex - 1].ChunkSize & 0x7FFFFFFF) + (chunkh.ChunkSize & 0x7FFFFFFF) + 8;
                if ((HeaderStack[StackIndex - 1].ChunkSize & 0x80000000) == 0x80000000)
                {
                    temp2 |= 0x80000000;
                }
                HeaderStack[StackIndex - 1].ChunkSize = temp2;
            }
            File.BaseStream.Seek(temp, SeekOrigin.Begin);
            return true;
        }
        return false;
    }

    public int Cur_Chunk_Depth()
    {
        return StackIndex;
    }

    public uint Cur_Chunk_Length()
    {
        return HeaderStack[StackIndex - 1].ChunkSize & 0x7FFFFFFF;
    }

    public bool Begin_Micro_Chunk(byte id)
    {
        MCHeader.ChunkType = id;
        MicroChunkPosition = (int)File.BaseStream.Seek(0, SeekOrigin.Current);
        MCHeader.ChunkSize = 0;
        if (this.SimpleWrite(MCHeader) == Marshal.SizeOf(MCHeader))
        {
            InMicroChunk = true;
            return true;
        }
        return false;
    }

    public bool End_Micro_Chunk()
    {
        Debug.Assert(InMicroChunk, "InMicroChunk");
        int temp = (int)File.BaseStream.Seek(0, SeekOrigin.Current);
        File.BaseStream.Seek(MicroChunkPosition, SeekOrigin.Begin);
        if (File.WriteAs(MCHeader) == Marshal.SizeOf(MCHeader))
        {
            File.BaseStream.Seek(temp, SeekOrigin.Begin);
            InMicroChunk = false;
            return true;
        }
        return false;
    }

    public uint WriteWide(string source)
    {
        uint written = SimpleWrite((uint)source.Length);
        written += Write(Encoding.Unicode.GetBytes(source));
        return written;
    }

    public uint SimpleWrite<T>(T source) where T : struct
    {
        if (typeof(T).IsEnum)
        {
            return SimpleWrite(Convert.ToInt32(source));
        }
        if (source is bool b)
        {
            return SimpleWrite((byte)(b ? 1 : 0));
        }
        var length = Marshal.SizeOf(source);
        byte[] output = new byte[length];

        nint ptr = nint.Zero;
        try
        {
            ptr = Marshal.AllocHGlobal(length);
            Marshal.StructureToPtr(source, ptr, true);
            Marshal.Copy(ptr, output, 0, length);
        }
        finally
        {
            Marshal.FreeHGlobal(ptr);
        }

        Write(output);
        return (uint)length;
    }

    public uint Write(byte[] buf)
    {
        File.Write(buf);

        uint temp = (HeaderStack[StackIndex - 1].ChunkSize & 0x7FFFFFFF) + (uint)buf.Length;
        if ((HeaderStack[StackIndex - 1].ChunkSize & 0x80000000) == 0x80000000)
        {
            temp |= 0x80000000;
        }
        HeaderStack[StackIndex - 1].ChunkSize = temp;
        if (InMicroChunk)
        {
            MCHeader.ChunkSize = checked((byte)(MCHeader.ChunkSize + (char)buf.Length));
        }
        return (uint)buf.Length;
    }

    public uint Write(string str)
    {
        ushort length = (ushort)str.Length;
        uint result = this.SimpleWrite(length);
        result += Write(Encoding.Default.GetBytes(str));
        return result;
    }


    public void WriteMicro<T>(uint microChunkId, T obj) where T : struct => WriteMicro(checked((byte)microChunkId), obj);
    public void WriteMicro<T>(byte microChunkId, T obj) where T : struct
    {
        Begin_Micro_Chunk(microChunkId);
        this.SimpleWrite(obj);
        End_Micro_Chunk();
    }
    public void WriteMicroString(uint microChunkId, string str) => WriteMicroString(checked((byte)microChunkId), str);
    public void WriteMicroString(byte microChunkId, string str)
    {
        Begin_Micro_Chunk(microChunkId);
        Write(Encoding.Default.GetBytes(str));
        End_Micro_Chunk();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            File?.Dispose();
        }
    }
}