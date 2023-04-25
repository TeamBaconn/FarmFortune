
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class SettingRenderer : MonoBehaviour
{
    public Button mainMenuButton;

    public void Start()
    {
        mainMenuButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene(0);
        });
    }
}
