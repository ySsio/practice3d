using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakAnimal : Animal
{

    // �����ްų�(�Ƹ� ����?���������ΰ�?) ���ݹ޾��� �� �ٵ���. �÷��̾� �ݴ� ��������
    public void Run(Vector3 _targetPos)
    {
        // ������ �ߴµ� Quaternion.LookRotation(������)�� ���� transform.position���� ������ �������� �ٶ󺸴� rotation�� ���ϴ� ����.
        // ��� �������� ������ǥ�谡 �ƴ� �����ǥ��� ��¥ ���ϴ� ��ġ �����ǿ��� transform.position�� ���ָ� target�� �ٶ󺸴� �������� ���ϴ� ��.
        // ���⼭�� target �ٶ󺸴� �ݴ������ ����ϱ� ���� �ݴ�� �����.
        direction = Quaternion.LookRotation(transform.position - _targetPos).eulerAngles;

        applySpeed = runSpeed;

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
