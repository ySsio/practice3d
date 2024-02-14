using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    // static은 클래스 변수를 선언하는 것. 모든 객체가 공유하는 변수
    // 인스턴스를 선언하지 않아도 클래스에서 바로 가져다 쓸 수 있음. WeaponManager.isChaneWeapon 이렇게

    
    // 무기 중복 교체 실행 방지. true인 동안에 무기 교체x
    public static bool isChangeWeapon = false;

    // ################## 현재 무기에 대한 정보 ################

    // 현재 무기 객체
    public static Transform currentWeapon; // Transform class로 정의한 이유는 이 변수가 껐다 키는 역할만 할거고,
                                           // 현재 무기가 Gun type 일 수도 있고 Hand type일 수 도 있는데
                                           // 이거를 한 개의 변수에 받기 위해 Transform type으로 지정한 것.

    // 현재 무기 애니메이션
    public static Animator currentWeaponAnim;


    // 현재 무기의 타입 (GUN / HAND / AXE / PICKAXE)
    [SerializeField]
    private string currentWeaponType; 

    // #########################################################

    // 무기 교체 딜레이
    [SerializeField]
    private float changeWeaponDelayTime; // 손 넣는 시간
    [SerializeField]
    private float changeWeaponEndDelayTime; // 무기 꺼내는 시간

    // 무기 종류 전부 관리 (= prefabs)
    [SerializeField]
    private Gun[] guns; // 무기 목록
    [SerializeField]
    private CloseWeapon[] hands; // 손 목록
    [SerializeField]
    private CloseWeapon[] axes; // 도끼 목록
    [SerializeField]
    private CloseWeapon[] pickaxes; // 도끼 목록

    // 무기 종류 이름으로 관리 (= string name)
    private Dictionary<string, Gun> gunDictionary = new Dictionary<string, Gun>();
    private Dictionary<string, CloseWeapon> handDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> axeDictionary = new Dictionary<string, CloseWeapon>();
    private Dictionary<string, CloseWeapon> pickaxeDictionary = new Dictionary<string, CloseWeapon>();

    // 필요한 컴포넌트
    // 한 종류 무기의 컨트롤러만 켜기 위해 모든 무기의 컨트롤러 받아옴
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
        // 무기 딕셔너리 초기화
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
                // 무기 교체 실행 (맨손)
                StartCoroutine(ChangeWeaponCoroutine("HAND", "맨손"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                // 무기 교체 실행 (서브머신건)
                StartCoroutine(ChangeWeaponCoroutine("GUN", "SubMachineGun1"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                // 무기 교체 실행 (도끼)
                StartCoroutine(ChangeWeaponCoroutine("AXE", "Axe"));
            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                // 무기 교체 실행 (곡괭이)
                StartCoroutine(ChangeWeaponCoroutine("PICKAXE", "Pickaxe"));
            }
        }
    }


    public IEnumerator ChangeWeaponCoroutine(string _type, string _name)
    {
        isChangeWeapon = true; // 이 코루틴 끝날 때까지 TryChaneWeapon() 무시 (= 무기교체 x)

        // 무기 교체 애니메이션 실행, 애니메이션 실행 동안 딜레이 부여
        currentWeaponAnim.SetTrigger("Weapon_Out");
        yield return new WaitForSeconds(changeWeaponDelayTime);

        // ################# 무기 교체를 실행하는 부분 ####################
        
        // 현재 무기와 그 무기의 controller을 비활성화
        CancelPreWeaponAction();

        // 바꿀 무기와 그 무기의 controller을 활성화
        WeaponChange(_type,_name);

        // ###################################################################

        yield return new WaitForSeconds(changeWeaponEndDelayTime);

        isChangeWeapon = false; // 다시 무기 교체 가능한 상태
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
        isChangeWeapon = true; // 이 코루틴 끝날 때까지 TryChaneWeapon() 무시 (= 무기교체 x)

        // 무기 교체 애니메이션 실행, 애니메이션 실행 동안 딜레이 부여
        currentWeaponAnim.SetTrigger("Weapon_Out");

        yield return new WaitForSeconds(changeWeaponDelayTime);

        currentWeapon.gameObject.SetActive(false);
    }

    public void WeaponOut()
    {
        isChangeWeapon = false; // 이 코루틴 끝날 때까지 TryChaneWeapon() 무시 (= 무기교체 x)

        currentWeapon.gameObject.SetActive(true);
    }

}

