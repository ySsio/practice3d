using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakAnimal : Animal
{

    // 위협받거나(아마 조준?당했을떄인가?) 공격받았을 때 뛰도록. 플레이어 반대 방향으로
    public void Run(Vector3 _targetPos)
    {
        // 전에도 했는데 Quaternion.LookRotation(포지션)은 현재 transform.position에서 설정한 포지션을 바라보는 rotation을 구하는 거임.
        // 대신 포지션이 월드좌표계가 아닌 상대좌표계라서 진짜 원하는 위치 포지션에서 transform.position를 뺴주면 target을 바라보는 방향으로 구하는 것.
        // 여기서는 target 바라보는 반대방향을 계산하기 위해 반대로 빼줬다.
        destination = new Vector3(transform.position.x - _targetPos.x, 0f, transform.position.z - _targetPos.z).normalized;

        nav.speed = runSpeed;

        currentTime = runTime;
        isWalking = false;
        isRunning = true;
        anim.SetBool("Run", isRunning);
    }

    public override void Damaged(int _dmg, Vector3 _targetPos)
    {
        base.Damaged(_dmg, _targetPos);
        if (!isDead)
            Run(_targetPos);
    }
}
