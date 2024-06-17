using UnityEngine;

public class MovimentoTelecamera : MonoBehaviour
{
    public float centreAngle = 39.19f; // Angolo centrale della rotazione
    public float rotationAngle = 100.0f; // Angolo di rotazione in gradi
    public float rotationSpeed = 5.0f; // Velocità di rotazione
    private float currentAngle = 0.0f; // Angolo corrente della camera
    private bool rotationRight = true; // Indica se la camera sta ruotando a destra
    
    void Start()
    {
        LoadCameraState();
    }

    void Update()
    {
        /*
         * =================================================================================
         * Calcola il movimento dell'angolo
         * =================================================================================
         */
        if (rotationRight)
        {
            currentAngle += rotationSpeed * Time.deltaTime;
            if (currentAngle >= rotationAngle)
            {
                currentAngle = rotationAngle;
                rotationRight = false;
            }
        }
        else
        {
            currentAngle -= rotationSpeed * Time.deltaTime;
            if (currentAngle <= -rotationAngle)
            {
                currentAngle = -rotationAngle;
                rotationRight = true;
            }
        }

        float currentAngleOffset = centreAngle + currentAngle;

        /*
         * =================================================================================
         * Applica la rotazione alla camera
         * =================================================================================
         */
        transform.localRotation = Quaternion.Euler(0, currentAngleOffset, 0);
    }

    /*
     * =================================================================================
     * Salva la posizione della telecamera quando si cambia scena
     * =================================================================================
     */
    public void SaveCameraState()
    {
        PlayerPrefs.SetFloat("CameraPosY", transform.position.y);
        PlayerPrefs.SetFloat("CameraRotY", transform.rotation.eulerAngles.y);
        PlayerPrefs.SetFloat("CurrentAngle", currentAngle);
        PlayerPrefs.SetInt("RotationRight", rotationRight ? 1 : 0);
        PlayerPrefs.Save();
    }

    /*
     * =================================================================================
     * Carica la posizione della telecamera quando si ritorna alla scena
     * =================================================================================
     */
    public void LoadCameraState()
    {
        if (PlayerPrefs.HasKey("CameraPosY"))
        {
            float posY = PlayerPrefs.GetFloat("CameraPosY");
            transform.position = new Vector3 (0, posY, 0);

            float rotY = PlayerPrefs.GetFloat("CameraRotY");
            transform.rotation = Quaternion.Euler (0, rotY, 0);

            currentAngle = PlayerPrefs.GetFloat("CurrentAngle");
            rotationRight = PlayerPrefs.GetInt("RotatingRight") == 1;
        }
    }

    /*
     * =================================================================================
     * Salva la posizione della telecamera quando si chiude il gioco
     * =================================================================================
     */
    private void OnAppQuit()
    {
        SaveCameraState();
    }
}
