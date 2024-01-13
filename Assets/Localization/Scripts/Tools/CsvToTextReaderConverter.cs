using System;
using System.IO;

public class CsvToTextReaderConverter
{
    public static TextReader ConvertCsvToTextReader(string csvFilePath)
    {
        try
        {
            // Open the CSV file using a StreamReader
            using (StreamReader sr = new StreamReader(csvFilePath))
            {
                // Read the contents of the CSV file into a string
                string csvContent = sr.ReadToEnd();

                // Create a StringReader from the CSV content
                StringReader stringReader = new StringReader(csvContent);

                // You can now use stringReader as a TextReader to read from the CSV data
                return stringReader;
            }
        }
        catch (Exception e)
        {
            // Handle any exceptions that may occur while reading the CSV file
            Console.WriteLine("Error reading CSV file: " + e.Message);
            return null;
        }
    }
}