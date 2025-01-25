using Assets.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class GameDecision : MonoBehaviour
{
    public static UnityEvent<Team> EndGameEvent = new UnityEvent<Team>();
    private TextMeshProUGUI TMPResult = null;
    
    private void Awake()
    {
        TMPResult = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        EndGameEvent.AddListener(WinDecision);
    }

    private void OnDisable()
    {
        EndGameEvent.RemoveListener(WinDecision);
    }

    private void WinDecision(Team team)
    {
        if (team == Global.g_MyTeam)
        {
            TMPResult.text = "ÆÐ¹è";
        }
        else
        {
            TMPResult.text = "½Â¸®";
        }
    }
}
