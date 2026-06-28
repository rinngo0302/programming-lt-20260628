using R3;
using TMPro;
using UnityEngine;

public class CountdownPresenter : MonoBehaviour
{
    [SerializeField, Tooltip("カウントダウン表示テキスト")]
    TextMeshProUGUI _countdownText;

    [SerializeField, Tooltip("カウントダウン中操作を無効化する全レーサーのKartController")]
    KartController[] _kartControllers;

    CountdownModel _model;

    void Awake()
    {
        _model = new CountdownModel();
        _model.DisplayText.Subscribe(text => _countdownText.text = text).AddTo(this);
        _model
            .IsCountingDown.Subscribe(isCountingDown =>
            {
                foreach (KartController kart in _kartControllers)
                {
                    kart.InputEnabled = !isCountingDown;
                }
            })
            .AddTo(this);
    }

    void Update()
    {
        _model.Tick(Time.deltaTime);
    }
}
