using System.IO;
using UnityEngine;

// Gestisce il salvataggio e il caricamento dei Backup
public static class BackupManager
{
    // Restituisce il percorso della cartella Backup
    public static string GetBackUpPath()
    {
        return Path.Combine(Directory.GetCurrentDirectory(), "Backup");
    }

    // Fa il backup o il ripristino della cartella scelta
    public static void BackUpFolder(string folder, string backupFolder, bool restore = false)
    {
        string backUpPath = Path.Combine(GetBackUpPath(), backupFolder);

        string source, destination;

        // Se restore è vero, l'operazione è di caricamento (ripristino) e non salvataggio
        if (!restore)
        {
            source = folder;
            destination = backUpPath;
        }
        else
        {
            source = backUpPath;
            destination = folder;
        }

        CopyDirectory(source, destination, true);

        Debug.Log("Backup terminato");
    }

    // Fa il backup dell'intera cartella (persistentDataPath)
    public static void BackUpAll(bool restore = false)
    {

        Debug.Log("Avvio backup di tutto");

        string persistent = Path.Combine(UnityEngine.Device.Application.persistentDataPath);
        string backUpPath = GetBackUpPath();

        string source, destination;

        // Se restore è vero, l'operazione è di caricamento (ripristino) e non salvataggio
        if (!restore)
        {
            source = persistent;
            destination = backUpPath;
        }
        else
        {
            source = backUpPath;
            destination = persistent;
        }             

        CopyDirectory(source, destination, true);

        Debug.Log("Backup terminato");
    }

    // Elimina la cartella di Backup
    public static void DeleteBackupDirectory()
    {
        Directory.Delete(GetBackUpPath(), true);
        Directory.CreateDirectory(GetBackUpPath());
    }

    // Copia una cartella (ed eventualmente le sottocartelle) da una sorgente a una destinazione
    public static void CopyDirectory(string sourceDir, string destinationDir, bool recursive)
    {

        // Prende informazioni della cartella sorgente
        var dir = new DirectoryInfo(sourceDir);

        // Controlla se source esiste
        if (!dir.Exists)
            throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

        // Prende informazioni sottocartelle di source
        DirectoryInfo[] dirs = dir.GetDirectories();

        // Crea la cartella di destinazione
        Directory.CreateDirectory(destinationDir);

        // Per ogni file, copia da sorgente a destinazione
        foreach (FileInfo file in dir.GetFiles())
        {

            string targetFilePath = Path.Combine(destinationDir, file.Name);
            try
            {
                file.CopyTo(targetFilePath, true);
            }
            catch (IOException)
            {
                // Debug.Log($"{file.Name} non sovrascritto.");
            }
        }

        // Se ricorsivo, fa l'operazione di copia anche per le sottocartelle
        if (recursive)
        {
            foreach (DirectoryInfo subDir in dirs)
            {
                string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                CopyDirectory(subDir.FullName, newDestinationDir, true);
            }
        }
    }
}