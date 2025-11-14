using UnityEngine;

public class RagdollController : MonoBehaviour
{
    private Rigidbody[] ragdollBodies;
    private Collider[] ragdollColliders;

    private Animator animator;
    private Rigidbody rootRb;
    private Collider rootCollider;

    void Awake()
    {
        animator = GetComponent<Animator>();

        // Le rigidbody + collider principal du zombie (capsule)
        rootRb = GetComponent<Rigidbody>();
        rootCollider = GetComponent<Collider>();

        ragdollBodies = GetComponentsInChildren<Rigidbody>();
        ragdollColliders = GetComponentsInChildren<Collider>();

        DisableRagdoll();
    }

    public void EnableRagdoll()
    {
        animator.enabled = false;

        rootRb.isKinematic = true;
        rootCollider.enabled = false;

        foreach (var rb in ragdollBodies)
            rb.isKinematic = false;

        foreach (var col in ragdollColliders)
            col.enabled = true;
    }

    public void EnableRagdoll(Vector3 impactDirection, float force)
    {
        animator.enabled = false;

        // On désactive la racine car la physique prend le relai
        rootRb.isKinematic = true;
        rootCollider.enabled = false;

        foreach (var rb in ragdollBodies)
        {
            rb.isKinematic = false;
            rb.AddForce(impactDirection * force, ForceMode.Impulse);
        }

        foreach (var col in ragdollColliders)
            col.enabled = true;
    }


    public void DisableRagdoll()
    {
        animator.enabled = true;

        rootRb.isKinematic = false;
        rootCollider.enabled = true;

        foreach (var rb in ragdollBodies)
        {
            if (rb != rootRb)
                rb.isKinematic = true;
        }

        foreach (var col in ragdollColliders)
        {
            if (col != rootCollider)
                col.enabled = false;
        }
    }
}

