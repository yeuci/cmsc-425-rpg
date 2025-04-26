using UnityEngine;

public class HoverSpellButton : MonoBehaviour
{
    public Item item;

    public void OnCursorEnter() {
        BattleManager.instance.displaySpellInformation(item.name, item.getItemDescription(), transform.position);
    }

    public void OnCursorExix() {
        BattleManager.instance.DestroyItemInfo();
    }
}
