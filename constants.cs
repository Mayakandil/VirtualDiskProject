using System;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Reflection.Metadata;
using System.Text;

public class FsConstants
{
    const byte cluster_size = 1024;
    const int cluster_Size = 1024;
    const int superBlock_cluster = 0;
    const int FAT_start_cluster = 1;
    const int FAT_end_cluster = 4;
    const int content_start_cluster = 5;
    const int root_first_cluster = 3;//idk

}