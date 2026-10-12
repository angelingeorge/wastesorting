
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [Header("Timer")]
    public float gameTime = 420f;
    public TMP_Text timerText;

    [Header("Messages")]
    public TMP_Text messageText;

    [Header("Warning")]
    public GameObject redWarningLight;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip buzzerSound;

    [Header("Waste Interaction")]
    public WasteInteraction wasteInteraction;

    [Header("Rat Ending Message")]
    public RatEndMessage ratEndMessage;

    private float timeRemaining;
    private bool gameRunning = true;
    private bool warningStarted = false;

    private WasteItem[] allWaste;

    void Start()
    {
        StartGame();
    }

    void Update()
    {
        if (!gameRunning)
        {
            return;
        }

        timeRemaining -= Time.deltaTime;

        if (timeRemaining <= 10f && !warningStarted)
        {
            warningStarted = true;
        }

        if (timeRemaining <= 0f)
        {
            timeRemaining = 0f;
            EndGame();
            UpdateTimerDisplay();
            return;
        }

        UpdateTimerDisplay();

        if (timeRemaining <= 10f && timeRemaining > 0f)
        {
            RedWarning();
        }

        CheckAllWasteCollected();
    }

    void StartGame()
    {
        timeRemaining = gameTime;
        gameRunning = true;
        warningStarted = false;

        if (messageText != null)
        {
            messageText.text = "";
        }

        if (redWarningLight != null)
        {
            redWarningLight.SetActive(false);
        }

        // Hide the ending panel when the game starts.
        if (ratEndMessage != null &&
            ratEndMessage.ratPanel != null)
        {
            ratEndMessage.ratPanel.SetActive(false);
        }

        allWaste = FindObjectsOfType<WasteItem>(true);

        foreach (WasteItem waste in allWaste)
        {
            if (waste != null)
            {
                waste.gameObject.SetActive(true);
            }
        }

        if (wasteInteraction != null)
        {
            wasteInteraction.enabled = true;
        }

        UpdateTimerDisplay();
    }

    void UpdateTimerDisplay()
    {
        if (timerText != null)
        {
            int minutes = Mathf.FloorToInt(timeRemaining / 60f);
            int seconds = Mathf.FloorToInt(timeRemaining % 60f);

            timerText.text =
                minutes.ToString("00") + ":" +
                seconds.ToString("00");
        }
    }

    public void WrongAnswer()
    {
        if (!gameRunning)
        {
            return;
        }

        timeRemaining -= 10f;

        if (timeRemaining < 0f)
        {
            timeRemaining = 0f;
        }

        UpdateTimerDisplay();

        if (timeRemaining <= 0f)
        {
            EndGame();
        }
    }

    void RedWarning()
    {
        if (redWarningLight != null)
        {
            float flash = Mathf.PingPong(Time.time * 2f, 1f);
            redWarningLight.SetActive(flash > 0.5f);
        }
    }

    void CheckAllWasteCollected()
    {
        if (allWaste == null || allWaste.Length == 0)
        {
            return;
        }

        foreach (WasteItem waste in allWaste)
        {
            if (waste != null && waste.gameObject.activeSelf)
            {
                return;
            }
        }

        CompleteGame();
    }

    void CompleteGame()
    {
        if (!gameRunning)
        {
            return;
        }

        gameRunning = false;

        if (wasteInteraction != null)
        {
            wasteInteraction.StopInteraction();
        }

        if (redWarningLight != null)
        {
            redWarningLight.SetActive(false);
        }

        if (messageText != null)
        {
            messageText.text = "";
        }

        if (ratEndMessage != null)
        {
            ratEndMessage.ShowMessage(
                "Amazing! You sorted all the waste! Great job!"
            );
        }

        Debug.Log("SUCCESS! ALL WASTE COLLECTED.");
    }

    void EndGame()
    {
        if (!gameRunning)
        {
            return;
        }

        gameRunning = false;
        timeRemaining = 0f;

        UpdateTimerDisplay();

        if (wasteInteraction != null)
        {
            wasteInteraction.StopInteraction();
        }

        if (redWarningLight != null)
        {
            redWarningLight.SetActive(false);
        }

        if (audioSource != null && buzzerSound != null)
        {
            audioSource.PlayOneShot(buzzerSound);
        }

        if (messageText != null)
        {
            messageText.text = "";
        }

        if (ratEndMessage != null)
        {
            ratEndMessage.ShowMessage(
                "Oops! Time is up! Don't give up. Let's try again!"
            );
        }

        Debug.Log("GAME ENDED");
    }
}
