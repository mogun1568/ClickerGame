using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    public Action BackButton = null;
    public Action TouchAction = null;

    public void Init()
    {
        BackButton -= OnBackButtonHandler;
        BackButton += OnBackButtonHandler;

        TouchAction -= OnTouchHandler;
        TouchAction += OnTouchHandler;
    }

    public void OnUpdate()
    {
        //if (EventSystem.current.IsPointerOverGameObject())
        //{
        //    return;
        //}

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackButton?.Invoke();
        }

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (Managers.Scene.CurrentScene.SceneType == Define.Scene.Login)
                TouchAction?.Invoke();
        }
    }

    private void OnBackButtonHandler()
    {
        if (Managers.UI.IsPopupActive())
        {
            Managers.UI.ClosePopupUI();
        }
        else
        {
            Managers.UI.ShowPopupUI<UI_GameQuit>("Popup_GameQuit");
        }
    }

    private void OnTouchHandler()
    {
        Debug.Log("È­¸é ÅÍÄ¡µÊ!");
        Managers.Scene.LoadScene(Define.Scene.GamePlay);
    }

    public void Clear()
    {
        BackButton -= OnBackButtonHandler;
        BackButton = null;

        TouchAction -= OnTouchHandler;
        TouchAction = null;
    }
}
