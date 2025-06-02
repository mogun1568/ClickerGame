using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Sound : UI_Popup
{
    enum Buttons
    {
        Button_MasterSound,
        Button_BGMSound,
        Button_SFXSound,
        Button_Close
    }

    enum Images
    {
        Button_MasterSound,
        Button_BGMSound,
        Button_SFXSound
    }

    private Sprite soundOnSprite;
    private Sprite soundOffSprite;

    void Awake()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));

        // Bind를 Button을로 했기 때문에 GetObject로 안됨
        BindEvent(GetButton((int)Buttons.Button_MasterSound).gameObject, (PointerEventData data) => { ToggleMaster(); }, Define.UIEvent.Click);
        BindEvent(GetButton((int)Buttons.Button_BGMSound).gameObject, (PointerEventData data) => { ToggleBGM(); }, Define.UIEvent.Click);
        BindEvent(GetButton((int)Buttons.Button_SFXSound).gameObject, (PointerEventData data) => { ToggleSFX(); }, Define.UIEvent.Click);
        BindEvent(GetButton((int)Buttons.Button_Close).gameObject, (PointerEventData data) => { ClosePopupUI(); }, Define.UIEvent.Click);

        soundOnSprite = Managers.Resource.Load<Sprite>($"Icon/Icon_PictoIcon_Sound_on");
        soundOffSprite = Managers.Resource.Load<Sprite>($"Icon/Icon_PictoIcon_Sound_off");
    }

    private void ToggleMaster()
    {

    }

    private void ToggleBGM()
    {

    }

    private void ToggleSFX()
    {

    }
}
