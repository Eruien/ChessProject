using Assets.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class UserMoney : MonoBehaviour
{
    public static UnityEvent changeUserMoneyText = new UnityEvent();
    private TextMeshProUGUI TMPUserMoney = null;
  
    private void Awake()
    {
        TMPUserMoney = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        changeUserMoneyText.AddListener(ChangeMoneyText);
    }

    private void ChangeMoneyText()
    {
        TMPUserMoney.text = Global.g_UserMoney.ToString();
    }
}
