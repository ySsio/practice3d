using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    // 무기가 화면 움직이는 방향 반대로 치우치도록 하는 스크립트


    // 기존위치
    private Vector3 originPos;
    
    // 현재 위치
    private Vector3 currentPos;

    // sway 한계.
    [SerializeField]
    private Vector3 limitPos;

    // 정조준 sway 한계
    [SerializeField]
    private Vector3 fineSightLimitPos;

    // 부드러운 움직임 정도
    [SerializeField]
    private Vector3 smoothSway;

    // 필요한 컴포넌트
    [SerializeField]
    private GunController theGunController; // finesightmode를 체크하기 위해 불러옴


    // Start is called before the first frame update
    void Start()
    {
        originPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.canPlayerMove)
            TrySway();

    }

    private void TrySway()
    {
        if (Input.GetAxisRaw("Mouse X") != 0 || Input.GetAxisRaw("Mouse Y") != 0)
            Swaying();
        else
            BackToOriginPos();
    }

    private void Swaying()
    {
        float _moveX = Input.GetAxisRaw("Mouse X");
        float _moveY = Input.GetAxisRaw("Mouse Y");
        if (!theGunController.isFineSightMode)
        {
            currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -_moveX, smoothSway.x), -limitPos.x, limitPos.x),
                           Mathf.Clamp(Mathf.Lerp(currentPos.y, -_moveY, smoothSway.y), -limitPos.y, limitPos.y)-1,
                           originPos.z);
        } else
        {
            currentPos.Set(Mathf.Clamp(Mathf.Lerp(currentPos.x, -_moveX, smoothSway.x), -fineSightLimitPos.x, fineSightLimitPos.x),
                           Mathf.Clamp(Mathf.Lerp(currentPos.y, -_moveY, smoothSway.y), -fineSightLimitPos.y, fineSightLimitPos.y)-1,
                           originPos.z);
        }

        transform.localPosition = currentPos;
    }

    private void BackToOriginPos()
    {
        currentPos = Vector3.Lerp(currentPos, originPos, smoothSway.x);
        transform.localPosition = currentPos;
    }
}
