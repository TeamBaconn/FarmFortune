using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorSetter : MonoBehaviour
{
    public Vector2 direction;
    private void Start()
    {
        Animator animator = GetComponent<Animator>();
        animator.SetFloat("horizontal", direction.x);
        animator.SetFloat("vertical", direction.y);
    }
}
