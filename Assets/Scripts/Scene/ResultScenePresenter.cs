using UnityEngine;
using UnityEngine.UI;

public class ResultScenePresenter : MonoBehaviour
{
    [SerializeField, Tooltip("もう一度プレイするボタン(ゲームシーンへ遷移)")]
    Button _retryButton;

    [SerializeField, Tooltip("タイトルへ戻るボタン")]
    Button _backToTitleButton;

    void Awake()
    {
        _retryButton.onClick.AddListener(SceneLoader.LoadGame);
        _backToTitleButton.onClick.AddListener(SceneLoader.LoadTitle);
    }
}
