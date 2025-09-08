using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SkinItem : UI_Base
{
    enum Buttons
    {
        Button_Purchase
    }

    enum Images
    {
        Icon_Item
    }

    enum Texts
    {
        Text_ItemName,
        Text_ItemInfo,
        Text_Purchase
    }

    private Define.ClassType _classType;
    private string _goName;
    private bool isOwned;

    // Start
    void Awake()
    {
        Init();
    }

    public override void Init()
    {
        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));
        Bind<TextMeshProUGUI>(typeof(Texts));

        _goName = gameObject.name;

        // Bind�� Button���� �߱� ������ GetObject�� �ȵ�
        BindEvent(GetButton((int)Buttons.Button_Purchase).gameObject, (PointerEventData data) => { PurchaseItem(); }, Define.UIEvent.Click, false);

        DataInit().Forget();
    }

    // ���� �����͵� ScriptableObject�� �����ұ� ��� ��
    private async UniTask DataInit()
    {
        Dictionary<string, ShopItemData> SkinItemDict = Managers.Resource.SkinItemDict;

        _classType = SkinItemDict[_goName].classType;

        Image icon = GetImage((int)Images.Icon_Item);
        icon.sprite = Managers.Resource.Load<Sprite>($"Icon/{SkinItemDict[_goName].shopItemIcon}");

        GetText((int)Texts.Text_ItemName).text = SkinItemDict[_goName].shopItemName;
        GetText((int)Texts.Text_ItemInfo).text = SkinItemDict[_goName].shopItemInfo;

        await UniTask.WaitUntil(() => Managers.Data.GameDataReady);

        if (Managers.Data.HasSkin(_classType, _goName))
        {
            if (_goName == Managers.Data.MyPlayerInfo.Skin)
                GetText((int)Texts.Text_Purchase).text = "���� ��Ų";
            else
                GetText((int)Texts.Text_Purchase).text = "����";

            isOwned = true;
        }
        else
        {
            if (SkinItemDict[_goName].shopItemPrice != 0)
                GetText((int)Texts.Text_Purchase).text = $"����\n{SkinItemDict[_goName].shopItemPrice.ToString("N0")}";
            else
                GetText((int)Texts.Text_Purchase).text = "����";
        }
    }

    private void PurchaseItem()
    {
        if (_goName == Managers.Data.MyPlayerInfo.Skin)
        {
            Managers.UI.ToastMessage.Show("�̹� ���� ���Դϴ�.");
            return;
        }

        if (isOwned)
        {
            ChangeSkin();
            return;
        }

        if (Managers.Resource.SkinItemDict[_goName].shopItemPrice > Managers.Game.MyPlayer.StatInfo.Coin)
        {
            Managers.UI.ToastMessage.Show("��尡 �����մϴ�.");
            return;
        }

        if (Managers.Resource.SkinItemDict[_goName].shopItemPrice == 0)
            return;

        Managers.Game.MyPlayer.StatInfo.Coin -= Managers.Resource.SkinItemDict[_goName].shopItemPrice;
        //Managers.Sound.Play("SFX_Cash_Register_Buy_Click_1", Define.Sound.SFX);
        ChangeSkin();
    }

    private void ChangeSkin()
    {
        Managers.Data.MyPlayerInfo.Skin = _goName;
        Managers.Data.AddSkin(_classType, _goName);
        Managers.Scene.LoadScene(Define.Scene.GamePlay);
    }
}
