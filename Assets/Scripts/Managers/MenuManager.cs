using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    #region Variables
    public GameObject baseMenu;
    private GameObject _currentMenu;
    private GameObject _previousMenu;
    #endregion

    #region MonoBehaviour callbacks
    private void Start()
    {
        if (baseMenu != null)
        {

            _currentMenu = baseMenu;
            _currentMenu.SetActive(true);
        }
    }
    #endregion

    #region Custom callbacks
    public void goToMenu(GameObject menu)
    {
        _previousMenu = _currentMenu;
        _currentMenu.SetActive(false);
        _currentMenu = menu;
        _currentMenu.SetActive(true);
    }

    public void goBack()
    {
        _currentMenu.SetActive(false);
        _currentMenu = _previousMenu;
        _previousMenu = null;
        _currentMenu.SetActive(true);
    }

    public void quit()
    {
        Application.Quit();
    }

    public void replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void play()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    #endregion
}
