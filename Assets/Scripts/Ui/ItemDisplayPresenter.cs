using R3;
using TMPro;
using UnityEngine;

public class ItemDisplayPresenter : MonoBehaviour
{
    [SerializeField, Tooltip("所持アイテム表示テキスト")]
    TextMeshProUGUI _itemText;

    public void Bind(ReadOnlyReactiveProperty<ItemData> heldItem)
    {
        heldItem
            .Subscribe(item => _itemText.text = item != null ? item.Type.ToString() : "-")
            .AddTo(this);
    }
}
