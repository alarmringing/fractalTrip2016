using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MusicDataSummarizer : MonoBehaviour {

	static int numHist = 10;
	int numSamples = 1024;
	AudioSource thisAudio;
	float[] history = new float[numHist];


	public float[] samples;
	public float pitch;
	public float output;
	public float curvature;
	public int[] freqs;

	Vector3 boxScale;

	float totalSum;
	float volume = 30f;



	// Use this for initialization
	void Start () {

		//Debug.Log ("history");
		//Debug.Log (history[0]);

		thisAudio = gameObject.GetComponent<AudioSource>();
		samples = new float[numSamples];

	}

	// Update is called once per frame
	void Update () {

		thisAudio.GetOutputData(samples, 0);
		output = outputSize();
		pitch = thisAudio.pitch;
		curvature = kurve();
		freqs = biggestFreqs().ToArray();

		//Debug.Log ("freqs");
		//Debug.Log (biggestFreqs()[0]);
		//Debug.Log ("curve");
		//Debug.Log (kurve());

	}

	float kurve() {
		float squareSum = 0;

		for(int i=0; i < history.Length; i++) {
			squareSum += history[i]*history[i];
		}
		float rms = Mathf.Sqrt(squareSum/(numHist));
		//Debug.Log ("rms");
		//Debug.Log (rms);


		float CsquareSum = 0;

		for(int i=0; i < samples.Length; i++) {
			CsquareSum += samples[i]*samples[i];
		}
		float Crms = Mathf.Sqrt(CsquareSum/(numSamples));
		//Debug.Log ("Crms");
		//Debug.Log (Crms);


		return Crms - rms;
	}

	float outputSize() {
		float squareSum = 0;

		for(int i=0; i < samples.Length; i++) {
			squareSum = samples[i]*samples[i];
		}
		float rms = Mathf.Sqrt(squareSum/(samples.Length));
		float totalOutput = Mathf.Clamp01(rms*volume);
		return totalOutput;
	}

	List<int> biggestFreqs() {
		List<float[]> list = new List<float[]> ();
		List<int> rList = new List<int> ();
		float[] max = new float[2];
		max [0] = 0;
		max [1] = 0; 
		for(int i=0;i<numSamples;i++) {
			if (samples [i] > max [1]) {
				max [0] = i;
				max [1] = samples [i];
			}
			if (samples [i] > 0.25) {
				float[] a = new float[2];
				a [0] = i;
				a [1] = samples [i];
				list.Add(a);
			}
		}
		if (list.Count == 0) {
			int mx = (int) max [0];
			rList.Add(mx);
		} else {
			while (list.Count > 0) {
				float[] lmx = new float[3];
				lmx [0] = 0;
				lmx [1] = 0;
				lmx [2] = 0;
				for(int j=0;j<list.Count;j++) {
					if (list[j] [1] > lmx [1]) {
						lmx [0] = list[j] [0];
						lmx [1] = list[j] [1];
						lmx [2] = j;
					}
				}
				rList.Add((int)lmx [0]);
				list.RemoveAt((int)lmx [2]);
			}
		}
	return rList; 
	}
}
