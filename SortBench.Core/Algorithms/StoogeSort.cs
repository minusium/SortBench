namespace SortBench.Core.Algorithms
{
    public class StoogeSort : ISortAlgorithm
    {
        // Function to implement stooge sort
        private static void DoStoogeSort(int[] arr, int l, int h)
        {
            if (l >= h)
                return;

            // If first element is smaller
            // than last, swap them
            if (arr[l] > arr[h])
            {
                (arr[l], arr[h]) = (arr[h], arr[l]);
            }

            // If there are more than 2
            // elements in the array
            if (h - l + 1 <= 2) return;
            var t = (h - l + 1) / 3;

            // Recursively sort first
            // 2/3 elements
            DoStoogeSort(arr, l, h - t);

            // Recursively sort last
            // 2/3 elements
            DoStoogeSort(arr, l + t, h);

            // Recursively sort first
            // 2/3 elements again to
            // confirm
            DoStoogeSort(arr, l, h - t);
        }

        public string Name => nameof(StoogeSort);

        public void Run(int[] target)
        {
            DoStoogeSort(target, 0, target.Length - 1);
        }
    }
}
