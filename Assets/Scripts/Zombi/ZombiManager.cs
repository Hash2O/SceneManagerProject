using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class ZombieManager : MonoBehaviour
{
    [SerializeField] private Transform _player;

    [SerializeField] private Image damageOverlay;
    [SerializeField] private float flashDuration = 0.4f;


    private NavMeshAgent _agent;
    private Animator _animatorAnim;
    private float _damage = 10f;

    // Start is called before the first frame update
    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindWithTag("Player").transform;
        _animatorAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        var dist = Vector3.Distance(transform.position, _player.position);

        if (dist > 2.0f)
        {
            //Au delà d'un mètre, le zombie marche vers lui
            _agent.SetDestination(_player.position);
            _animatorAnim.SetBool("isAttacking", false);
        }
        else
        {
            //_agent.isStopped = true;
            //Moins d'un mêtre, le zombie enclenche l'anim d'attaque
            _animatorAnim.SetBool("isAttacking", true);

        }


    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collision");
            StartCoroutine(MakingDamage());
        }

    }

    IEnumerator MakingDamage()
    {
        StartCoroutine(FlashDamage());

        yield return new WaitForSeconds(1.0f);
        Debug.Log("Player has been hit !");
    }

    IEnumerator FlashDamage()
    {
        float t = 0f;

        // Fade In (0 → 1)
        while (t < flashDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(0f, 1f, t / flashDuration);
            SetOverlayAlpha(alpha);
            yield return null;
        }

        // Fade Out (1 → 0)
        t = 0f;
        while (t < flashDuration)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / flashDuration);
            SetOverlayAlpha(alpha);
            yield return null;
        }

        SetOverlayAlpha(0f);
    }

    void SetOverlayAlpha(float a)
    {
        if (damageOverlay != null)
        {
            Color c = damageOverlay.color;
            c.a = a;
            damageOverlay.color = c;
        }
    }


}


