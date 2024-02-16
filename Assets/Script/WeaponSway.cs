using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    // ���Ⱑ ȭ�� �����̴� ���� �ݴ�� ġ��ġ���� �ϴ� ��ũ��Ʈ


    // ������ġ
    private Vector3 originPos;
    
    // ���� ��ġ
    private Vector3 currentPos;

    // sway �Ѱ�.
    [SerializeField]
    private Vector3 limitPos;

    // ������ sway �Ѱ�
    [SerializeField]
    private Vector3 fineSightLimitPos;

    // �ε巯�� ������ ����
    [SerializeField]
    private Vector3 smoothSway;

    // �ʿ��� ������Ʈ
    [SerializeField]
    private GunController theGunController; // finesightmode�� üũ�ϱ� ���� �ҷ���


    // Start is called before the first frame update
    void Start()
    {
        originPos = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.canPlayerMove && isActivated)
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
