using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtons : MonoBehaviour {

    public Transform image;

	// Use this for initialization
	void Start () 
    {
        if(image != null)
         image.GetComponent<Image>().enabled = false;
	}

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (MainMenu.storyMenu.IsActive())
            {
                MainMenu.storyMenu.gameObject.SetActive(false);
            }

            if (MainMenu.controlMenu.IsActive())
            {
                MainMenu.controlMenu.gameObject.SetActive(false);
            }

            if (MainMenu.creditsMenu.IsActive())
            {
                MainMenu.creditsMenu.gameObject.SetActive(false);
            }
        }
    }

    public void changeButtonColor()
    {
        gameObject.GetComponentInChildren<Text>().color = Color.green;
        if (image != null)
            image.GetComponentInChildren<Image>().enabled = true;
    }

    public void exitButtonColor()
    {
        gameObject.GetComponentInChildren<Text>().color = Color.black;
        if (image != null)
            image.GetComponentInChildren<Image>().enabled = false;
    }
    public void enableStoryMenu()
    {
        MainMenu.storyMenu.gameObject.SetActive(true);
    }

    public void enableControlMenu()
    {
        MainMenu.controlMenu.gameObject.SetActive(true);
    }

    public void enableCreditsMenu()
    {
        MainMenu.creditsMenu.gameObject.SetActive(true);
    }

    public void quitGame()
    {
        #if UNITY_EDITOR
             UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();
    }
}
