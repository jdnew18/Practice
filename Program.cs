using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArraySearchMethods
{
    class Program
    {
        /// <summary>
        /// Present a series of methods which find a single duplicate pair in an array of ints.
        /// Calculate how long each different method takes on average and display the result.
        /// Assume the array is unsorted and contains either 0 or 1 duplicate pairs.
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            // Number of times to run each duplicate search method.
            int numIterations = 10;

            InvokeMethod(new Func<int[], int?>(FindDuplicateInArrayBruteForce), numIterations, "brute force");

            InvokeMethod(new Func<int[], int?>(FindDuplicateInArrayBruteForceSlightlyOptimized), numIterations, "slightly optimized brute force");

            InvokeMethod(new Func<int[], int?>(FindDuplicateInArrayBruteForceSlightlyOptimizedBackwards), numIterations, "slightly optimized brute force backwards");

            // We could incorporate sorting, but since the fastest possible sort is O(n log n), which we
            // have already accomplished with FindDuplicateInArrayBruteForceSlightlyOptimized and
            // FindDuplicateInArrayBruteForceSlightlyOptimizedBackwards, there's not really a point.

            InvokeMethod(new Func<int[], int?>(FindDuplicateInArrayBruteForceSlightlyOptimizedBackwardsParallelized), numIterations, "slightly optimized brute force backwards parallelized");

            Console.ReadLine();
        }

        /// <summary>
        /// Returns an array of random integers with 1 duplicate pair.
        /// The duplicate value is almost always 6, but can be 7.
        /// </summary>
        /// <returns></returns>
        public static int[] GenerateArrayWithSingleDuplicatePair()
        {
            int minRange = 0;
            int maxRange = 9999;
            int total = 10000;

            // Generate a list of integers ranging from 0 to 9999.
            List<int> numbers = Enumerable.Range(minRange, maxRange).ToList();

            Random rng = new Random();

            // Randomly shuffle the values in the list.
            int n = numbers.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                int value = numbers[k];
                numbers[k] = numbers[n];
                numbers[n] = value;
            }

            // Convert the list to an array of integers.
            int[] array = numbers.Take(total).ToArray();

            // Set a random index's value in the array to 6.
            // If that index's value is already 6, set the
            // index's value to 7.
            int randomIndex = rng.Next(maxRange);
            if (array[randomIndex] != 6)
            {
                array[randomIndex] = 6;
            }
            else
            {
                array[randomIndex] = 7;
            }

            return array;
        }

        /// <summary>
        /// Returns the duplicate value in the array using the
        /// most obvious brute force method - that is, take the 1st
        /// value in the array and compare it to all other values.
        /// If no match is found, take the 2nd value in the array and compare
        /// it to the all other values. Continue until duplicate is found.
        /// </summary>
        /// <param name="array">Array of random integers with 0 or 1 duplicate pairs.</param>
        /// <returns>Value of duplicate pair or null.</returns>
        private static int? FindDuplicateInArrayBruteForce(int[] array)
        {
            int? duplicateValue = null;

            // Cycle through each starting index.            
            for (int i = 0; i < array.Length; i++)
            {
                // If the starting index is equal to the searching index,
                // then their values are the duplicate values.
                for (int j = 0; j < array.Length && j != i; j++)
                {
                    if (array[i] == array[j])
                    {
                        duplicateValue = array[i];
                        break;
                    }
                }

                // Break if duplicate values found.
                if (duplicateValue.HasValue)
                {
                    break;
                }
            }

            return duplicateValue;
        }

        /// <summary>
        /// Returns the duplicate value in the array using a
        /// slightly optimized brute force method - that is, take the 1st
        /// value in the array and compare it to the 2nd through nth (last) values.
        /// If no match is found, take the 2nd value in the array and compare
        /// it to the 3rd through nth values. Continue until duplicate is found.
        /// </summary>
        /// <param name="array">Array of random integers with 0 or 1 duplicate pairs.</param>
        /// <returns>Value of duplicate pair or null.</returns>
        private static int? FindDuplicateInArrayBruteForceSlightlyOptimized(int[] array)
        {
            int? duplicateValue = null;

            // Cycle through each starting index.
            for (int i = 0; i < array.Length; i++)
            {
                // Cycle through each searching index after the starting index.
                for (int j = i + 1; j < array.Length; j++)
                {
                    // If the starting index is equal to the searching index,
                    // then their values are the duplicate values.
                    if (array[i] == array[j])
                    {
                        duplicateValue = array[i];
                        break;
                    }
                }

                // Break if duplicate value found.
                if (duplicateValue.HasValue)
                {
                    break;
                }
            }

            return duplicateValue;
        }

        /// <summary>
        /// Returns the duplicate value in the array using a
        /// backwards, slightly optimized brute force method - that is,
        /// take the 2nd-to-last value in the array and compare it to the nth (last) value.
        /// If no match is found, take the 3rd-to-last value in the array and compare
        /// it to the 2nd-to-last through nth values. Continue until duplicate is found.
        /// </summary>
        /// <param name="array">Array of random integers with 0 or 1 duplicate pairs.</param>
        /// <returns>Value of duplicate pair or null.</returns>
        private static int? FindDuplicateInArrayBruteForceSlightlyOptimizedBackwards(int[] array)
        {
            int? duplicateValue = null;

            // Cycle through each starting index.
            for (int i = array.Length - 2; i >= 0; i--)
            {
                // Cycle through each searching index after the starting index.
                for (int j = i + 1; j < array.Length; j++)
                {
                    // If the starting index is equal to the searching index,
                    // then their values are the duplicate values.
                    if (array[i] == array[j])
                    {
                        duplicateValue = array[i];
                        break;
                    }
                }

                // Break if duplicate value found.
                if (duplicateValue.HasValue)
                {
                    break;
                }
            }

            return duplicateValue;
        }

        /// <summary>
        /// Returns the duplicate value in the array using a
        /// backwards, slightly optimized brute force method - that is,
        /// take the 2nd-to-last value in the array and compare it to the nth (last) value.
        /// If no match is found, take the 3rd-to-last value in the array and compare
        /// it to the 2nd-to-last through nth values. Continue until duplicate is found.
        /// 
        /// In this algorithm, the array is split into 4 equally sized pieces and then
        /// each of those pieces are separately searched in parallel using the above algorithm.
        /// If the duplicate is not found, the 4 arrays are combined into 2 arrays
        /// and the above search is performed again.
        /// If that fails, the 2 arrays are combined into one array and the above
        /// search is performed again.
        /// </summary>
        /// <param name="array">Array of random integers with 0 or 1 duplicate pairs.</param>
        /// <returns>Value of duplicate pair or null.</returns>
        private static int? FindDuplicateInArrayBruteForceSlightlyOptimizedBackwardsParallelized(int[] array)
        {
            int? duplicateValue = null;

            // Number of threads to use.
            int numLevelsOfParallelizations = 4;

            // Find duplicate value using the given number of threads.
            duplicateValue = UseNumLevelsOfParallelization(array, numLevelsOfParallelizations);

            // If the duplicate value was found, return it.
            // Otherwise, reduce the number of threads by half and
            // attempt the find the duplicate value again.
            if (duplicateValue.HasValue)
            {
                return duplicateValue;
            }
            else
            {
                numLevelsOfParallelizations = numLevelsOfParallelizations / 2;
            }

            // Find duplicate value using the given number of threads.
            duplicateValue = UseNumLevelsOfParallelization(array, numLevelsOfParallelizations);

            // If the duplicate value was found, return it.
            // Otherwise, reduce the number of threads by half and
            // attempt the find the duplicate value again.
            if (duplicateValue.HasValue)
            {
                return duplicateValue;
            }
            else
            {
                numLevelsOfParallelizations = numLevelsOfParallelizations / 2;
            }

            // Find duplicate value using the given number of threads.
            duplicateValue = UseNumLevelsOfParallelization(array, numLevelsOfParallelizations);

            return duplicateValue;
        }

        /// <summary>
        /// Use the given number of threads to find the duplicate using the method
        /// FindDuplicateInArrayBruteForceSlightlyOptimizedBackwards in parallel.
        /// </summary>
        /// <param name="array">Array of random integers with 0 or 1 duplicate pairs.</param>
        /// <param name="numLevelsOfParallelizations">Number of threads to use.</param>
        /// <returns>Value of duplicate pair or null.</returns>
        private static int? UseNumLevelsOfParallelization(int[] array, int numLevelsOfParallelizations)
        {
            int? duplicateValue = null;

            // Split up the array of random integers into numLevelsOfParallelizations
            // of equally sized arrays.
            int[][] arrays = new int[numLevelsOfParallelizations][];

            // Populate the arrays with the data from array of random integers.
            for (int i = 0; i < numLevelsOfParallelizations; i++)
            {
                arrays[i] = new int[array.Length / numLevelsOfParallelizations];
                Array.Copy(array, i * arrays[i].Length, arrays[i], 0, arrays[i].Length);
            }

            // Run the FindDuplicateInArrayBruteForceSlightlyOptimizedBackwards
            // search method in parallel using the split-up arrays.
            Parallel.For(0, arrays.Length, (i, loopState) => 
            {
                int? temp = FindDuplicateInArrayBruteForceSlightlyOptimizedBackwards(arrays[i]);

                if (temp.HasValue)
                {
                    duplicateValue = temp.Value;
                    loopState.Stop();
                }
            });

            return duplicateValue;
        }

        /// <summary>
        /// Run the search method, which finds the duplicate value in an array of random integers
        /// with 0 or 1 duplicate pairs, the given number of times.
        /// Calculate how long the search method takes on average and display the result.
        /// </summary>
        /// <param name="method">Search method.</param>
        /// <param name="numIterations">Number of times to run the search method.</param>
        /// <param name="methodName">Name of the search method.</param>
        static void InvokeMethod(Delegate method, int numIterations, string methodName)
        {
            Stopwatch stopwatch = new Stopwatch();
            int? duplicateValue = null;
            int totalTimeTaken = 0;
            int averageTimeTaken = 0;
            object[] args = new object[1];

            // Run the search method numIterations times
            // and sum up the total time taken for all iterations.
            for (int i = 0; i < numIterations; i++)
            {
                // Instantiate an array of ints with one or no pairs of duplicate values.
                args[0] = GenerateArrayWithSingleDuplicatePair();

                // Start the timer.
                stopwatch = Stopwatch.StartNew();

                // Find the duplicate value.
                duplicateValue = (int?)method.DynamicInvoke(args);

                // Stop the timer.
                stopwatch.Stop();

                // Add the time taken to the sum.
                totalTimeTaken = totalTimeTaken + (int)stopwatch.ElapsedMilliseconds;
            }

            // Calculate average time taken to find duplicate value;
            averageTimeTaken = totalTimeTaken / numIterations;

            // Print duplicate value and average time taken to find it to screen.
            string duplicateValueString = (duplicateValue.HasValue ? duplicateValue.Value.ToString() : "No duplicate found");
            Console.WriteLine("The duplicate value using " + methodName + " is " + duplicateValueString + " which took " + averageTimeTaken + " milliseconds to find.");
        }
    }
}