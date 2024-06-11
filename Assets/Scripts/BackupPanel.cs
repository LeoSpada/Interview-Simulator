using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BackupPanel : MonoBehaviour
{
    //public GameObject confirmPanel;
    public TextMeshProUGUI backupPath;

    public void Start()
    {
        backupPath.text += "\n" + BackupManager.GetBackUpPath();
    }
    public void Save()
    {
        BackupManager.BackUpAll();
        // Debug.Log("Click salvatggio");
    }

    public void Load()
    {
        BackupManager.BackUpAll(true);
        //  Debug.Log("Click caricamento");
    }

    public void LoadCVScene()
    {
        GameManager.instance.LoadScene("CV_Question_Creator");
    }
}
