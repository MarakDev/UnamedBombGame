using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;

public class GameManager : MonoBehaviour
{

    public static GameManager _GMinstance;

    public bool bombExploded { get; set; } = false;

    //frame stop
    private float referenceFrame = 0;
    private bool frameStopCheck = false;
    //
    //camera shaking
    private CameraShaking cameraShaking;
    //

    public int fps = 499;

    // Propiedad pública para acceder al Singleton
    private void Awake()
    {
        // Verifica si ya existe una instancia
        if (_GMinstance != null && _GMinstance != this)
        {
            Destroy(gameObject); // Destruye el objeto duplicado
        }
        else
        {
            _GMinstance = this;
            DontDestroyOnLoad(gameObject); // Persiste el objeto entre escenas
        }
    }

    private void Start()
    {
        cameraShaking = FindObjectOfType<CameraShaking>();
    }


    public void Update()
    {
        Application.targetFrameRate = fps;
        EnhancedVisuals();
    }

    private void EnhancedVisuals()
    {
        if (referenceFrame < Time.frameCount && frameStopCheck)
        {
            Time.timeScale = 1;
            frameStopCheck = false;
        }

        //Debug.Log(bombExploded);

        if (bombExploded)
        {
            //cambiar esto a cinemachine
            if(cameraShaking != null)
                cameraShaking.enabled = true;
            bombExploded = false;
        }
    }

    //Efecto de pararse el tiempo cuando al jugador le afecta una explosion
    public void FrameStop(int totalTimeStop)
    {
        if (totalTimeStop > 50)
            totalTimeStop = 50;

        referenceFrame = Time.frameCount + totalTimeStop;
        Time.timeScale = 0;

        frameStopCheck = true;
        //Debug.Log("real frame: " + Time.frameCount + "referenceFrame" + referenceFrame + "   - totaltimestop: " + totalTimeStop);
    }

    public void ReloadScene(string scene)
    {
        Destroy(cameraShaking);
        SceneManager.LoadScene(scene);
    }

    
}
