using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ZombiRagdollManager : MonoBehaviour
{
    [SerializeField] private Transform _player;

    private NavMeshAgent _agent;
    private Animator _animatorAnim;

    private int hitCount = 0;  // <-- Compteur de collisions
    private RagdollController ragdollController;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindWithTag("Player").transform;
        _animatorAnim = GetComponent<Animator>();

        ragdollController = GetComponent<RagdollController>();
    }

    void Update()
    {
        if (_agent.enabled == false)
            return;

        float dist = Vector3.Distance(transform.position, _player.position);

        if (dist > 2.0f)
        {
            _agent.SetDestination(_player.position);
            _animatorAnim.SetBool("isAttacking", false);
        }
        else
        {
            _animatorAnim.SetBool("isAttacking", true);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            hitCount++;

            if (hitCount == 2)
            {
                KillZombie(collision);   // ← pour récupérer la direction de l’impact
            }
            else
            {
                StartCoroutine(MakingDamage());
            }
        }
    }

    void KillZombie(Collision hitInfo = null)
    {
        Debug.Log("Zombie is dead → entering ragdoll state");

        _agent.enabled = false;
        _animatorAnim.enabled = false;

        Vector3 impactDir = Vector3.zero;

        if (hitInfo != null)
            impactDir = (transform.position - hitInfo.contacts[0].point).normalized;
        else
            impactDir = (transform.position - _player.position).normalized;

        ragdollController.EnableRagdoll(impactDir, 8f);  // force réglable
    }


    IEnumerator MakingDamage()
    {
        yield return new WaitForSeconds(1.0f);
        Debug.Log("Player hit by zombie!");
    }
}
