using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class EnemyAIBase : MonoBehaviour
{
    [Header("AI Configuration")]
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform target;

    private void Awake()
    {
        target = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        agent.SetDestination(target.position);
        transform.LookAt(target);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene(2);
        }
    }
}