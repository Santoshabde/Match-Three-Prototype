using System.Linq;
using UnityEngine;

namespace SNGames.CommonModule
{
    public class RagDollSystem : MonoBehaviour
    {
        [SerializeField] private Collider[] initialCollidersToActivate;
        [SerializeField] private Rigidbody[] initialRigidBodiesToActivate;

        private Rigidbody[] rigidbodies;
        private Collider[] colliders;
        private Animator animator;

        protected virtual void Awake()
        {
            rigidbodies = GetComponentsInChildren<Rigidbody>();
            colliders = GetComponentsInChildren<Collider>();
            animator = GetComponent<Animator>();

            DeactivateRagDoll();
        }

        public virtual void ActivateRagDoll(Vector3 force = default, Transform forcePoint = null)
        {
            if (animator) animator.enabled = false;

            rigidbodies.ToList().ForEach(rb =>
            {
                rb.isKinematic = false;
                rb.detectCollisions = true;
            });

            colliders.ToList().ForEach(col => col.enabled = true);

            if (force != Vector3.zero && forcePoint != null)
            {
                rigidbodies.ToList().ForEach(rb => rb.AddForceAtPosition(force, forcePoint.position, ForceMode.Impulse));
            }
        }

        public virtual void DeactivateRagDoll()
        {
            if (animator) animator.enabled = true;

            rigidbodies.ToList().ForEach(rb =>
            {
                rb.isKinematic = true;
                rb.detectCollisions = false;
            });

            colliders.ToList().ForEach(col => col.enabled = false);

            if (initialCollidersToActivate != null)
                initialCollidersToActivate.ToList().ForEach(col => col.enabled = true);

            if (initialRigidBodiesToActivate != null)
                initialRigidBodiesToActivate.ToList().ForEach(rb => rb.isKinematic = false);
        }
    }
}
