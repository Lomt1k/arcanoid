using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [SerializeField]
    private int level = 1;
    private static GameController instance;

    public static GameController Instance
    {
        get => instance;
    }

    public int Level
    {
        get => level;
    }

    void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// вызывается при проигрыше в игре
    /// </summary>
    public void GameOver()
    {
        Camera.main.GetComponent<AudioSource>().Stop();
        ShowMessage.Show("Игра окончена");
        Time.timeScale = 0f;
    }

    /// <summary>
    /// вызывается при окончании уровня
    /// </summary>
    public void CompleteLevel()
    {
        if (level == 3)
        {
            SceneManager.LoadScene("MainMenu");
            Destroy(FindObjectOfType<GameController>().gameObject);
            return;
        }

        level++;
        SceneManager.LoadScene("Level" + level);
    }
}
