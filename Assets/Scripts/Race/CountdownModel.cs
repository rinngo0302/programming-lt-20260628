using R3;

// レース開始時に3,2,1,GO!を1秒間隔で表示し、終了するとIsCountingDownがfalseになる
// (docs/spec/01-scenes.md)。
public sealed class CountdownModel
{
    readonly ReactiveProperty<string> _displayText;
    readonly ReactiveProperty<bool> _isCountingDown = new(true);
    int _secondsRemaining;
    float _timer;

    public ReadOnlyReactiveProperty<string> DisplayText => _displayText;
    public ReadOnlyReactiveProperty<bool> IsCountingDown => _isCountingDown;

    public CountdownModel(int startSeconds = 3)
    {
        _secondsRemaining = startSeconds;
        _displayText = new ReactiveProperty<string>(startSeconds.ToString());
    }

    public void Tick(float deltaTime)
    {
        if (!_isCountingDown.CurrentValue)
        {
            return;
        }

        _timer += deltaTime;
        if (_timer < 1f)
        {
            return;
        }

        _timer -= 1f;
        _secondsRemaining--;

        if (_secondsRemaining > 0)
        {
            _displayText.Value = _secondsRemaining.ToString();
        }
        else if (_secondsRemaining == 0)
        {
            _displayText.Value = "GO!";
        }
        else
        {
            _isCountingDown.Value = false;
            _displayText.Value = "";
        }
    }
}
