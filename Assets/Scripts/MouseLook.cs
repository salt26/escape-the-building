using UnityEngine;
using System.Collections;

public class MouseLook
{
    /* TODO : 나중에 마우스 감도를 원하는 값으로 설정할 수 있도록 할 것! */
    public float XSensitivity = 2f;
    public float YSensitivity = 2f;
    public bool clampVerticalRotation = true;
    public float MinimumX = -90F;
    public float MaximumX = 90F;
    public bool smooth;
    public float smoothTime = 5f;
    public bool lockCursor = true;

    private Move player;
    private Quaternion characterTargetRot;
    private Quaternion cameraTargetRot;
    private bool cursorIsLocked = true;

    public void UpdateSensitivity()
    {
        if (XSensitivity != MainManager.mm.mouseSensitivity)
        {
            XSensitivity = MainManager.mm.mouseSensitivity;
            YSensitivity = MainManager.mm.mouseSensitivity;
        }
    }

    // LookRotation 함수를 사용하기 전에 초기화해주는 함수
    public void Init(Transform character, Transform camera)
    {
        characterTargetRot = character.localRotation;
        cameraTargetRot = camera.localRotation;

        player = GameObject.Find("Player").GetComponent<Move>();
    }

    // 커서의 움직임에 따라 주인공의 몸통과 머리를 회전하는 함수
    public void LookRotation(Transform character, Transform camera)
    {
        // 커서가 잠긴 상태에서만 주인공의 몸통 및 머리 회전이 가능함.
        if (lockCursor && cursorIsLocked && !player.isExhausted)
        {
            float yRot = Input.GetAxisRaw("Mouse X") * XSensitivity;
            float xRot = Input.GetAxisRaw("Mouse Y") * YSensitivity;

            // 몸통과 머리 회전
            characterTargetRot *= Quaternion.Euler(0f, yRot, 0f);
            cameraTargetRot *= Quaternion.Euler(-xRot, 0f, 0f);

            if (clampVerticalRotation)
                cameraTargetRot = ClampRotationAroundXAxis(cameraTargetRot);

            if (smooth)
            {
                character.localRotation = Quaternion.Slerp(character.localRotation, characterTargetRot,
                    smoothTime * Time.deltaTime);
                camera.localRotation = Quaternion.Slerp(camera.localRotation, cameraTargetRot,
                    smoothTime * Time.deltaTime);
            }
            else
            {
                character.localRotation = characterTargetRot;
                camera.localRotation = cameraTargetRot;
            }

        }

        UpdateCursorLock();
    }

    // 메뉴 실행 등의 상황에서 커서 잠금을 해제할 때 사용하는 함수
    public void SetCursorLock(bool value)
    {
        lockCursor = value;
        if (!lockCursor)
        {
            // lockCursor를 false로 설정해 놓으면 무조건 커서가 풀린 상태가 됨.
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void UpdateCursorLock()
    {
        // lockCursor를 true로 설정해 놓으면 상황에 따라 적절히 커서가 잠기거나 풀림.
        if (lockCursor)
            InternalLockUpdate();
    }

    private void InternalLockUpdate()
    {
        /*
        // Esc키를 누르면 커서 잠금이 해제됨.
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            cursorIsLocked = false;
        }
        // 마우스 왼쪽 키를 누르면 커서가 잠김.
        else if (Input.GetMouseButtonUp(0))
        {
            cursorIsLocked = true;
        }
         * */

        if (cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else if (!cursorIsLocked)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    // 고개의 회전 각도를 제한하는 함수.
    Quaternion ClampRotationAroundXAxis(Quaternion q)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;

        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);

        angleX = Mathf.Clamp(angleX, MinimumX, MaximumX);

        q.x = Mathf.Tan(0.5f * Mathf.Deg2Rad * angleX);

        return q;
    }


}
