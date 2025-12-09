public class DirectoryEntry
{
    public string Name { get; set; } = string.Empty;
    public byte Attr { get; set; }
    public uint FirstCluster { get; set; }
    public uint FileSize { get; set; }

    // extra info for locating entry on disk (not stored in on-disk 32 bytes)
    public uint OwningCluster { get; set; }
    public int OffsetInCluster { get; set; }
}
