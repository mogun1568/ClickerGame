using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager
{
    public Action BackButton = null;
    //public Action TouchAction = null;

    public void Init()
    {
        BackButton -= OnBackButtonHandler;
        BackButton += OnBackButtonHandler;

        //TouchAction -= OnTouchHandler;
        //TouchAction += OnTouchHandler;
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
    }

    private void OnBackButtonHandler()
    {
        if (Managers.UI.IsPopupActive())
        {
            GameObject go = Managers.UI.getPopStackTop();
            if (go == null)
                return;

            if (go.GetComponent<UI_Offline>() != null)
                return;
            if (go.GetComponent<UI_Resurrection>() != null)
                return;

            Managers.UI.ClosePopupUI();
        }
        else
        {
            Managers.UI.ShowPopupUI<UI_GameQuit>("Popup_GameQuit");
        }
    }

    private void OnTouchHandler()
    {
        Logging.Log("È­¸é ÅÍÄ¡µÊ!");
    }
}
