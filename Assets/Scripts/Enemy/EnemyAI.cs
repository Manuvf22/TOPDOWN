using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float detectionRange = 15f;
    [SerializeField] private float stopDistance = 1.5f;

    protected NavMeshAgent agent;
    protected Transform player;
    protected bool playerDetected;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    protected virtual void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        playerDetected = distance <= detectionRange;

        if (playerDetected)
            OnPlayerDetected();
        else
            OnPlayerLost();
    }

    protected virtual void OnPlayerDetected()
    {
        if (Vector3.Distance(transform.position, player.position) > stopDistance)
            agent.SetDestination(player.position);
        else
            agent.ResetPath();
    }

    protected virtual void OnPlayerLost()
    {
        agent.ResetPath();
    }
}