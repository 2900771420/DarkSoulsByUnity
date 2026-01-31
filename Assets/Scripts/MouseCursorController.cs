using UnityEngine;

public class MouseCursorController : MonoBehaviour
{
    // 是否在游戏开始时隐藏鼠标
    public bool hideCursorOnStart = true;

    private bool isCursorVisible;

    void Start()
    {
        if (hideCursorOnStart)
        {
            HideCursor();
        }
        else
        {
            ShowCursor();
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isCursorVisible)
            {
                HideCursor();
            }
            else
            {
                ShowCursor();
            }
        }
    }

    void HideCursor()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        isCursorVisible = false;
    }

    void ShowCursor()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        isCursorVisible = true;
    }
}

