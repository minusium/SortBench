namespace SortBench.Core.Algorithms
{
    internal class CountSort : ISortAlgorithm
    {
        private static void DoCountSort(int[] array, int size)
        {
            var output = new int[size + 1];

            // Find the largest element of the array
            var max = array[0];
            for (var i = 1; i < size; i++)
            {
                if (array[i] > max)
                    max = array[i];
            }
            var count = new int[max + 1];

            // Initialize count array with all zeros.
            //for (var i = 0; i < max; ++i)
            //{
            //    count[i] = 0;
            //}

            // Store the count of each element
            for (var i = 0; i < size; i++)
            {
                count[array[i]]++;
            }

            // Store the cumulative count of each array
            for (var i = 1; i <= max; i++)
            {
                count[i] += count[i - 1];
            }

            // Find the index of each element of the original array in count array, and
            // place the elements in output array
            for (var i = size - 1; i >= 0; i--)
            {
                output[count[array[i]] - 1] = array[i];
                count[array[i]]--;
            }

            // Copy the sorted elements into original array
            for (var i = 0; i < size; i++)
            {
                array[i] = output[i];
            }
        }

        public string Name => nameof(CountSort);

        public void Run(int[] target)
        {
            DoCountSort(target, target.Length);
        }

        public ulong CalculateRequiredMemory(uint maxSize, int maxValue)
        {
            return maxSize * sizeof(int) + (ulong)maxValue * sizeof(int);
        }
    }
}
