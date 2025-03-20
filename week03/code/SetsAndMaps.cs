using System.Text.Json;
using System.Xml.Schema;

public static class SetsAndMaps
{
    /// <summary>
    /// The words parameter contains a list of two character 
    /// words (lower case, no duplicates). Using sets, find an O(n) 
    /// solution for returning all symmetric pairs of words.  
    ///
    /// For example, if words was: [am, at, ma, if, fi], we would return :
    ///
    /// ["am & ma", "if & fi"]
    ///
    /// The order of the array does not matter, nor does the order of the specific words in each string in the array.
    /// at would not be returned because ta is not in the list of words.
    ///
    /// As a special case, if the letters are the same (example: 'aa') then
    /// it would not match anything else (remember the assumption above
    /// that there were no duplicates) and therefore should not be returned.
    /// </summary>
    /// <param name="words">An array of 2-character words (lowercase, no duplicates)</param>
    public static string[] FindPairs(string[] words)
    {
        // TODO Problem 1 - ADD YOUR CODE HERE

        // Ensure no duplicates
        var cleanWords = new HashSet<string>(words);

        // Create set
        var uniquePairs = new HashSet<string>();

        // Loop through the input array, add element to set if it nor its reverse isn't 
        // stored yet or it isn't a double char string.
        // Else remove reversed sibling and add to set "{reversed} & {element}" construction.
        foreach (string word in cleanWords.ToArray()) {
            var reverse = word.ToCharArray();
            Array.Reverse(reverse);
            string reversed = new string(reverse);

            if (!uniquePairs.Contains(word) && !uniquePairs.Contains(reversed) && reverse[0] != reverse[1]) {
                uniquePairs.Add(word);
            } else if (reverse[0] != reverse[1]) {
                uniquePairs.Remove(reversed);
                uniquePairs.Add($"{reversed} & {word}");
            }
        }

        // Return the array filtering potential unhandled strings
        return Array.FindAll(uniquePairs.ToArray(), (pair) => pair.Length > 2);
    }

    /// <summary>
    /// Read a census file and summarize the degrees (education)
    /// earned by those contained in the file.  The summary
    /// should be stored in a dictionary where the key is the
    /// degree earned and the value is the number of people that 
    /// have earned that degree.  The degree information is in
    /// the 4th column of the file.  There is no header row in the
    /// file.
    /// </summary>
    /// <param name="filename">The name of the file to read</param>
    /// <returns>fixed array of divisors</returns>
    public static Dictionary<string, int> SummarizeDegrees(string filename)
    {
        var degrees = new Dictionary<string, int>();
        foreach (var line in File.ReadLines(filename))
        {
            var fields = line.Split(",");
            // TODO Problem 2 - ADD YOUR CODE HERE

            // Get degree field to use as key and compare
            string degree = fields[3];

            // Add key to dictionary if it wasn't there, otherwise, increment.
            if (!degrees.ContainsKey(degree)) {
                degrees[degree] = 1;
            } else {
                degrees[degree] += 1;
            }
        }

        return degrees;
    }

    /// <summary>
    /// Determine if 'word1' and 'word2' are anagrams.  An anagram
    /// is when the same letters in a word are re-organized into a 
    /// new word.  A dictionary is used to solve the problem.
    /// 
    /// Examples:
    /// is_anagram("CAT","ACT") would return true
    /// is_anagram("DOG","GOOD") would return false because GOOD has 2 O's
    /// 
    /// Important Note: When determining if two words are anagrams, you
    /// should ignore any spaces.  You should also ignore cases.  For 
    /// example, 'Ab' and 'Ba' should be considered anagrams
    /// 
    /// Reminder: You can access a letter by index in a string by 
    /// using the [] notation.
    /// </summary>
    public static bool IsAnagram(string word1, string word2)
    {
        // TODO Problem 3 - ADD YOUR CODE HERE

        // Turn to lower case and avoid spaces
        string lower1 = string.Join(string.Empty, word1.ToLower().Split(' '));
        string lower2 = string.Join(string.Empty, word2.ToLower().Split(' '));

        // Discard cases with different lengths
        if (lower1.Length != lower2.Length) {
            return false;
        }

        // Create two dictionaries to store letters and occurrences
        var dict1 = new Dictionary<char, int>();
        var dict2 = new Dictionary<char, int>();

        // Store letters and their occurrences
        for (int i = 0; i < lower1.Length; i++) {
            char letter1 = lower1[i];
            char letter2 = lower2[i];

            if (!dict1.ContainsKey(letter1)) {
                dict1[letter1] = 1;
            } else {
                dict1[letter1] += 1;
            }
        

            if (!dict2.ContainsKey(letter2)) {
                dict2[letter2] = 1;
            } else {
                dict2[letter2] += 1;
            }
            
        }

        // Union dictionary keys to confirm letters equality
        var union = dict1.Keys.ToHashSet().Union(dict2.Keys.ToHashSet());

        if (dict1.Count != union.ToList().Count || dict2.Count != union.ToList().Count){
            return false;
        }

        // Compare occurrences
        foreach (char letter in dict1.Keys.ToArray()) {
            if (dict1[letter] != dict2[letter]) {
                return false;
            }
        }      
        
        return true;
    }

    /// <summary>
    /// This function will read JSON (Javascript Object Notation) data from the 
    /// United States Geological Service (USGS) consisting of earthquake data.
    /// The data will include all earthquakes in the current day.
    /// 
    /// JSON data is organized into a dictionary. After reading the data using
    /// the built-in HTTP client library, this function will return a list of all
    /// earthquake locations ('place' attribute) and magnitudes ('mag' attribute).
    /// Additional information about the format of the JSON data can be found 
    /// at this website:  
    /// 
    /// https://earthquake.usgs.gov/earthquakes/feed/v1.0/geojson.php
    /// 
    /// </summary>
    public static string[] EarthquakeDailySummary()
    {
        const string uri = "https://earthquake.usgs.gov/earthquakes/feed/v1.0/summary/all_day.geojson";
        using var client = new HttpClient();
        using var getRequestMessage = new HttpRequestMessage(HttpMethod.Get, uri);
        using var jsonStream = client.Send(getRequestMessage).Content.ReadAsStream();
        using var reader = new StreamReader(jsonStream);
        var json = reader.ReadToEnd();
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        var featureCollection = JsonSerializer.Deserialize<FeatureCollection>(json, options);

        // TODO Problem 5:
        // 1. Add code in FeatureCollection.cs to describe the JSON using classes and properties 
        // on those classes so that the call to Deserialize above works properly.
        // 2. Add code below to create a string out each place a earthquake has happened today and its magitude.
        // 3. Return an array of these string descriptions.

        // Create a collection to store output strings
        var summary = new List<string>();

        // Loop on features array inside featurCollection pushing format array into summary
        Array.ForEach(featureCollection.features, (Feature feature) => summary.Add($"{feature.properties.place} - Mag {feature.properties.mag}"));

        // Return summary turned into array
        return summary.ToArray();
    }
}