using UnityEngine;
using UnityEngine.UI;

public class TitleScenePresenter : MonoBehaviour
{
    [SerializeField, Tooltip("ゲームシーンへ遷移するボタン")]
    Button _startButton;

    void Awake()
    {
        _startButton.onClick.AddListener(SceneLoader.LoadGame);
    }
}
