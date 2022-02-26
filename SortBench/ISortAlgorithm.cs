namespace SortBench
{
    public interface ISortAlgorithm
    {
        string Name { get; }

        void Run(int[] target);
    }
}
