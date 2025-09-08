using UnityEngine;

public class UI_Loading : MonoBehaviour
{
    public bool isShow;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void Show()
    {
        isShow = true;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        isShow = false;
        gameObject.SetActive(false);
    }
}
