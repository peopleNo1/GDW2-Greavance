using UnityEngine;

[RequireComponent (typeof(Animator))]
public class Animated : MonoBehaviour
{
    private Animator animator;
    [SerializeField] bool isTrue = false;
    [SerializeField] string boolName;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void Flip()
    {
        isTrue = !isTrue;
        animator.SetBool(boolName, isTrue);
        
    }
}
