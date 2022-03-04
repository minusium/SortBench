namespace SortBench.Algorithms
{
    public class StoogeSort : ISortAlgorithm
    {
        // Function to implement stooge sort
        private static void stoogesort(int[] arr, int l, int h)
        {
            if (l >= h)
                return;

            // If first element is smaller
            // than last, swap them
            if (arr[l] > arr[h])
            {
                int t = arr[l];
                arr[l] = arr[h];
                arr[h] = t;
            }

            // If there are more than 2
            // elements in the array
            if (h - l + 1 > 2)
            {
                int t = (h - l + 1) / 3;

                // Recursively sort first
                // 2/3 elements
                stoogesort(arr, l, h - t);

                // Recursively sort last
                // 2/3 elements
                stoogesort(arr, l + t, h);

                // Recursively sort first
                // 2/3 elements again to
                // confirm
                stoogesort(arr, l, h - t);
            }
        }

        public string Name => nameof(StoogeSort);

        public void Run(int[] target)
        {
            stoogesort(target, 0, target.Length - 1);
        }
    }
}
