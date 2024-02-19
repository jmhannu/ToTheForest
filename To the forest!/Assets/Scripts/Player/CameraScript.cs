using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public Vector3 cameraPos;
    public float shakeDuration; 
    public float shakeStrength; 
    public bool shaking;
    public float power = 0.5f;
    public float duration = 0.1f;
    public float slowDownAmount = 1f;
    private GameObject player;

    private Transform cameraTransform;
    private Vector3 currentPos;
    private float initialDuration; 
    Vector3 startPostion;

    void Start()
    {
        player = GameObject.Find("Player");
        shaking = false;
        

        cameraTransform = Camera.main.transform;

        initialDuration = duration; 
        startPostion = cameraTransform.localPosition;
    }
    void Update()
    {
        if(shaking)
        {
            if (duration == initialDuration) startPostion = transform.localPosition;
                    
            if (duration > 0)
            {
                cameraTransform.localPosition = startPostion + UnityEngine.Random.insideUnitSphere * power;
                duration -= (Time.deltaTime * 7.0f);
                Debug.Log(duration);
            }

            else
            {
                shaking = false;
                cameraTransform.localPosition = startPostion;

            }
        }

        else transform.position = player.transform.position + cameraPos;
    }

    public void ShakeCamera()
    {
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        float inTime = 0.2f;
        currentPos = transform.position;

        while(inTime > 0.2f)
        {
            cameraTransform.localPosition = startPostion + Random.insideUnitSphere * power;
            inTime -= Time.deltaTime;

            cameraTransform.localPosition = startPostion + Random.insideUnitSphere * power;
            duration -= Time.deltaTime * slowDownAmount;

            yield return null;
        }
        transform.position = currentPos;

        cameraTransform.localPosition = currentPos;
    }
}
