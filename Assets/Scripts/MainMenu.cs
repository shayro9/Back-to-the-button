using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject tutorialText;
    // Start is called before the first frame update
    void Awake()
    {
        PlayerPrefs.SetFloat("volume", 1);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    public void StartTutorial()
    {
        StartCoroutine(WaitTutorial());
    }
    IEnumerator WaitTutorial()
    {
        yield return new WaitForSeconds(1.6f);
        tutorialText.SetActive(true);
    }
    public void QuitGame()
    {
        Application.Quit();
    }
    public void changeVolume(float newVolume)
    {
        PlayerPrefs.SetFloat("volume", newVolume);
        AudioListener.volume = PlayerPrefs.GetFloat("volume");
    }
}
