using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Image[] heartIcons;
    [SerializeField] private TextMeshProUGUI enemiesCountText;
    [SerializeField] private TextMeshProUGUI winText;
    [SerializeField] private TextMeshProUGUI loseText;
    [SerializeField] private GameObject winLoseContainer;
    [SerializeField] private Button replayButton;
    
    private int heartCount;
    private int enemiesLeftCount;
    private int totalEnemies;

    private void Awake()
    {
        winText.gameObject.SetActive(false);
        loseText.gameObject.SetActive(false);
        replayButton.gameObject.SetActive(false);
        winLoseContainer.SetActive(false);
        replayButton.onClick.AddListener(ReplayGame);
    }

    private void Start()
    {
        
        heartCount = heartIcons.Length - 1;
        GameManager.Instance.OnPlayerGetHurt += UpdatePlayerLifeUI;
        GameManager.Instance.OnEnemyDie += UpdateEnemyCountUI;
        GameManager.Instance.OnAllEnemiesDie += ShowWinText;
        GameManager.Instance.OnPlayerDie += ShowLoseText;
    }

    public void SetupEnemiesCountText(int amount)
    {
        enemiesLeftCount = amount;
        totalEnemies = amount;
        enemiesCountText.text = 
            enemiesLeftCount + "/" + totalEnemies;
    }

    private void UpdatePlayerLifeUI()
    {
        heartIcons[heartCount].gameObject.SetActive(false);
        heartCount--;
    }

    private void UpdateEnemyCountUI()
    {
        enemiesLeftCount -= 1;
        enemiesCountText.text = 
            enemiesLeftCount + "/" + totalEnemies;
    }

    private void ShowWinText()
    {
        winLoseContainer.SetActive(true);
        winText.gameObject.SetActive(true);
        replayButton.gameObject.SetActive(true);
    }

    private void ShowLoseText()
    {
        winLoseContainer.SetActive(true);
        loseText.gameObject.SetActive(true);
        replayButton.gameObject.SetActive(true);
    }

    private void ReplayGame()
    {
        SceneManager.LoadScene("Gameplay");
    }
}
