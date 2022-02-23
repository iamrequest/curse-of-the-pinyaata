using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

// Source: https://support.unity.com/hc/en-us/articles/115000341143-How-do-I-read-and-write-data-from-a-text-file-
public static class SaveSystem  {
    static string filename = "save.json";
    public static void SavePlayerData(SaveData saveData) {
        string path = Application.persistentDataPath + "/" + filename;

        string json = JsonUtility.ToJson(saveData);
        StreamWriter writer = new StreamWriter(path, false);
        writer.Write(json);
        writer.Close();
    }

    public static SaveData LoadPlayerData() {
        string path = Application.persistentDataPath + "/" + filename;

        // Check if file exists
        if(! File.Exists(path)) {
            Debug.LogError("File does not exist in " + path);
            return null;
        }

        StreamReader reader = new StreamReader(path);
        string json = reader.ReadToEnd();
        reader.Close();

        SaveData saveData = JsonUtility.FromJson<SaveData>(json);
        return saveData;
    }

    public static void DeletePlayerData() {
        string path = Application.persistentDataPath + "/" + filename;
        File.Delete(path);
    }

    /*
    Aside: If I wanted to save/load this as binary instead of json, this is how it's done. No need to do anything fancy
        -- Save --
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, saveData);
        stream.Close();

        -- Load --
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Open);
        SaveData saveData = formatter.Deserialize(stream) as SaveData;
        stream.Close();
     */
}
