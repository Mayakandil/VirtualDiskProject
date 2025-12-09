using System;

public static class FsConstants
{
    // Basic disk layout
    public const int CLUSTER_SIZE   = 1024;
    public const int CLUSTER_COUNT  = 1000;

    public const int SUPERBLOCK_CLUSTER   = 0;
    public const int FAT_START_CLUSTER    = 1;
    public const int FAT_END_CLUSTER      = 4;
    public const int ROOT_DIR_FIRST_CLUSTER = 5;
    public const int CONTENT_START_CLUSTER  = 6;

    // End-of-chain marker for FAT
    public const uint END_OF_CHAIN = 0xFFFFFFFF;
}
