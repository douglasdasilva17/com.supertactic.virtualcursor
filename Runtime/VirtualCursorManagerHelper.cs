using UnityEngine;

namespace Supertactic.VirtualCursor
{
    /// <summary>
    /// This script allows the VirtualCursorHoverHandler to work without 
    /// requiring a direct dependency on the VirtualCursorManager.
    /// </summary>

    public class VirtualCursorManagerHelper : MonoBehaviour
    {
        private VirtualCursorManager _virtualCursorManager;

        private void Awake()
        {
            _virtualCursorManager = FindObjectOfType<VirtualCursorManager>();
        }

        public void UpdateCursor(bool value)
        {
            if (_virtualCursorManager != null)
            {
                _virtualCursorManager.UpdateCursor(value);
            }
        }

        public void SwitchCursor(string cursorType)
        {
            if (_virtualCursorManager != null)
            {
                _virtualCursorManager.SwitchCursor(cursorType);
            }
        }

        public void DeselectCursor()
        {
            if (_virtualCursorManager != null)
            {
                _virtualCursorManager.DeselectCursor();
            }
        }
    }
}
