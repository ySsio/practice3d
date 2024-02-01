using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerController : MonoBehaviour
{
    // 속도 조절 변수
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float crouchSpeed;

    private float applySpeed; // walk or run

    [SerializeField]
    private float jumpForce;

    // 상태 변수 (walk/run)
    private bool isRun = false;
    private bool isCrouch = false;
    private bool isGround = true; // 공중에서 점프 못하게

    // 앉았을 때 얼마나 앉을지 결정하는 변수
    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;

    private CapsuleCollider capsuleCollider; // ground에 닿아있는지 판정하기 위한 콜라이더 변수

    // 감도
    [SerializeField]
    private float lookSensitivity;

    // 카메라 범위
    [SerializeField]
    private float cameraRotationLimit; // 카메라 위아래로 돌릴 때 각도를 제한을 둠
    private float currentCameraRotationX = 0f; // 정면으로 디폴트 세팅

    [SerializeField]
    private Camera theCamera;

    private Rigidbody myRigid;


    // Start is called before the first frame update
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        // theCamera = FindObjectOfType<Camera>(); // hierarchy 내에서 camera object를 찾아서 넣어줌 (한 개일때만 사용 가능, 이번엔 SerializeField로 구현함
        myRigid = GetComponent<Rigidbody>();
        applySpeed = walkSpeed; // default speed
        originPosY = theCamera.transform.localPosition.y; // 앉을 때 카메라 위치를 수정
        applyCrouchPosY = originPosY;
    }

    // Update is called once per frame
    void Update()
    {
        
        IsGround();
        TryJump();
        TryRun();
        TryCrouch();
        Move();
        CameraRotation(); // 고개 위 아래로 회전만 구현
        CharacterRotation(); // 좌우로 시야 회전하는거는 캐릭터 자체를 회전시켜서 구현함
    }

    private void TryCrouch()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Crouch();
        }
    }

    private void Crouch()
    {
        isCrouch = !isCrouch; // toggle
        if (isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        } else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }

        // 이렇게 구현하면 순간이동함
        //theCamera.transform.localPosition = new Vector3(theCamera.transform.localPosition.x, applyCrouchPosY, theCamera.transform.localPosition.z); // y만 수정이 안됨

        // coroutine을 이용해 부드러운 움직임 구현
        // coroutine : 명령문 병렬처리를 위해 만들어진 개념임 (하나의 cpu로 빠르게 왔다갔다 하면서 구현됨. 실제 병행은 아님)
        StartCoroutine(CrouchCoroutine());

    }

    IEnumerator CrouchCoroutine()
    {
        float _posY = theCamera.transform.localPosition.y;
        int count = 0; // 보간이 무한히 반복되고 수렴하지 않아서 연산 횟수에 제한을 둠.

        while(_posY != applyCrouchPosY) // 원하는 위치까지 다다를 때까지 계속 실행
        {
            count++;
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.3f); // 보간 함수, 시작점과 끝점 사이의 0.3의 비율 지점을 반환함. 계속 _posY가 변하면서 목적값에 수렴하게 됨
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);
            if (count > 15)
                break;
            yield return null;
        }
        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0); // 인위적으로 목표지점으로 설정
    }

    private void TryJump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGround) // 버튼을 누르는 순간을 의미
        {
            Jump();
        } 
    }

    private void Jump()
    {
        // 앉은 상태에서 점프 시, 앉은 상태 해제
        if (isCrouch)
            Crouch();
        myRigid.velocity = transform.up * jumpForce;
    }

    private void IsGround()
    {
        // Vector3.down을 쓴 이유
        // 1. transform.left 와 transform.down은 없다
        // 2. -transform.up을 사용하면 transform 객체의 상태에 의존적임. 절대좌표계에서 (0,-1,0)이 아니라는 뜻
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.1f); 
        // (시작, 방향, 거리) 인듯? 객체 중심에서 절대적인 아래 방향으로 콜라이더의 반 거리만큼 쏜다. 배꼽에서 발까지 ray를 쏜 느낌
        // 0.1f은 약간의 여유 값
        // # 왜 콜라이더로 안하고 ㅋㅋ raycast로 하지
    }

    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift)) // 누르고 있는 상태를 의미
        {
            Running();
        }
        // # 왜 else로 안하고 if문 두개로 하지... 고쳐보니까 똑같은거 같은데
        if (Input.GetKeyUp(KeyCode.LeftShift))  // 버튼을 떼는 동작을 의미
        {
            RunningCancel();
        }
    }

    private void Running()
    {
        // 앉은 상태에서 달리기 시, 앉은 상태 해제
        if (isCrouch)
            Crouch();
        isRun = true;
        applySpeed = runSpeed;
    }

    private void RunningCancel()
    {
        isRun = false;
        applySpeed = walkSpeed;
    }

    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal"); // 좌 우
        float _moveDirZ = Input.GetAxisRaw("Vertical"); // 앞 뒤

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;

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
