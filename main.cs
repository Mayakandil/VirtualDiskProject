using System;
using System.Text;


class Program
{
    static void Main()
    {
        Virtual_disk_operation vd = new Virtual_disk_operation();
        vd.Initialize("virtualdisk.bin");

        //write to cluster(0)

        byte[] data = System.Text.Encoding.UTF8.GetBytes("Hello Virtual Disk");
        vd.WriteCluster(0, data);


        //read cluster 0 
        byte[] readData = vd.readcluster(0);
        Console.WriteLine(System.Text.Encoding.UTF8.GetString(readData));


        Console.WriteLine($"Disk size: {vd.GetDiskSize()} bytes");
        vd.CloseDisk();


                        //task2 "SuperBlock"


        
        Console.WriteLine("\n--- Superblock Manager  ---");

        // Initialize Superblock 
        SuperblockManager sm = new SuperblockManager("virtualdisk.bin");

        // Read superblock
        byte[] superData = sm.ReadSuperblock();
        Console.WriteLine($"Superblock initially: {BitConverter.ToString(superData, 0, 16)} ... (first 16 bytes)");

        // Write superblock
        byte[] newSuperblock = new byte[FsConstants.CLUSTER_SIZE];
        string headerText = "This is the Superblock Header!";
        Array.Copy(Encoding.UTF8.GetBytes(headerText), newSuperblock, headerText.Length);
        sm.WriteSuperblock(newSuperblock);

    }

    

}