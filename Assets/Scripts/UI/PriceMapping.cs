using TMPro;
using UnityEngine;

public class PriceMapping : MonoBehaviour
{
    private TextMeshProUGUI TMPMonsterPrice = null;
    private string monsterType = string.Empty;
    
    private void Awake()
    {
        TMPMonsterPrice = GetComponent<TextMeshProUGUI>();
        monsterType = gameObject.transform.parent.parent.name;
        TMPMonsterPrice.text = Managers.Data.monsterDict[monsterType].monsterPrice.ToString();
    }
}
