using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackupPanel : MonoBehaviour
{

    public void Save()
    {
        BackupManager.BackUpAll();
        Debug.Log("Click salvatggio");
    }

    public void Load()
    {
        BackupManager.BackUpAll(true);
        Debug.Log("Click caricamento");
    }
}
