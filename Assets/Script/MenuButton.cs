using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour
{
    public Button Menubutton;

    void Start()
    {
        Menubutton.onClick.AddListener(Menu);
    }
    private void Menu()
    {
        SceneManager.LoadScene("MainMenu");
    }



}
