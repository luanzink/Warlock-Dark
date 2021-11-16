using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeManaManager : MonoBehaviour
{
    public GameObject canvasGD;
    public Image healthBar;
    public Image manaBar;
    public Text hpText;
    public Text mpText;
    

    public float totalHP;
    public float totalMP;
    public float actualHealth;
    public float actualMana;
    private float calculateLife;   

    void Start()
    {
        canvasGD.SetActive(false);
        Time.timeScale = 1;
        actualHealth = totalHP;
        actualMana = totalMP;
    }

    void Update()
    {
        calculateLife = actualHealth / totalHP;
        healthBar.fillAmount = Mathf.MoveTowards(healthBar.fillAmount, calculateLife, Time.deltaTime);
        hpText.text = "" + (int)actualHealth;

        if (actualMana < totalMP)
        {           
            manaBar.fillAmount = Mathf.MoveTowards(manaBar.fillAmount, 1f, Time.deltaTime * 0.07f);
            actualMana = Mathf.MoveTowards(actualMana / totalMP, 1f, Time.deltaTime * 0.07f) * totalMP;
            //if (actualMana == totalMP)
                //manaBar.fillAmount = Mathf.MoveTowards(actualMana, 1F, Time.deltaTime*0.1f);
        }

        if(actualMana < 0)
        {
            actualMana = 0;
        }
        mpText.text = "" + Mathf.FloorToInt(actualMana);

        if (actualHealth <= 0)
        {
            canvasGD.SetActive(true);
            Time.timeScale = 0;
            Debug.Log("YOU DIED");
        }
            
    }

    public void Damage(float damage)
    {
        if (actualHealth >= 0)
        actualHealth -= damage;
    }

    public void ReduceMana(float manaCost)
    {
        if (manaCost <= actualMana)
        {
            actualMana -= manaCost;
            manaBar.fillAmount = actualMana/ totalMP;            
            //enoughMana = true;
        }
        
    }
}
