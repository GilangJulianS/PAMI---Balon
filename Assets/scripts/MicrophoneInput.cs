using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MicrophoneInput : MonoBehaviour {
	
	AudioSource audio_source;
	public Text text;
	public Transform balloon;
	public float sensitivity = 100;
	public float loudness = 0;
	public int threshold;
	public float growth;

	// Use this for initialization
	void Start () {
		
		audio_source = GetComponent<AudioSource>();

		//add the rest of the code like this
		audio_source.clip = Microphone.Start(null, true, 10, 44100);
		audio_source.loop = true; 
		while (!(Microphone.GetPosition(null) > 0)) { }
		audio_source.Play();

		//Do not Mute the audio Source.
	}
	
	// Update is called once per frame
	void Update () {
		loudness = GetAveragedVolume() * sensitivity;
		if (loudness > threshold) {
			balloon.localScale += new Vector3 (0.01F, 0.01F, 0.01F);
			Debug.Log (balloon.localScale.x);
			if (balloon.localScale.x > 0.9F) {
				Destroy (balloon.gameObject);
			}
		}
		text.text = "" + loudness; 
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
		balloon.localScale = new Vector3 (0.5F, 0.5F, 0.5F);
	}
}
