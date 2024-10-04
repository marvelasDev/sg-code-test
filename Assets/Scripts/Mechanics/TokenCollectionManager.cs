using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;
using TMPro;

namespace Platformer.Mechanics
{
    public class TokenCollectionManager : MonoBehaviour
    {
        public static TokenCollectionManager Instance;
        Animator animator;// = tokenCountText.GetComponent<Animator>();

        public TextMeshProUGUI tokenCountText;

        private int tokenCount = 0;

        // MV - Use a Unity Action for token collection event (cleaner than delegate/event combo)
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

        // Update the token UI and animate the change.
        private void UpdateTokenUI()
        {
            tokenCountText.text = /*"Score: "*/tokenCount.ToString();

            AnimateTokenUI();
        }

        // Simple scale animation for the UI.
        private void AnimateTokenUI()
        {
            // Assuming we have an Animator attached to the UI element for animation.
            //Animator animator = tokenCountText.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("Collect");
            }
            //else
            //{
            //    // If no Animator, use a simple scale animation.
            //    StartCoroutine(ScaleText());
            //}
        }

        // Coroutine for a simple scale effect on the token count text.
        /*private IEnumerator ScaleText()
        {
            Vector3 originalScale = tokenCountText.transform.localScale;
            tokenCountText.transform.localScale = originalScale * 1.2f;

            yield return new WaitForSeconds(0.2f);

            tokenCountText.transform.localScale = originalScale;
        }*/
    }
}