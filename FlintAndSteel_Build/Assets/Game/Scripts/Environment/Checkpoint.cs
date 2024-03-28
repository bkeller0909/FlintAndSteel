using UnityEngine;

//simple class to add to checkpoint triggers
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(AudioSource))]
public class Checkpoint : MonoBehaviour
{
	private Health health;

	private GameObject[] checkpoints;
	private AudioSource aSource;

	[SerializeField] private GameObject pirateFlag;
	[SerializeField] private GameObject scallyFlag;

	public bool checkpointActive;

	//setup
	void Awake()
	{
		aSource = GetComponent<AudioSource>();
		if(tag != "Respawn")
		{
			tag = "Respawn";
			Debug.LogWarning ("'Checkpoint' script attached to object without the 'Respawn' tag, tag has been assigned automatically", transform);	
		}
		GetComponent<Collider>().isTrigger = true;
		
		checkpointActive = false;
	}
	
	//more setup
	void Start()
	{
		checkpoints = GameObject.FindGameObjectsWithTag("Respawn");
		health = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
		if(!health)
			Debug.LogError("For Checkpoint to work, the Player needs 'Health' script attached", transform);
	}

    private void Update()
    {
        if (checkpointActive) 
		{
			scallyFlag.SetActive(true);
			pirateFlag.SetActive(false);
		}
		else
		{
			scallyFlag.SetActive(false);
			pirateFlag.SetActive(true);
		}
    }

    //set checkpoint
    void OnTriggerEnter(Collider other)
	{
		if(other.transform.tag == "Player" && health)
		{
			//set respawn position in players health script
			health.respawnPos = transform.position;
			health.currentHealth = 3;

			//toggle checkpoints
			if(!checkpointActive)
			{
				foreach (GameObject checkpoint in checkpoints)
					checkpoint.GetComponent<Checkpoint>().checkpointActive = false;

				aSource.Play();

                checkpointActive = true;
            }
		}
	}
}