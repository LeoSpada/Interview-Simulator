using TMPro;
using UnityEngine;

// Gestisce la schermata di Backup
public class BackupPanel : MonoBehaviour
{
    public TextMeshProUGUI backupPath;

    public void Start()
    {
        backupPath.text += "\n" + BackupManager.GetBackUpPath();
    }
    public void Save(bool overwrite)
    {
        if (overwrite) BackupManager.DeleteBackupDirectory();
            BackupManager.BackUpAll();
    }

    public void Load()
    {
        BackupManager.BackUpAll(true);
    }


    public void LoadCVScene()
    {
        GameManager.instance.LoadScene("Scena_Titolo");
    }
}
