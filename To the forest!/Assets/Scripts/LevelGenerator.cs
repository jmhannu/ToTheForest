using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour

{
    [Header("Level Settings")]
    [SerializeField] int runLength;
    [SerializeField] int sectionLength;
    [SerializeField] int activeSections;

    [SerializeField] GameObject endScene;

    private GameObject levelSections; 
    private List<GameObject> sections;
    private List<GameObject> inGame;

    private int counter;
    private int totalSections;
    private int currentPos;
    private bool generating;

    void Start()
    {
        levelSections = new GameObject("LevelSections");

        LevelManager.InitGame(sectionLength, runLength);
        sections = LevelManager.LoadObjects("Sections");

        currentPos = sectionLength;
        generating =  false;

        inGame = new List<GameObject> { GameObject.Find("StartSection") };
        counter = 1;

        if (totalSections > 1) RandomSection();

        totalSections = Mathf.CeilToInt((float)LevelManager.FinishDistance / (float)sectionLength);
    }

    void Update()
    {
        if (!generating) 
        {
            if(LevelManager.Running)
            {
                generating = true;

                if (counter >= totalSections) EndScene();
                else StartCoroutine(NextSection());

                if (counter >= totalSections - 1)
                {
                    LevelManager.Last = true;
                    EndScene();
                }
            }
        }
    }

    IEnumerator NextSection()
    {
        RandomSection(); 

        if (inGame.Count > activeSections)
        {
            Destroy(inGame[0]);
            inGame.RemoveAt(0);
            LevelManager.RemovedSection(sectionLength);
        }

        yield return new WaitForSeconds(sectionLength / LevelManager.CurrentSpeed);

        generating = false;
    }
    
    void RandomSection()
    {
        int section = UnityEngine.Random.Range(0, sections.Count - 1);
        inGame.Add(Instantiate(sections[section], new Vector3(0, 0, currentPos), Quaternion.identity));
        inGame[^1].transform.parent = levelSections.transform;
        counter++;
        currentPos += sectionLength;
        LevelManager.AddedSection(sectionLength);
    }


    void EndScene()
    {
        Instantiate(endScene, new Vector3(0, 0, currentPos), Quaternion.identity);
        LevelManager.Running = false;
    }
}
