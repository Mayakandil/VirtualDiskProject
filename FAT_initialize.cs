using System;
using System.Collections.Generic;

public class FAT
{
    private readonly int[] _table;
    private readonly Virtual_disk_operation _disk;

    public FAT(Virtual_disk_operation disk)
    {
        _disk = disk;
        _table = new int[FsConstants.CLUSTER_COUNT];
    }

   
    public void InitializeNew()
    {
        // reserved clusters (superblock + FAT + root dir)
        for (int i = 0; i <= FsConstants.FAT_END_CLUSTER; i++)
        {
            _table[i] = -1; // reserved
        }

        // باقي الكلاسترز free = 0
        for (int i = FsConstants.CONTENT_START_CLUSTER;
             i < FsConstants.CLUSTER_COUNT; i++)
        {
            _table[i] = 0;
        }
    }

    public int Get(int cluster) => _table[cluster];

    public void Set(int cluster, int value) => _table[cluster] = value;

        public void ReadAllFat()
    {
        byte[] fatBytes = new byte[FsConstants.CLUSTER_COUNT * 4];
        int offset = 0;

        for (int c = FsConstants.FAT_START_CLUSTER;
             c <= FsConstants.FAT_END_CLUSTER; c++)
        {
            byte[] clusterData = _disk.ReadCluster(c);
            Array.Copy(clusterData, 0, fatBytes, offset, clusterData.Length);
            offset += clusterData.Length;
        }

        for (int i = 0; i < FsConstants.CLUSTER_COUNT; i++)
        {
            _table[i] = BitConverter.ToInt32(fatBytes, i * 4);
        }
    }

    
    public void WriteAllFat()
    {
        byte[] fatBytes = new byte[FsConstants.CLUSTER_COUNT * 4];

        for (int i = 0; i < FsConstants.CLUSTER_COUNT; i++)
        {
            byte[] entryBytes = BitConverter.GetBytes(_table[i]);
            Array.Copy(entryBytes, 0, fatBytes, i * 4, 4);
        }

        int offset = 0;
        for (int c = FsConstants.FAT_START_CLUSTER;
             c <= FsConstants.FAT_END_CLUSTER; c++)
        {
            byte[] chunk = new byte[FsConstants.CLUSTER_SIZE];
            Array.Copy(fatBytes, offset, chunk, 0, FsConstants.CLUSTER_SIZE);
            offset += FsConstants.CLUSTER_SIZE;

            _disk.WriteCluster(c, chunk);
        }
    }

    // Allocate just finds free clusters, doesn’t link them
    public List<int> Allocate(int count)
    {
        List<int> free = new List<int>();

        for (int i = FsConstants.CONTENT_START_CLUSTER;
             i < FsConstants.CLUSTER_COUNT && free.Count < count; i++)
        {
            if (_table[i] == 0)
                free.Add(i);
        }

        if (free.Count < count)
            return new List<int>(); // مش كفاية free clusters

        return free;
    }
}
