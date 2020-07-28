using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    // Class that gives the cursor a sprite
    public class CursorSprite : MonoBehaviour
    {
        [SerializeField] private Texture2D cursorArrow;
        [SerializeField] private float hotspotX;
        [SerializeField] private float hotspotY;
        void Start()
        {
            Vector2 offset = new Vector2(hotspotX, hotspotY);
            Cursor.SetCursor(cursorArrow, offset, CursorMode.ForceSoftware);
        }
    }
}
