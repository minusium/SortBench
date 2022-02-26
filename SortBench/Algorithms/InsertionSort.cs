namespace SortBench.Algorithms
{
    public class InsertionSort : ISortAlgorithm
    {
        public string Name => nameof(InsertionSort);

        public void Run(int[] target)
        {
            for (int i = 1; i < target.Length; ++i)
            {
                int key = target[i];
                int j = i - 1;

                // Move elements of arr[0..i-1],
                // that are greater than key,
                // to one position ahead of
                // their current position
                while (j >= 0 && target[j] > key)
                {
                    target[j + 1] = target[j];
                    j = j - 1;
                }
                target[j + 1] = key;
            }
        }
    }
}
