using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuBehaviour : MonoBehaviour
{
    #region Variables
    public GameObject baseMenu;
    private GameObject _currentMenu;
    #endregion

    #region MonoBehaviour callbacks
    private void Start()
    {
        _currentMenu = baseMenu;
        _currentMenu.SetActive(true);
    }
    #endregion

    #region Custom callbacks
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
    #endregion
}
