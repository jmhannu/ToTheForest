using System;
using System.Collections;
using UnityEngine;


public class PlayerAnimate : MonoBehaviour
{
    [SerializeField] float jumpHeight;
    [SerializeField] CameraScript cameraScript;

    [Header("Player sounds")]
    [SerializeField] AudioClip rollSound;
    [SerializeField] AudioClip jumpSound;
    [SerializeField] AudioClip crashSound;

    bool runsRoutine;

    private float startHeight;

    private Animator foxAnimator;
    private AudioSource playSound;
    private BoxCollider boxCollider;


    void Start()
    {
        jumpHeight = 7.0f;

        foxAnimator = GameObject.Find("Fox").GetComponent<Animator>();

        startHeight = transform.position.y;

        boxCollider = GetComponent<BoxCollider>();
        playSound = GetComponent<AudioSource>();

        runsRoutine = false;
    }

    private void Update()
    {
        if (LevelManager.State == LevelManager.GameState.Won) Finished();
    }

    public void StartAnimation(int _type)
    {
        switch (_type)
        {
            case 2: StartCoroutine(Jump());
                    break;
            case 3: StartCoroutine(Roll());
                    break;
         }
    }

    IEnumerator Jump()
    {
        runsRoutine = true; 
        float angle = 30f;
        Vector3 byAngle = Vector3.left * angle;

        playSound.clip = jumpSound;
        playSound.Play();

        for (int i = 0; i < 4; i++)
        {
            float inTime;
            if (i == 0 || i == 2) inTime = 0.3f;
            else inTime = 0.2f;

            Quaternion fromAngle = transform.rotation;
            Quaternion toAngle = Quaternion.Euler(transform.eulerAngles + byAngle);

            for (float t = 0.0f; t < 1.0f + (Time.deltaTime / inTime); t += Time.deltaTime / inTime)
            {

                float clampT = Math.Clamp(t, 0.0f, 1.0f);
                transform.rotation = Quaternion.Lerp(fromAngle, toAngle, clampT);

                if (i == 0)
                {
                    float up = Mathf.Lerp(startHeight, jumpHeight, clampT);
                    transform.position = new Vector3(transform.position.x, up, transform.position.z);
                }
                else if (i == 2)
                {
                    float down = Mathf.Lerp(jumpHeight, startHeight, clampT);
                    transform.position = new Vector3(transform.position.x, down, transform.position.z);
                }

                yield return null;
            }

            if (i == 0 || i == 2) byAngle *= -1;
        }

        runsRoutine = false;
    }

    IEnumerator Roll()
    {
        runsRoutine = true; 
        playSound.clip = rollSound;
        playSound.Play();

        foxAnimator.Play("Fox_Somersault_InPlace");

        float downDist = 0.2f;
        Vector3 foxScale = transform.localScale;
        transform.localScale = foxScale * 0.9f;

        boxCollider.center = new Vector3(0, boxCollider.center.y - downDist, 0);

        AnimatorClipInfo[] animationInfo = foxAnimator.GetCurrentAnimatorClipInfo(0);
        float rolltime = animationInfo[0].clip.length + 0.3f;

        yield return new WaitForSeconds(rolltime);

        boxCollider.center = new Vector3(0, boxCollider.center.y + downDist, 0);
        transform.localScale = foxScale;

        runsRoutine = false; 
    }

    public void Finished()
    {
        foxAnimator.Play("Fox_Jump");
    }

    public void RunningAnimation()
    {
        foxAnimator.SetTrigger("Run");
    }

    public void Crashed()
    {
        playSound.clip = crashSound;
        playSound.Play();
        boxCollider.enabled = false;
        cameraScript.shaking = true;
        foxAnimator.Play("Fox_Falling");
    }

    public bool isRunning()
    {
        return runsRoutine;
    }
}
