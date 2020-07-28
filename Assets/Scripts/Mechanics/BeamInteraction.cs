using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mechanics
{
    // This class handles all laser beam intercations with the environment
    public class BeamInteraction : MonoBehaviour
    {
        // Variables visible in editor
        [SerializeField] private float speed;
        [SerializeField] private float diffractionStrength;
        [SerializeField] private float maxInteractions;
        [SerializeField] private float onReloadDelay = 2f;
        [SerializeField] private AudioClip interactionSound;
        [SerializeField] private AudioClip shotSound;
        [SerializeField] private AudioClip reloadSound;
        [SerializeField] private GameObject trajectoryOriginal;

        // Private variables;
        private Rigidbody rb;
        private float remainingInteractions;
        private bool isFired;
        private bool canBeFired;
        private bool runOnce;
        private AudioSource beamAudioSource;
        private GameObject trajectory;

        // Unity specific variables
        void Start()
        {
            runOnce = true;
            isFired = false;
            rb = GetComponent<Rigidbody>();
            beamAudioSource = transform.Find("Beam Sound Player").GetComponent<AudioSource>();
            remainingInteractions = maxInteractions;
        }

        void Update()
        {
            if (!isFired) { rb.velocity = Vector3.zero; } // not fired yet
            else { rb.velocity = transform.forward * speed; canBeFired = false; } // in flight

            // after interacting max times
            if (remainingInteractions < 0 && runOnce)
            {
                runOnce = false;
                isFired = false;
                rb.velocity = Vector3.zero;
                transform.Find("Explosion").GetComponent<ParticleSystem>().Play();
                transform.GetChild(0).gameObject.SetActive(false);
                StartCoroutine(Reload());
            }
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.GetComponent<BeamTerminator>()) { remainingInteractions = -1; }
            else { Physics.IgnoreCollision(collision.collider, GetComponent<Collider>(), true); }
        }
        void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Obstacle"))
            {
                remainingInteractions--;

                // Find the normal to the plane 
                Vector3 behindPoint = transform.position - speed * 3f * rb.velocity.normalized * Time.deltaTime;
                Vector3 collisionNormal = Vector3.zero;
                RaycastHit hit;
                if (Physics.Raycast(behindPoint, transform.forward, out hit)) { collisionNormal = hit.normal; }
                transform.position = hit.point;

                // Debug arrow
                Quaternion toNormalRotation = Quaternion.FromToRotation(Vector3.forward, hit.normal);

                // Get collision parameters
                float reflectivity = other.GetComponent<ObstacleProperties>().GetReflectivity();
                float angle = Quaternion.Angle(transform.rotation, other.transform.rotation);

                // Decide whether to reflect or refract
                if (reflectivity > 0) { Reflect(collisionNormal); }
                else { Refract(other.gameObject, collisionNormal); }
            }
            if (other.CompareTag("Diffraction Zone"))
            {
                Diffract(other);
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Obstacle"))
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, - transform.forward, out hit)) 
                {
                    transform.position = hit.point + transform.forward.normalized * 0.3f;
                    RefractOut(other.gameObject, hit.normal);
                }
            }
        }

        // Public Functions
        public float GetRemainingInteractions() { return remainingInteractions; }
        public float GetMaxInteractions() { return maxInteractions; }
        public bool IsFired() { return isFired; }
        public bool CanBeFired() { return canBeFired; }
       
        public void SetCanBeFired(bool canBeFired) { this.canBeFired = canBeFired; }

        public void Fire() 
        {
            if (trajectory != null) { Destroy(trajectory); trajectory = null; }
            isFired = true; 
            transform.GetChild(0).gameObject.SetActive(true);
            trajectory = Instantiate(trajectoryOriginal, transform);
            beamAudioSource.PlayOneShot(shotSound);
        }

        public void SetMaxInteractions(int n)
        {
            maxInteractions = n;
            remainingInteractions = n;
        }

        // Private functions
        private void Reflect(Vector3 planeNormal)
        {
            // Rotate transform to face reflected direction
            Vector3 reflectionDirection = Vector3.Reflect(transform.forward, planeNormal);
            Quaternion toReflectionRotation = Quaternion.FromToRotation(Vector3.forward, reflectionDirection.normalized);
            transform.rotation = toReflectionRotation;

            // Move in the new direction being faced
            rb.velocity = transform.forward * speed;

            // play interaction sound
            beamAudioSource.PlayOneShot(interactionSound);
        }

        private void Refract(GameObject obstacle, Vector3 planeNormal)
        {
            if (obstacle.GetComponent<ObstacleProperties>().GetRefractiveIndex() <= 1f) { return; }
            Debug.Assert(obstacle.CompareTag("Obstacle") && obstacle.GetComponent<ObstacleProperties>().GetRefractiveIndex() >= 1f);

            // Refraction angles
            float incidentAngle = Vector3.Angle(planeNormal, -transform.forward);
            float refractedAngle = Mathf.Rad2Deg * Mathf.Asin(Mathf.Sin(Mathf.Deg2Rad * incidentAngle) / obstacle.GetComponent<ObstacleProperties>().GetRefractiveIndex());

            // Rotate by refracted angle around axis orthogonal to plane normal and direction being faced
            Vector3 rotationAxis = Vector3.Cross(planeNormal, transform.forward);
            transform.Rotate(rotationAxis, refractedAngle);

            // Move in the new direction being faced
            rb.velocity = transform.forward * speed;

            // Play interaction sound
            beamAudioSource.PlayOneShot(interactionSound);
        }

        private void RefractOut(GameObject obstacle, Vector3 planeNormal)
        {
            if (obstacle.GetComponent<ObstacleProperties>().GetRefractiveIndex() <= 1f) { return; }
            Debug.Assert(obstacle.CompareTag("Obstacle") && obstacle.GetComponent<ObstacleProperties>().GetRefractiveIndex() >= 1f);

            // Refraction angles
            float incidentAngle = Vector3.Angle(planeNormal, -transform.forward);
            float refractedAngle = Mathf.Rad2Deg * Mathf.Asin(Mathf.Sin(Mathf.Deg2Rad * incidentAngle) * obstacle.GetComponent<ObstacleProperties>().GetRefractiveIndex());

            // Rotate by refracted angle around axis orthogonal to plane normal and direction being faced
            Vector3 rotationAxis = Vector3.Cross(planeNormal, transform.forward);
            transform.Rotate(rotationAxis, refractedAngle);

            // Move in the new direction being faced
            rb.velocity = transform.forward * speed;

            // Play interaction sound
            beamAudioSource.PlayOneShot(interactionSound);
        }

        private void Diffract(Collider diffZone)
        {
            Debug.Assert(diffZone.gameObject.CompareTag("Diffraction Zone"));

            GameObject obstacle = diffZone.transform.parent.gameObject;
            Bounds obstacleBounds = obstacle.GetComponent<Renderer>().bounds;

            if (transform.position.y > obstacleBounds.min.y && transform.position.y < obstacleBounds.max.y)
            {
                // rotate about y
                Quaternion targetRotation = Quaternion.LookRotation(obstacle.transform.position - transform.position);
                targetRotation.x = 0;
                targetRotation.z = 0;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, diffractionStrength);

            }
            else if (transform.position.z > obstacleBounds.min.z && transform.position.z < obstacleBounds.max.z)
            {
                // rotate about z
                Quaternion targetRotation = Quaternion.LookRotation(obstacle.transform.position - transform.position);
                targetRotation.x = 0;
                targetRotation.y = 0;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, diffractionStrength);
            }
            else
            {
                // rotate about x
                Quaternion targetRotation = Quaternion.LookRotation(obstacle.transform.position - transform.position);
                targetRotation.y = 0;
                targetRotation.z = 0;
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, diffractionStrength);
            }
        }

        private IEnumerator Reload()
        {
            beamAudioSource.PlayOneShot(reloadSound);
            yield return new WaitForSeconds(onReloadDelay);
            canBeFired = true;
            remainingInteractions = maxInteractions;
            runOnce = true;
        }
    }
}

