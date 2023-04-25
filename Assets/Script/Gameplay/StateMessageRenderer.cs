using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StateMessageRenderer : MonoBehaviour
{
    public GameState state;
    public GameObject target;
    public Button closeButton;

    private void Start()
    {
        closeButton.onClick.AddListener(() =>
        {
            target.SetActive(false);
            CursorRenderer.Instance.SetMessageIndex(0);
        });
    }

    private void OnEnable()
    { 
        EventManager.StartListening<OnGameStateChange>(OnGameStateChange);
    }

    private void OnDisable()
    { 
        EventManager.StopListening<OnGameStateChange>(OnGameStateChange);
    }

    private void OnGameStateChange(EventParam param)
    {
        OnGameStateChange eventParam = param as OnGameStateChange;
        if (eventParam.state != state) return;
        target.SetActive(true);
        CursorRenderer.Instance.SetMessageIndex(1);
    }
}
