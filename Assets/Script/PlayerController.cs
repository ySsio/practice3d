using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class PlayerController : MonoBehaviour
{
    // �ӵ� ���� ����
    [SerializeField]
    private float walkSpeed;
    [SerializeField]
    private float runSpeed;
    [SerializeField]
    private float crouchSpeed;

    private float applySpeed; // walk or run

    [SerializeField]
    private float jumpForce;

    // ���� ���� (walk/run)
    private bool isWalk = false;
    private bool isRun = false;
    private bool isCrouch = false;
    private bool isGround = true; // ���߿��� ���� ���ϰ�

    // ������ üũ ����
    private Vector3 lastPos; // �� �������� �÷��̾� ��ġ ��� (�������� �ִ���/ ������ üũ)

    // �ɾ��� �� �󸶳� ������ �����ϴ� ����
    [SerializeField]
    private float crouchPosY;
    private float originPosY;
    private float applyCrouchPosY;

    private CapsuleCollider capsuleCollider; // ground�� ����ִ��� �����ϱ� ���� �ݶ��̴� ����

    // ����
    [SerializeField]
    private float lookSensitivity;

    // ī�޶� ����
    [SerializeField]
    private float cameraRotationLimit; // ī�޶� ���Ʒ��� ���� �� ������ ������ ��
    private float currentCameraRotationX = 0f; // �������� ����Ʈ ����

    // �ʿ��� ������Ʈ
    [SerializeField]
    private Camera theCamera;
    private Rigidbody myRigid;
    private GunController theGunController;
    private CrossHair theCrosshair;


    // Start is called before the first frame update
    void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        // theCamera = FindObjectOfType<Camera>(); // hierarchy ������ camera object�� ã�Ƽ� �־��� (�� ���϶��� ��� ����, �̹��� SerializeField�� ������
        myRigid = GetComponent<Rigidbody>();
        applySpeed = walkSpeed; // default speed
        originPosY = theCamera.transform.localPosition.y; // ���� �� ī�޶� ��ġ�� ����
        applyCrouchPosY = originPosY;
        theGunController = FindObjectOfType<GunController>();
        theCrosshair = FindObjectOfType<CrossHair>(); // # ũ�ν��� �÷��̾ �ִ´ٴ� ����?
    }

    // Update is called once per frame
    void Update()
    {
        
        IsGround();
        TryJump();
        TryRun();
        TryCrouch();
        float moveSpeed = Move();
        MoveCheck(moveSpeed);
        CameraRotation(); // �� �� �Ʒ��� ȸ���� ����
        CharacterRotation(); // �¿�� �þ� ȸ���ϴ°Ŵ� ĳ���� ��ü�� ȸ�����Ѽ� ������
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
        theCrosshair.CrouchingAnimation(isCrouch);

        if (isCrouch)
        {
            applySpeed = crouchSpeed;
            applyCrouchPosY = crouchPosY;
        } else
        {
            applySpeed = walkSpeed;
            applyCrouchPosY = originPosY;
        }

        // �̷��� �����ϸ� �����̵���
        //theCamera.transform.localPosition = new Vector3(theCamera.transform.localPosition.x, applyCrouchPosY, theCamera.transform.localPosition.z); // y�� ������ �ȵ�

        // coroutine�� �̿��� �ε巯�� ������ ����
        // coroutine : ��ɹ� ����ó���� ���� ������� ������ (�ϳ��� cpu�� ������ �Դٰ��� �ϸ鼭 ������. ���� ������ �ƴ�)
        StartCoroutine(CrouchCoroutine());

    }

    IEnumerator CrouchCoroutine()
    {
        float _posY = theCamera.transform.localPosition.y;
        int count = 0; // ������ ������ �ݺ��ǰ� �������� �ʾƼ� ���� Ƚ���� ������ ��.

        while(_posY != applyCrouchPosY) // ���ϴ� ��ġ���� �ٴٸ� ������ ��� ����
        {
            count++;
            _posY = Mathf.Lerp(_posY, applyCrouchPosY, 0.3f); // ���� �Լ�, �������� ���� ������ 0.3�� ���� ������ ��ȯ��. ��� _posY�� ���ϸ鼭 �������� �����ϰ� ��
            theCamera.transform.localPosition = new Vector3(0, _posY, 0);
            if (count > 15)
                break;
            yield return null;
        }
        theCamera.transform.localPosition = new Vector3(0, applyCrouchPosY, 0); // ���������� ��ǥ�������� ����
    }

    private void TryJump()
    {
        if(Input.GetKeyDown(KeyCode.Space) && isGround) // ��ư�� ������ ������ �ǹ�
        {
            Jump();
        } 
    }

    private void Jump()
    {
        // ���� ���¿��� ���� ��, ���� ���� ����
        if (isCrouch)
            Crouch();
        myRigid.velocity = transform.up * jumpForce;
    }

    private void IsGround()
    {
        // Vector3.down�� �� ����
        // 1. transform.left �� transform.down�� ����
        // 2. -transform.up�� ����ϸ� transform ��ü�� ���¿� ��������. ������ǥ�迡�� (0,-1,0)�� �ƴ϶�� ��
        isGround = Physics.Raycast(transform.position, Vector3.down, capsuleCollider.bounds.extents.y + 0.3f);
        

        theCrosshair.JumpAnimation(!isGround);
    }

    private void TryRun()
    {
        if (Input.GetKey(KeyCode.LeftShift)) // ������ �ִ� ���¸� �ǹ�
        {
            Running();
        }
        // # �� else�� ���ϰ� if�� �ΰ��� ����... ���ĺ��ϱ� �Ȱ����� ������
        if (Input.GetKeyUp(KeyCode.LeftShift))  // ��ư�� ���� ������ �ǹ�
        {
            RunningCancel();
        }
    }

    private void Running()
    {
        // ���� ���¿��� �޸��� ��, ���� ���� ����
        if (isCrouch)
            Crouch();

        // �޸��� �� ������ ����
        theGunController.CancelFineSight();

        isRun = true;
        theCrosshair.RunningAnimation(isRun);
        applySpeed = runSpeed;
    }

    private void RunningCancel()
    {
        isRun = false;
        theCrosshair.RunningAnimation(isRun);
        applySpeed = walkSpeed;
    }

    private float Move() // �ӵ��� ũ�⸦ ��ȯ (�ӷ� ��ȯ)
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal"); // �� ��
        float _moveDirZ = Input.GetAxisRaw("Vertical"); // �� ��

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * applySpeed;

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
        return _velocity.magnitude;
    }

    // isWalk�� �����ϱ� ���� �Լ�
    private void MoveCheck(float moveSpeed)
    {
        if (!isRun && !isCrouch && isGround) 
        {
            if (moveSpeed > 0.01f)
                isWalk = true;
            else
                isWalk = false;

            theCrosshair.WalkingAnimation(isWalk);
            lastPos = transform.position;
        }
        
    }

    private void CameraRotation()
    {
        float _xRotation = Input.GetAxisRaw("Mouse Y"); // x�� ���� ȸ���̶� �̷��� ���ѵ� ������..���� = �� �Ʒ� ȸ��
        float _cameraRotationX = _xRotation * lookSensitivity;
        currentCameraRotationX -= _cameraRotationX;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -cameraRotationLimit, cameraRotationLimit);

        theCamera.transform.localEulerAngles = new Vector3(currentCameraRotationX, 0f, 0f);
    }

    private void CharacterRotation()
    {
        float _yRotation = Input.GetAxisRaw("Mouse X");
        Vector3 _characterRotationY = new Vector3(0f,_yRotation, 0f) * lookSensitivity; // ���Ϸ� �ޱ� form
        myRigid.MoveRotation(myRigid.rotation * Quaternion.Euler(_characterRotationY)); // MoveRotation()�Լ��� quaternion form���� input�� �ޱ� ������ quaternion���� ��ȯ
        
    }

}
