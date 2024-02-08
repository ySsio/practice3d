using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOV : MonoBehaviour
{
    [SerializeField] private float viewAngle; // �þ߰� (120��)
    [SerializeField] private float viewDistance; // �þ߰Ÿ� (10����)
    [SerializeField] private LayerMask targetMask; // Ÿ�ٸ���ũ (�÷��̾�).. �÷��̾� ���̸� �������� ��ũ��Ʈ ¥�°���

    private Pig thePig;

    void Start()
    {
        thePig = GetComponent<Pig>();
    }

    void Update()
    {
        View();
    }

    private Vector3 BoundaryAngle(float _angle)
    {
        _angle += transform.eulerAngles.y;
        
        // Debug.DrawRay()�� �ϱ� ���� �������� ������ ���� "��ǥ"�� ���ϴ� ������.
        // ������ z��, ���� x���̰� �þ߰��� z�� �����̴ϱ� z���� cos�̰� x���� sin��
        // Mathf.Deg2Rad�� 180/pi �� ���� ����. pi = 180�� �ϱ� 1rad = 180/pi �̱� ������.
        return new Vector3(Mathf.Sin(_angle * Mathf.Deg2Rad),0,Mathf.Cos(_angle * Mathf.Deg2Rad));
    }

    private void View()
    {
        Vector3 _leftBoundary = BoundaryAngle(-viewAngle * 0.5f);
        Vector3 _rightBoundary = BoundaryAngle(viewAngle * 0.5f);

        Debug.DrawRay(transform.position + transform.up, _leftBoundary, Color.red);
        Debug.DrawRay(transform.position + transform.up, _rightBoundary, Color.red);

        // Physics.OverlapSphere(������,�Ÿ�,���̾��ũ) : ���� �ݰ� �ȿ� �ִ� �ݶ��̴��� ��� �޾ƿ��� �Լ�
        Collider[] _target = Physics.OverlapSphere(transform.position, viewDistance, targetMask);

        for (int i = 0; i < _target.Length; i++)
        {
            Transform _targetTf = _target[i].transform;
            if(_targetTf.name == "Player")
            {
                // _dir : �������� �÷��̾�� ���ϴ� ���⺤��, _angle : ���� ���麤�Ϳ��� _dir ������ ��
                Vector3 _dir = (_targetTf.position - transform.position).normalized; // transform.position���� _targetTf.position���� ���ϴ� ���⺤�� ��������. ���� - ����
                float _angle = Vector3.Angle(_dir, transform.forward); // ���� ���� ���Ϳ� �÷��̾���� ���⺤�� ���� ������ �����.

                if (_angle < viewAngle * .5f)
                {
                    // ��(���̾ �ٸ� �ݶ��̴�)�� �þ߰� ������ ��Ȳ���� �������� �ʵ��� raycast�� ���� �Ѵ�.
                    RaycastHit _hit;
                    if (Physics.Raycast(transform.position + transform.up, _dir, out _hit, viewDistance))
                    {
                        if (_hit.transform.name == "Player")
                        {
                            Debug.DrawRay(transform.position + transform.up, _dir * viewDistance, Color.blue);
                            thePig.Run(_hit.transform.position);
                        }
                        
                    }
                }
            }
        }
    }
}
