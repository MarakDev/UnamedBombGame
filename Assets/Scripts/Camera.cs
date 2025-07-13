using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    //Target al que seguria la camara, el jugador
    [SerializeField] public Transform target;
    //valores minimos y maximos para delimitar los bordes de la camara
    [SerializeField] public float minValueX, maxValueX;
    //valores minimos y maximos para delimitar los bordes de la camara
    [SerializeField] public float minValueY, maxValueY;

    [SerializeField] public float offsetX = 18, offsetY;


    private CameraShaking chameraShaking;
    //nivel actual del juego (pasar al GM)
    private int level = 1;
    private Vector2 currentPosition;

    private void Start()
    {
        chameraShaking = GetComponent<CameraShaking>();

        if (target == null)
        {
            Debug.Break();
            Debug.Log("falta");
        }
            
    }

    void Update()
    {
        UpdateCamPosition();

        if(chameraShaking.enabled == false)
        {
            currentPosition = transform.position;
        }
    }

    //añadir el camera shaking a este script

    //Updatea el cambio de pantallas en base a la posicion del jugador
    private void UpdateCamPosition()
    {
        //si el player se pasa del valor maximo del camera bound cambia de pantalla a la siguiente
        if (target.position.x > maxValueX)
        {
            chameraShaking.enabled = false;

            //ahora mismo lo hace sumando la posicion a la anterior, seguro q hay una manera mas elegante de hacer esto
            transform.position = new Vector3(currentPosition.x + offsetX, currentPosition.y, -10);

            //actualizamos los nuevos valores maximos
            minValueX += offsetX;
            maxValueX += offsetX;

            currentPosition = transform.position;
        }
        //lo mismo para el minimo
        if (target.position.x < minValueX)
        {
            chameraShaking.enabled = false;

            transform.position = new Vector3(currentPosition.x - offsetX, currentPosition.y, -10);

            minValueX -= offsetX;
            maxValueX -= offsetX;

            currentPosition = transform.position;
        }

        //si el player se pasa del valor maximo del camera bound cambia de pantalla a la siguiente
        if (target.position.y > maxValueY)
        {
            //temporal
            chameraShaking.enabled = false;

            //ahora mismo lo hace sumando la posicion a la anterior, seguro q hay una manera mas elegante de hacer esto
            transform.position = new Vector3(currentPosition.x, currentPosition.y + offsetY, -10);

            //actualizamos los nuevos valores maximos
            minValueY += offsetY;
            maxValueY += offsetY;

            currentPosition = transform.position;
        }
        //lo mismo para el minimo
        if (target.position.y < minValueY)
        {
            chameraShaking.enabled = false;

            transform.position = new Vector3(currentPosition.x, currentPosition.y - offsetY, -10);

            minValueY -= offsetY;
            maxValueY -= offsetY;

            currentPosition = transform.position;
        }

    }

}
