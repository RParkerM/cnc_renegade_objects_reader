using System.Text;

namespace RenData.Packaging;

/// <summary>
/// Reproduces the package-ID hash used by Renegade's PackageEditor (TTFS). Recovered by
/// decompiling PackageEditor.exe: the ID is a zlib CRC-32 over the package metadata
/// (name ++ version ++ author ++ uint32LE(Type)), then folded with each file's
/// (CRC, size) via zlib's crc32_combine, in file order.
/// </summary>
public static class PackageCrc
{
    private static readonly uint[] Table = BuildTable();

    private static uint[] BuildTable()
    {
        var t = new uint[256];
        for (uint i = 0; i < 256; i++)
        {
            uint c = i;
            for (int k = 0; k < 8; k++)
                c = (c & 1) != 0 ? 0xEDB88320u ^ (c >> 1) : c >> 1;
            t[i] = c;
        }
        return t;
    }

    /// <summary>Standard zlib CRC-32. Chains like zlib: <c>Crc32(Crc32(0, a), b) == Crc32(0, a++b)</c>.</summary>
    public static uint Crc32(uint crc, ReadOnlySpan<byte> data)
    {
        crc ^= 0xFFFFFFFFu;
        foreach (var b in data)
            crc = Table[(crc ^ b) & 0xFF] ^ (crc >> 8);
        return crc ^ 0xFFFFFFFFu;
    }

    // --- zlib crc32_combine (GF(2) operator matrices) ---

    private static uint Gf2MatrixTimes(uint[] mat, uint vec)
    {
        uint sum = 0;
        int i = 0;
        while (vec != 0)
        {
            if ((vec & 1) != 0) sum ^= mat[i];
            vec >>= 1;
            i++;
        }
        return sum;
    }

    private static void Gf2MatrixSquare(uint[] square, uint[] mat)
    {
        for (int n = 0; n < 32; n++)
            square[n] = Gf2MatrixTimes(mat, mat[n]);
    }

    /// <summary>zlib <c>crc32_combine</c>: CRC of a stream whose first part has <paramref name="crc1"/> and whose next <paramref name="len2"/> bytes have <paramref name="crc2"/>.</summary>
    public static uint Crc32Combine(uint crc1, uint crc2, long len2)
    {
        if (len2 <= 0) return crc1;

        var even = new uint[32];
        var odd = new uint[32];

        odd[0] = 0xEDB88320u;      // CRC-32 polynomial
        uint row = 1;
        for (int n = 1; n < 32; n++) { odd[n] = row; row <<= 1; }

        Gf2MatrixSquare(even, odd);
        Gf2MatrixSquare(odd, even);

        do
        {
            Gf2MatrixSquare(even, odd);
            if ((len2 & 1) != 0) crc1 = Gf2MatrixTimes(even, crc1);
            len2 >>= 1;
            if (len2 == 0) break;

            Gf2MatrixSquare(odd, even);
            if ((len2 & 1) != 0) crc1 = Gf2MatrixTimes(odd, crc1);
            len2 >>= 1;
        } while (len2 != 0);

        return crc1 ^ crc2;
    }

    /// <summary>
    /// Computes a package's ID exactly as PackageEditor does. Strings are hashed as their raw
    /// bytes (Latin1/ASCII, no length prefix or terminator).
    /// </summary>
    public static uint ComputeId(PackageClass package)
    {
        var meta = new List<byte>();
        meta.AddRange(Encoding.Latin1.GetBytes(package.Name));
        meta.AddRange(Encoding.Latin1.GetBytes(package.Version));
        meta.AddRange(Encoding.Latin1.GetBytes(package.Owner));
        meta.AddRange(BitConverter.GetBytes(package.Type)); // uint32 little-endian

        uint crc = Crc32(0, System.Runtime.InteropServices.CollectionsMarshal.AsSpan(meta));
        foreach (var file in package.Files)
            crc = Crc32Combine(crc, file.FileCRC, file.FileSize);
        return crc;
    }
}
