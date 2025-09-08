using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TabUI : UI_Base
{
    enum GameObjects
    {
        Tab_Focus
    }

    private TabGroup _tabGroup;
    private TextMeshProUGUI _text;
    private Color _open, _close;

    void Awake()
    {
        Init();
    }

    public override void Init()
    {
        _tabGroup = GetComponentInParent<TabGroup>();
        _text = GetComponent<TextMeshProUGUI>();
        _open = Color.white;
        _close = new Color32(74, 127, 247, 255);

        Bind<GameObject>(typeof(GameObjects));
        // UI 핸들러로 이벤트를 탐지해서 작동하는 방식으로 Button 컴포넌트와 상관없음
        BindEvent(gameObject, (PointerEventData data) => { ClickTab(); }, Define.UIEvent.Click);

        // TabGroup이 TabUI보다 먼저 Init되서 생기는 문제 때문에 아래로 임시
        if (gameObject.name != "TabMenu_Stat")
            CloseTab();
    }

    private void ClickTab()
    {
        if (_tabGroup._curTabMenu == this)
            return;

        _tabGroup.SelectTab(gameObject.name);
        _tabGroup._curTabMenu = this;
        _text.color = _open;
        GetObject((int)GameObjects.Tab_Focus).SetActive(true);
    }

    public void CloseTab()
    {
        _text.color = _close;
        GetObject((int)GameObjects.Tab_Focus).SetActive(false);
    }
}
