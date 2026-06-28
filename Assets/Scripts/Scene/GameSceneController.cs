using System.Linq;
using R3;
using UnityEngine;

// ゲームシーン全体のレース進行を司令する。RaceModelの生成、チェックポイント通過の
// 検知、順位再計算、HUDのBind、レース終了時のリザルトシーン遷移を行う。
public class GameSceneController : MonoBehaviour
{
    [SerializeField, Tooltip("コースデータ")]
    CourseData _courseData;

    [SerializeField, Tooltip("プレイヤーのKartController")]
    KartController _playerKart;

    [SerializeField, Tooltip("CPUのKartController(順位確定時の表示名はCPU1,2,3...)")]
    KartController[] _cpuKarts;

    [SerializeField, Tooltip("通過順に並べたチェックポイント")]
    Checkpoint[] _checkpoints;

    [SerializeField, Tooltip("順位/ラップ表示Presenter")]
    RaceHudPresenter _raceHudPresenter;

    [SerializeField, Tooltip("所持アイテム表示Presenter")]
    ItemDisplayPresenter _itemDisplayPresenter;

    [SerializeField, Tooltip("総ラップ数")]
    int _lapsToWin = 3;

    RaceModel _raceModel;
    KartController[] _allKarts;

    void Awake()
    {
        _allKarts = new[] { _playerKart }.Concat(_cpuKarts).ToArray();
        _raceModel = new RaceModel(
            _allKarts.Length,
            _checkpoints.Length,
            _lapsToWin,
            playerRacerIndex: 0
        );

        foreach (Checkpoint checkpoint in _checkpoints)
        {
            checkpoint
                .Passed.Subscribe(collider => OnCheckpointPassed(checkpoint.Index, collider))
                .AddTo(this);
        }

        _raceHudPresenter.Bind(_raceModel.CreateRacerOutput(0));
        _itemDisplayPresenter.Bind(_playerKart.ItemHolder.HeldItem);
        _raceModel.RaceEnded.Subscribe(_ => OnRaceEnded()).AddTo(this);
    }

    void Update()
    {
        Vector3[] positions = _allKarts.Select(kart => kart.transform.position).ToArray();
        _raceModel.RecalculateRanks(positions, _courseData.Checkpoints);
    }

    void OnCheckpointPassed(int checkpointIndex, Collider other)
    {
        KartController kart = other.GetComponent<KartController>();
        if (kart == null)
        {
            return;
        }

        int racerIndex = System.Array.IndexOf(_allKarts, kart);
        if (racerIndex < 0)
        {
            return;
        }

        _raceModel.NotifyCheckpointPassed(racerIndex, checkpointIndex);
    }

    void OnRaceEnded()
    {
        RaceResultData.RacerResult[] results = new RaceResultData.RacerResult[_allKarts.Length];
        for (int i = 0; i < _allKarts.Length; i++)
        {
            string name = i == 0 ? "プレイヤー" : $"CPU{i}";
            results[i] = new RaceResultData.RacerResult(
                name,
                _raceModel.Racers[i].Rank.CurrentValue,
                i == 0
            );
        }

        RaceResultData.SetFinalResults(results);
        SceneLoader.LoadResult();
    }
}
