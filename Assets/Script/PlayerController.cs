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
    private float cameraRotationLimit; // ī�޶� ���Ʒ��� ���� �� ������ ������ ��
    private float currentCameraRotationX = 0f; // �������� ����Ʈ ����

    [SerializeField]
    private Camera theCamera;

    private Rigidbody myRigid;


    // Start is called before the first frame update
    void Start()
    {
        // theCamera = FindObjectOfType<Camera>(); // hierarchy ������ camera object�� ã�Ƽ� �־��� (�� ���϶��� ��� ����, �̹��� SerializeField�� ������
        myRigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CameraRotation(); // �� �� �Ʒ��� ȸ���� ����
        CharacterRotation(); // �¿�� �þ� ȸ���ϴ°Ŵ� ĳ���� ��ü�� ȸ�����Ѽ� ������
    }

    private void Move()
    {
        float _moveDirX = Input.GetAxisRaw("Horizontal"); // �� ��
        float _moveDirZ = Input.GetAxisRaw("Vertical"); // �� ��

        Vector3 _moveHorizontal = transform.right * _moveDirX;
        Vector3 _moveVertical = transform.forward * _moveDirZ;

        Vector3 _velocity = (_moveHorizontal + _moveVertical).normalized * walkSpeed;

        myRigid.MovePosition(transform.position + _velocity * Time.deltaTime);
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
