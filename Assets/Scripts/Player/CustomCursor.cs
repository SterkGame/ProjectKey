using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomCursor : MonoBehaviour
{

    public Texture2D cursorTexture;
    private Vector2 hotSpot = new Vector2(35, 35);

    void Start()
    {
        
        Cursor.SetCursor(cursorTexture, hotSpot, CursorMode.Auto);
    }

    void OnDestroy()
    {
        
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
