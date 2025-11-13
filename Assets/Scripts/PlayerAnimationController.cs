using UnityEngine;
using UnityEngine.Rendering;


public class PlayerAnimationController : MonoBehaviour
{
    public enum animID
    {
        IDLE,
        JUMP,
        FALL,
        WALK,
        RUN
    };
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public RuntimeAnimatorController jumpAnim;
    public RuntimeAnimatorController fallAnim;
    public RuntimeAnimatorController idleAnim;
    public RuntimeAnimatorController walkAnim;
    public RuntimeAnimatorController runAnim;

    Animator source;

    void Start()
    {
        source = GetComponent<Animator>();
    }

    public void PlayAnimation(animID id)
    {
        switch (id)
        {
            case animID.JUMP:
                source.runtimeAnimatorController = jumpAnim;
                break;
            case animID.WALK:
                source.runtimeAnimatorController = walkAnim;
                break;;
            case animID.FALL:
                source.runtimeAnimatorController = fallAnim;
                break;
            case animID.IDLE:
                source.runtimeAnimatorController = idleAnim;
                break;
            case animID.RUN:
                source.runtimeAnimatorController = runAnim;
                break;
            default:
                return;
        }
    }
}
