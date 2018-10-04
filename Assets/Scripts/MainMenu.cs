using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public static RawImage storyMenu;
    public static RawImage controlMenu;
    public static RawImage creditsMenu;

    private AsyncOperation async;
    public RawImage fadeOut;
    float timer;
    public float fadeOutTimer;
    int multiply = 0;
    bool startinggame = false;

    // Use this for initialization
    void Start()
    {

        fadeOut.gameObject.SetActive(false);
        storyMenu = GameObject.Find("StoryPanel").GetComponent<RawImage>();
        controlMenu = GameObject.Find("ControlPanel").GetComponent<RawImage>();
        creditsMenu = GameObject.Find("CreditsPanel").GetComponent<RawImage>();

        storyMenu.gameObject.SetActive(false);
        controlMenu.gameObject.SetActive(false);
        creditsMenu.gameObject.SetActive(false);

    }

    IEnumerator FadeOut(string scene)
    {
        async = SceneManager.LoadSceneAsync(scene);
        while (!async.isDone)
        {
            Debug.Log(async.progress);
            Color temp = fadeOut.color;
            temp.a = async.progress;
            fadeOut.color = temp;
            yield return null;
        }
    }


    // Update is called once per frame
    void Update()
    {
        //Cursor.lockState = CursorLockMode.None;

        fadeOutTimer += Time.deltaTime * multiply;

        if (fadeOutTimer < 5 && fadeOutTimer != 0 && startinggame == true)
        {
            Color temp = fadeOut.color;
            temp.a = 0.01f;
            fadeOut.color += temp;
        }

        if (fadeOutTimer > 5 && startinggame == true)
        {
            SceneManager.LoadScene("Hunting");
        }
    }

    public void startGame()
    {
        if (!creditsMenu.gameObject.activeInHierarchy && !controlMenu.gameObject.activeInHierarchy && !storyMenu.gameObject.activeInHierarchy)
        {
        startinggame = true;
        multiply = 1;
        fadeOut.gameObject.SetActive(true);
        }
    }
}
