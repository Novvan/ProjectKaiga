using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBehaviour : MonoBehaviour
{
    public GameObject baseMenu;

    private GameObject _currentMenu;

    private void Start()
    {
        _currentMenu = baseMenu;
        _currentMenu.SetActive(true);
    }

    public void goToMenu(GameObject menu)
    {
        _currentMenu.SetActive(false);
        _currentMenu = menu;
        _currentMenu.SetActive(true);
    }

    public void quit()
    {
        Application.Quit();
    }

    public void play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
