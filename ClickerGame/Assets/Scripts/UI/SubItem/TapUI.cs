using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TapUI : UI_Base
{
    enum GameObjects
    {
        Tap_Focus
    }

    private TapGroup _tapGroup;
    private TextMeshProUGUI _text;
    private Color _open, _close;

    private void Start()
    {
        Init();
    }

    public override void Init()
    {
        _tapGroup = GetComponentInParent<TapGroup>();
        _text = GetComponent<TextMeshProUGUI>();
        _open = Color.white;
        _close = new Color32(74, 127, 247, 255);

        Bind<GameObject>(typeof(GameObjects));
        // UI 핸들러로 이벤트를 탐지해서 작동하는 방식으로 Button 컴포넌트와 상관없음
        BindEvent(gameObject, (PointerEventData data) => { ClickTap(); }, Define.UIEvent.Click);
    }

    private void ClickTap()
    {
        if (_tapGroup._curTapMenu == this)
            return;

        _tapGroup.SelectTap(gameObject.name);
        _tapGroup._curTapMenu = this;
        _text.color = _open;
        GetObject((int)GameObjects.Tap_Focus).SetActive(true);
    }

    public void CloseTap()
    {
        _text.color = _close;
        GetObject((int)GameObjects.Tap_Focus).SetActive(false);
    }
}
