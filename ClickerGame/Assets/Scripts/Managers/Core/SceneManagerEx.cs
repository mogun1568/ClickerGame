using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerEx
{
    public BaseScene CurrentScene { get { return GameObject.FindFirstObjectByType<BaseScene>(); } }

    public UI_Loading loading;

    public void Init()
    {
        loading = Managers.Resource.Instantiate("UI/Popup/Popup_Loading").GetComponent<UI_Loading>();
        loading.Hide();
    }

    public void LoadScene(Define.Scene type)
    {
        Managers.Clear();

        loading.Show();

        SceneManager.LoadScene(GetSceneName(type));
    }

    string GetSceneName(Define.Scene type)
    {
        string name = System.Enum.GetName(typeof(Define.Scene), type);
        return name;
    }

    public void Clear()
    {
        CurrentScene.Clear();
    }
}
