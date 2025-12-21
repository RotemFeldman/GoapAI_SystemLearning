
public static class StringExtensions
{
    // https://en.wikipedia.org/wiki/Fowler%E2%80%93Noll%E2%80%93Vo_hash_function#FNV-1_hash
    public static int ComputeFNV1aHash(this string str)
    {
        uint hash = 2166136261;
        foreach (var c in str)
        {
            hash = (hash ^ c) * 16777619;
        }
        return unchecked((int)hash);
    }
}
