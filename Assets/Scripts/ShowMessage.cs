using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShowMessage : MonoBehaviour
{
    static ShowMessage instance;
    Coroutine coroutine;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        instance.GetComponent<Text>().text = "";
    }

    //ShowMessage.Show() - для вывода сообщения на экран
    public static void Show(string _text, float _lifetime = 3f)
    {
        instance.GetComponent<Text>().text = _text;
        instance.StopAllCoroutines();
        instance.coroutine = instance.StartCoroutine(instance.HideMessage(_lifetime));
    }

    IEnumerator HideMessage(float _time)
    {
        yield return new WaitForSecondsRealtime(_time);
        instance.GetComponent<Text>().text = "";

        //если gameOver - делаем рестарт уровня
        if (Time.timeScale == 0f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Time.timeScale = 1f;
            Ball.SetSpeed(5f); //возвращаем обычную скорость полета шаров
        }
    }
}
