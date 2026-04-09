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

    public void SetAni(bool yea)
    {
        isTrue = yea;
        animator.SetBool(boolName, isTrue);
    }
}
