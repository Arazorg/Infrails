using UnityEngine;

public class LootboxUI : MonoBehaviour
{
    public void ShowLoot()
    {
        GetComponentInParent<ShopLootPanelUI>().ShowLoot();
    }
}
