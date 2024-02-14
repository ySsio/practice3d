using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    // static�� Ŭ���� ������ �����ϴ� ��. ��� ��ü�� �����ϴ� ����
    // �ν��Ͻ��� �������� �ʾƵ� Ŭ�������� �ٷ� ������ �� �� ����. WeaponManager.isChaneWeapon �̷���

    
    // ���� �ߺ� ��ü ���� ����. true�� ���ȿ� ���� ��üx
    public static bool isChangeWeapon = false;

    // ################## ���� ���⿡ ���� ���� ################

    // ���� ���� ��ü
    public static Transform currentWeapon; // Transform class�� ������ ������ �� ������ ���� Ű�� ���Ҹ� �ҰŰ�,
                                           // ���� ���Ⱑ Gun type �� ���� �ְ� Hand type�� �� �� �ִµ�
                                           // �̰Ÿ� �� ���� ������ �ޱ� ���� Transform type���� ������ ��.

    // ���� ���� �ִϸ��̼�
    public static Animator currentWeaponAnim;


    // ���� ������ Ÿ�� (GUN / HAND / AXE / PICKAXE)
    [SerializeField]
    private string currentWeaponType; 

    // #########################################################

    // ���� ��ü ������
    [SerializeField]
    private float changeWeaponDelayTime; // �� �ִ� �ð�
    [SerializeField]
    private float changeWeaponEndDelayTime; // ���� ������ �ð�

    // ���� ���� ���� ���� (= prefabs)
    [SerializeField]
    private Gun[] guns; // ���� ���
    [SerializeField]
    private CloseWeapon[] hands; // �� ���
    [SerializeField]
    private CloseWeapon[] axes; // ���� ���
    [SerializeField]
    private CloseWeapon[] pickaxes; // ���� ���

    // ���� ���� �̸����� ���� (= string name)
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    private Dictionary<string, CloseWeapon> handDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> axeDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> pickaxeDictionary = new Dictionary<string, CloseWeapon>();

    // �ʿ��� ������Ʈ
    // �� ���� ������ ��Ʈ�ѷ��� �ѱ� ���� ��� ������ ��Ʈ�ѷ� �޾ƿ�
    [SerializeField]
    private GunController theGunController;
    [SerializeField]
    private HandController theHandController;
    [SerializeField]
    private AxeController theAxeController;
    [SerializeField]
    private PickaxeController thePickaxeController;


    

    
    // Start is called before the first frame update
    void Start()
    {
        // ���� ��ųʸ� �ʱ�ȭ
        for (int i = 0; i < guns.Length; i++)
        {
            gunDictionary.Add(guns[i].gunName, guns[i]);
        }

        for (int i = 0; i < hands.Length; i++)
        {
            handDictionary.Add(hands[i].closeWeaponName, hands[i]);
        }

        for (int i = 0; i < axes.Length; i++)
        {
            axeDictionary.Add(axes[i].closeWeaponName, axes[i]);
        }
        for (int i = 0; i < pickaxes.Length; i++)
        {
            pickaxeDictionary.Add(pickaxes[i].closeWeaponName, pickaxes[i]);
        }

    }


    // Update is called once per frame
    void Update()
    {
        TryChangeWeapon();
    }

    private void TryChangeWeapon()
    {
        if (!isChangeWeapon)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                // ���� ��ü ���� (�Ǽ�)
                StartCoroutine(ChangeWeaponCoroutine("HAND", "�Ǽ�"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                // ���� ��ü ���� (����ӽŰ�)
                StartCoroutine(ChangeWeaponCoroutine("GUN", "SubMachineGun1"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                // ���� ��ü ���� (����)
                StartCoroutine(ChangeWeaponCoroutine("AXE", "Axe"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                // ���� ��ü ���� (���)
                StartCoroutine(ChangeWeaponCoroutine("PICKAXE", "Pickaxe"));
            }
        }
    }


    public IEnumerator ChangeWeaponCoroutine(string _type, string _name)
    {
        isChangeWeapon = true; // �� �ڷ�ƾ ���� ������ TryChaneWeapon() ���� (= ���ⱳü x)

        // ���� ��ü �ִϸ��̼� ����, �ִϸ��̼� ���� ���� ������ �ο�
        currentWeaponAnim.SetTrigger("Weapon_Out");
        yield return new WaitForSeconds(changeWeaponDelayTime);

        // ################# ���� ��ü�� �����ϴ� �κ� ####################
        
        // ���� ����� �� ������ controller�� ��Ȱ��ȭ
        CancelPreWeaponAction();

        // �ٲ� ����� �� ������ controller�� Ȱ��ȭ
        WeaponChange(_type,_name);

        // ###################################################################

        yield return new WaitForSeconds(changeWeaponEndDelayTime);

        isChangeWeapon = false; // �ٽ� ���� ��ü ������ ����
    }

    private void CancelPreWeaponAction()
    {
        switch (currentWeaponType)
        {
            case "GUN":
                theGunController.CancelFineSight();
                theGunController.CancelReload();
                theGunController.enabled = false;
                break;
            case "HAND":
                theHandController.enabled = false;
                break;
            case "AXE":
                theAxeController.enabled = false;
                break;
            case "PICKAXE":
                thePickaxeController.enabled = false;
                break;
        }
    }


    private void WeaponChange(string _type, string _name)
    {
        switch (_type)
        {
            case "GUN":
                theGunController.GunChange(gunDictionary[_name]);
                theGunController.enabled = true;
                break;
            case "HAND":
                theHandController.CloseWeaponChange(handDictionary[_name]);
                theHandController.enabled = true;
                break;
            case "AXE":
                theAxeController.CloseWeaponChange(axeDictionary[_name]);
                theAxeController.enabled = true;
                break;
            case "PICKAXE":
                thePickaxeController.CloseWeaponChange(pickaxeDictionary[_name]);
                thePickaxeController.enabled = true;
                break;
        }
        
        currentWeaponType = _type;
    }

    public IEnumerator WeaponInCoroutine()
    {
        isChangeWeapon = true; // �� �ڷ�ƾ ���� ������ TryChaneWeapon() ���� (= ���ⱳü x)

        // ���� ��ü �ִϸ��̼� ����, �ִϸ��̼� ���� ���� ������ �ο�
        currentWeaponAnim.SetTrigger("Weapon_Out");

        yield return new WaitForSeconds(changeWeaponDelayTime);

        currentWeapon.gameObject.SetActive(false);
    }

    public void WeaponOut()
    {
        isChangeWeapon = false; // �� �ڷ�ƾ ���� ������ TryChaneWeapon() ���� (= ���ⱳü x)

        currentWeapon.gameObject.SetActive(true);
    }

}

