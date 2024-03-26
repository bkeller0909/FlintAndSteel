using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TextEvent : MonoBehaviour
{
    [Header("Message Customization")]
    [SerializeField] string[] nameText;
    [SerializeField, TextArea] string[] messageText;
    [SerializeField] Sprite[] speakerImage;
    [SerializeField] int numOfMessages = 1;
    int currentMessage;

    [Header("GameObject References")]
    [SerializeField] GameObject messageGO;
    [SerializeField] GameObject nameTextGO;
    [SerializeField] GameObject messageTextGO;
    [SerializeField] GameObject speakerImageGO;

    bool messageActive;
    GameObject playerGO;

    void Awake()
    {
        currentMessage = 0;
        nameTextGO.GetComponent<TextMeshProUGUI>().text = nameText[currentMessage];
        messageTextGO.GetComponent<TextMeshProUGUI>().text = messageText[currentMessage];
        speakerImageGO.GetComponent<Image>().sprite = speakerImage[currentMessage];

        messageActive = false;
        playerGO = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        messageGO.SetActive(messageActive);

        // If the player presses any key, exit the message
        if (messageActive && Input.GetKeyDown(KeyCode.E) || messageActive && Input.GetButtonDown("Dialogue Skip"))
        {
            NextMessage();
        }

        // Disable player abilities and freeze time while the message is active
        playerGO.GetComponent<Throwing>().enabled = !messageActive;
        playerGO.GetComponent<PlayerAttackScript>().enabled = !messageActive;
        playerGO.GetComponent<PlayerMove>().enabled = !messageActive; 
        if (messageActive)
        {
            Time.timeScale = 0.0f;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            messageActive = true;
        }
    }

    private void NextMessage()
    {
        currentMessage++;

        if (currentMessage <= numOfMessages - 1) 
        {
            nameTextGO.GetComponent<TextMeshProUGUI>().text = nameText[currentMessage];
            messageTextGO.GetComponent<TextMeshProUGUI>().text = messageText[currentMessage];
            speakerImageGO.GetComponent<Image>().sprite = speakerImage[currentMessage];
        }
        else
        {
            messageActive = false;
            Time.timeScale = 1.0f;
            gameObject.SetActive(false);
        }
    }
}
