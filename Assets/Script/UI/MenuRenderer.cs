using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuRenderer : MonoBehaviour
{
    public Button startButton;
    public Button continueButton;
    public Button exitButton;
    void Start()
    {
        exitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
        int init = PlayerPrefs.GetInt("Init");
        if (init < 1)
        {
            Destroy(continueButton.gameObject);
        }
        else
        {
            continueButton.onClick.AddListener(() =>
            {
                SceneManager.LoadScene(1);
            });
        }
        startButton.onClick.AddListener(() =>
        {
            PlayerPrefs.SetInt("Init", 0);
            SceneManager.LoadScene(1);
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
