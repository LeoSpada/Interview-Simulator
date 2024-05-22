using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class TextDataHandler : MonoBehaviour
{
    public TextData textData;
    private string savePath;

    private void Awake()
    {
        savePath = Application.persistentDataPath + "/textData.json";
    }

    public void SaveText()
    {
        string jsonData = JsonUtility.ToJson(textData);
        File.WriteAllText(savePath, jsonData);
    }

    public void LoadText()
    {
        if (File.Exists(savePath))
        {
            string jsonData = File.ReadAllText(savePath);
            textData = JsonUtility.FromJson<TextData>(jsonData);
        }
    }
}