using System;
using System.IO;
using System.Text;
public class Virtual_disk_operation
{
    private const int ClusterSize = 1024;
    private FileStream diskStream; //what the hell
    private string diskPath;

    //Opens the virtual disk file and prepares it for reading/writing.
    // Think: What if the file does not exist?
    public void Initialize(String path)
    {
        diskPath = path;
        try
        {
            if (!File.Exists(path))
            {
                Console.WriteLine("Disk file not foound - creating new virtual disk........");
                diskStream = new FileStream(path, FileMode.CreateNew, FileAccess.ReadWrite);
            }
            else
            {
                diskStream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error initializing cirtual disk: {ex.Message}");
        }

    }

 //reads the 1024 bytes from cluster n
    public Byte[]  readcluster (int n)
    {
        if (diskStream == null)
            throw new InvalidOperationException("disk not intialized.");

        byte[] buffer = new byte[ClusterSize];
        long offset = n * ClusterSize;

        if (offset >= diskStream.Length)
            throw new ArgumentOutOfRangeException("cluster number exceeds disk size. ");

        diskStream.Seek(offset, SeekOrigin.Begin);
        int bytesRead = diskStream.Read(buffer, 0, ClusterSize);

        return buffer;
    }


    //writes 1024 bytes to cluster n
    public void WriteCluster(int n, byte[] data)
    {
        if (diskStream == null)
            throw new InvalidOperationException(" Disk is not intialized. ");
        if (data.Length > ClusterSize)
            throw new ArgumentOutOfRangeException(" Data exceeds cluster size");

        long offset = n * ClusterSize;
        diskStream.Seek(offset, SeekOrigin.Begin);

        try
        {
            diskStream.Write(data, 0, data.Length);
            diskStream.Flush();
        }
        catch (IOException ex)
        {
            Console.WriteLine($"I/O error during writing :{ex.Message} ");
        }
    }


    //closes the disk safley
    // ⚬ Think: What if the user closed the file during a write operation?
    public void CloseDisk()
    {
        try
        {
            diskStream?.Flush();
            diskStream?.Close();
            diskStream = null;
            Console.WriteLine("Disk closed safely. ");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error closeing disk: {ex.Message}");

        }

    }



    // ⚬ Returns the total size of the disk in bytes
    public long GetDiskSize()
    {
        if (diskStream == null)
            throw new InvalidOperationException("Disk is not initialized. ");
        return diskStream.Length;
    }
   


}



