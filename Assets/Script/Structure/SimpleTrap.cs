using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTrap : MonoBehaviour
{

    private Rigidbody[] rigid; // ����� �������� �޾ƿ�. iskinematic �����ϰ� gravity üũ. �ڿ������� ������
    [SerializeField] private GameObject go_Meat;
    [SerializeField] private int damage;

    private bool isActivated = false;

    private AudioSource theAudio;
    [SerializeField] private AudioClip sound_Activate;

    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponentsInChildren<Rigidbody>();
        theAudio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (isActivated)
            return;

        if (other.tag == "Untagged")
            return;

        

        isActivated = true;
        Destroy(go_Meat); // ��� ���ֱ�
        theAudio.clip = sound_Activate;
        theAudio.Play();


        for (int i = 0; i < rigid.Length; i++)
        {
            rigid[i].isKinematic = false;
            rigid[i].useGravity = true;
        }

        if (other.transform.name == "Player")
        {
            other.transform.GetComponent<StatusController>().DecreaseHP(damage);
        }
    }
}
