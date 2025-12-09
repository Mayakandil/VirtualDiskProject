using System;
using System.IO;

public class Virtual_disk_operation
{
    private const int ClusterSize = FsConstants.CLUSTER_SIZE;

    private FileStream _stream;
    private string _path = "";

    public void Initialize(string path)
    {
        _path = path;

        if (!File.Exists(path))
        {
            Console.WriteLine("Disk file not found - creating new virtual disk...");
            _stream = new FileStream(path, FileMode.CreateNew, FileAccess.ReadWrite);

            
            byte[] zero = new byte[ClusterSize];
            for (int i = 0; i < FsConstants.CLUSTER_COUNT; i++)
            {
                _stream.Write(zero, 0, zero.Length);
            }
            _stream.Flush();
        }
        else
        {
            _stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
        }
    }

    public byte[] ReadCluster(int clusterNumber)
    {
        if (_stream == null)
            throw new InvalidOperationException("Disk is not initialized.");

        if (clusterNumber < 0 || clusterNumber >= FsConstants.CLUSTER_COUNT)
            throw new ArgumentOutOfRangeException(nameof(clusterNumber));

        byte[] buffer = new byte[ClusterSize];
        long offset = (long)clusterNumber * ClusterSize;

        _stream.Seek(offset, SeekOrigin.Begin);
        _stream.Read(buffer, 0, ClusterSize);

        return buffer;
    }

    public void WriteCluster(int clusterNumber, byte[] data)
    {
        if (_stream == null)
            throw new InvalidOperationException("Disk is not initialized.");
        if (data == null)
            throw new ArgumentNullException(nameof(data));
        if (data.Length > ClusterSize)
            throw new ArgumentException("Data exceeds cluster size.");

        long offset = (long)clusterNumber * ClusterSize;
        _stream.Seek(offset, SeekOrigin.Begin);
        _stream.Write(data, 0, data.Length);

        
        if (data.Length < ClusterSize)
        {
            byte[] pad = new byte[ClusterSize - data.Length];
            _stream.Write(pad, 0, pad.Length);
        }

        _stream.Flush();
    }

    public long GetDiskSize()
    {
        if (_stream == null)
            throw new InvalidOperationException("Disk is not initialized.");
        return _stream.Length;
    }

    public void Close()
    {
        _stream?.Flush();
        _stream?.Close();
        _stream = null;
    }
}
