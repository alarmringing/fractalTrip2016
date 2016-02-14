using UnityEngine;
using System.Collections;

public class Fractal1 : MonoBehaviour {

	public float curvature = 0f;
	public int[] freqs = {500, 130, 80, 50};



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

	public Mesh mesh;
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

	public IEnumerator CreateChildren () {
		if (depth != 0)
			i = 0;
		for (int j = i; j < childDirections.Length; j++) {
			yield return new WaitForSeconds(0.5f);
			new GameObject("Fractal Child").AddComponent<Fractal1>().
			Initialize(this, j);
		}
	}

	void Update() {
		Vector3 scaleNow = transform.localScale;
		gameObject.transform.localScale = scaleNow * (1 + curvature/1000);
		gameObject.transform.position = new Vector3(transform.position.x, transform.position.y - 0.0003f, transform.position.z);
	}

	void Start () {
		if (materials == null) {
			InitializeMaterials();
		}
		gameObject.AddComponent<MeshFilter>().mesh = mesh;
		gameObject.AddComponent<MeshRenderer>().material = materials[depth];
		gameObject.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
		i = 1;
		if (depth < maxDepth) {
			StartCoroutine(CreateChildren());
		}
	}

	private void Initialize (Fractal1 parent, int childIndex) {
		mesh = parent.mesh;
		material = parent.material;
		Color c = Color.white;
		materials = parent.materials;
		maxDepth = parent.maxDepth;


		for (int n = 0; n < 3; n++) {
			c[n] = freqs[n] * c.maxColorComponent / 1023;// * material.color[n];
		}
		c [3] = (float)freqs [3] / 1023;
		depth = parent.depth + 1;
		//material.color = c;
		materials[depth].color = c;
		childScale = parent.childScale;
		transform.parent = parent.transform;
		transform.localScale = Vector3.one * childScale;
		transform.localPosition = (childDirections[childIndex] - curvature * modDirections[childIndex]) * (0.5f + 0.5f * childScale);
		transform.localRotation = childOrientations[childIndex];
	}
}
