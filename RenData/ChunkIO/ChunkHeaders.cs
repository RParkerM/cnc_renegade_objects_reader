using System.Runtime.InteropServices;
using System.Text;

namespace RenData.ChunkIO;

[StructLayout(LayoutKind.Sequential)]
public struct ChunkHeader
{
    public uint ChunkType;
    public uint ChunkSize;

    public override string ToString()
    {
        StringBuilder sb = new();

        byte[] bytes = BitConverter.GetBytes(ChunkType);

        // Display bytes in hex format
        sb.AppendLine($"Chunk Type: {ChunkType}");
        sb.Append("Hexadecimal Representation: ");
        foreach (byte b in bytes)
        {
            sb.Append($"{b:X2} "); // Format byte as two-digit hex
        }
        sb.AppendLine();

        // Display bytes as characters
        sb.Append("Character Representation: ");
        foreach (byte b in bytes)
        {
            sb.Append($"{(char)b} ");
        }

        sb.AppendLine();
        sb.AppendLine($"Chunk Size: {ChunkSize & 0x7FFFFFFF}");

        sb.AppendLine($"Contains Chunks: {((ChunkSize & 0x80000000) > 0 ? "true" : "false")}");
        return sb.ToString();
    }
}

[StructLayout(LayoutKind.Sequential)]
public struct MicroChunkHeader
{
    public byte ChunkType;
    public byte ChunkSize;

    public override string ToString()
    {
        StringBuilder sb = new();

        // Display bytes in hex format
        sb.AppendLine("Chunk Type:");
        sb.Append($"Hexadecimal Representation: {ChunkType:X2}");
        sb.AppendLine();

        // Display bytes as characters
        sb.Append($"Character Representation: {(char)ChunkType}");

        sb.AppendLine();
        sb.AppendLine($"Chunk Size: {ChunkSize}");

        return sb.ToString();
    }
}

[StructLayout(LayoutKind.Sequential)]
public struct MicroChunkHeader2
{
    public ushort ChunkType;
    public ushort ChunkSize;
}

public static class ChunkExtensions
{
    public static string GetString(this ChunkHeader header)
    {
        return $"ChunkHeader: {{ChunkType: 0x{header.ChunkType:X2}, ChunkSize: {header.ChunkSize & 0x7FFFFFFF}}}, Contains Chunks: {((header.ChunkSize & 0x80000000) > 0 ? "true" : "false")}";
    }

    public static string GetString(this MicroChunkHeader header)
    {
        return $"ChunkHeader: {{ChunkType: 0x{header.ChunkType:X2}, ChunkSize: {header.ChunkSize}}}";
    }
}
