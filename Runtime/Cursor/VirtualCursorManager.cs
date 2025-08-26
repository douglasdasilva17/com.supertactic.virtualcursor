using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem.Users;

namespace Supertactic.VirtualCursor
{
    public class VirtualCursorManager : MonoBehaviour
    {
        public static VirtualCursorManager current;

        [Header("Cursor Settings")]
        [SerializeField] private RectTransform cursorTransform;
        [SerializeField] private float cursorSpeed = 1000f;
        [SerializeField] private Animator cursorAnimator;
        [SerializeField] private bool hideCursor;
        [SerializeField] private float padding = 50f;
        private bool previousMouseState;
        private Mouse virtualMouse;
        private Mouse currentMouse;
        
        [Header("Cursor Texture")]
        [SerializeField] private Texture2D cursorTextureNormal;
        [SerializeField] private Texture2D cursorTextureHover;

        [Header("Canvas Settings")]
        [SerializeField] private Canvas canvas;
        [SerializeField] private RectTransform canvasRectTransform;

        [Header("Input Settings")]
        [SerializeField] private PlayerInput playerInput;

        private Camera mainCamera;

        [Header("Click Settings")]
        [SerializeField] private ParticleSystem clickParticle;
        [SerializeField] private Vector3 virtualCursorPositionOffset = Vector3.zero;
        [SerializeField] private Vector3 cursorPositionOffset = Vector3.zero;

        [Header("Control Schema")]
        public UnityEvent<string> onControlSchemeChanged;
        private const string gamepadScheme = "Gamepad";
        private const string mouseScheme = "Keyboard&Mouse";
        private string previousControlScheme = "";

        private void Awake()
        {
            current = this;
        }

        private void OnEnable()
        {
            HideCursor(hideCursor);

            mainCamera = Camera.main;
            currentMouse = Mouse.current;

            UpdateCursor(true);

            if (virtualMouse == null)
            {
                virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
            }
            else if (!virtualMouse.added)
            {
                InputSystem.AddDevice(virtualMouse);
            }

            InputUser.PerformPairingWithDevice(virtualMouse, playerInput.user);

            if (cursorTransform != null)
            {
                Vector2 cursorPosition = cursorTransform.anchoredPosition;
                InputState.Change(virtualMouse, cursorPosition);
            }

            InputSystem.onAfterUpdate += UpdateMotion;
        }

        private void OnDisable()
        {
            if (virtualMouse != null && virtualMouse.added) InputSystem.RemoveDevice(virtualMouse);
            InputSystem.onAfterUpdate -= UpdateMotion;
        }

        private void Update()
        {
            DetectControlScheme();

            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mouseScreenPos = Input.mousePosition;
                mouseScreenPos.z = 10f; // Distance from the camera to world plane
                Vector3 worldPos = Camera.main.ScreenToWorldPoint(mouseScreenPos);

                // Apply position offset
                worldPos += cursorPositionOffset;

                InstantiateAndPlay(worldPos, Quaternion.identity);
            }
        }


        public void InstantiateAndPlay(Vector3 position, Quaternion rotation, float destroyDelay = 1)
        {
            if (clickParticle == null)
            {
                return;
            }

            ParticleSystem ps = Instantiate(clickParticle, position, rotation) as ParticleSystem;
            ps.Play();
            Destroy(ps.gameObject, destroyDelay);
        }

        private void DetectControlScheme()
        {
            // Detect keyboard/mouse movement
            bool mouseMoved = currentMouse.delta.ReadValue().sqrMagnitude > 0.01f;
            bool keyPressed = Keyboard.current.anyKey.wasPressedThisFrame;

            // Detect gamepad movement
            bool gamepadMoved = Gamepad.current != null && Gamepad.current.leftStick.ReadValue().sqrMagnitude > 0.01f;

            if ((mouseMoved || keyPressed) && previousControlScheme != mouseScheme)
            {
                SwitchToMouseScheme();
            }
            else if (gamepadMoved && previousControlScheme != gamepadScheme)
            {
                SwitchToGamepadScheme();
            }
        }

        public static Vector2 GetVirtualMousePosition()
        {
            if (current.virtualMouse != null)
            {
                return current.virtualMouse.position.ReadValue();
            }
            return Vector2.zero;
        }

        private void HideCursor(bool cursorHidden)
        {
            if (cursorHidden)
            {
                Cursor.visible = false;
                //  Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                Cursor.visible = true;
                // Cursor.lockState = CursorLockMode.None;            
            }
        }

        private void SwitchToMouseScheme()
        {
            cursorTransform.gameObject.SetActive(false);
            HideCursor(false);
            currentMouse.WarpCursorPosition(virtualMouse.position.ReadValue());
            previousControlScheme = mouseScheme;
            SwitchCursor("Normal");
            onControlSchemeChanged?.Invoke(mouseScheme);
        }

        private void SwitchToGamepadScheme()
        {
            cursorTransform.gameObject.SetActive(true);
            HideCursor(true);
            InputState.Change(virtualMouse.position, currentMouse.position.ReadValue());
            AnchorCursor(currentMouse.position.ReadValue());
            previousControlScheme = gamepadScheme;
            onControlSchemeChanged?.Invoke(gamepadScheme);
        }

        public void SwitchCursor(string cursor)
        {
            switch (cursor)
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

        private void UpdateMotion()
        {
            if (virtualMouse == null || Gamepad.current == null) return;

            Vector2 deltaValue = Gamepad.current.leftStick.ReadValue() * cursorSpeed * Time.deltaTime;
            Vector2 currentPosition = virtualMouse.position.ReadValue();
            Vector2 newPosition = currentPosition + deltaValue;

            newPosition.x = Mathf.Clamp(newPosition.x, padding, Screen.width - padding);
            newPosition.y = Mathf.Clamp(newPosition.y, padding, Screen.height - padding);

            InputState.Change(virtualMouse.position, newPosition);
            InputState.Change(virtualMouse.delta, deltaValue);

            bool aButtonIsPressed = Gamepad.current.aButton.IsPressed();
            if (previousMouseState != aButtonIsPressed)
            {
                virtualMouse.CopyState<MouseState>(out var mouseState);
                mouseState.WithButton(MouseButton.Left, aButtonIsPressed);
                InputState.Change(virtualMouse, mouseState);
                previousMouseState = aButtonIsPressed;
            }

            AnchorCursor(newPosition);
        }

        private void AnchorCursor(Vector2 newPosition)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasRectTransform,
                newPosition,
                canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : mainCamera,
                out Vector2 anchoredPosition
            );

            cursorTransform.anchoredPosition = anchoredPosition;
        }

        public void UpdateCursor(bool value)
        {
            cursorAnimator.SetBool("Fade", value);
        }

        public void DeselectCursor()
        {
            GameObject myEventSystem = GameObject.Find("EventSystem");
            myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(null);
        }
    }
}