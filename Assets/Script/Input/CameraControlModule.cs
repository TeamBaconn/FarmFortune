using UnityEngine;

public class CameraControlModule : IStateModule
{
    private UserInput input;
    public CameraControlModule(UserInput input)
    {
        this.input = input;
    }
    public void Enter()
    { 
    }

    public void Exit()
    { 
    }

    public void UpdateLogic()
    {
        float edgeBoundary = Screen.height * input.edgeBoundaryPercent;
        float edgeDistance = Mathf.Min(Screen.height - Input.mousePosition.y, Input.mousePosition.y);
        float direction = Mathf.Sign(Screen.height / 2 - Input.mousePosition.y) * -1;
        if (edgeBoundary >= edgeDistance)
        { 
            float speed = Mathf.Lerp(input.maxSpeed, input.minSpeed, edgeDistance/edgeBoundary);
            input.transform.position += Vector3.up * (direction * speed * Time.deltaTime);

            //Fix outbound
            float land = (Mathf.CeilToInt((input.targetPlayer.landManager.availableLands.Count - 1) / Global.MAX_LAND_PER_SQUARE) + 1) * Global.SQUARE_OFFSET_Y;
            if (input.transform.position.y > input.initPosition.y)
            {
                Vector3 fixPos = input.transform.position;
                fixPos.y = input.initPosition.y;
                fixPos.z = -10;
                input.transform.position = fixPos;
            }else if(input.transform.position.y < input.initPosition.y + land)
            {
                Vector3 fixPos = input.transform.position;
                fixPos.y = input.initPosition.y + land;
                fixPos.z = -10;
                input.transform.position = fixPos;
            }
        }

    }

    public void UpdatePhysics()
    { 
    }
}