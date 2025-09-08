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
        // UI �ڵ鷯�� �̺�Ʈ�� Ž���ؼ� �۵��ϴ� ������� Button ������Ʈ�� �������
        BindEvent(gameObject, (PointerEventData data) => { ClickTab(); }, Define.UIEvent.Click);

        // TabGroup�� TabUI���� ���� Init�Ǽ� ����� ���� ������ �Ʒ��� �ӽ�
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
