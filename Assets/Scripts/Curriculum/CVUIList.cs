using UnityEngine;

public class CVUIList : MonoBehaviour
{
    public GameObject entryPrefab;
    public Transform contentRoot;

    public void GetCV()
    {
        var entries = CVManager.GetAllCV();

        foreach (var e in entries)
        {
            Debug.Log("e = " + e.name + " " + e.surname + " " + e.job);
            //var newObj = Instantiate(entryPrefab, contentRoot);
            //newObj.GetComponent<CVUIListEntry>().Setup(e);
        }
    }
}
