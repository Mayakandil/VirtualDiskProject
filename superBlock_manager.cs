using System;
using System.IO;

public class SuperblockManager
{
    private readonly string _diskPath;

    public SuperblockManager(string diskPath)
    {
        _diskPath = diskPath;

        if (!File.Exists(_diskPath))
        {
            InitializeNewDisk();
        }
    }

    private void InitializeNewDisk()
    {
        using var fs = new FileStream(_diskPath, FileMode.Create, FileAccess.Write);
        byte[] zeroCluster = new byte[FsConstants.CLUSTER_SIZE];

        for (int i = 0; i < FsConstants.CLUSTER_COUNT; i++)
        {
            fs.Write(zeroCluster, 0, zeroCluster.Length);
        }
    }

    public byte[] ReadSuperblock()
    {
        using var fs = new FileStream(_diskPath, FileMode.Open, FileAccess.Read);
        byte[] buffer = new byte[FsConstants.CLUSTER_SIZE];

        fs.Seek((long)FsConstants.SUPERBLOCK_CLUSTER * FsConstants.CLUSTER_SIZE, SeekOrigin.Begin);
        fs.Read(buffer, 0, buffer.Length);

        return buffer;
    }

    public void WriteSuperblock(byte[] data)
    {
        if (data.Length > FsConstants.CLUSTER_SIZE)
            throw new ArgumentException("Superblock data exceeds cluster size.");

        using var fs = new FileStream(_diskPath, FileMode.Open, FileAccess.Write);
        fs.Seek((long)FsConstants.SUPERBLOCK_CLUSTER * FsConstants.CLUSTER_SIZE, SeekOrigin.Begin);
        fs.Write(data, 0, data.Length);
    }
}
