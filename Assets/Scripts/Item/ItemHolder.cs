using R3;

// 各レーサーが保持できるアイテムは1つまで(docs/spec/05-items.md)。
public sealed class ItemHolder
{
    readonly ReactiveProperty<ItemData> _heldItem = new(null);

    public ReadOnlyReactiveProperty<ItemData> HeldItem => _heldItem;

    public bool TryAcquire(ItemData item)
    {
        if (_heldItem.CurrentValue != null)
        {
            return false;
        }

        _heldItem.Value = item;
        return true;
    }

    public ItemData ConsumeItem()
    {
        ItemData item = _heldItem.CurrentValue;
        _heldItem.Value = null;
        return item;
    }
}
