using UnityEngine;

public class HoverButton : MonoBehaviour
{
    public Item item;

    public void OnCursorEnter() {
        BattleManager.instance.displayItemInformation(item.name, item.getItemDescription(), transform.position);
    }

    public void OnCursorExix() {
        BattleManager.instance.DestroyItemInfo();
    }
}
