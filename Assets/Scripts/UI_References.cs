using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_References : MonoBehaviour {

    private AsyncOperation async;
    public RawImage fadeOut;
    float timer;
    public float fadeOutTimer;
    int multiply = 0;
    bool endinggame = false;

    //Player
    public Transform player;

    //Health and Hunger
    public Transform border;
    public Transform hpBar_Image;
    public Transform hungerBar_Image;
    public Transform hpBar_Text;
    public Transform hungerBar_Text;

    //Time for flashing
    float time = 0;
    float flashTime = 1.0f;

    //Popups and Powerups
    public Transform popup_border;
    public Transform popup_Text;
    public Transform powerup_border;
    public Transform powerup_Text;
    public Transform[] animal_images;

    //Number of animals eaten
    int numOfMiceEaten = 0;
    int numOfFrogsEaten = 0;
    int numOfRabbitsEaten = 0;
    int numOfSkunksEaten = 0;
    int numOfWolvesEaten = 0;
    int touchCheese = 0;

    //GameOver
    public Transform gameover_image;

    int deathCounter;
    int timeForCouner = 0;

    //Gameover stats
    public Transform border_stats;
    public Transform death_text;
    public Transform time_text;
    public Transform animals_eaten_text;
    float time_in_days = 0;
    public Transform day_night_controller;

    //Minimap stuff
    public Transform minimapBorder;
    public Transform timePanel;

    //Detection UI
    public Transform dection_border;
    public Transform detection_bar;
    public Transform detection_text;

    // Use this for initialization
    void Start()
    {
        //Hunger and Health
        hpBar_Text.GetComponent<Text>().text = "Health: 100%";
        hungerBar_Text.GetComponent<Text>().text = "Hunger: 100%";
        border.GetComponent<Image>().color = Color.white;

        //Game Over stuff
        gameover_image.GetComponent<Image>().enabled = false;
        border_stats.GetComponent<Image>().enabled = false;
        death_text.GetComponent<Text>().enabled = false;
        time_text.GetComponent<Text>().enabled = false;
        animals_eaten_text.GetComponent<Text>().enabled = false;

        //Powerups and popups
        powerup_border.GetComponent<Image>().enabled = false;
        powerup_Text.GetComponent<Text>().enabled = false;
        for (int i = 0; i < animal_images.Length; i++)
        {
            animal_images[i].GetComponent<Image>().enabled = false;
        }

        //disablePopup();

        //Disable sound detection
        disableDetectionBar();

        //Cheese Tutorial
        enablePopup();
        popup_Text.GetComponent<Text>().text = "Touch the Item to Carry it with You";

    }

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.None;

        fadeOutTimer += Time.deltaTime * multiply;

        if (fadeOutTimer > 5 && endinggame == true)
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    //Change health
    public void setHealth(float health)
    {
        float calHealth = health / 100.0f;
        hpBar_Text.GetComponent<Text>().text = "Health: " + (int)health + "%";
        hpBar_Image.GetComponent<Image>().fillAmount = calHealth;
    }

    //Change Hunger
    public void setHunger(float hunger)
    {
        float calHunger = hunger / 100.0f;
        hungerBar_Text.GetComponent<Text>().text = "Hunger: " + (int)hunger + "%";
        hungerBar_Image.GetComponent<Image>().fillAmount = calHunger;
    }

    public void flashWarning()
    {
        time += Time.deltaTime;
        if (flashTime < time)
        {
            if (border.GetComponent<Image>().color == Color.red)
                border.GetComponent<Image>().color = Color.white;
            else if (border.GetComponent<Image>().color == Color.white)
                border.GetComponent<Image>().color = Color.red;

            time = 0;
        }

    }

    //Shows popup
    public void enablePopup()
    {
        popup_border.GetComponent<Image>().enabled = true;
        popup_Text.GetComponent<Text>().enabled = true;
    }

    //Disables popup
    public void disablePopup()
    {
        popup_border.GetComponent<Image>().enabled = false;
        popup_Text.GetComponent<Text>().enabled = false;
    }

    //Tutorial for Cheese/Items
    public void cheeseTut()
    {
        if (touchCheese < 1)
        {
            enablePopup();
            popup_Text.GetComponent<Text>().text = "Click Left Mouse Button to Let Go of the Item";
            touchCheese++;
        }
    }

    //Shows the current powerup
    public void showSkill(string animal)
    {
        Debug.Log("Showing Skill");
        //Reset Picture
        for (int i = 0; i < animal_images.Length; i++)
        {
            animal_images[i].GetComponent<Image>().enabled = false;
        }

        powerup_border.GetComponent<Image>().enabled = true;
        powerup_Text.GetComponent<Text>().enabled = true;

        if (animal == "mouse")
        {
            if (numOfMiceEaten < 1)
            {
                enablePopup();
                popup_Text.GetComponent<Text>().text = "Press 'F' to Shrink/Unshrink";
            }

            animal_images[0].GetComponent<Image>().enabled = true;
            powerup_Text.GetComponent<Text>().text = "Ability: Shrink";
            numOfMiceEaten++;
        }
        else if (animal == "frog")
        {
            if (numOfFrogsEaten < 1)
            {
                enablePopup();
                popup_Text.GetComponent<Text>().text = "Press 'Space' to Jump";
            }
            animal_images[1].GetComponent<Image>().enabled = true;
            powerup_Text.GetComponent<Text>().text = "Ability: Jump";
            numOfFrogsEaten++;
        }
        else if (animal == "rabbit")
        {
            if (numOfRabbitsEaten < 1)
            {
                enablePopup();
                popup_Text.GetComponent<Text>().text = "Press 'F' to Run Faster";
            }
            animal_images[2].GetComponent<Image>().enabled = true;
            powerup_Text.GetComponent<Text>().text = "Ability: Speed";
            numOfRabbitsEaten++;
        }
        else if (animal == "skunk")
        {
            if (numOfSkunksEaten < 1)
            {
                enablePopup();
                popup_Text.GetComponent<Text>().text = "Press 'F' to Spray Your Enemies Away";
            }
            animal_images[4].GetComponent<Image>().enabled = true;
            powerup_Text.GetComponent<Text>().text = "Ability: Spray";
            numOfSkunksEaten++;
        }
        else if (animal == "wolf")
        {      
            animal_images[5].GetComponent<Image>().enabled = true;
            powerup_Text.GetComponent<Text>().text = "";
            numOfSkunksEaten++;
        }
        else if (animal == "bear")
        {
            animal_images[6].GetComponent<Image>().enabled = true;
            powerup_Text.GetComponent<Text>().text = "";
            numOfSkunksEaten++;
        }
    }

    //Enable game over
    public void showGameOver()
    {
        day_night_controller.GetComponent<DayNightCircle>().gameOver = true;
        gameover_image.GetComponent<Image>().enabled = true;
        border_stats.GetComponent<Image>().enabled = true;
        death_text.GetComponent<Text>().enabled = true;
        time_text.GetComponent<Text>().enabled = true;
        animals_eaten_text.GetComponent<Text>().enabled = true;

        time_in_days = day_night_controller.GetComponent<DayNightCircle>().numOfDaysTaken();

        death_text.GetComponent<Text>().text = "You Died a Total of " + deathCounter + " times";

        time_text.GetComponent<Text>().text = "You survived for: " + time_in_days + " days";

        animals_eaten_text.GetComponent<Text>().text = "You managed to Eat: \n" +
            (numOfMiceEaten > 0 ? numOfMiceEaten + (numOfMiceEaten == 1 ? " Mouse \n" : " Mice \n") : "") +
            (numOfFrogsEaten > 0 ? numOfFrogsEaten + (numOfFrogsEaten == 1 ? " Frog \n" : " Frogs \n") : "") +
            (numOfRabbitsEaten > 0 ? numOfRabbitsEaten + (numOfRabbitsEaten == 1 ? " Rabbit \n" : " Rabbits \n") : "") +
            (numOfSkunksEaten > 0 ? numOfSkunksEaten + (numOfSkunksEaten == 1 ? " Skunk \n" : " Skunks \n") : "") +
            (numOfWolvesEaten > 0 ? numOfWolvesEaten + (numOfWolvesEaten == 1 ? " Wolf \n" : " Wolves \n") : "");

        endGame();
    }

    //Minimap change
    public void changeMinimapBorder()
    {
        minimapBorder.GetComponent<RectTransform>().anchoredPosition = new Vector3(-42f, -129f, -11.7f);
        timePanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(-86f, -274f, 0f);

    }

    public void revertMinimapBorder()
    {
        minimapBorder.GetComponent<RectTransform>().anchoredPosition = new Vector3(-129f, -129f, -11.7f);
        timePanel.GetComponent<RectTransform>().anchoredPosition = new Vector3(-122f, -271f, 0f);
    }

    //Detection bar
    public void enableDetectionBar()
    {
        dection_border.GetComponent<Image>().enabled = true;
        detection_bar.GetComponent<Image>().enabled = true;
        detection_text.GetComponent<Text>().enabled = true;
    }

    public void disableDetectionBar()
    {
        dection_border.GetComponent<Image>().enabled = false;
        detection_bar.GetComponent<Image>().enabled = false;
        detection_text.GetComponent<Text>().enabled = false;
    }

    public void addDeathCount()
    {
        if (timeForCouner == 0)
        {
            deathCounter++;
            timeForCouner++;
        }
    }

    public void resetCounterTimer()
    {
        timeForCouner = 0;
        day_night_controller.GetComponent<DayNightCircle>().playerDead = false;
    }

    public void endGame()
    {
        //if (!creditsMenu.gameObject.activeInHierarchy && !controlMenu.gameObject.activeInHierarchy)
        //{
        endinggame = true;
        multiply = 1;
        fadeOut.gameObject.SetActive(true);
        //}
    }

    public void enableTackle()
    {
        enablePopup();
        popup_Text.GetComponent<Text>().text = "Press 'F' to Tackle";
    }

    public void disableTackle()
    {
        disablePopup();
    }
}
