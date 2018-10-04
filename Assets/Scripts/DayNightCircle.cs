using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DayNightCircle : MonoBehaviour {

    public GameObject sun;
    public GameObject moon;
    public Transform sun_Image;
    public Transform moon_Image;
    public Transform time_text;
    public float dayNightSpeed;
    public Time time;
    public bool night_time;
    bool dayTime = false;

    int night = 0;
    int day = 1;
    public bool playerDead = false;
    public bool gameOver = false;

    private float numOfDays = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        sun.transform.RotateAround(Vector3.zero, Vector3.right, dayNightSpeed * Time.deltaTime);
        moon.transform.RotateAround(Vector3.zero, Vector3.right, dayNightSpeed * Time.deltaTime);
        sun.transform.LookAt(Vector3.zero);
        moon.transform.LookAt(Vector3.zero);

        if (night_time == true && playerDead == false && gameOver == false)
        {
            if (night == 0)
            {
                numOfDays += 0.5f;
                night++;
            }
        }

        if (dayTime == true && playerDead == false && gameOver == false)
        {
            if (day == 0)
            {
                numOfDays += 0.5f;
                day++;
            }
        }

        //Day Time
        if(sun.transform.rotation.x >= 0.0f)
        {
            night = 0;
            night_time = false;
            dayTime = true;
            sun_Image.GetComponent<Image>().enabled = true;
            moon_Image.GetComponent<Image>().enabled = false;
            time_text.GetComponent<Text>().text = "Day Time";
        }

        //Nigh Time
        if(moon.transform.rotation.x > 0.0f)
        {
            day = 0;
            night_time = true;
            dayTime = false;
            sun_Image.GetComponent<Image>().enabled = false;
            moon_Image.GetComponent<Image>().enabled = true;
            time_text.GetComponent<Text>().text = "Night Time";
        }
	}

    public float numOfDaysTaken()
    {
        playerDead = true;
        return numOfDays;
    }
}
