using UnityEngine;
using System.Collections;

public class MouseLook
{
    /* 나중에 마우스 감도를 원하는 값으로 설정할 수 있도록 할 것! */
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
    private float turningAngle;

    // LookRotation 함수를 사용하기 전에 초기화해주는 함수
    public void Init(Transform character, Transform camera)
    {
        characterTargetRot = character.localRotation;
        cameraTargetRot = camera.localRotation;

        player = GameObject.Find("Player").GetComponent<Move>();
        turningAngle = 0f;
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
        //if (player.isExhausted) camera.localRotation = Quaternion.Euler(90f, 0f, 0f);

        /*
        // 몸은 가만히 있고 고개(시야)만 돌리는 기능. A(왼쪽 화살표)키 또는 D(오른쪽 화살표)키를 눌러 돌릴 수 있음.
        // -90도 ~ 90도 사이에서 움직임
        if ((Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) && turningAngle > -90f) turningAngle -= 5f;
        else if ((Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && turningAngle < 90f) turningAngle += 5f;
        turningAngle = Mathf.Clamp(turningAngle, -90f, 90f);

        // 고개(시야)를 돌린 후 키를 놓았을 때 다시 정면을 바라보도록 함.
        if (!(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) ||
            Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) && turningAngle != 0f)
        {
            if (Mathf.Abs(turningAngle) < 2f) turningAngle = 0f;
            else if (turningAngle < 0f) turningAngle += 3f;
            else if (turningAngle > 0f) turningAngle -= 3f;
        }

        // 고개만 회전하는 기능 반영
        camera.localRotation = Quaternion.Euler(camera.localRotation.eulerAngles.x, turningAngle, 0f);
        */

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
