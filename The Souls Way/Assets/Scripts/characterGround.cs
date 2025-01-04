using UnityEngine;

//This script is used by both movement and jump to detect when the character is touching the ground

public class characterGround : MonoBehaviour
{
        private bool onGround;
       
        [Header("Collider Settings")]
        [SerializeField] private float distanceToGround = 1.15f;
        [SerializeField] private Vector3 rayOffset1;
        [SerializeField] private Vector3 rayOffset2;

        [Header("Layer Masks")]
        [SerializeField] private LayerMask groundLayer;
 

        private void Update()
        {
            //Determine if the player is stood on objects on the ground layer, using a pair of raycasts
            onGround = Physics2D.Raycast(transform.position + rayOffset1, Vector2.down, distanceToGround, groundLayer) || Physics2D.Raycast(transform.position + rayOffset2, Vector2.down, distanceToGround, groundLayer);
        }

        private void OnDrawGizmos()
        {
            //Draw the ground colliders on screen for debug purposes
            if (onGround) { Gizmos.color = Color.green; } else { Gizmos.color = Color.red; }
            Gizmos.DrawLine(transform.position + rayOffset1, transform.position + rayOffset1 + Vector3.down * distanceToGround);
            Gizmos.DrawLine(transform.position + rayOffset2, transform.position + rayOffset2 + Vector3.down * distanceToGround);
        }

        //Send ground detection to other scripts
        public bool GetOnGround() { return onGround; }
}