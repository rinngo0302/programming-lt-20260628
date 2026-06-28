using R3;
using TMPro;
using UnityEngine;

public class RaceHudPresenter : MonoBehaviour
{
    [SerializeField, Tooltip("順位表示テキスト")]
    TextMeshProUGUI _rankText;

    [SerializeField, Tooltip("ラップ数表示テキスト")]
    TextMeshProUGUI _lapText;

    [SerializeField, Tooltip("総ラップ数")]
    int _totalLaps = 3;

    public void Bind(RaceModel.RacerOutput output)
    {
        output.Rank.Subscribe(rank => _rankText.text = $"{rank}位").AddTo(this);
        output
            .Lap.Subscribe(lap => _lapText.text = $"{Mathf.Min(lap + 1, _totalLaps)}/{_totalLaps}")
            .AddTo(this);
    }
}
