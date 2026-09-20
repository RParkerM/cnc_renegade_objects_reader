namespace RenData.Packaging;

/// <summary>
/// One entry in a package's file table (a <c>FILE</c> chunk).
/// </summary>
public sealed class PackageFileEntry
{
    /// <summary>CRC32 of the virtual file name; also the base name of the blob in the sibling <c>files/</c> directory.</summary>
    public uint FileCRC { get; set; }

    /// <summary>Uncompressed size of the file in bytes.</summary>
    public uint FileSize { get; set; }

    /// <summary>Virtual path of the file (backslash separated), e.g. <c>mp_canyon+\43.dds</c>.</summary>
    public string FileName { get; set; } = string.Empty;

    /// <summary>
    /// The name this entry has as a flat blob in the sibling <c>files/</c> directory:
    /// the 8-digit uppercase hex CRC, a dot, then the virtual name with <c>'\'</c> flattened to <c>'_'</c>
    /// (e.g. <c>47F793B9.mp_canyon+_43.dds</c>). Verified to match every blob in the sample data.
    /// </summary>
    public string BlobFileName => $"{FileCRC:X8}.{FileName.Replace('\\', '_')}";

    public override string ToString() => $"{FileName} (0x{FileCRC:X8}, {FileSize} bytes)";
}

/// <summary>
/// A single installed package as stored in <c>packages.dat</c> (a <c>PCKG</c> chunk),
/// which is also the exact content of the matching <c>&lt;crc&gt;.tpi</c> file.
/// </summary>
public sealed class PackageClass
{
    /// <summary>CRC32 of the package name (<c>HEAD</c> chunk); matches the <c>.tpi</c> / package folder name.</summary>
    public uint PackageCRC { get; set; }

    /// <summary>Package name, e.g. <c>C&amp;C_Canyon</c> (<c>DATA</c> chunk).</summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>Package version string, e.g. <c>1.0</c> (<c>DATA</c> chunk).</summary>
    public string Version { get; set; } = string.Empty;

    /// <summary>Package owner/author, e.g. <c>Westwood</c> (<c>DATA</c> chunk).</summary>
    public string Owner { get; set; } = string.Empty;

    /// <summary>
    /// Trailing uint32 in the <c>DATA</c> chunk. Constant <c>2</c> in every observed sample;
    /// exact meaning unconfirmed (likely a package type/format tag). Preserved verbatim so files round-trip.
    /// </summary>
    public uint Type { get; set; } = 2;

    /// <summary>The file table (<c>FILE</c> chunks), in file order.</summary>
    public List<PackageFileEntry> Files { get; } = [];

    /// <summary>
    /// Computes the package ID (the <c>&lt;id&gt;.tpi</c> / install id) exactly as PackageEditor does,
    /// from the current name/version/owner/type and file table. See <see cref="PackageCrc"/>.
    /// </summary>
    public uint ComputeId() => PackageCrc.ComputeId(this);

    /// <summary>Recomputes <see cref="PackageCRC"/> from the current contents and returns it.</summary>
    public uint RecomputeId() => PackageCRC = ComputeId();

    public override string ToString() => $"{Name} {Version} by {Owner} (0x{PackageCRC:X8}, {Files.Count} files)";
}
