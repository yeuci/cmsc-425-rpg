using UnityEngine;

public class CursorManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private Texture2D cursorTexture;
    private Vector2 cursorHotspot;
    void Start()
    {
        cursorHotspot = new Vector2(0, 0);
        Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.Auto);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
