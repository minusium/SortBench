namespace SortBench.Core.Algorithms
{
    public class HeapSort : ISortAlgorithm
    {
        public string Name => nameof(HeapSort);

        private static void DoHeapSort(int[] arr)
        {
            var n = arr.Length;

            // Build heap (rearrange array)
            for (var i = n / 2 - 1; i >= 0; i--)
                Heapify(arr, n, i);

            // One by one extract an element from heap
            for (var i = n - 1; i > 0; i--)
            {
                // Move current root to end
                (arr[0], arr[i]) = (arr[i], arr[0]);

                // call max heapify on the reduced heap
                Heapify(arr, i, 0);
            }
        }

        // To heapify a subtree rooted with node i which is
        // an index in arr[]. n is size of heap
        private static void Heapify(int[] arr, int n, int i)
        {
            // Recursion is converted to a loop
            while (true)
            {
                var largest = i; // Initialize largest as root
                var l = 2 * i + 1; // left = 2*i + 1
                var r = 2 * i + 2; // right = 2*i + 2

                // If left child is larger than root
                if (l < n && arr[l] > arr[largest]) largest = l;

                // If right child is larger than largest so far
                if (r < n && arr[r] > arr[largest]) largest = r;

                // If largest is not root
                if (largest == i) return;

                (arr[i], arr[largest]) = (arr[largest], arr[i]);

                // Recursively heapify the affected sub-tree
                i = largest;
            }
        }

        public void Run(int[] target)
        {
            DoHeapSort(target);
        }
    }
}
