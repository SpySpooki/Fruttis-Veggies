using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayGameButton : MonoBehaviour
{
    public Button PlayGamebutton;

    void Start()
    {
        PlayGamebutton.onClick.AddListener(PlayGame);
    }
    private void PlayGame()
    {
        SceneManager.LoadScene("SampleScene");
    }

}
