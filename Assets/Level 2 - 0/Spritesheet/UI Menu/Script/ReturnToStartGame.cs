using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToStartGame : MonoBehaviour
{
    public string targetSentence = "I love cries";
    public string sceneToLoad = "Start Game";

    private string inputText = "";
    private bool isClearingInput = false;

    public Animator anim;

    private void Start()
    {
        // Get the Animator component
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        Player player = FindObjectOfType<Player>();
        GameManager gameManager = FindObjectOfType<GameManager>();

        // Listen for user input
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // Check if the entered sentence matches the target sentence
            if (inputText.Trim().ToLower() == targetSentence.ToLower())
            {
                // Unpause the game
                Time.timeScale = 1f;

                // Access the player's hitpoint and gameManager's dollar directly
                if (player != null)
                    player.hitpoint = 10;

                if (gameManager != null)
                    gameManager.dollar = 0;

                // Set the "work" parameter to true in the animator
                if (anim != null)
                    anim.SetBool("work", true);

                player.transform.position = new Vector3(100000000000000000f, 1000000000000000f, 0);
                // Load the specified scene immediately
                SceneManager.LoadScene(sceneToLoad);
            }

            // Start the delay for clearing the input text
            if (!isClearingInput)
            {
                isClearingInput = true;
                Invoke("ClearInputText", 0f);
            }
        }
        else if (!Input.inputString.Equals("P", System.StringComparison.OrdinalIgnoreCase) &&
                 !Input.inputString.ToLower().Contains("w") &&
                 !Input.inputString.ToLower().Contains("a") &&
                 !Input.inputString.ToLower().Contains("s") &&
                 !Input.inputString.ToLower().Contains("d") &&
                 !Input.GetKeyDown(KeyCode.LeftArrow) &&
                 !Input.GetKeyDown(KeyCode.UpArrow) &&
                 !Input.GetKeyDown(KeyCode.DownArrow) &&
                 !Input.GetKeyDown(KeyCode.Space))
        {
            // Capture user input
            inputText += Input.inputString;
        }
    }

    private void ClearInputText()
    {
        // Clear the input text
        inputText = "";
        isClearingInput = false;
    }
}

