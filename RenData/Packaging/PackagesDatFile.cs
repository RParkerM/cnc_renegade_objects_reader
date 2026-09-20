using RenData.ChunkIO;

namespace RenData.Packaging;

/// <summary>
/// Reader/writer for Renegade TTFS <c>packages.dat</c> files and the individual
/// <c>&lt;crc&gt;.tpi</c> package-info files, using the Westwood chunk format
/// (<see cref="ChunkLoadClass"/> / <see cref="ChunkSaveClass"/>).
///
/// <para>
/// Layout — a flat sequence of top-level <c>PCKG</c> chunks (one per package), each containing:
/// <list type="bullet">
///   <item><c>HEAD</c>: uint32 packageCRC, uint32 fileCount</item>
///   <item><c>DATA</c>: wwstring name, wwstring version, wwstring owner, uint32 type(=2)</item>
///   <item><c>FILE</c> × fileCount: uint32 fileCRC, uint32 fileSize, wwstring fileName</item>
/// </list>
/// A <c>.tpi</c> file is the body of a single <c>PCKG</c> chunk (i.e. the HEAD/DATA/FILE
/// sub-chunks without the enclosing <c>PCKG</c> header).
/// </para>
/// </summary>
public static class PackagesDatFile
{
    // Chunk IDs are 4-char tags stored little-endian, so a hex dump shows them reversed
    // (e.g. "PCKG" appears as bytes 'G' 'K' 'C' 'P'). As a uint that is the tag read big-endian.
    private static uint Tag(string s) => ((uint)s[0] << 24) | ((uint)s[1] << 16) | ((uint)s[2] << 8) | s[3];

    private static readonly uint CHUNKID_PACKAGE = Tag("PCKG");
    private static readonly uint CHUNKID_HEAD = Tag("HEAD");
    private static readonly uint CHUNKID_DATA = Tag("DATA");
    private static readonly uint CHUNKID_FILE = Tag("FILE");

    // ── Load ──────────────────────────────────────────────────────────────────

    /// <summary>Loads every package from a <c>packages.dat</c> file.</summary>
    public static List<PackageClass> Load(string path)
    {
        using var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
        return Load(stream);
    }

    /// <summary>Loads every package from a stream positioned at the start of the package list.</summary>
    public static List<PackageClass> Load(Stream stream)
    {
        var packages = new List<PackageClass>();
        var cload = new ChunkLoadClass(stream);

        while (cload.Open_Chunk())
        {
            if (cload.Cur_Chunk_ID == CHUNKID_PACKAGE)
            {
                packages.Add(LoadPackage(cload));
            }
            cload.Close_Chunk();
        }

        return packages;
    }

    /// <summary>Loads a single package from a <c>.tpi</c> file (a bare package body, no <c>PCKG</c> header).</summary>
    public static PackageClass LoadTpi(string path)
    {
        using var stream = new FileStream(path, FileMode.Open, FileAccess.Read);
        return LoadTpi(stream);
    }

    /// <summary>Loads a single package from a <c>.tpi</c> stream (a bare package body, no <c>PCKG</c> header).</summary>
    public static PackageClass LoadTpi(Stream stream)
    {
        // A .tpi is the *body* of a PCKG chunk, so there is no outer chunk to open.
        // Wrap it in a synthetic PCKG chunk header so the same chunk loader can walk it.
        using var wrapped = new MemoryStream();
        Span<byte> header = stackalloc byte[8];
        long length = stream.Length - stream.Position;
        BitConverter.TryWriteBytes(header[..4], CHUNKID_PACKAGE);
        BitConverter.TryWriteBytes(header[4..], (uint)length | 0x80000000u); // contains sub-chunks
        wrapped.Write(header);
        stream.CopyTo(wrapped);
        wrapped.Position = 0;

        return Load(wrapped).Single();
    }

    private static PackageClass LoadPackage(ChunkLoadClass cload)
    {
        var package = new PackageClass();

        while (cload.Open_Chunk())
        {
            switch (cload.Cur_Chunk_ID)
            {
                case var id when id == CHUNKID_HEAD:
                    package.PackageCRC = cload.Read<uint>();
                    _ = cload.Read<uint>(); // fileCount — recomputed from Files on save
                    break;

                case var id when id == CHUNKID_DATA:
                    package.Name = cload.ReadString();
                    package.Version = cload.ReadString();
                    package.Owner = cload.ReadString();
                    package.Type = cload.Read<uint>();
                    break;

                case var id when id == CHUNKID_FILE:
                    var entry = new PackageFileEntry
                    {
                        FileCRC = cload.Read<uint>(),
                        FileSize = cload.Read<uint>(),
                    };
                    entry.FileName = cload.ReadString();
                    package.Files.Add(entry);
                    break;
            }

            cload.Close_Chunk();
        }

        return package;
    }

    // ── Save ──────────────────────────────────────────────────────────────────

    /// <summary>Writes packages to a <c>packages.dat</c> file. An unedited file round-trips bit-for-bit.</summary>
    public static void Save(string path, IEnumerable<PackageClass> packages)
    {
        using var stream = new FileStream(path, FileMode.Create, FileAccess.Write);
        Save(stream, packages);
    }

    /// <summary>Writes packages to a stream. An unedited file round-trips bit-for-bit.</summary>
    public static void Save(Stream stream, IEnumerable<PackageClass> packages)
    {
        var csave = new ChunkSaveClass(stream);

        foreach (var package in packages)
        {
            csave.Begin_Chunk(CHUNKID_PACKAGE);
            SavePackageBody(csave, package);
            csave.End_Chunk();
        }

        stream.Flush();
    }

    /// <summary>Writes a single package as a <c>.tpi</c> file (bare package body, no <c>PCKG</c> header).</summary>
    public static void SaveTpi(string path, PackageClass package)
    {
        using var stream = new FileStream(path, FileMode.Create, FileAccess.Write);
        SaveTpi(stream, package);
    }

    /// <summary>Writes a single package as a bare package body (no <c>PCKG</c> header) to a stream.</summary>
    public static void SaveTpi(Stream stream, PackageClass package)
    {
        // Serialize as a normal package, then strip the 8-byte outer PCKG header.
        using var buffer = new MemoryStream();
        Save(buffer, [package]);
        buffer.Position = 8;
        buffer.CopyTo(stream);
        stream.Flush();
    }

    private static void SavePackageBody(ChunkSaveClass csave, PackageClass package)
    {
        csave.Begin_Chunk(CHUNKID_HEAD);
        csave.SimpleWrite(package.PackageCRC);
        csave.SimpleWrite((uint)package.Files.Count);
        csave.End_Chunk();

        csave.Begin_Chunk(CHUNKID_DATA);
        csave.Write(package.Name);
        csave.Write(package.Version);
        csave.Write(package.Owner);
        csave.SimpleWrite(package.Type);
        csave.End_Chunk();

        foreach (var file in package.Files)
        {
            csave.Begin_Chunk(CHUNKID_FILE);
            csave.SimpleWrite(file.FileCRC);
            csave.SimpleWrite(file.FileSize);
            csave.Write(file.FileName);
            csave.End_Chunk();
        }
    }
}
