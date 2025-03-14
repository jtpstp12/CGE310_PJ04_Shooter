using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Playground"); // โหลดฉาก Playground
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit(); // ออกจากเกม (ใช้ได้เมื่อ Build เกม)
    }
}
