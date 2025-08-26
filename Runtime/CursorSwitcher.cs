using UnityEngine;

namespace Supertactic.VirtualCursor 
{
public class CursorSwitcher : MonoBehaviour
    {
        public static CursorSwitcher Instance;

        [SerializeField] private Texture2D cursorTextureNormal;
        [SerializeField] private Texture2D cursorTextureHover;

        private bool isCursorVisible = false;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public void ChangeCursor(string type)
        {
            switch (type)
            {
                case "Normal":
                    Vector2 normalHotspot = new Vector2(cursorTextureNormal.width / 2, cursorTextureNormal.height / 2);
                    Cursor.SetCursor(cursorTextureNormal, normalHotspot, CursorMode.Auto);
                    break;

                case "Hover":
                    Vector2 hoverHotspot = new Vector2(cursorTextureHover.width / 2, cursorTextureHover.height / 2);
                    Cursor.SetCursor(cursorTextureHover, hoverHotspot, CursorMode.Auto);
                    break;
            }
        }

        public void ToggleCursor(string state)
        {
            switch (state)
            {
                case "Show":
                    isCursorVisible = true;
                    UpdateCursorState();
                    break;
                case "Hide":
                    isCursorVisible = false;
                    UpdateCursorState();
                    break;
                case "ShowHide":
                    isCursorVisible = !isCursorVisible;
                    UpdateCursorState();
                    break;
            }
        }

        private void UpdateCursorState()
        {
            Cursor.visible = isCursorVisible;
            Cursor.lockState = isCursorVisible ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }
}