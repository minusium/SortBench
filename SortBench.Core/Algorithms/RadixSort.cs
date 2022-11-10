namespace SortBench.Core.Algorithms
{
    internal class RadixSort : ISortAlgorithm
    {
        private static int GetMax(int[] arr, int n)
        {
            var mx = arr[0];
            for (var i = 1; i < n; i++)
                if (arr[i] > mx)
                    mx = arr[i];
            return mx;
        }

        // A function to do counting sort of arr[] according to
        // the digit represented by exp.
        private static void DoCountSort(int[] arr, int n, int exp)
        {
            var output = new int[n]; // output array
            int i;
            var count = new int[10];

            // initializing all elements of count to 0
            for (i = 0; i < 10; i++)
                count[i] = 0;

            // Store count of occurrences in count[]
            for (i = 0; i < n; i++)
                count[(arr[i] / exp) % 10]++;

            // Change count[i] so that count[i] now contains
            // actual
            //  position of this digit in output[]
            for (i = 1; i < 10; i++)
                count[i] += count[i - 1];

            // Build the output array
            for (i = n - 1; i >= 0; i--)
            {
                output[count[(arr[i] / exp) % 10] - 1] = arr[i];
                count[(arr[i] / exp) % 10]--;
            }

            // Copy the output array to arr[], so that arr[] now
            // contains sorted numbers according to current
            // digit
            for (i = 0; i < n; i++)
                arr[i] = output[i];
        }

        // The main function to that sorts arr[] of size n using
        // Radix Sort
        private static void DoRadixSort(int[] arr, int n)
        {
            // Find the maximum number to know number of digits
            var m = GetMax(arr, n);

            // Do counting sort for every digit. Note that
            // instead of passing digit number, exp is passed.
            // exp is 10^i where i is current digit number
            for (var exp = 1; m / exp > 0; exp *= 10)
                DoCountSort(arr, n, exp);
        }

        public string Name => nameof(RadixSort);

        public void Run(int[] target)
        {
            DoRadixSort(target, target.Length);
        }

        public ulong CalculateRequiredMemory(uint maxSize, int maxValue)
        {
            return maxSize * sizeof(int) + 10 * sizeof(int);
        }
    }
}
