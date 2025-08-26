# Supertactic Virtual Cursor

The **Supertactic Virtual Cursor** is a Unity package that provides a fully customizable **virtual cursor system** for UI navigation.  
It is designed to work seamlessly with **UGUI** and supports both **controller** and **keyboard** input, making UI interactions smooth and intuitive.  

---

## Features

- Customizable **virtual cursor** for UGUI.  
- Supports **keyboard** and **controller** navigation.  
- Lightweight and easy to integrate into existing projects.  
- Compatible with Unity **2022.3+**.  
- Includes a **sample scene** demonstrating usage.  

---

## How to Install

1. In the top menu, go to **Window > Package Manager**.  
2. Click **+ > Add package from git URL...**  
3. Paste the following line and click **Add**:  

```json
https://github.com/douglasdasilva17/com.supertactic.virtualcursor.git

```

---

##  How to Use

1. Import the **Samples**. Check the **Samples** section.
2. Navigate to the `Samples/VirtualCursorSamples/Prefabs` folder inside **Supertactic Virtual Cursor**.
3. Drag and drop the `VirtualCursorManager` prefab into your scene.
4. In the `VirtualCursorManager`, assign your `InputSystem_Actions` to the **PlayerInput** component.
5. Configure your `InputActions_Actions` as needed (keyboard, gamepad, etc.).
6. Drag and drop the `VirtualCursor` prefab into your Canvas.
7. In the `VirtualCursorManager`, under the **Cursor Settings** header, assign the `Cursor Transform` and `Cursor Animator` fields.
8. In the `VirtualCursorManager`, under the **Canvas Settings** header, assign the `Canvas` and `Canvas RectTransform` fields.
9. Run the scene — your cursor is ready to navigate UIs!

---

## Samples

This package includes a demo scene to help you get started:  

- **Virtual Cursor Sample**  
A basic scene showing the Virtual Cursor in action with UI buttons.  

To import it:  
1. Go to **Window > Package Manager**.  
2. Select **Supertactic Virtual Cursor**.  
3. Expand the **Samples** section and click **Import**.  

---

## License

This project is licensed under the **Supertactic License**.  
See the [LICENSE](LICENSE) file for details.