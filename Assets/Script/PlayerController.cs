using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed;

    [SerializeField]
    private float lookSensitivity;

    [SerializeField]
    private float cameraRotationLimit; // 카메라 위아래로 돌릴 때 각도를 제한을 둠
    private float currentCameraRotationX = 0f; // 정면으로 디폴트 세팅

    [SerializeField]
    private Camera theCamera;

    private Rigidbody myRigid;


    // Start is called before the first frame update
    void Start()
    {
        // theCamera = FindObjectOfType<Camera>(); // hierarchy 내에서 camera object를 찾아서 넣어줌 (한 개일때만 사용 가능, 이번엔 SerializeField로 구현함
        myRigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CameraRotation(); // 고개 위 아래로 회전만 구현
        CharacterRotation(); // 좌우로 시야 회전하는거는 캐릭터 자체를 회전시켜서 구현함
    }

    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal"); // 좌 우
        float _moveDirZ = Input.GetAxisRaw("Vertical"); // 앞 뒤

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * walkSpeed;

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
    }

    private void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y"); // x축 기준 회전이라 이렇게 정한듯 변수명..ㅋㅋ = 위 아래 회전
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f,_yRotation, 0f) * lookSensitivity; // 오일러 앵글 form
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY)); // MoveRotation()함수가 quaternion form으로 input을 받기 때문에 quaternion으로 변환
        
    }
}
