using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public Button TutorialButton;

    void Start()
    {
        TutorialButton.onClick.AddListener(Tutorial);
    }
    private void Tutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
        
    

}
