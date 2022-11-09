namespace SortBench.Core.Algorithms
{
    public class SelectionSort : ISortAlgorithm
    {
        public string Name => nameof(SelectionSort);

        private static void DoSelectionSort(int[] arr)
        {
            var n = arr.Length;

            // One by one move boundary of unsorted subarray
            for (var i = 0; i < n - 1; i++)
            {
                // Find the minimum element in unsorted array
                var minIdx = i;
                for (var j = i + 1; j < n; j++)
                    if (arr[j] < arr[minIdx])
                        minIdx = j;

                // Swap the found minimum element with the first
                // element
                (arr[minIdx], arr[i]) = (arr[i], arr[minIdx]);
            }
        }

        public void Run(int[] target)
        {
            DoSelectionSort(target);
        }
    }
}
