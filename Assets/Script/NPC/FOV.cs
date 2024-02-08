using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOV : MonoBehaviour
{
    [SerializeField] private float viewAngle; // 시야각 (120도)
    [SerializeField] private float viewDistance; // 시야거리 (10미터)
    [SerializeField] private LayerMask targetMask; // 타겟마스크 (플레이어).. 플레이어 보이면 도망가는 스크립트 짜는거임

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
        
        // Debug.DrawRay()를 하기 위해 레이저가 끝나는 지점 "좌표"를 구하는 과정임.
        // 정면이 z축, 옆이 x축이고 시야각이 z축 기준이니까 z값이 cos이고 x값이 sin임
        // Mathf.Deg2Rad는 180/pi 의 역수 값임. pi = 180도 니까 1rad = 180/pi 이기 때문임.
        return new Vector3(Mathf.Sin(_angle * Mathf.Deg2Rad),0,Mathf.Cos(_angle * Mathf.Deg2Rad));
    }

    private void View()
    {
        Vector3 _leftBoundary = BoundaryAngle(-viewAngle * 0.5f);
        Vector3 _rightBoundary = BoundaryAngle(viewAngle * 0.5f);

        Debug.DrawRay(transform.position + transform.up, _leftBoundary, Color.red);
        Debug.DrawRay(transform.position + transform.up, _rightBoundary, Color.red);

        // Physics.OverlapSphere(기준점,거리,레이어마스크) : 일정 반경 안에 있는 콜라이더를 모두 받아오는 함수
        Collider[] _target = Physics.OverlapSphere(transform.position, viewDistance, targetMask);

        for (int i = 0; i < _target.Length; i++)
        {
            Transform _targetTf = _target[i].transform;
            if(_targetTf.name == "Player")
            {
                // _dir : 돼지에서 플레이어로 향하는 방향벡터, _angle : 돼지 정면벡터에서 _dir 사이의 각
                Vector3 _dir = (_targetTf.position - transform.position).normalized; // transform.position에서 _targetTf.position으로 향하는 방향벡터 단위벡터. 종점 - 시점
                float _angle = Vector3.Angle(_dir, transform.forward); // 정면 방향 벡터와 플레이어로의 방향벡터 사이 각도를 계산함.

                if (_angle < viewAngle * .5f)
                {
                    // 벽(레이어가 다른 콜라이더)에 시야가 가리는 상황에는 감지되지 않도록 raycast를 굳이 한다.
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
