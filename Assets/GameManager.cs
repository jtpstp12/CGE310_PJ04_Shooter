using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement; // ใช้สำหรับ Restart Scene
using StarterAssets; // เพิ่มบรรทัดนี้


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject gameOverPanel; // ลาก GameOverPanel เข้าไปใน Inspector
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI killCountText;

    private float elapsedTime = 0f;
    private int killCount = 0;
    private bool isGameActive = true;


    void Start()
    {
        // ซ่อน Cursor และล็อกไว้ที่กลางจอ
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
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
    }

    public void GameOver()
    {
        isGameActive = false;
        gameOverPanel.SetActive(true);

        // ปลดล็อก Cursor ให้ผู้เล่นสามารถใช้เมาส์ได้
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // ปิดการควบคุมของผู้เล่น
        StarterAssets.FirstPersonController player = FindObjectOfType<StarterAssets.FirstPersonController>();
        if (player != null)
        {
            player.enabled = false;
        }

        Time.timeScale = 0f; // หยุดเวลา
    }


    public void RestartGame()
    {
        Time.timeScale = 1f; // คืนค่าเวลา
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // โหลดฉากใหม่
    }
}
