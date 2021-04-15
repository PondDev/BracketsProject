using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

public class SampleDatabase
{
    #region Singleton
    private static readonly SampleDatabase instance = new SampleDatabase();

    private SampleDatabase() {}

    static SampleDatabase()
    {
        Instance.LoadSampleDatabase();
    }
    
    public static SampleDatabase Instance
    {
        get {return instance;}
    }
    #endregion

    private Dictionary<int, Sample> database = new Dictionary<int, Sample>();
    public static string sampleDataPath = Application.dataPath + "/StreamingAssets/SampleData.json";

    void LoadSampleDatabase()
    {
        string text = File.ReadAllText(sampleDataPath);
        List<Sample> samples = JsonConvert.DeserializeObject<List<Sample>>(text);

        foreach (Sample sample in samples)
        {
            sample.Init();
            database.Add(sample.id, sample);
        }
    }

    public Sample GetSampleByID(int id)
    {
        return database[id];
    }
}
