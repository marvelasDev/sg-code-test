using UnityEngine;
using TMPro;

namespace Platformer.Auth
{
    public class EmailDisplay : MonoBehaviour
    {
        public TextMeshProUGUI emailText;

        void Start()
        {
            // Retrieve the saved email address from PlayerPrefs
            string savedEmail = PlayerPrefs.GetString("emailAddress", "No email found");

            // Assign it to the TextMeshPro text object
            if (emailText != null)
            {
                emailText.text = savedEmail;
            }
            else
            {
                Debug.LogWarning("Email TextMeshProUGUI is not assigned in the inspector.");
            }
        }
    }
}