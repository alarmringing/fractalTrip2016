using UnityEngine;
using System.Collections;

public class Fractal1 : MonoBehaviour {

	float generatableRadius = 30f;
	float growthInterval = 0.5f;
	int meshPatternCount = 0;
	bool isAbsoluteParent = true;
	bool hasChild = false;
	float lastSpore = 0f;
	float lastChild = 0f;
	public float curvature = 0f;
	public float scaleMagnitude = 2.0f;
	public int[] freqs = {500, 130, 80, 50};
	public GameObject AudioPlayer;
	public GameObject Player;
	GameObject rootObject = null;
	MusicDataSummarizer musicDataSummarizer;

	private static Vector3[] childDirections = {
		Vector3.down,
		Vector3.right,
		Vector3.left,
		Vector3.forward,
		Vector3.back,
//		Vector3.up
	};

	private static Vector3[] modDirections = {
		Vector3.zero, Vector3.forward + Vector3.up, Vector3.back + Vector3.up, Vector3.left + Vector3.up, Vector3.right + Vector3.up
	};

	private static Quaternion[] childOrientations = {
		Quaternion.identity,
		Quaternion.Euler(90f,0,0),
		Quaternion.Euler(-90f,0,0),
		Quaternion.Euler(0,0,90f),
		Quaternion.Euler(0,0,-90f),
//		Quaternion.Euler(45f, 20f, -45f),
//		Quaternion.Euler(-45f, 20f, 45f),
//		Quaternion.Euler(45f, 20f, 45f),
//		Quaternion.Euler(-45f, 20f, -45f),
//		Quaternion.Euler(0,180f,0)
	};

	public Mesh[] meshes = new Mesh[]{null, null, null};
	public Material material;
	private Material[] materials;

	int i;

	public int maxDepth = 3;

	public int depth = 0;

	private void InitializeMaterials () {
		materials = new Material[maxDepth + 1];
		for (int i = 0; i <= maxDepth; i++) {
			float t = i / (maxDepth - 1f);
			t *= t;
			materials[i] = new Material(material);
			materials[i].color =
				Color.Lerp(Color.yellow, Color.red, (float)i / maxDepth);
		}
	}

	public float childScale;

	public void CreateChildren () {
		if (depth != 0)
			i = 0;
		for (int j = i; j < childDirections.Length; j++) {
			//yield return new WaitForSeconds(growthInterval);
			new GameObject("Fractal Child").AddComponent<Fractal1>().Initialize(this, j);

		}
	}

	void Update() {
		curvature = musicDataSummarizer.curvature;
		if(musicDataSummarizer.freqs.Length >= 3) freqs = musicDataSummarizer.freqs;
		Vector3 scaleNow = transform.localScale;

		//scale!! 
		if(gameObject == rootObject && transform.localScale.x < scaleMagnitude)
		{
			transform.localScale *= 1.001f;
			//gameObject.transform.localScale = scaleNow * (1 + curvature/1000);
		}
		gameObject.transform.position = new Vector3(transform.position.x, transform.position.y - 0.0003f, transform.position.z);
	

		if(!hasChild && (Time.time > lastChild + growthInterval) && (depth < maxDepth))
		{
			CreateChildren();
			hasChild = true;
			lastChild = Time.time;
		}


		if(isAbsoluteParent && (depth == 0) && (Time.time > lastSpore + growthInterval * 5))
		{
			new GameObject("New Fractal Root").AddComponent<Fractal1>().AddNewRoot(this);	
			lastSpore = Time.time;
		}
	
	}

	void Start () {
		Debug.Log("Start run rn");

		if(rootObject == null) rootObject = gameObject;

		lastSpore = Time.time;
		lastChild = Time.time;

		musicDataSummarizer = AudioPlayer.GetComponent<MusicDataSummarizer>();
		if (materials == null) {
			InitializeMaterials();
		}
		gameObject.AddComponent<MeshFilter>().mesh = meshes[meshPatternCount%3];
		gameObject.AddComponent<MeshRenderer>().material = materials[depth];
		gameObject.AddComponent<MeshCollider>();
		gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
		i = 1;

	}

	private void Initialize (Fractal1 parent, int childIndex) {

		//inherit all variables
		meshes = parent.meshes;
		rootObject = parent.rootObject;
		isAbsoluteParent = false;
		meshPatternCount = parent.meshPatternCount + 1;
		material = parent.material;
		Color c = Color.white;
		materials = parent.materials;
		maxDepth = parent.maxDepth;
		AudioPlayer = parent.AudioPlayer;
		musicDataSummarizer = parent.musicDataSummarizer;
		Player = parent.Player;

		for (int n = 0; n < 3; n++) {
			c[n] = parent.freqs[n] * c.maxColorComponent / 1024;// * material.color[n];
			Debug.Log("freqs[" + n + "] is " + parent.freqs[n]);
		}
		Debug.Log("c is " + c.ToString());
		depth = parent.depth + 1;
		material.color = c;
		materials[depth].color = c;
		childScale = parent.childScale;
		transform.parent = parent.transform;
		transform.localScale = Vector3.one * childScale;
		transform.localPosition = (childDirections[childIndex] - curvature * modDirections[childIndex]) * (0.5f + 0.5f * childScale);
		transform.localRotation = childOrientations[childIndex];
	}

	void AddNewRoot(Fractal1 parent)
	{
		//inherit all variables
		isAbsoluteParent = false;
		rootObject = gameObject;
		childScale = parent.childScale;
		meshPatternCount = Random.Range(0,2);
		meshes = parent.meshes;
		depth = parent.depth;
		material = parent.material;
		Color c = Color.white;
		materials = parent.materials;
		maxDepth = parent.maxDepth;
		AudioPlayer = parent.AudioPlayer;
		musicDataSummarizer = parent.musicDataSummarizer;
		Player = parent.Player;

		//take parent's size
		transform.localScale = parent.transform.localScale;

		//where to be generated?

		Vector3 newPoint = Random.insideUnitSphere * parent.generatableRadius;
		transform.position = parent.transform.position + newPoint;
	}
}
