using System.Linq;
using TMPro;
using UnityEngine;

public class ResultRankingPresenter : MonoBehaviour
{
    [SerializeField, Tooltip("1〜4位を表示するテキスト(配列順=表示順)")]
    TextMeshProUGUI[] _rankingTexts;

    [SerializeField, Tooltip("プレイヤーの強調表示色")]
    Color _playerColor = Color.yellow;

    [SerializeField, Tooltip("通常表示色")]
    Color _normalColor = Color.white;

    void Awake()
    {
        RaceResultData.RacerResult[] results = RaceResultData.FinalResults;
        if (results == null)
        {
            return;
        }

        RaceResultData.RacerResult[] sorted = results.OrderBy(result => result.Rank).ToArray();
        for (int i = 0; i < _rankingTexts.Length && i < sorted.Length; i++)
        {
            _rankingTexts[i].text = $"{sorted[i].Rank}位 {sorted[i].Name}";
            _rankingTexts[i].color = sorted[i].IsPlayer ? _playerColor : _normalColor;
        }
    }
}
