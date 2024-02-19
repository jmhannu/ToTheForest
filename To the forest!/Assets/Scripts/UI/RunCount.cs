using TMPro;
using UnityEngine;

public class RunCount : MonoBehaviour
{
    private TextMeshProUGUI countText;

    void Start()
    {
        countText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        countText.text = LevelManager.Counter.ToString() + ("/") + LevelManager.FinishDistance.ToString();
    }
}
