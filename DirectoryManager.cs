using System;
using System.Text;

public class DirectoryManager
{
    private readonly Virtual_disk_operation _disk;

    public DirectoryManager(Virtual_disk_operation disk)
    {
        _disk = disk;
    }

    // تخزّن سطر واحد: filename|startCluster\n
    public void WriteFileEntry(string filename, int startCluster)
    {
        string line = filename + "|" + startCluster + "\n";
        byte[] data = Encoding.UTF8.GetBytes(line);

        _disk.WriteCluster(FsConstants.ROOT_DIR_FIRST_CLUSTER, data);
    }

    public string ReadRootDirectory()
    {
        byte[] data = _disk.ReadCluster(FsConstants.ROOT_DIR_FIRST_CLUSTER);
        return Encoding.UTF8.GetString(data);
    }
}
