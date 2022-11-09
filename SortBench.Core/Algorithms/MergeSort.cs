namespace SortBench.Core.Algorithms
{
    public class MergeSort : ISortAlgorithm
    {
        public string Name => nameof(MergeSort);

        // Merges two subarrays of []arr.
        // First subarray is arr[l..m]
        // Second subarray is arr[m+1..r]
        private static void Merge(int[] arr, int l, int m, int r)
        {
            // Find sizes of two
            // subarrays to be merged
            var n1 = m - l + 1;
            var n2 = r - m;

            // Create temp arrays
            var left = new int[n1];
            var right = new int[n2];
            int i, j;

            // Copy data to temp arrays
            for (i = 0; i < n1; ++i)
                left[i] = arr[l + i];
            for (j = 0; j < n2; ++j)
                right[j] = arr[m + 1 + j];

            // Merge the temp arrays

            // Initial indexes of first
            // and second subarrays
            i = 0;
            j = 0;

            // Initial index of merged
            // subarray array
            var k = l;
            while (i < n1 && j < n2)
            {
                if (left[i] <= right[j])
                {
                    arr[k] = left[i];
                    i++;
                }
                else
                {
                    arr[k] = right[j];
                    j++;
                }
                k++;
            }

            // Copy remaining elements
            // of L[] if any
            while (i < n1)
            {
                arr[k] = left[i];
                i++;
                k++;
            }

            // Copy remaining elements
            // of R[] if any
            while (j < n2)
            {
                arr[k] = right[j];
                j++;
                k++;
            }
        }

        // Main function that
        // sorts arr[l..r] using
        // merge()
        private static void DoMergeSort(int[] arr, int l, int r)
        {
            if (l >= r) return;

            // Find the middle
            // point
            var m = l + (r - l) / 2;

            // Sort first and
            // second halves
            DoMergeSort(arr, l, m);
            DoMergeSort(arr, m + 1, r);

            // Merge the sorted halves
            Merge(arr, l, m, r);
        }

        public void Run(int[] target)
        {
            DoMergeSort(target, 0, target.Length - 1);
        }
    }
}
