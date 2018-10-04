using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SesameHealth : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;
    [SerializeField]
    private float currentHealth = 0f;
    [SerializeField]
    private float maxHunger;
    [SerializeField]
    private float currentHunger = 0f;

    //UI
    public Transform ui_refs;

    //temporarily
    public bool died = false;

    // Use this for initialization
    void Start()
    {
        currentHealth = maxHealth;
        currentHunger = maxHunger;
        ui_refs.GetComponent<UI_References>().setHealth(currentHealth);
        ui_refs.GetComponent<UI_References>().setHunger(currentHunger);
    }

    void Update()
    {
        //decrementHealth(0.1f);
        if((currentHealth < 20.0f && currentHealth > 0.0f) || (currentHunger < 20.0f && currentHunger > 0.0f))
            ui_refs.GetComponent<UI_References>().flashWarning();
        decrementHunger(0.01f);
    }

    public void decrementHealth(float dmg)
    {
        if (currentHealth > 0f && currentHealth <= maxHealth)
        {
            currentHealth -= dmg;
            if (currentHealth < 0)
                currentHealth = 0;
            ui_refs.GetComponent<UI_References>().setHealth(currentHealth);

        }

        if (currentHealth <= 0f)
        {
            died = true;
            ui_refs.GetComponent<UI_References>().addDeathCount();
            //ui_refs.GetComponent<UI_References>().showGameOver();

        }
    }

    public void incrementHealth(float dmg)
    {
        if (currentHealth > 0f && currentHealth <= maxHealth)
        {
            currentHealth += dmg;
            if (currentHealth > 100)
                currentHealth = 100;
            ui_refs.GetComponent<UI_References>().setHealth(currentHealth);

        }
    }

    public float getHealth()
    {
        return currentHealth;
    }

    public void decrementHunger(float dmg)
    {
        if (currentHunger > 0f && currentHunger <= maxHunger)
        {
            currentHunger -= dmg;
            if (currentHunger < 0)
                currentHunger = 0;
            ui_refs.GetComponent<UI_References>().setHunger(currentHunger);
        }

        if (currentHunger <= 0f)
        {
            died = true;
            ui_refs.GetComponent<UI_References>().addDeathCount();
            //ui_refs.GetComponent<UI_References>().showGameOver();
        }
    }

    public void incrementHunger(float dmg)
    {
        if (currentHunger > 0f && currentHunger <= maxHunger)
        {
            currentHunger += dmg;
            if (currentHunger > 100)
                currentHunger = 100;
            ui_refs.GetComponent<UI_References>().setHunger(currentHunger);
        }
    }

    public float getHunger()
    {
        return currentHunger;
    }

    public bool isDead()
    {
        return died;
    }

   public void resetHunger(float hunger)
    {
        if (hunger >= 100)
            currentHunger = 100;

        ui_refs.GetComponent<UI_References>().setHunger(currentHunger);
    }

   public void resetHealth(float health)
   {
       if (health >= 100)
           currentHealth = 100;

       ui_refs.GetComponent<UI_References>().setHealth(currentHealth);
   }

}
