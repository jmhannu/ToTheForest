using System.Collections;
using TMPro;
using UnityEngine;

public class CountDown : MonoBehaviour
{
    [SerializeField] PlayerController player; 
    
    private TextMeshProUGUI countText; 
    private int count;
    private float countTime; 

    void Start()
    {
        this.gameObject.SetActive(true);
        countText = GetComponent<TextMeshProUGUI>();
        count = 3;
        countTime = 1; 

        StartCoroutine(Counting());
    }

    private void Update()
    {
        if (LevelManager.State == LevelManager.GameState.Won) Success();
        else if (LevelManager.State == LevelManager.GameState.Failed) Fail();
    }

    IEnumerator Counting()
    {
        while (count > 0) 
        {
            countText.text = count.ToString();
            yield return new WaitForSeconds(countTime);
            count--;

        }

        countText.text = "RUN!";
        player.StartRun();

        yield return new WaitForSeconds(countTime);

        countText.enabled = false;
    }

     void Fail()
    {
        countText.enabled = true;
        countText.text = "FAIL";
    }

     void Success()
    {
        countText.enabled = true;
        countText.text = "SUCCESS!";
    }
}
