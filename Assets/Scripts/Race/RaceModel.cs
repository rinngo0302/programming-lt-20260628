using System.Collections.Generic;
using R3;

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

    public RaceModel(int racerCount)
    {
        _racers = new RacerState[racerCount];
        for (int i = 0; i < racerCount; i++)
        {
            _racers[i] = new RacerState();
        }
    }

    public IReadOnlyList<RacerState> Racers => _racers;

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
}
