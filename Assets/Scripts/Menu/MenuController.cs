using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public void LoadMainMenu ()
    {
        SceneManager.LoadScene(0);
    }

    public void ReloadScene ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadPreviousScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void QuitGame()
    {
        Debug.Log("Fermeture de l'application...");
        Application.Quit();
    }

    public void LoadCredits()
    {
        transform.Find("[Panel] StartMenu").gameObject.SetActive(false);
        transform.Find("[Panel] Credits").gameObject.SetActive(true);
    }

    public void UnloadCredits()
    {
        transform.Find("[Panel] Credits").gameObject.SetActive(false);
        transform.Find("[Panel] StartMenu").gameObject.SetActive(true);
    }
    
}
