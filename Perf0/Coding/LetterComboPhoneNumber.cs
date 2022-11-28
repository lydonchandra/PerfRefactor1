using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace PerfRefactor1.Coding;

public class LetterComboPhoneNumber
{
    private static readonly Dictionary<char, char[]> PhoneDic = new()
    {
        { '2', new [] { 'a', 'b', 'c' } },
        { '3', new [] { 'd', 'e', 'f' } },
        { '4', new [] { 'g', 'h', 'i' } },
        { '5', new [] { 'j', 'k', 'l' } },
        { '6', new [] { 'm', 'n', 'o' } },
        { '7', new [] { 'p', 'q', 'r', 's' } },
        { '8', new [] { 't', 'u', 'v' } },
        { '9', new [] { 'w', 'x', 'y', 'z' } }
    };
    
    public static IList<string> LetterCombinations(string digits)
    {
        char[] validDigits = PhoneDic.Keys.ToArray();
        
        ISet<char> letterSet = new HashSet<char>();
        List<string> results = new();
        if (string.IsNullOrWhiteSpace(digits)) return results;
                                                                           
        if (digits.ToCharArray().Any(digit => !validDigits.Contains(digit)))
        {
            throw new ArgumentException($"Invalid digits {digits}");
        }

        foreach (char digit in digits)
        {
            foreach (char s in PhoneDic[digit])
            {
                letterSet.Add(s);
            }
        }

        return GenerateLetters(letterSet);

    }

    private static IList<string> GenerateLetters(ISet<char> letterSet)
    {
        List<string> generatedPermutations = new (); 
        string currentPermutation = string.Empty;
        string elementsToPermute = string.Join(string.Empty, letterSet.ToArray());
        Permute(generatedPermutations, currentPermutation, elementsToPermute);
        return generatedPermutations;
    }

    public static void Permute(
        List<string> generatedPermutations, string currentPermutation, string elementsToPermute )
    {
        if (string.IsNullOrEmpty(elementsToPermute))
        {
            if (!string.IsNullOrWhiteSpace(currentPermutation))
            {
                generatedPermutations.Add(currentPermutation);
            }

            return;
        }
        
        for (int idx = 0; idx < elementsToPermute.Length; idx++)
        {
            char element = elementsToPermute[idx];
            string nextPermutation = currentPermutation + element;
            string remainingElements = elementsToPermute.Remove(idx, 1);
            Permute(generatedPermutations, nextPermutation, remainingElements);
        }
    }

    public static void Main(string[] args)
    {
        IList<string> results = LetterCombinations("234");
        Console.WriteLine(results);
    }
}


// Permutation(
// GeneratedPermutations
// CurrentPermutation
// ElementsToPermute)

// if ElementsToPermute not empty then
//     for element in ElementsToPermute do
//         nextPermutation = CurrentPermutation + element 
//         RemainingElements = RemainingElements.Remove(element)
//         Permutation(GeneratedPermutations, nextPermutation, RemainingElements)
//     end
// else
//     add CurrentPermutation to GeneratedPermutations
// end
//
// #1 Permutation( [], [], [abc])
//     for element = a
//         nextPermutation = [a]
//         RemainingElements = [bc]
//         Permutation([], [a], [bc])
//         // currentPermutation = [a]
//         #1a  for element = b
//             nextPermutation = [ab]
//             RemainingElement = [c]
//             Permutation([], [ab], [c])
//             // currentPermutation = [ab]
//             #1a1 for element = c
//                 nextPermutation = [abc]
//                 RemainingElement = []
//                 Permutation([], [abc], [])
//                     GeneratedPermutation = [abc]    ***
//                 return
//         #1b for element = c
//             nextPermutation = [ac]
//             RemainingElement = [b]
//             Permutation(GeneratedPermutations, [ac], [b])
//             // currentPermutation = [ac]
//             #1b1 for element = b
//                 nextPermutation = [acb]
//                 RemainingElement = []
//                 Permutation(GenerationPermutations, [acb], [])
//                     //currentPermutation = [acb]
//                 GenerationPermutation += [acb]
            