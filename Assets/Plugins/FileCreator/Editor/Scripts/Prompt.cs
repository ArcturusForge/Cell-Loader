using System;
using UnityEditor;
using UnityEngine;

public class Prompt : EditorWindow
{
    string description, inputText;
    string okButton, cancelButton;
    bool initializedPosition = false;
    Action onOKButton;
    Action onCancelButton;
    Func<string, bool> onCompareInput;

    bool shouldClose = false;
    Vector2 maxScreenPos;

    #region OnGUI()
    void OnGUI()
    {
        // Check if Esc/Return have been pressed
        var e = Event.current;
        if (e.type == EventType.KeyDown)
        {
            switch (e.keyCode)
            {
                // Escape pressed
                case KeyCode.Escape:
                    onCancelButton?.Invoke();
                    shouldClose = true;
                    e.Use();
                    break;

                // Enter pressed
                case KeyCode.Return:
                case KeyCode.KeypadEnter:
                    if (onCompareInput == null || onCompareInput.Invoke(inputText))
                    {
                        onOKButton?.Invoke();
                        shouldClose = true;
                        e.Use();
                    }
                    break;
            }
        }

        if (shouldClose)
        {  // Close this dialog
            Close();
            //return;
        }

        // Draw our control
        var rect = EditorGUILayout.BeginVertical();

        EditorGUILayout.Space(12);
        EditorGUILayout.LabelField(description);

        EditorGUILayout.Space(8);
        GUI.SetNextControlName("inText");
        inputText = EditorGUILayout.TextField("", inputText);
        GUI.FocusControl("inText");   // Focus text field
        EditorGUILayout.Space(12);

        // Draw OK / Cancel buttons
        var r = EditorGUILayout.GetControlRect();
        r.width /= 2;
        if (GUI.Button(r, okButton) && (onCompareInput == null || onCompareInput.Invoke(inputText)))
        {
            onOKButton?.Invoke();
            shouldClose = true;
        }

        r.x += r.width;
        if (GUI.Button(r, cancelButton))
        {
            inputText = null;   // Cancel - delete inputText
            onCancelButton?.Invoke();
            shouldClose = true;
        }

        EditorGUILayout.Space(8);
        EditorGUILayout.EndVertical();

        // Force change size of the window
        if (rect.width != 0 && minSize != rect.size)
        {
            minSize = maxSize = rect.size;
        }

        // Set dialog position next to mouse position
        if (!initializedPosition && e.type == EventType.Layout)
        {
            initializedPosition = true;

            // Move window to a new position. Make sure we're inside visible window
            var mousePos = GUIUtility.GUIToScreenPoint(Event.current.mousePosition);
            mousePos.x += 32;
            if (mousePos.x + position.width > maxScreenPos.x) mousePos.x -= position.width + 64; // Display on left side of mouse
            if (mousePos.y + position.height > maxScreenPos.y) mousePos.y = maxScreenPos.y - position.height;

            position = new Rect(mousePos.x, mousePos.y, position.width, position.height);

            // Focus current window
            Focus();
        }
    }
    #endregion OnGUI()

    #region Show()
    public static void Show(string title, string description, string inputText, Action<string> onOk, Action onCancel, Func<string,bool> onCompare, string okButton, string cancelButton)
    {
        // Make sure our popup is always inside parent window, and never offscreen
        // So get caller's window size
        var maxPos = GUIUtility.GUIToScreenPoint(new Vector2(Screen.width, Screen.height));

        var window = CreateInstance<Prompt>();
        window.maxScreenPos = maxPos;
        window.titleContent = new GUIContent(title);
        window.description = description;
        window.inputText = inputText;
        window.okButton = okButton;
        window.cancelButton = cancelButton;
        window.onCompareInput = onCompare;
        window.onOKButton += () => onOk?.Invoke(window.inputText);
        window.onCancelButton += onCancel;
        window.ShowModal();
    }

    public static void Show(string title, string description, Action<string> onOk, Action onCancel, Func<string, bool> onCompare, string okButton = "OK", string cancelButton = "Cancel")
    {
        Show(title, description, "", onOk, onCancel, onCompare, okButton, cancelButton);
    }

    public static void Show(string title, string description, Action<string> onOk, Action onCancel, string okButton = "OK", string cancelButton = "Cancel")
    {
        Show(title, description, "", onOk, onCancel, null, okButton, cancelButton);
    }
    #endregion Show()
}