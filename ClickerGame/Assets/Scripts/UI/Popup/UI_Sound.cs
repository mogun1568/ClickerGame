using UnityEngine;
using UnityEngine.Audio;
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

    private AudioMixer _audioMixer;
    private Sprite _soundOnSprite;
    private Sprite _soundOffSprite;

    void Awake()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));

        _audioMixer = Managers.Sound.audioMixer;

        // Bind를 Button을로 했기 때문에 GetObject로 안됨
        BindEvent(GetButton((int)Buttons.Button_MasterSound).gameObject, (PointerEventData data) => { ToggleSound("Master", Images.Button_MasterSound); }, Define.UIEvent.Click);
        BindEvent(GetButton((int)Buttons.Button_BGMSound).gameObject, (PointerEventData data) => { ToggleSound("BGM", Images.Button_BGMSound); }, Define.UIEvent.Click);
        BindEvent(GetButton((int)Buttons.Button_SFXSound).gameObject, (PointerEventData data) => { ToggleSound("SFX", Images.Button_SFXSound); }, Define.UIEvent.Click);
        BindEvent(GetButton((int)Buttons.Button_Close).gameObject, (PointerEventData data) => { ClosePopupUI(); }, Define.UIEvent.Click);

        _soundOnSprite = Managers.Resource.Load<Sprite>($"Icon/Icon_PictoIcon_Sound_on");
        _soundOffSprite = Managers.Resource.Load<Sprite>($"Icon/Icon_PictoIcon_Sound_off");

        UpdateIcon("Master", Images.Button_MasterSound);
        UpdateIcon("BGM", Images.Button_BGMSound);
        UpdateIcon("SFX", Images.Button_SFXSound);
    }

    private void UpdateIcon(string exposedParam, Images imageEnum)
    {
        if (_audioMixer.GetFloat(exposedParam, out float volume))
        {
            GetImage((int)imageEnum).sprite = (volume < -79f) ? _soundOffSprite : _soundOnSprite;
        }
    }

    private void ToggleSound(string exposedParam, Images imageEnum)
    {
        float volume;
        _audioMixer.GetFloat(exposedParam, out volume);

        // 볼륨 조절 (켜기: 0f / 끄기: -80f)
        if (volume < -79f)
        {
            _audioMixer.SetFloat(exposedParam, 0f);
            GetImage((int)imageEnum).sprite = _soundOnSprite;
        }
        else
        {
            _audioMixer.SetFloat(exposedParam, -80f);
            GetImage((int)imageEnum).sprite = _soundOffSprite;
        }
    }
}
