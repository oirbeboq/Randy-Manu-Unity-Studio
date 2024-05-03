using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public static Timer instance;
    public Slider slider1;
    public Slider slider2;

    public float gameTime;
    public float timeDuration = 12f;
    public float time;

    public bool isRunning;

    public Image background_1;
    public Image fill_1;

    public Image background_2;
    public Image fill_2;

    public GameObject gameOverScreen;


    public void Start()
    {
        instance = this;


        isRunning = true;

        time = timeDuration;

        background_1.enabled = true;
        fill_1.enabled = true;

        background_2.enabled = true;
        fill_2.enabled = true;

        gameOverScreen.SetActive(false);
    }

    public void ResetTime()
    {
        isRunning = false;
        time = timeDuration;
        gameTime = 12f;
    }
    public void Update()
    {
        if (isRunning)
        {
            time -= Time.deltaTime;
            gameTime = Mathf.Clamp01(time / timeDuration);


            slider1.value = gameTime;
            slider2.value = gameTime;
        }

        if (time <= 0)
        {
            GameOver();

        }


    }
    public void GameOver()
    {
        //freeze game
        Time.timeScale = 0;

        gameOverScreen.SetActive(true);

        fill_1.enabled = false;
        fill_2.enabled = false;


        if (Input.GetKeyDown(KeyCode.Return))
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(0);

        }
    }

}
