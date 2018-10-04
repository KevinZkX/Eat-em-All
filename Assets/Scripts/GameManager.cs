using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : MonoBehaviour {

    public GameObject[] maps;
    public PlayerController sesame;
    public DayNightCircle dayNight;
    List<GameObject> day_time_animals;
    List<GameObject> night_time_animals;
    GameObject sesame_current_map;
    GameObject latest_spawn_point;
    [SerializeField]
    int map_number;
    int las_map_number;
    //audio
    AudioSource night;
    AudioSource relax;
    AudioSource gloomy;
    AudioSource fight;
    AudioSource lack;
    AudioSource land;
    AudioSource wind;
    GameObject player;
    GameObject[] rats;
    GameObject[] frogs;

    public bool isnight;

	// Use this for initialization
	void Start () {
        latest_spawn_point = new GameObject();
        day_time_animals = new List<GameObject>();

        player = GameObject.FindGameObjectWithTag("Player");
        AudioSource[] ad_list = this.GetComponents<AudioSource>();
        night = ad_list[0];
        relax = ad_list[1];
        gloomy = ad_list[2];
        fight = ad_list[3];
        lack = ad_list[4];
        land = ad_list[5];
        wind = ad_list[6];

        las_map_number = 0;
        isnight = false;
	}
	
    void checktoplay()
    {
        if (map_number == 0)
        {
            map_bgm01();
            if (rats.Length > 0)
            {
                foreach (GameObject go in rats)
                {
                    go.GetComponent<AudioSource>().Stop();
                }
            }

            if (frogs.Length > 0)
            {
                foreach (GameObject go in frogs)
                {
                    go.GetComponent<AudioSource>().Stop();
                }
            }
        }
        if (map_number == 1)
        {
            map_bgm01();
            if (rats.Length > 0)
            {
                foreach (GameObject go in rats)
                {
                    go.GetComponent<AudioSource>().Play();
                }
            }

            if (frogs.Length > 0)
            {
                foreach (GameObject go in frogs)
                {
                    go.GetComponent<AudioSource>().Stop();
                }
            }
        }
        if (map_number == 2)
        {
            map_bgm2();
            if (rats.Length > 0)
            {
                foreach (GameObject go in rats)
                {
                    go.GetComponent<AudioSource>().Stop();
                }
            }

            if (frogs.Length > 0)
            {
                foreach (GameObject go in frogs)
                {
                    go.GetComponent<AudioSource>().Play();
                }
            }
        }
        if (map_number == 3)
        {
            map_bgm3();
            if (rats.Length > 0)
            {
                foreach (GameObject go in rats)
                {
                    go.GetComponent<AudioSource>().Stop();
                }
            }

            if (frogs.Length > 0)
            {
                foreach (GameObject go in frogs)
                {
                    go.GetComponent<AudioSource>().Stop();
                }
            }
        }
        if (map_number == 4)
        {
            map_bgm4();
            if (rats.Length > 0)
            {
                foreach (GameObject go in rats)
                {
                    go.GetComponent<AudioSource>().Stop();
                }
            }

            if (frogs.Length > 0)
            {
                foreach (GameObject go in frogs)
                {
                    go.GetComponent<AudioSource>().Stop();
                }
            }
        }
        if (map_number == 5)
        {
            map_bgm5();
            if (rats.Length > 0)
            {
                foreach (GameObject go in rats)
                {
                    go.GetComponent<AudioSource>().Stop();
                }
            }

            if (frogs.Length > 0)
            {
                foreach (GameObject go in frogs)
                {
                    go.GetComponent<AudioSource>().Stop();
                }
            }
        }
    }
	// Update is called once per frame
	void Update () {
        CurrentMap();
        if (las_map_number != map_number)
        {
            checktoplay();
            las_map_number = map_number;
        }
        rats = GameObject.FindGameObjectsWithTag("Npc");
        frogs = GameObject.FindGameObjectsWithTag("Frog");
        
       
	}

    void CurrentMap ()
    {
        sesame_current_map = sesame.current_map;
        latest_spawn_point = sesame.spawn_point;
        if (sesame_current_map)
        {
            if (sesame_current_map.name.Contains("Cat"))
            {
                map_number = 0;
            }
            if (sesame_current_map.name.Contains("Mouse"))
            {
                map_number = 1;
            }
            if (sesame_current_map.name.Contains("Frog"))
            {
                map_number = 2;
            }
            if (sesame_current_map.name.Contains("Rabbit"))
            {
                map_number = 3;
            }
            if (sesame_current_map.name.Contains("Wolf"))
            {
                map_number = 4;
            }
            if (sesame_current_map.name.Contains("Bear"))
            {
                map_number = 5;
            }
        }
        
    }
    
   

    void map_bgm01()
    {
        Debug.Log("bgm01 start");
        if(isnight)
        {
            night.Play();
            relax.Stop();
            gloomy.Stop();
            fight.Stop();
            lack.Stop();
            
            wind.Stop();
        }
        else
        {
            night.Stop();
            relax.Play();
            gloomy.Stop();
            fight.Stop();
            lack.Stop();
           
            wind.Stop();
        }
    }
    void map_bgm2()
    {
        if (isnight)
        {
            night.Play();
            relax.Stop();
            gloomy.Stop();
            fight.Stop();
            lack.Play();
           
            wind.Stop();
        }
        else
        {
            night.Stop();
            relax.Play();
            gloomy.Stop();
            fight.Stop();
            lack.Play();
            
            wind.Stop();
        }
    }
    void map_bgm3()
    {
        if (isnight)
        {
            night.Play();
            relax.Stop();
            gloomy.Stop();
            fight.Stop();
            lack.Stop();

            wind.Stop();
        }
        else
        {
            night.Stop();
            relax.Stop();
            gloomy.Play();
            fight.Stop();
            lack.Stop();

            wind.Stop();
        }
    }
    void map_bgm4()
    {
        if (isnight)
        {
            night.Play();
            relax.Stop();
            gloomy.Stop();
            fight.Stop();
            lack.Stop();

            wind.Play();
        }
        else
        {
            night.Stop();
            relax.Stop();
            gloomy.Play();
            fight.Stop();
            lack.Stop();

            wind.Play();
        }
    }
    void map_bgm5()
    {
       
            night.Stop();
            relax.Stop();
            gloomy.Stop();
            fight.Play();
            lack.Stop();
            wind.Stop();
        
    }
    public void ActiveMaps ()
    {
        
        if (map_number == 1)
        {
            Debug.Log("frog map fall down");
            maps[map_number + 1].GetComponent<Animator>().SetBool("FrogAppear", true);
        }

        if (map_number == 2)
        {
            Debug.Log("frog map fall down");
            maps[map_number + 1].GetComponent<Animator>().SetBool("RabbitFall", true);
        }

        if (map_number == 3)
        {
            Debug.Log("frog map fall down");
            maps[map_number + 1].GetComponent<Animator>().SetBool("WolfFall", true);
        }

        if (map_number == 4)
        {
            Debug.Log("frog map fall down");
            maps[map_number + 1].GetComponent<Animator>().SetBool("TigerFall", true);
        }
        land.Play();
    }
}
