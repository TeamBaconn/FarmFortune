using UnityEngine;

[RequireComponent(typeof(Animator))]
public class WorkerEntity : StateMachine
{
    [HideInInspector]
    public Player owner;
    [HideInInspector]
    public Land currentLand;
    [HideInInspector]
    public Vector2 target;
    [HideInInspector]
    public bool isMoving = false;
    [HideInInspector]
    public Animator animator;

    [Header("Stats")]
    public float speed = 1f;
    public float workTime = 120f;

    public void Init(Player owner)
    {
        this.owner = owner;
    }

    private void Awake()
    {
        target = transform.position;

        animator = GetComponent<Animator>();
        defaultState = "WorkerFreeState";
        stateList.Add(new WorkerFreeState(this));
        stateList.Add(new WorkerWorkState(this));

    }

    protected override void Update()
    {
        base.Update();
        
        Vector2 direction = target - (Vector2)transform.position;
        isMoving = direction.magnitude > 0.1f;
        animator.SetBool("isMoving", isMoving);
        if (!isMoving) return;
        direction.Normalize();
        animator.SetFloat("horizontal", direction.x);
        animator.SetFloat("vertical", direction.y);

        transform.position = (Vector2)transform.position + direction * speed * Time.deltaTime;
    }
    
    public void LookAt(Vector2 target)
    {
        Vector2 direction = target - (Vector2)transform.position;
        direction.Normalize();
        animator.SetFloat("horizontal", direction.x);
        animator.SetFloat("vertical", direction.y);
    }
}