using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using StarterAssets;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public event System.Action OnGameEnd; // Event ที่ใช้แจ้งว่าจบเกม

    public GameObject gameOverPanel;
    public GameObject finishGamePanel; // UI เมื่อจบเกม
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI killCountText;
    public TextMeshProUGUI finishTimeText;
    public TextMeshProUGUI bestTimeText;

    private float elapsedTime = 0f;
    private int killCount = 0;
    private bool isGameActive = true;
    public int killTarget = 10; // จำนวนศัตรูที่ต้องฆ่าก่อนจบเกม
    private float bestTime;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        bestTime = PlayerPrefs.GetFloat("BestTime", float.MaxValue);
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (isGameActive)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerUI();
        }
    }

    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void AddKill()
    {
        killCount++;
        killCountText.text = "Kills: " + killCount;

        if (killCount >= killTarget) // เช็คว่าฆ่าถึงเป้าหมายหรือยัง
        {
            FinishGame();
        }
    }

    public void GameOver()
    {
        isGameActive = false;
        gameOverPanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        StarterAssets.FirstPersonController player = FindObjectOfType<StarterAssets.FirstPersonController>();
        if (player != null)
        {
            player.enabled = false;
        }

        Time.timeScale = 0f;
        OnGameEnd?.Invoke(); // แจ้ง event ว่าเกมจบแล้ว
    }

    public void FinishGame()
    {
        isGameActive = false;
        finishGamePanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        StarterAssets.FirstPersonController player = FindObjectOfType<StarterAssets.FirstPersonController>();
        if (player != null)
        {
            player.enabled = false;
        }

        Time.timeScale = 0f;

        finishTimeText.text = "Your Time: " + timerText.text;

        if (elapsedTime < bestTime)
        {
            bestTime = elapsedTime;
            PlayerPrefs.SetFloat("BestTime", bestTime);
            PlayerPrefs.Save();
        }

        bestTimeText.text = "Best Time: " + string.Format("{0:00}:{1:00}", Mathf.FloorToInt(bestTime / 60), Mathf.FloorToInt(bestTime % 60));

        OnGameEnd?.Invoke(); // แจ้ง event ว่าเกมจบแล้ว
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
