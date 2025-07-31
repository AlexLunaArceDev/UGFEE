using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class LocalAnalyticsService : IAnalyticsService
{
    private const string _analyticsFileName = "localAnalytics.jsonl";
    private string AnalyticsFilePath => Path.Combine(Application.persistentDataPath, _analyticsFileName);

    public void CleanupService()
    {
        //Nothing to Cleanup for now
    }

    private void SaveAnalyticsToFile(Dictionary<string, object> data)
    {
        string jsonLine = JsonConvert.SerializeObject(data);

        //Adding the data to the file in a new line
        try
        {
            File.AppendAllText(AnalyticsFilePath, jsonLine + Environment.NewLine);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error writing analytics to file: {ex.Message}");
        }
    }

    public void Flush()
    {
        Debug.Log("------ FLUSHING ANALYTICS FILE ------");

        try
        {
            if (File.Exists(AnalyticsFilePath))
            {
                string[] lines = File.ReadAllLines(AnalyticsFilePath);
                foreach (string line in lines)
                {
                    Debug.Log(line);
                }
            }
            else
            {
                Debug.Log("No analytics file found");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error reading analytics file: {ex.Message}");
        }

        Debug.Log("-------------------------------------");
    }

    public void Register(Dictionary<string, object> parameters = null)
    {
        var newRegister = new Dictionary<string, object>
        {
            { "type", "analyticsRegister" },
            { "timestamp", DateTime.UtcNow.ToString("MM-dd-yyyy HH:mm:ss") },
            { "userCohort", Bootstrapper.AssignedCohort.ToString() }, 
            { "parameters", parameters }
        };

        SaveAnalyticsToFile(newRegister);
    }

    public void Screen(UIScreenBase screen, Dictionary<string, object> parameters = null)
    {
        if (parameters == null)
            parameters = new Dictionary<string, object>();

        parameters["ScreenDisplayed"] = screen.ScreenName;
        Register(parameters);
    }

    public void Track(string eventName, Dictionary<string, object> parameters = null)
    {
        if (parameters == null)
            parameters = new Dictionary<string, object>();

        parameters["Event"] = eventName;
        Register(parameters);
    }
}
