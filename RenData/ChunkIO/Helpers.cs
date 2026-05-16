using System.Runtime.InteropServices;

namespace RenData.ChunkIO;

public static class Helpers
{
    public static int WriteAs<T>(this BinaryWriter writer, T source) where T : struct
    {
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

        writer.Write(output);
        return length;
    }
}

// Regex to convert C++ macros to C# Read/Write Micro functions:
// READ MICRO CHUNK
//READ_MICRO_CHUNK\(cload, (\S*), (\S*)\);
//case $1:\n\t\t\t\t\tcload.Read(ref $2);\n\t\t\t\t\tbreak;

// READ MICRO CHUNK
//READ_MICRO_CHUNK\(cload, (\S*), (\S*)\);
//case $1:\n\t\t\t\t\tcload.Read(ref $2);\n\t\t\t\t\tbreak;

// WRITE MICRO CHUNK
//WRITE_MICRO_CHUNK\(csave, (\S*), (\S*)\);
//csave.WriteMicro($1, $2);

// WRITE MICRO CHUNK WWSTRING
//WRITE_MICRO_CHUNK_WWSTRING\(csave, (\S*), (\S*)\);
//ArgumentNullException.ThrowIfNull($2);\n\t\t\t\t\t\tcsave.WriteMicroString($1, $2);