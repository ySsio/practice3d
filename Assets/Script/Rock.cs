using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rock : MonoBehaviour
{
    [SerializeField]
    private int hp; // ������ ü��

    [SerializeField]
    private float destroyTime; // ���� ���� �ð�

    [SerializeField]
    private SphereCollider col; // ��ü �ݶ��̴� : ��� ��Ʈ ����, �ı��Ǹ� ���־� ��

    // �ʿ��� ���ӿ�����Ʈ (����)
    [SerializeField]
    private GameObject go_rock; // �Ϲ� ����
    [SerializeField]
    private GameObject go_debris; // ���� ����
    [SerializeField]
    private GameObject go_effect_prefabs; // ä�� ����Ʈ
    [SerializeField]
    private GameObject go_rock_item_prefabs; // ������ ������ ������

    // ������ ������ ���� ����
    [SerializeField]
    private int count;


    // ȿ���� �̸�
    [SerializeField]
    private string strike_Sound;
    [SerializeField]
    private string destroy_Sound;


    public void Mining()
    {
        SoundManager.instance.PlaySE(strike_Sound);
        hp--;
        if(hp<=0)
        {
            Destruction(); // �ı���Ű�� �Լ�
            return;
        }

        var clone = Instantiate(go_effect_prefabs, col.bounds.center, Quaternion.identity);
        Destroy(clone, 2f);
    }

    private void Destruction()
    {
        SoundManager.instance.PlaySE(destroy_Sound);
        
        col.enabled = false;

        for (int i = 0; i < count; i++)
        {
            Instantiate(go_rock_item_prefabs, go_rock.transform.position, Quaternion.identity);
        }

        Destroy(go_rock); // ���� ���� �ƿ� �޸𸮿��� �����ع���
        go_debris.SetActive(true);
        Destroy(go_debris, destroyTime); // destroyTime ��ŭ�� �ð� �ڿ� �޸𸮿��� ���ŵ�
    }
}
