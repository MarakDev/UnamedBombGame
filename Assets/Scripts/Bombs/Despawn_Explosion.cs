using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Despawn_Explosion : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        AnimatorStateInfo stateinfo = animator.GetCurrentAnimatorStateInfo(0);

        if (stateinfo.normalizedTime >= 0.95f)
            Destroy(this.gameObject);
    }
}
