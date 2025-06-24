using System.Collections;
using TMPro;
using UnityEngine;

public class UI_ToastMessage : UI_Base
{
    enum Texts
    {
        ToastMessage
    }

    private TextMeshProUGUI _toastMessage;
    [HideInInspector]
    public CanvasGroup _canvasGroup;    // 투명도 위함
    private Coroutine _fadeCoroutine;

    void Awake()
    {
        Init();
    }

    public override void Init()
    {
        Bind<TextMeshProUGUI>(typeof(Texts));

        _toastMessage = GetText((int)Texts.ToastMessage);
        _canvasGroup = GetComponent<CanvasGroup>();
        _canvasGroup.alpha = 0f;
    }

    public void Show(string msg)
    {
        _toastMessage.text = msg;

        if (_fadeCoroutine != null)
            StopCoroutine(_fadeCoroutine);

        _fadeCoroutine = StartCoroutine(FadeInOut());
    }

    // 페이드 인/아웃 코루틴
    IEnumerator FadeInOut()
    {
        _canvasGroup.alpha = 1f;
        yield return new WaitForSeconds(2f);
        _canvasGroup.alpha = 0f;

        _fadeCoroutine = null;
    }
}
