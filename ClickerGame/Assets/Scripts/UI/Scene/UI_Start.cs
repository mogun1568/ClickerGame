using Cysharp.Threading.Tasks;
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

    void Awake()
    {
        Init();
    }

    public override void Init()
    {
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<GameObject>(typeof(GameObjects));

        // Bind를 Button을로 했기 때문에 GetObject로 안됨
        BindEvent(GetButton((int)Buttons.Button_Google).gameObject, (PointerEventData data) => { LoginGoogle(); }, Define.UIEvent.Click);
        BindEvent(GetButton((int)Buttons.Button_Gest).gameObject, (PointerEventData data) => { LoginGest(); }, Define.UIEvent.Click);
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

    // 화면 터치

    private async UniTask UIInitAsync()
    {
        await UniTask.WaitUntil(() => Managers.Firebase.CheckFirebaseDone);

        if (Managers.Firebase.GoogleLogIn)
        {
            Logging.Log("구글 로그인 상태");
            GetObject((int)GameObjects.Welcome).SetActive(true);
        }    
        else
        {
            if (Managers.Data.HasLocalData())
            {
                Logging.Log("로컬 데이터 있는 상태");
                GetObject((int)GameObjects.Welcome).SetActive(true);
            }    
            else
            {
                Logging.Log("둘 다 아닌 상태");
                GetObject((int)GameObjects.Group_Login).SetActive(true);
            }   
        }
    }
}
