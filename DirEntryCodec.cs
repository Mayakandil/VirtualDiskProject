using System;
using System.Text;

public static class DirectoryEntryCodec
{
    // 32 bytes → DirectoryEntry
    public static DirectoryEntry ParseEntry(byte[] buffer, int offset, uint owningCluster)
    {
        DirectoryEntry entry = new DirectoryEntry
        {
            OwningCluster = owningCluster,
            OffsetInCluster = offset
        };

        byte firstByte = buffer[offset];
        if (firstByte == 0x00)
        {
            entry.Name = string.Empty;
            return entry;
        }

        byte[] nameBytes = new byte[11];
        Array.Copy(buffer, offset, nameBytes, 0, 11);
        entry.Name = DirectoryNameHelper.Parse8Dot3Name(nameBytes);

        entry.Attr = buffer[offset + 11];

        entry.FirstCluster = BitConverter.ToUInt32(buffer, offset + 12);
        entry.FileSize     = BitConverter.ToUInt32(buffer, offset + 16);

        return entry;
    }

    // DirectoryEntry → 32 bytes
    public static void WriteEntry(byte[] buffer, int offset, DirectoryEntry entry)
    {
        if (string.IsNullOrEmpty(entry.Name))
        {
            for (int i = 0; i < 32; i++)
                buffer[offset + i] = 0x00;
            return;
        }

        string formatted = DirectoryNameHelper.FormatNameTo8Dot3(entry.Name);
        byte[] nameBytes = Encoding.ASCII.GetBytes(formatted);
        Array.Copy(nameBytes, 0, buffer, offset, 11);

        buffer[offset + 11] = entry.Attr;

        BitConverter.GetBytes(entry.FirstCluster).CopyTo(buffer, offset + 12);
        BitConverter.GetBytes(entry.FileSize).CopyTo(buffer, offset + 16);

        for (int i = 20; i < 32; i++)
            buffer[offset + i] = 0x00;
    }
}
