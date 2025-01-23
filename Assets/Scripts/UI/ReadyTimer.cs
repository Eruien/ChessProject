using Assets.Scripts;
using TMPro;
using UnityEngine;

public class ReadyTimer : MonoBehaviour
{
    private TextMeshProUGUI TMPTimer = null;
    public static float currentTime = 99;
    private bool IsGameStart = false;

    private void Awake()
    {
        TMPTimer = GetComponent<TextMeshProUGUI>();
        TMPTimer.text = currentTime.ToString();
    }

    private void Update()
    {
        if (!IsGameStart)
        {
            currentTime -= Time.deltaTime;
            TMPTimer.text = currentTime.ToString("0");

            if (currentTime <= 0)
            {
                currentTime = 99;
                TimeUpGameStart();
            }
        }
       
    }

    private void TimeUpGameStart()
    {
        C_GameStartPacket gameStartPacket = new C_GameStartPacket();
        gameStartPacket.m_IsGameStart = true;
        IsGameStart = true;

        SessionManager.Instance.GetServerSession().Send(gameStartPacket.Write());
    }
}
