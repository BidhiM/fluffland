using UnityEngine;

public class PipeAnimationController : MonoBehaviour
{
    [SerializeField] private Animator pipeAnimator;

    void Start()
    {
        if (pipeAnimator == null) pipeAnimator = GetComponent<Animator>();
        //find the component if its not there
    }

    public void TriggerPipeAnimation()
    {
        if (pipeAnimator != null) pipeAnimator.SetBool("First Click", true);
    }
}