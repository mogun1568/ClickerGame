using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    public Action BackButton = null;

    public void Init()
    {
        BackButton -= OnBackButtonHandler;
        BackButton += OnBackButtonHandler;
    }

    public void OnUpdate()
    {
        //if (EventSystem.current.IsPointerOverGameObject())
        //{
        //    return;
        //}

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            BackButton.Invoke();
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

    public void Clear()
    {
        BackButton -= OnBackButtonHandler;
        BackButton = null;
    }
}
