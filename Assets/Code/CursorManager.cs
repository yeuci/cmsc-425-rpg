using UnityEngine;

public class CursorManager : MonoBehaviour
{
    [SerializeField] private Texture2D cursorTexture;

    void Start()
    {
        if (cursorTexture != null)
        {
            Vector2 hotspot = new Vector2(0, 0);
            Cursor.SetCursor(cursorTexture, hotspot, CursorMode.Auto);
        }
    }
}
