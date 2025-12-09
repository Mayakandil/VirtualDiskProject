using System.Collections.Generic;

public static class Directory
{
    
    public static List<DirectoryEntry> ReadDirectory(uint startCluster)
    {
        return new List<DirectoryEntry>();
    }

    public static List<DirectoryEntry> ListRoot()
    {
        return ReadDirectory((uint)FsConstants.ROOT_DIR_FIRST_CLUSTER);
    }
}
