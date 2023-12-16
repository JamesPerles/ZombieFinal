using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    public float countdownTime = 10.0f; 
    private float currentTime; 
    public TextMeshProUGUI countdownText; 

    private void Start()
    {
        currentTime = countdownTime; 
        UpdateUI();
    }

    private void Update()
    {
        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime; 
            UpdateUI();
        }
        else
        {
            currentTime = 0;
            SceneManager.LoadScene("MainScene2");
        }
    }

    private void UpdateUI()
    {
        if (countdownText != null)
        {
            countdownText.text = currentTime.ToString("F1"); 
        }
    }
}
