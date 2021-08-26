using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public GameObject canvasPause;
    
    // Start is called before the first frame update
    void Start()
    {
        canvasPause.SetActive(false);
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   
    public void ButtonPause()
    {
        canvasPause.SetActive(true);
        Time.timeScale = 0;
    }
    public void ButtonVoltar()
    {
        canvasPause.SetActive(false);
        Time.timeScale = 1;
    }

    public void SceneLoad(string scena)
    {
        SceneManager.LoadScene(scena);
        Time.timeScale = 1;
    }
   
}
