using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleTrap : MonoBehaviour
{

    private Rigidbody[] rigid; // 보드와 나뭇가지 받아옴. iskinematic 해제하고 gravity 체크. 자연스럽게 쓰러짐
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
        Destroy(go_Meat); // 고기 없애기
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
