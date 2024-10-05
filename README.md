# Marlon V. - Unity Technical Test Submission (Skillionaire)

## Brief Summary of Approaches to Fixes

## 1. Fix Existing Issue: Tokens Not Visible
While the cause wasn't immediately obvious, I eventually noticed the TokenSpin.png sprite had incorrect Import Settings. Changing the following fixed this display issue, and its animation were still intact in runtime: <br>
- Its Texture Type needed to be changed from Default to Sprite<br>
- Since it is a spritesheet, its Sprite Mode had to be set to Multiple.

## 2. New Feature: Implement Double Jump
In PlayerController.cs, I added new variables for jumpCount and maxJumps to handle double jump logic while in mid-air in the Update() and ComputeVelocity() methods. Then in the UpdateJumpState() method, I reset the jumpCount to zero upon landing.

## 3. Add UI: Token Counter & User Information
- I added two TextMeshProUGUI text elements to a new Canvas named "HUD"; one to display the Score, and one to display the currently logged-in user's email address. I also changed the Canvas Scaler component to 'Scale with Screen Size' for proper scaling across all [landscaped] form factors. <br>
- Created new class named TokenCollectionUI Manager.cs to handle the score incrementing and also animate the updated score text, using a simple UnityAction implementation. I added this class to the 'Tokens' gameobject. 

## 4. User Authentication Integration
- Created a new login Scene named LoginScene.unity, with TMP_InputFields for name and password, as well as a Login button. As with the HUD, I also adjusted the Canvas Scaler component for proper scaling across all [landscaped] form factors.<br>
- Created a LoginManager class, where I referenced the APIs in the provided Documentation. I used my Postman desktop client to test the 'POST' data retrieval call from the auth/login endpoint using the provided Fakerson credentials. In Postman I also tested the auth/authenticated-user-details endpoint via GET.<br>
- In Play Mode, if either of the input fields are blank or the entered credentials are wrong, you get a 401 error. If the connection is otherwise unsuccessful, it lets you know in the Console. If authentication is successful, yellow 'Success!' text appears above the Login button, along with displaying the user's email for 2 seconds before proceeding to load the main game scene (SampleScene.unity). I then save the JWT access token returned by the API as a string to PlayerPrefs.
- I then created a new class named EmailDisplay.cs and attached it to the HUD gameobject. Per the instructions, this class displays the saved JWT string in the EmailText (TMP) game object I have at the bottom left of the screen.

### Issues I ran into for Item #4 were:
- My initial endpoint urls were incorrectly typed.
- The JWT accessToken also kept returning as 'null'. Upon debugging in Visual Studio, I realized it was nested in the JSON object inside another layer named "data".
- The email address was also nested inside two layers named "data" and "user". After changing the JWTResponse and UserDetails classes to reflect these hierarchies, both the JTW accessToken and the user email retrieval worked properly.


## Thank you. I enjoyed this code challenge!
