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
            CVManager.DebugCV(e);
            var newObj = Instantiate(entryPrefab, contentRoot);
            newObj.GetComponent<CVUIListEntry>().Setup(e);
        }
    }

    public void DestroyList()
    {
        for (var i = contentRoot.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(contentRoot.transform.GetChild(i).gameObject);
        }
    }
}
