using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using TMPro;

namespace Platformer.Mechanics
{
    public class TokenCollectionUIManager : MonoBehaviour
    {
        public static TokenCollectionUIManager Instance;
        Animator animator;

        public TextMeshProUGUI tokenCountText;

        private int tokenCount = 0;

        /// <summary> MV - Here, I use a Unity Action for token collection event (it's cleaner than delegate/event & OnEnable/OnDisable combo,
        /// plus we don't have to worry about unsubscribing)
        /// </summary>
        public UnityAction onTokenCollected;

        private void Awake()
        {
            // MV - Singleton to ensure there is only one instance of this manager (This should successfully address on-device race condition edge cases, etc.)
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }

            UpdateTokenUI();
        }

        private void Start()
        {
            // MV - cache the animator reference
            animator = tokenCountText.GetComponent<Animator>();
        }

        // Method to handle token collection.
        public void CollectToken()
        {
            tokenCount++;
            UpdateTokenUI();

            // MV - Trigger the UnityAction when a token is collected.
            if (onTokenCollected != null)
            {
                onTokenCollected.Invoke();
            }
        }

        // Update the token UI text and animate it.
        private void UpdateTokenUI()
        {
            tokenCountText.text = tokenCount.ToString();

            AnimateTokenUI();
        }

        // Simple scale animation for the UI.
        private void AnimateTokenUI()
        {
            if (animator != null)
            {
                animator.SetTrigger("Collect");
            }
        }
    }
}