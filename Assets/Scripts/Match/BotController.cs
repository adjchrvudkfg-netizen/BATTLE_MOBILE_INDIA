using UnityEngine;
using UnityEngine.AI;
using BattleMobileIndia.Gameplay;

namespace BattleMobileIndia.Match
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class BotController : MonoBehaviour, IDamageable
    {
        public float health = 100f;
        public float roamRadius = 30f;
        public float repickInterval = 4f;

        private NavMeshAgent _agent;
        private float _timer;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            PickNewDestination();
        }

        private void Update()
        {
            _timer += Time.deltaTime;
            if (_timer >= repickInterval || (!_agent.pathPending && _agent.remainingDistance <= 1.2f))
            {
                _timer = 0f;
                PickNewDestination();
            }
        }

        private void PickNewDestination()
        {
            Vector3 randomDir = Random.insideUnitSphere * roamRadius;
            randomDir += transform.position;

            if (NavMesh.SamplePosition(randomDir, out NavMeshHit hit, roamRadius, NavMesh.AllAreas))
            {
                _agent.SetDestination(hit.position);
            }
        }

        public void TakeDamage(float amount)
        {
            health -= amount;
            if (health <= 0f)
            {
                health = 0f;
                Destroy(gameObject);
            }
        }
    }
}

