using UnityEngine;
using UnityEngine.AI;

//[RequireComponent(typeof(Animator))]
//[RequireComponent(typeof(Rigidbody))]
//[RequireComponent(typeof(NavMeshAgent))]
public class BasicEnemyAi : MonoBehaviour
{
	// Movement and Rotation
	public float rotationSpeed = 100f; // Speed of rotation
	public float maxSpeed = 5f;       // Movement speed
	public float patrolSpeed = 3f;    // Speed for patrolling

	// Components
	private Animator animator;
	private Rigidbody rb;
	[SerializeField] private NavMeshAgent agent;

	// Player Detection
	[SerializeField] public Transform player;
	public LayerMask whatIsGround, whatIsPlayer;
	public float sightRange, attackRange, fieldOfView = 90f;

	// Patrol
    private Vector3 patrolStartPoint;
	private Vector3 patrolEndPoint;
	private bool movingToEndPoint = true;
	public float patrolDistance = 10f;

	// Attack
	public float timeBetweenAttacks;
	private bool alreadyAttacked;
	public GameObject bulletPrefab;
	public Transform bulletSpawnPoint;

	// Health
	public float health = 100;

	// States
	private bool playerInSightRange, playerInAttackRange, inView = true;

	private void Awake()
	{
		animator = GetComponent<Animator>();
		rb = GetComponent<Rigidbody>();
		//agent = GetComponent<NavMeshAgent>();
		//player = GameObject.FindGameObjectsWithTag("Player")[0].transform;

		// Set patrol points
		patrolStartPoint = transform.position;
		patrolEndPoint = patrolStartPoint + transform.forward * patrolDistance;
	}

	private void Update()
	{
		// Update player detection states
		playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer) && IsPlayerInFront();
		playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer) && IsPlayerInFront();

		if (health > 0)
		{
			if (!playerInSightRange && !playerInAttackRange)
			{
				Patroling();
			}
			else if (playerInSightRange && !playerInAttackRange)
			{
				if (inView)
				{
					inView = false;
				}
				ChasePlayer();
			}
			else if (playerInAttackRange && playerInSightRange)
			{
				AttackPlayer();
			}
		}
		else
		{
			Die();
		}
	}

	private void Patroling()
	{
		

		if (movingToEndPoint)
		{
			agent.SetDestination(patrolEndPoint);
			if (Vector3.Distance(transform.position, patrolEndPoint) < 2f)
				movingToEndPoint = false;
		}
		else
		{
			agent.SetDestination(patrolStartPoint);
			if (Vector3.Distance(transform.position, patrolStartPoint) < 2f)
				movingToEndPoint = true;
		}

		agent.speed = patrolSpeed;
		animator.SetBool("isWalking", true); // Walk animation
	}

	private void ChasePlayer()
	{
		Debug.Log("Chasing Player");

		agent.SetDestination(player.position);
		animator.SetBool("isWalking", true); // Run animation

		// Rotate toward the player
		Vector3 direction = (player.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
	}

	private void AttackPlayer()
	{
		Debug.Log("Attacking Player");
		agent.SetDestination(transform.position); // Stop moving

		player.GetComponent<PlayerHealth>().TakeDamage(10f); // Apply damage to the player
															 // Rotate toward the player
		Vector3 direction = (player.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

		if (!alreadyAttacked)
		{
			//animator.SetBool("isWalking", false); // Run animation
			GameObject bullet = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);

			Rigidbody rb = bullet.GetComponent<Rigidbody>();
			if (rb != null)
			{
				rb.velocity = bulletSpawnPoint.forward * 30f;
			}
			alreadyAttacked = true;

			Invoke(nameof(ResetAttack), timeBetweenAttacks);
		}
	}

	private void ResetAttack()
	{
		alreadyAttacked = false;
		//animator.SetBool("isWalking", true);
	}

	public void TakeDamage(int damage)
	{
		health -= damage;

		if (health <= 0) Invoke(nameof(Die), 0.5f);
	}

	private void Die()
	{
		Debug.Log("Enemy Died");
		animator.SetTrigger("Die");
		animator.ResetTrigger("Die");
		Destroy(gameObject, 2f);
	}

	private bool IsPlayerInFront()
	{
		if (player == null) return false;

		Vector3 directionToPlayer = (player.position - transform.position).normalized;
		float angleToPlayer = Vector3.Angle(transform.forward, directionToPlayer);

		return angleToPlayer <= fieldOfView / 2f;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		Gizmos.DrawWireSphere(transform.position, sightRange);

		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, attackRange);

		// Draw FOV cone
		Gizmos.color = Color.blue;
		Vector3 fovLine1 = Quaternion.Euler(0, fieldOfView / 2, 0) * transform.forward * sightRange;
		Vector3 fovLine2 = Quaternion.Euler(0, -fieldOfView / 2, 0) * transform.forward * sightRange;
		Gizmos.DrawLine(transform.position, transform.position + fovLine1);
		Gizmos.DrawLine(transform.position, transform.position + fovLine2);

		// Draw patrol path
		Gizmos.color = Color.green;
		Gizmos.DrawLine(patrolStartPoint, patrolEndPoint);
	}
}
