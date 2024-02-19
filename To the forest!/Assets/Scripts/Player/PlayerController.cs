using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] float currentSpeed;

    public bool controlState;
    private bool baseState;

    private KeyCode[] letterKeys;
    private KeyCode[] arrowKeys;

    private float startZ;

    PlayerAnimate playerAnimate; 

    void Awake()
    {
        currentSpeed = 5.0f;
        LevelManager.SetSpeed(currentSpeed);

        baseState = false;

        letterKeys = new KeyCode[] { KeyCode.A, KeyCode.D, KeyCode.W, KeyCode.S };
        arrowKeys = new KeyCode[] { KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.UpArrow, KeyCode.DownArrow };

        startZ = transform.position.z;

        playerAnimate = GetComponent<PlayerAnimate>();
    }

    void Update()
    {
        if (baseState)
        {
            MovePlayer(Vector3.forward);
            LevelManager.Counting((int)(transform.position.z - startZ));

            CheckKeys();
        }

        if (LevelManager.State == LevelManager.GameState.Won || LevelManager.State == LevelManager.GameState.Failed) baseState = false;
    }

    void CheckKeys()
    {
        for (int i = 0; i < letterKeys.Length; i++)
        {
            if (Input.GetKey(letterKeys[i]) || Input.GetKey(arrowKeys[i]))
            {

                if (i == 0)
                {
                    MovePlayer(Vector3.left);
                }

                if (i == 1)
                {
                     MovePlayer(Vector3.right);
                }

                if (!playerAnimate.isRunning() && (i == 2 || i == 3))
                {
                    playerAnimate.StartAnimation(i);
                }
            }
        }
    }

    void MovePlayer(Vector3 _moveVector)
    {
        transform.Translate(currentSpeed * Time.deltaTime * _moveVector);
    }

    public void StartRun()
    {
        playerAnimate.RunningAnimation();
        Invoke("Run", 0.75f);
    }

    void Run()
    {
        baseState = true;
        controlState = true;
        LevelManager.Running = true;
    }

    private void OnTriggerEnter(Collider _collider)
    {
        baseState = false;
        playerAnimate.Crashed();
        LevelManager.EndGame(false);
    }
}
