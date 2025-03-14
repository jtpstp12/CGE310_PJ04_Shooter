using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Playground"); // ��Ŵ�ҡ Playground
    }

    public void QuitGame()
    {
        Debug.Log("Quit Game");
        Application.Quit(); // �͡�ҡ�� (��������� Build ��)
    }
}
