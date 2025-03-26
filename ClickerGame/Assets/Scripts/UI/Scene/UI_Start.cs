using Cysharp.Threading.Tasks;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Start : UI_Scene
{
    enum Buttons
    {
        Button_Google,
        Button_Gest
    }

    enum GameObjects
    {
        Group_Login,
        Welcome
    }

    private void Awake()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));

        // Bind�� Button���� �߱� ������ GetObject�� �ȵ�
        BindEvent(GetButton((int)Buttons.Button_Google).gameObject, (PointerEventData data) => { LoginGoogle(); }, Define.UIEvent.Click);
        BindEvent(GetButton((int)Buttons.Button_Gest).gameObject, (PointerEventData data) => { LoginGest(); }, Define.UIEvent.Click);
        // �ӽ�
        BindEvent(GetObject((int)GameObjects.Welcome), (PointerEventData data) => { LoginGest(); }, Define.UIEvent.Click);

        GetObject((int)GameObjects.Group_Login).SetActive(false);
        GetObject((int)GameObjects.Welcome).SetActive(false);

        UIInitAsync().Forget();
    }

    private void LoginGoogle()
    {
        Managers.Firebase.OnSignIn();
    }

    private void LoginGest()
    {
        Managers.Scene.LoadScene(Define.Scene.GamePlay);
    }

    // ȭ�� ��ġ

    private async UniTask UIInitAsync()
    {
        await UniTask.WaitUntil(() => Managers.Firebase.CheckFirebaseDone);

        if (Managers.Firebase.IsLogIn)
            GetObject((int)GameObjects.Welcome).SetActive(true);
        else
        {
            if (Managers.Data.HasLocalData())
                GetObject((int)GameObjects.Welcome).SetActive(true);
            else
                GetObject((int)GameObjects.Group_Login).SetActive(true);
        }
    }
}
