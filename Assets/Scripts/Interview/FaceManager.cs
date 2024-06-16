using UnityEngine;

public class FaceManager : MonoBehaviour
{
    public Material Materiale_Bocca;
    public Material Materiale_Occhi;
    public Texture[] Texture_Bocca;
    public Texture[] Texture_Occhi;
    
   public void ChangeExpression(bool positive)
    {
        if (positive)
        {
            Materiale_Bocca.mainTexture = Texture_Bocca[Random.Range(1, 2)];
            Materiale_Occhi.mainTexture = Texture_Occhi[Random.Range(2, 3)];
        }
        else
        {
            Materiale_Bocca.mainTexture = Texture_Bocca[Random.Range(0, 1)];
            Materiale_Occhi.mainTexture = Texture_Occhi[Random.Range(0, 1)];
        }
    }
}