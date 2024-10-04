using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using TMPro;
using UnityEngine.SceneManagement;

namespace Platformer.Auth
{
    public class LoginManager : MonoBehaviour
    {
        public TMP_InputField usernameField;
        public TMP_InputField passwordField;
        public Button loginButton;
        public TextMeshProUGUI resultText;

        private string loginEndpoint = "https://api-dev.skillionairegames.com/api/auth/login";
        private string userDetailsEndpoint = "https://api-dev.skillionairegames.com/api/auth/authenticated-user-details";

        void Start()
        {
            loginButton.onClick.AddListener(OnLoginButtonClicked);
        }

        void OnLoginButtonClicked()
        {
            string username = usernameField.text;
            string password = passwordField.text;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                resultText.text = "Please enter both username and password.";
            }
            else
            {
                StartCoroutine(Login(username, password));
            }
        }

        IEnumerator Login(string username, string password)
        {
            // MV - Prepare the form data
            WWWForm form = new WWWForm();
            form.AddField("name", username);
            form.AddField("password", password);

            // MV - Make the login request
            using (UnityWebRequest www = UnityWebRequest.Post(loginEndpoint, form))
            {
                yield return www.SendWebRequest();

                // MV - Handle unsuccessful login
                if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.Log("Error: " + www.error);
                    resultText.text = "Error: " + www.error;
                }
                else if (www.responseCode == 404)
                {
                    Debug.Log("Error: Endpoint not found (404). Please check the URL.");
                    resultText.text = "Error: Endpoint not found (404). Please check the URL.";
                }
                else
                {
                    // MV - Handle successful login
                    var jsonResponse = www.downloadHandler.text;
                    Debug.Log("Login successful: " + jsonResponse);

                    // MV - Parse the JSON response to get JWT token
                    JWTResponse jwtResponse = JsonUtility.FromJson<JWTResponse>(jsonResponse);
                    //JWTResponse jwtResponse = JsonConvert.DeserializeObject<JWTResponse>(jsonResponse); //initial NewtonSoftJson version that became two cumbersome to use


                    Debug.Log(jwtResponse.data.accessToken);  // MV - Now, access the correct remote field

                    if (!string.IsNullOrEmpty(jwtResponse.data.accessToken))
                    {
                        // MV - Save JWT locally, per the spec
                        PlayerPrefs.SetString("jwtToken", jwtResponse.data.accessToken);
                        PlayerPrefs.Save();

                        StartCoroutine(GetUserDetails(jwtResponse.data.accessToken));
                    }
                    else
                    {
                        resultText.text = "Login failed. Invalid username or password.";
                    }

                    // MV - Brief pause before loading main game, for an initial visualconfirmation of correct email address right here on Login screen
                    Invoke("LoadGameScene", 2.0f); 
                }
            }
        }

        private void LoadGameScene()
        {
            SceneManager.LoadSceneAsync("SampleScene");
        }

        IEnumerator GetUserDetails(string token)
        {
            UnityWebRequest www = UnityWebRequest.Get(userDetailsEndpoint);
            www.SetRequestHeader("Authorization", "Bearer " + token);

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                resultText.text = "Error fetching user details: " + www.error;
            }
            else
            {
                var userDetailsResponse = www.downloadHandler.text;
                UserDetails userDetails = JsonUtility.FromJson<UserDetails>(userDetailsResponse);
                resultText.text = "Success! Logged in as: " + userDetails.data.user.email;

                if (!string.IsNullOrEmpty(userDetails.data.user.email))
                {
                    //// MV - Makes email address persist across scenes
                    PlayerPrefs.SetString("emailAddress", userDetails.data.user.email);
                    PlayerPrefs.Save();
                }
            }


        }
    }

    [System.Serializable]
    public class JWTResponse
    {
        public Data data;  // MV - AccessToken sits in a Nested structure. Without this data prefix, accessToken always returns null

        [System.Serializable]
        public class Data
        {
            public string accessToken;  // MV - JWT token is nested inside 'data' on the backend
        }
    }

    [System.Serializable]
    public class UserDetails
    {
        public Data data;  // MV - This represents the "data" parent object

        [System.Serializable]
        public class Data
        {
            public User user;  // MV - This represents the "user" object inside "data"
        }

        [System.Serializable]
        public class User
        {
            public string email;  // MV - This represents the "email" field inside "user"
        }
    }
}