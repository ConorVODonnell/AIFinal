using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    Transform target;
    NavMeshAgent agent;
    int currentNode;
    int previousNode;
    public bool diamondPatrol;
    List<Transform> myPatrol = new List<Transform>();

    public enum EnemyState
    {
        patrol,
        chase
    };

    EnemyState enemyState = EnemyState.patrol;

    void Start()
    {
        ChoosePatrol();
        agent = GetComponent<NavMeshAgent>();
        currentNode = Random.Range(0, myPatrol.Count);
        previousNode = currentNode;
        Patrol();
    }

    void Update()
    {
        switch(enemyState)
        {
            case EnemyState.patrol: Patrol();  break;
            case EnemyState.chase: Chase();  break;
            default: break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "node")
        {
            previousNode = currentNode;
            while (currentNode == previousNode)
                currentNode = Random.Range(0, myPatrol.Count);

        } else

        if(other.tag == "Player")
            enemyState = EnemyState.chase;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            enemyState = EnemyState.patrol;
    }

    void ChoosePatrol()
    {
        if (diamondPatrol)
        {
            for (int i = 0; i < 4; i++)
            {
                myPatrol.Add(GameManager.gm.nodes[i]);
            }
        }
        else
            for (int i = 4; i < 6; i++)
            {
                myPatrol.Add(GameManager.gm.nodes[i]);
            }
    }

    void Patrol()
    {
        agent.destination = myPatrol[currentNode].transform.position;

    }

    void Chase()
    {
        agent.destination = GameManager.gm.player.transform.position;
    }
}
