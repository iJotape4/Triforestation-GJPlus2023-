#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Collections.Generic;
using System;
using System.Text.RegularExpressions;
using UnityEditor.Localization;
using UnityEditor.Localization.Plugins.CSV;


/// <summary>
/// This class imports the localization tables from a Google Sheet into the project.
/// It uses the Google Drive API to download the CSV files from the Google Sheet.
/// It uses the Unity Localization package to create the localization tables.
/// It inherits from Monobehaviour but it must be used in Edit Mode.
/// </summary>
public class GoogleSheetsReader : MonoBehaviour
{
    private const string fileId = "1YSfXEhDW_dEqDznnkFRApH2AYfmP7AEMODrGabkFIqI";
    private const string apiKey = "AIzaSyDLGaq2kRUa1H9AqajxQknNvTVRg3MEFQU"; // Replace with your actual API key
    private const string exportUrl = "https://www.googleapis.com/drive/v3/files/{0}/export?mimeType=text/csv&key={1}";
    private string filePath = Path.Combine("Assets", "Localization", "LocalizationTables");
    private List<string> ranges = new List<string>() { "Dialogues!A1:F100", "FTUE!A1:F100", "UI!A1:F100", "Challenges!A1:F100", "Fauna!A1:F100", "Flora!A1:F100", "Biomas!A1:F100" };

    private static Dictionary<string, Dictionary<string, string>> localizationTable;

#if UNITY_EDITOR
    [MenuItem("DevTools/Read Localization Google Sheet",false, 100)]
    private static void ReadGoogleSheetInEditMode()
    {
        GoogleSheetsReader reader = FindObjectOfType<GoogleSheetsReader>();
        if (reader == null)
        {
            reader = new GameObject("reader").AddComponent<GoogleSheetsReader>();
        }
        reader.StartCoroutine(reader.ReadData());

    }
#endif

#if UNITY_EDITOR
    /// <summary>
    /// Destroy the object if Application is playing
    /// </summary>
    private void Start()
    { 
        if(Application.isPlaying)
        DestroyImmediate(this);
    }
#endif

    /// <summary>
    /// Reads data from G-Drive File
    /// </summary>
    /// <returns></returns>
    IEnumerator ReadData()
    {
        foreach (var range in ranges)
        {
            string url = $"https://sheets.googleapis.com/v4/spreadsheets/{fileId}/values/{range}?key={apiKey}";

            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError("Error: " + webRequest.error + "in sheet " + range);
                }
                else
                {
                    string data = webRequest.downloadHandler.text;

                    // Parse JSON response
                    JToken json = JToken.Parse(data);
                    if (json["values"] != null)
                    {
                        // Extract values from JSON
                        string[][] values = json["values"].Select(t => t.Select(v => v.ToString()).ToArray()).ToArray();

                        // Convert to CSV format
                        string csvContent = string.Join("\n", values.Select(row => string.Join(",", row)));
                        string rangeName = RemoveRangeFromString(range);
                        string desiredPath = Path.Combine(filePath, rangeName);

                        // Write CSV data to file
                        if (!Directory.Exists(desiredPath))
                        {
                            Directory.CreateDirectory(desiredPath);
                            AssetDatabase.Refresh();
                        }

                        File.WriteAllText(Path.Combine(desiredPath, (rangeName + ".csv")), csvContent);
                        Debug.Log("CSV data exported successfully.");

                        CheckLocalitazionTableExists(rangeName, Path.Combine(desiredPath));
                        Debug.Log("Localization table created sucessfully");
                    }
                    else
                    {
                        Debug.LogError("Failed to parse JSON response.");
                    }
                }
            }
            AssetDatabase.Refresh();
        }
    }

    static string RemoveRangeFromString(string input)
    {
        // Define the regular expression pattern
        string pattern = @"!\w+:\w+";
        // Use Regex.Replace to remove the matched pattern
        string result = Regex.Replace(input, pattern, "");
        return result;
    }

    void CheckLocalitazionTableExists(string tableName, string path)
    {
        // Create a new localization table collection
        if(LocalizationEditorSettings.GetStringTableCollection(tableName))     
            UpdateTable(tableName, path);
        else
            CreateTable(tableName, path);             
    }

    private void CreateTable(string tableName, string path)
    {
        LocalizationEditorSettings.CreateStringTableCollection(tableName, path);
        UpdateTable(tableName, path);
    }

    private void UpdateTable(string tableName, string path)
    {
        var tableCollection = LocalizationEditorSettings.GetStringTableCollection(tableName);
        if(tableCollection.Extensions.Count > 0)
        {
            tableCollection.RemoveExtension(tableCollection.Extensions[0]);
        }

        CsvExtension csvExtension = new CsvExtension();
        csvExtension.File = Path.Combine(path,(tableName+".csv"));
        tableCollection.AddExtension(csvExtension);

        try { Csv.ImportInto(CsvToTextReaderConverter.ConvertCsvToTextReader(csvExtension.File), tableCollection); }
        catch(Exception e) { Debug.LogError("Maybe do you have a null value in your cells. check the table "+tableName + "\n"+e.Message); }
    }
}