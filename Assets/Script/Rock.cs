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
    private AudioSource audioSource;
    [SerializeField]
    private AudioClip effect_sound;
    [SerializeField]
    private AudioClip effect_sound2;

    public void Mining()
    {
        audioSource.clip = effect_sound;
        audioSource.Play();
        
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
        audioSource.clip = effect_sound2;
        audioSource.Play();

        Destroy(go_rock); // ���� ���� �ƿ� �޸𸮿��� �����ع���
        col.enabled = false;

        go_debris.SetActive(true);
        Destroy(go_debris, destroyTime); // destroyTime ��ŭ�� �ð� �ڿ� �޸𸮿��� ���ŵ�
    }
}
