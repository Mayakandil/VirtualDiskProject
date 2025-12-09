using System;
using System.Collections.Generic;

public class ChainHelper
{
    private readonly FAT _fat;

    public ChainHelper(FAT fat)
    {
        _fat = fat;
    }

    // يربط list من الكلاسترز على شكل chain في FAT
    public int CreateChain(List<int> clusters)
    {
        if (clusters == null || clusters.Count == 0)
            return -1;

        for (int i = 0; i < clusters.Count - 1; i++)
        {
            _fat.Set(clusters[i], clusters[i + 1]);
        }

        // آخر واحد end-of-chain
        _fat.Set(clusters[clusters.Count - 1], -1);

        // start cluster
        return clusters[0];
    }

    // يمشي في الchain من start ويرجع list
    public List<int> FollowChain(int start)
    {
        List<int> chain = new List<int>();

        int current = start;
        while (current != -1 && current >= 0 && current < FsConstants.CLUSTER_COUNT)
        {
            chain.Add(current);
            int next = _fat.Get(current);
            if (next == 0) break; // no link
            current = next;
        }

        return chain;
    }

    public void FreeChain(int start)
    {
        int current = start;

        while (current != -1 && current != 0)
        {
            int next = _fat.Get(current);
            _fat.Set(current, 0);
            current = next;
        }
    }
}
