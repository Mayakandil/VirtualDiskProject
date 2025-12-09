using System;
using System.Collections.Generic;
using System.Text;

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Virtual OS Starting ===");

        // 1) Virtual disk
        var disk = new Virtual_disk_operation();
        disk.Initialize("virtualdisk.bin");
        Console.WriteLine("Disk initialized. Size = " + disk.GetDiskSize());

        // 2) Superblock
        var super = new SuperblockManager("virtualdisk.bin");
        byte[] superData = super.ReadSuperblock();
        Console.WriteLine("Superblock first 16 bytes: " +
                          BitConverter.ToString(superData, 0, 16));

        byte[] newSuper = new byte[FsConstants.CLUSTER_SIZE];
        string header = "This is the Superblock Header!";
        Array.Copy(Encoding.UTF8.GetBytes(header), newSuper, header.Length);
        super.WriteSuperblock(newSuper);
        Console.WriteLine("Superblock updated.");

        // 3) FAT
        var fat = new FAT(disk);
        fat.InitializeNew();   // fresh FAT
        fat.WriteAllFat();
        fat.ReadAllFat();
        Console.WriteLine("FAT initialized and loaded.");

        // 4) Allocate and create chain
        List<int> allocated = fat.Allocate(3);
        if (allocated.Count == 0)
        {
            Console.WriteLine("Not enough free clusters.");
            return;
        }

        Console.WriteLine("Allocated clusters: " + string.Join(", ", allocated));

        var chainHelper = new ChainHelper(fat);
        int startCluster = chainHelper.CreateChain(allocated);
        Console.WriteLine("Chain start cluster = " + startCluster);

        List<int> chain = chainHelper.FollowChain(startCluster);
        Console.WriteLine("Chain: " + string.Join(" -> ", chain) + " -> -1");

        // 5) Simple root directory manager
        var dirManager = new DirectoryManager(disk);
        dirManager.WriteFileEntry("test.txt", startCluster);

        Console.WriteLine("Root directory content:");
        Console.WriteLine(dirManager.ReadRootDirectory());

        // 6) Test DirectoryNameHelper
        string input = "hello.txt";
        string formatted = DirectoryNameHelper.FormatNameTo8Dot3(input);
        Console.WriteLine("Formatted 8.3: " + formatted);

        byte[] nameBytes = Encoding.ASCII.GetBytes(formatted);
        string parsedName = DirectoryNameHelper.Parse8Dot3Name(nameBytes);
        Console.WriteLine("Parsed name: " + parsedName);

        // 7) Test DirectoryEntryCodec
        byte[] buffer = new byte[32];

        DirectoryEntry original = new DirectoryEntry
        {
            Name = "test.txt",
            Attr = 0x20,
            FirstCluster = 15,
            FileSize = 1000,
            OwningCluster = 7,
            OffsetInCluster = 0
        };

        DirectoryEntryCodec.WriteEntry(buffer, 0, original);
        DirectoryEntry parsedEntry =
            DirectoryEntryCodec.ParseEntry(buffer, 0, original.OwningCluster);

        Console.WriteLine("Parsed entry:");
        Console.WriteLine("Name: " + parsedEntry.Name);
        Console.WriteLine("Attr: " + parsedEntry.Attr);
        Console.WriteLine("FirstCluster: " + parsedEntry.FirstCluster);
        Console.WriteLine("FileSize: " + parsedEntry.FileSize);

        Console.WriteLine("=== Virtual OS Finished ===");
    }
}
