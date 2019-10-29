using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;


// Validates names of social values to avoid duplicates
public class SocialValueValidation {

    private const int ERROR_THRESHOLD = 2;
    private static Regex lettersOnly = new Regex("[^a-zA-Z]");


    public static string ValidateName(string name, List<string> values)
    {
        // First, remove all non-letters from the string
        name = lettersOnly.Replace(name, "");

        // Convert string to lowercase for consistency
        name = name.ToLower();

        // Remove "social" from the string to avoid some keys having it and others not.
        name = name.Replace("social", "");

        // If the user added any other characters for whatever reason, but it otherwise matches, return.
        foreach (string s in values)
        {
            if (name.Contains(s))
                return s;
        }

        // Compute the Levenshtein Distance of the given name relative to all values in the list.
        // This can correct minor typos but is not a full blown spell check and should not be used as such.
        foreach (string s in values)
        {
            if (LevenshteinDistance(name, s) < ERROR_THRESHOLD)
                return s;
        }

        return name;
    }


    /// <summary>
    /// Calculates the Levenshtein Distance between two strings, s and t.
    /// </summary>
    /// <returns>Levenshtein Distance</returns>
    public static int LevenshteinDistance(string s, string t)
    {
        // Get the lengths
        int m = s.Length;
        int n = t.Length;

        // If m is 0, return n. If n is 0, return m.
        if (m == 0)
            return n;
        if (n == 0)
            return m;

        // Create a matrix with m+1 rows and n+1 columns.
        int[,] matrix = new int[m+1, n+1];

        // Initialize first row to 0...m
        for (int i = 0; i <= m; matrix[i, 0] = i++){ }
        
        // Initialize first row to 0...n
        for (int j = 0; j <= n; matrix[0, j] = j++){ }

        // Loop through all rows starting at 1
        for (int i = 1; i <= m; i++)
        {
            // Loop through all collumns starting at 1
            for (int j = 1; j <= n; j++)
            {
                // If the characters in the strings at each point are equal, cost is 0. Otherwise, it's 1.
                int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

                // Set the current cell equal to the minimum of the cells above, to the left, and above/to the left + cost
                matrix[i, j] = Mathf.Min(matrix[i-1, j] + 1, matrix[i, j-1] + 1, matrix[i-1, j-1] + cost);
            }
        }

        return matrix[m, n];
    }

}
