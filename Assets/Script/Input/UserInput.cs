using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Player))]
public class UserInput : StateMachine
{
    [HideInInspector]
    public Item holdItem;

    [HideInInspector]
    public Player targetPlayer;

    [HideInInspector]
    public Vector2 initPosition;

    [Range(0.0f, 1.0f)]
    public float edgeBoundaryPercent = 0.3f;
    public float maxSpeed = 10f;
    public float minSpeed = 0f;

    private void Awake()
    {
        initPosition = transform.position;
        targetPlayer = GetComponent<Player>();

        defaultState = "AssignFarmState";
        stateList.Add(new AssignFarmState(this));
        stateList.Add(new PickItemState(this));
    }
    private void OnEnable()
    {
        EventManager.StartListening<OnItemPickUp>(OnItemPickUp);
        EventManager.StartListening<OnItemDrop>(OnItemDrop);
    }

    private void OnDisable()
    {
        EventManager.StopListening<OnItemPickUp>(OnItemPickUp);
        EventManager.StopListening<OnItemDrop>(OnItemDrop);
    }

    private void OnItemPickUp(EventParam param)
    {
        OnItemPickUp eventParam = param as OnItemPickUp;
        if (!eventParam.player.Equals(targetPlayer)) return;
        holdItem = eventParam.item;
        ChangeState("PickItemState");
    }

    private void OnItemDrop(EventParam param)
    {
        OnItemDrop eventParam = param as OnItemDrop;
        if (!eventParam.player.Equals(targetPlayer)) return;
        holdItem = null;
        ResetState();
    }

    protected override void Update()
    {
        base.Update();

    }
}
