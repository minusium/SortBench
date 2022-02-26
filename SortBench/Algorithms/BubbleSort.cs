namespace SortBench.Algorithms
{
    public class BubbleSort : ISortAlgorithm
    {
        public string Name => nameof(BubbleSort);

        public void Run(int[] target)
        {
            for (int i = 0; i < target.Length - 1; i++)
            {
                for (int j = 0; j < target.Length - i - 1; j++)
                {
                    if (target[j] > target[j + 1])
                    {
                        // swap temp and arr[i]
                        int temp = target[j];
                        target[j] = target[j + 1];
                        target[j + 1] = temp;
                    }
                }
            }
        }
    }
}
