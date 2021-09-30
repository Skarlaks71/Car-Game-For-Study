using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIParking : MonoBehaviour
{
    private CarController carController;
    private float verticalMove;
    private float horizontalMove;

    public float speedToTarget = 5f;
    public float reachedTargetDst = 7f;
    public float reverseToDst = 10f;
    public NavMeshAgent agent;
    public Vector3 targetPosition;
    public Transform targetTransform;

    private void Start()
    {
        carController = GetComponent<CarController>();
        //target = agent.transform.position;
    }

    private void Update()
    {
        SetTargetPosition(targetTransform.position);
        verticalMove = 0f;
        horizontalMove = 0f;

        float distanceToTarget = (targetPosition - transform.position).magnitude;
        //Debug.Log("DstToTarget: "+distanceToTarget);
        if (distanceToTarget > reachedTargetDst)
        {
            Vector3 dirToMovePos = (targetPosition - transform.position).normalized;
            float dot = Vector3.Dot(transform.forward, dirToMovePos);

            if (dot > 0)
            {
                // Target in front
                verticalMove = 1f;

                float stopDst = 2.5f;
                float stopSpeed = 10f;

                if(distanceToTarget < stopDst && carController.GetSpeed() > stopSpeed)
                {
                    verticalMove = -1f;
                }
            }
            else
            {
                if(distanceToTarget > reverseToDst)
                {
                    verticalMove = 1f;
                }
                else
                {
                    verticalMove = -1f;
                }
            }

            float angleToDir = Vector3.SignedAngle(transform.forward, dirToMovePos, Vector3.up);
            //Debug.Log(angleToDir);
            if (angleToDir > 0)
            {
                horizontalMove = 1f;
            }
            else
            {
                horizontalMove = -1f;
            }
        }
        else
        {
            if(carController.GetSpeed() > speedToTarget)
            {
                verticalMove = -1f;
            }
            else
            {
                verticalMove = 0f;
            }
            horizontalMove = 0f;
        }
        
        carController.GetExternalInputs(verticalMove, horizontalMove);

    }

    public void SetTargetPosition(Vector3 targetPos)
    {
        targetPosition = targetPos;
    }

}
