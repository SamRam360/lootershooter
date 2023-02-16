using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TopDown.Core
{
    public class Ladder : MonoBehaviour
    {
        private PlayerMovement playerMovement;
        private bool isPlayerOnLadder = false;

        void Start()
        {
            // Get the PlayerMovement component from the player object
            playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerOnLadder = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                isPlayerOnLadder = false;
            }
        }

        public bool IsPlayerOnLadder()
        {
            return isPlayerOnLadder;
        }

        public void DetachPlayer(GameObject player)
        {
            // Set the player's isOnLadder property to false
            playerMovement.SetIsOnLadder(false);

            // Unparent the player from the ladder object
            player.transform.parent = null;
        }
    }
}
