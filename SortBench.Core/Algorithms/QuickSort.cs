namespace SortBench.Core.Algorithms
{
    public class QuickSort : ISortAlgorithm
    {
        public string Name => nameof(QuickSort);

        // This function takes last element as pivot, places
        // the pivot element at its correct position in sorted
        // array, and places all smaller (smaller than pivot)
        // to left of pivot and all greater elements to right
        // of pivot 
        private static int Partition(int[] arr, int low, int high)
        {
            // pivot
            var pivot = arr[high];

            // Index of smaller element and
            // indicates the right position
            // of pivot found so far
            var i = (low - 1);

            for (var j = low; j <= high - 1; j++)
            {

                // If current element is smaller
                // than the pivot
                if (arr[j] < pivot)
                {

                    // Increment index of
                    // smaller element
                    i++;
                    (arr[i], arr[j]) = (arr[j], arr[i]);
                }
            }
            (arr[i + 1], arr[high]) = (arr[high], arr[i + 1]);
            return i + 1;
        }

        // The main function that implements QuickSort
        //          arr[] --> Array to be sorted,
        //          low --> Starting index,
        //          high --> Ending index
        private static void DoQuickSort(int[] arr, int low, int high)
        {
            if (low >= high) return;
            // pi is partitioning index, arr[p]
            // is now at right place
            var pi = Partition(arr, low, high);

            // Separately sort elements before
            // partition and after partition
            DoQuickSort(arr, low, pi - 1);
            DoQuickSort(arr, pi + 1, high);
        }

        public void Run(int[] target)
        {
            DoQuickSort(target, 0, target.Length - 1);
        }
    }
}
