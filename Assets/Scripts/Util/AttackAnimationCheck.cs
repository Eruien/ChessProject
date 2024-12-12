using UnityEngine;
using UnityEngine.Events;

public class AttackAnimationCheck : MonoBehaviour
{
    [SerializeField]
    public UnityEvent attackAnimationStartEvent;

    [SerializeField]
    public UnityEvent attackAnimationEndEvent;

    [SerializeField]
    public UnityEvent attackProjectTileEvent;

    private void OnAttackStart()
    {
        attackAnimationStartEvent.Invoke();
    }

    private void OnAttackEnd()
    {
        attackAnimationEndEvent.Invoke();
    }

    private void OnAttackProjectTile()
    {
        attackProjectTileEvent.Invoke();
    }
}
