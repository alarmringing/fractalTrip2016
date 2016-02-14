using UnityEngine;
using System.Collections;

public class SkyboxUpdater : MonoBehaviour {

	public RenderTexture trippyTexture;

	/*
	void loadSkyBox(string skyboxName) 
	{
		string[] skyboxTextures = new string[]{"_FrontTex", "_BackTex", "_LeftTex", "_RightTex", "_UpTex", "_DownTex"};
		string path = "file://" + Application.dataPath + "/Skybox/FractalTextures/";

		WWW[] fileName =  new WWW[6];
		for(int i = 0;i < 6; i++)
		{
			fileName[i] = new WWW(path + skyboxName + skyboxTextures[i] + ".png");
		}
		Material mat = RenderSettings.skybox;
		for (int i = 0;i < 6; i++)
		{
			//yield www[i];   // wait for all images to finish
			mat.SetTexture(skyboxTextures[i],fileName[i].texture);
		}
	}*/

	void Start()
	{
		
	}

	void Update()
	{
		//updates teture to the trippy one
		//Texture2D trippyTexture = Resources.Load("trippy_dynamic") as Texture2D;
		gameObject.GetComponent<Renderer>().material.mainTexture = trippyTexture;
	}
}
