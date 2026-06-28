using System;
using System.Collections.Generic;
using R3;
using UnityEngine;

public sealed class RaceModel
{
    public sealed class RacerState
    {
        readonly ReactiveProperty<int> _lap = new(0);
        readonly ReactiveProperty<int> _lastPassedCheckpointIndex = new(-1);
        readonly ReactiveProperty<int> _rank = new(0);

        public ReadOnlyReactiveProperty<int> Lap => _lap;
        public ReadOnlyReactiveProperty<int> LastPassedCheckpointIndex =>
            _lastPassedCheckpointIndex;
        public ReadOnlyReactiveProperty<int> Rank => _rank;
        public bool HasFinished { get; internal set; }
        public int FinishOrder { get; internal set; } = -1;

        internal void SetLap(int value)
        {
            _lap.Value = value;
        }

        internal void SetLastPassedCheckpointIndex(int value)
        {
            _lastPassedCheckpointIndex.Value = value;
        }

        internal void SetRank(int value)
        {
            _rank.Value = value;
        }
    }

    public struct RacerOutput
    {
        public ReadOnlyReactiveProperty<int> Lap { get; set; }
        public ReadOnlyReactiveProperty<int> LastPassedCheckpointIndex { get; set; }
        public ReadOnlyReactiveProperty<int> Rank { get; set; }
    }

    readonly RacerState[] _racers;
    readonly int _checkpointCount;
    readonly int _lapsToWin;
    readonly int _playerRacerIndex;
    readonly Subject<Unit> _raceEnded = new();
    int _nextFinishOrder;

    public RaceModel(int racerCount, int checkpointCount, int lapsToWin, int playerRacerIndex)
    {
        _checkpointCount = checkpointCount;
        _lapsToWin = lapsToWin;
        _playerRacerIndex = playerRacerIndex;
        _racers = new RacerState[racerCount];
        for (int i = 0; i < racerCount; i++)
        {
            _racers[i] = new RacerState();
        }
    }

    public IReadOnlyList<RacerState> Racers => _racers;
    public Observable<Unit> RaceEnded => _raceEnded;
    public bool IsRaceEnded { get; private set; }

    public RacerOutput CreateRacerOutput(int racerIndex)
    {
        RacerState racer = _racers[racerIndex];
        return new RacerOutput
        {
            Lap = racer.Lap,
            LastPassedCheckpointIndex = racer.LastPassedCheckpointIndex,
            Rank = racer.Rank,
        };
    }

    // ショートカットによる不正なラップ加算を防ぐため、通過済み番号+1以外は無視する。
    // プレイヤーゴール後は仕様上レース終了のため、以降の通過は反映しない。
    public void NotifyCheckpointPassed(int racerIndex, int checkpointIndex)
    {
        if (IsRaceEnded)
        {
            return;
        }

        RacerState racer = _racers[racerIndex];
        int expectedNextCheckpointIndex =
            (racer.LastPassedCheckpointIndex.CurrentValue + 1) % _checkpointCount;
        if (checkpointIndex != expectedNextCheckpointIndex)
        {
            return;
        }

        racer.SetLastPassedCheckpointIndex(checkpointIndex);

        bool isFinishLine = checkpointIndex == _checkpointCount - 1;
        if (!isFinishLine)
        {
            return;
        }

        racer.SetLap(racer.Lap.CurrentValue + 1);

        if (!racer.HasFinished && racer.Lap.CurrentValue >= _lapsToWin)
        {
            racer.HasFinished = true;
            racer.FinishOrder = _nextFinishOrder++;

            if (racerIndex == _playerRacerIndex)
            {
                IsRaceEnded = true;
                _raceEnded.OnNext(Unit.Default);
            }
        }
    }

    // 順位の優先順位: 1.ゴール済み(早い順) 2.ラップ数 3.直近チェックポイント番号
    // 4.次チェックポイントまでの距離(近い方が上位)。docs/spec/02-race-rules.md参照。
    // レース終了後は未ゴールレーサーの順位もその時点で確定させるため再計算しない。
    public void RecalculateRanks(Vector3[] racerPositions, Vector3[] checkpointPositions)
    {
        if (IsRaceEnded)
        {
            return;
        }

        int racerCount = _racers.Length;
        int[] order = new int[racerCount];
        for (int i = 0; i < racerCount; i++)
        {
            order[i] = i;
        }

        Array.Sort(order, (a, b) => CompareRacers(a, b, racerPositions, checkpointPositions));

        for (int i = 0; i < racerCount; i++)
        {
            _racers[order[i]].SetRank(i + 1);
        }
    }

    int CompareRacers(int a, int b, Vector3[] racerPositions, Vector3[] checkpointPositions)
    {
        RacerState racerA = _racers[a];
        RacerState racerB = _racers[b];

        if (racerA.HasFinished != racerB.HasFinished)
        {
            return racerA.HasFinished ? -1 : 1;
        }

        if (racerA.HasFinished)
        {
            return racerA.FinishOrder.CompareTo(racerB.FinishOrder);
        }

        int lapCompare = racerB.Lap.CurrentValue.CompareTo(racerA.Lap.CurrentValue);
        if (lapCompare != 0)
        {
            return lapCompare;
        }

        int checkpointCompare = racerB.LastPassedCheckpointIndex.CurrentValue.CompareTo(
            racerA.LastPassedCheckpointIndex.CurrentValue
        );
        if (checkpointCompare != 0)
        {
            return checkpointCompare;
        }

        float distanceA = DistanceToNextCheckpoint(racerA, racerPositions[a], checkpointPositions);
        float distanceB = DistanceToNextCheckpoint(racerB, racerPositions[b], checkpointPositions);
        return distanceA.CompareTo(distanceB);
    }

    float DistanceToNextCheckpoint(
        RacerState racer,
        Vector3 racerPosition,
        Vector3[] checkpointPositions
    )
    {
        int nextCheckpointIndex =
            (racer.LastPassedCheckpointIndex.CurrentValue + 1) % checkpointPositions.Length;
        return Vector3.Distance(racerPosition, checkpointPositions[nextCheckpointIndex]);
    }
}
