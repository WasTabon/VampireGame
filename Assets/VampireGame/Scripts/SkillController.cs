using System;
using TMPro;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    public static SkillController Instance;

    [SerializeField] private TextMeshProUGUI _moneyText;
    
    [SerializeField] private GameObject _lieSkillBuyButton;
    [SerializeField] private GameObject _invisibleSkillBuyButton;

    public int money;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        string lie = PlayerPrefs.GetString("lie", "no");
        if (lie == "no")
        {
            _lieSkillBuyButton.gameObject.SetActive(true);
        }
        else
        {
            _lieSkillBuyButton.gameObject.SetActive(false);
        }
        
        string invisible = PlayerPrefs.GetString("invisible", "no");
        if (invisible == "no")
        {
            _invisibleSkillBuyButton.gameObject.SetActive(true);
        }
        else
        {
            _invisibleSkillBuyButton.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        _moneyText.text = money.ToString();
    }

    public void SetActiveSkill(string skill)
    {
        PlayerPrefs.SetString("skill", skill);
        PlayerPrefs.Save();
    }

    public void BuyLieSkill()
    {
        if (money >= 50)
        {
            money -= 50;
            _lieSkillBuyButton.gameObject.SetActive(false);
            PlayerPrefs.SetString("lie", "yes");
        }
    }
    public void BuyInvisibleSkill()
    {
        if (money >= 50)
        {
            money -= 50;
            _invisibleSkillBuyButton.gameObject.SetActive(false);
            PlayerPrefs.SetString("invisible", "yes");
        }
    }
}
