namespace Iron_Software
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Provides functionality to convert old mobile keypad inputs (multi-tap) into text.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Handles old-style phone keypad input conversion.
        /// </summary>
        class OldPhoneConverter
        {

            /// <summary>
            /// Maps numeric keypad sequences to their corresponding characters.
            /// </summary>
            /// <param name="sequence">The numeric sequence (e.g., "222" for 'C').</param>
            /// <param name="result">The current result string.</param>
            /// <returns>The updated result string with the mapped character appended if valid.</returns>
            public static string MapSequenceToChar(string sequence, string result)
            {

                // Dictionary mapping numeric key sequences to characters.
                var charMap = new Dictionary<string, string>()
                {
                    {"1", "&"}, {"11", "'"}, {"111", "("}, {"1111", ")"},
                    {"2", "A"}, {"22", "B"}, {"222", "C"},
                    {"3", "D"}, {"33", "E"}, {"333", "F"},
                    {"4", "G"}, {"44", "H"}, {"444", "I"},
                    {"5", "J"}, {"55", "K"}, {"555", "L"},
                    {"6", "M"}, {"66", "N"}, {"666", "O"},
                    {"7", "P"}, {"77", "Q"}, {"777", "R"}, {"7777", "S"},
                    {"8", "T"}, {"88", "U"}, {"888", "V"},
                    {"9", "W"}, {"99", "X"}, {"999", "Y"}, {"9999", "Z"},
                    {"0", " "}
                };

                // If the sequence exists in the dictionary, append the corresponding character.
                if (charMap.ContainsKey(sequence))
                {
                    result += charMap[sequence];
                }

                return result;
            }

            /// <summary>
            /// Converts an input string representing old phone keypad presses into text.
            /// </summary>
            /// <param name="input">The user input (e.g., "44 33 555 555 666#").</param>
            /// <returns>The converted text result.</returns>
            public static string OldPhonePad(string input)
            {
                string result = string.Empty;
                string sequence = string.Empty;

                for (int i = 0; i < input.Length; i++)
                {
                    char current = input[i];
                    char next = (i < input.Length - 1) ? input[i + 1] : '\0';

                    // '#' indicates the end of input. Finalize the current sequence and stop.
                    if (current == '#')
                    {
                        if (!string.IsNullOrEmpty(sequence))
                            result = MapSequenceToChar(sequence, result);
                        break;
                    }

                    // Space separates sequences.
                    else if (current == ' ')
                    {
                        if (!string.IsNullOrEmpty(sequence))
                        {
                            result = MapSequenceToChar(sequence, result);
                            sequence = string.Empty;
                        }
                        continue;
                    }

                    // '*' acts as backspace — removes the last character in the result.
                    else if (current == '*')
                    {
                        if (result.Length > 0)
                            result = result.Substring(0, result.Length - 1);
                        continue;
                    }

                    // Handle numeric characters (build up sequences).
                    else if (char.IsDigit(current))
                    {
                        sequence += current;

                        // If the next char is different, finalize the current sequence.
                        if (current != next)
                        {
                            result = MapSequenceToChar(sequence, result);
                            sequence = string.Empty;
                        }
                    }
                }

                return result;
            }
        }

        /// <summary>
        /// A simple test method that compares actual and expected outputs and prints pass/fail results.
        /// </summary>
        /// <param name="input">The keypad input string.</param>
        /// <param name="expected">The expected output after conversion.</param>
        public static void AssertTest(string input, string expected)
        {
            string actual = OldPhoneConverter.OldPhonePad(input);
            if (actual == expected)
            {
                Console.WriteLine($"✅ PASS | Input: {input} → Output: {actual}");
            }
            else
            {
                Console.WriteLine($"❌ FAIL | Input: {input} → Expected: {expected}, Got: {actual}");
            }
        }

        /// <summary>
        /// Main method: runs automated test cases for OldPhonePad conversion.
        /// </summary>
        static void Main(string[] args)
        {
            Console.WriteLine("Running OldPhonePad tests...");
            Console.WriteLine("------------------------------");

            // ✅ Automated test cases
            AssertTest("222 2 22#", "CAB");
            AssertTest("44 33 555 555 666#", "HELLO");
            AssertTest("8 666 666 0 3 33#", "TOO DE");
            AssertTest("444 33 33 9 9 0 111#", "IEEWW (");
            AssertTest("2 22 222#", "ABC");
            AssertTest("9 99 999 9999#", "WXYZ");
            AssertTest("33 0 666#", "E O");
            AssertTest("222 2*22#", "CB");

            Console.WriteLine("------------------------------");
            Console.WriteLine("Custom test:");
            Console.WriteLine("Input: 8 88777444666*664#");
            Console.WriteLine("Output: " + OldPhoneConverter.OldPhonePad("8 88777444666*664#"));

            // Interactive testing 
            // Console.WriteLine("Please enter input:");
            // string? input = Console.ReadLine(); 
            // string result = OldPhoneConverter.OldPhonePad(input); 
            //  Console.WriteLine(result);
        }
    }
}
