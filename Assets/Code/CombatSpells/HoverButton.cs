using UnityEngine;
using UnityEngine.EventSystems;

public class HoverButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Item item;

    public void OnPointerEnter(PointerEventData eventData)
    {
        BattleManager.instance.displayItemInformation(item.name, item.getItemDescription(), transform.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        BattleManager.instance.DestroyItemInfo();
    }
}
