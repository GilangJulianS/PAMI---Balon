using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Controller : MonoBehaviour {

	public Text textNotif;
	public GameObject[] balloonPrefabs;
	public Transform spawner;
	AudioSource[] audio_source;
	public Text textSound;
	public GameObject balloon;
	public GameObject popEffect;
	public float sensitivity = 100;
	public float loudness = 0;
	public int threshold;
	public float growth;
	private Vector2 initPos;
	private Vector2 finalPos;
	private GameObject selectedBalloon;

	// Use this for initialization
	void Start () {
		audio_source = GetComponents<AudioSource>();
		//add the rest of the code like this
		audio_source[0].clip = Microphone.Start(null, true, 10, 44100);
		audio_source[0].loop = true; 
		while (!(Microphone.GetPosition(null) > 0)) { }
		audio_source[0].Play();
		//Do not Mute the audio Source.
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.touchCount > 0) {
			if (Input.GetTouch (0).phase == TouchPhase.Began) {
				initPos = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
				Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
				RaycastHit hit;
				textNotif.text = "Tap";
				// TILL HERE IT WORKS 
				if (Physics.Raycast(ray, out hit, Mathf.Infinity)){
					textNotif.text = "Hit";
					if (hit.transform.tag == "balloon") {
						selectedBalloon = hit.transform.gameObject;
					} else {
						selectedBalloon = null;
					}
				}
			} else if (Input.GetTouch (0).phase == TouchPhase.Moved) {
				Vector3 deltaPos = Input.GetTouch (0).deltaPosition;
				if (selectedBalloon != null) {
					selectedBalloon.GetComponent<Rigidbody> ().AddForce(deltaPos.x, deltaPos.y, 0);
				}

			} else if (Input.GetTouch (0).phase == TouchPhase.Ended) {
				finalPos = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
				Vector2 deltaPos = finalPos - initPos;
				float distance = Mathf.Sqrt (Mathf.Pow (deltaPos.x, 2) + Mathf.Pow (deltaPos.y,2));
				textNotif.text = "" + distance;
				if (selectedBalloon != null && distance < 100) {
					onBalloonTap (selectedBalloon);
				}
				selectedBalloon = null;
			}
		}

		loudness = GetAveragedVolume() * sensitivity;
		if (loudness > threshold && balloon != null) {
			balloon.transform.localScale += new Vector3 (growth, growth, growth);
			Debug.Log (balloon.transform.localScale.x);
			if (balloon.transform.localScale.x > 0.9F) {
				pop (balloon.gameObject);
			}
		}
		textSound.text = "" + loudness; 
	}

	void onBalloonTap(GameObject balloon){
		pop (balloon);
	}

	void pop(GameObject balloon){
		Debug.Log ("POP!");
		Vector3 balloonPos = balloon.transform.position;
		float balloonScale = balloon.transform.localScale.x;
		DestroyImmediate (balloon);
		audio_source[1].Play();
		balloon = null;
		GameObject newPop = (GameObject) Instantiate (popEffect, balloonPos, new Quaternion ());
		newPop.transform.localScale = new Vector3 (balloonScale, balloonScale, balloonScale);
		//balloon = GameObject.FindGameObjectWithTag ("balloon");
		//Instantiate (balloonPrefab, spawner.position, new Quaternion ());
	}

	float GetAveragedVolume(){ 
		float[] data = new float[256];
		float a = 0;
		GetComponent<AudioSource>().GetOutputData(data,0);
		foreach(float s in data)
		{
			a += Mathf.Abs(s);
		}
		return a/256;
	}

	public void Reset(){
		balloon.transform.localScale = new Vector3 (0.5F, 0.5F, 0.5F);
	}

	public void instantiateBalloon(){
		GameObject newBalloon;
		int idx = Random.Range (0, balloonPrefabs.Length);
		newBalloon = Instantiate(balloonPrefabs[idx], new Vector3(spawner.position.x + Random.Range(-0.2f,0.2f),spawner.position.y,spawner.position.z), Quaternion.Euler(new Vector3(270,0,0))) as GameObject;
		balloon = newBalloon;
//		balloon.transform.localScale = new Vector3 (0.2f, 0.2f, 0.2f);
	}
}
