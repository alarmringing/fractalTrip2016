/* OpenProcessing Tweak of *@*http://www.openprocessing.org/sketch/8941*@* */


using UnityEngine;
using System.Collections;

public class SkyboxUpdater : MonoBehaviour {

	public RenderTexture trippyTexture;
	public GameObject AudioPlayer;
	AudioSource playedAudio;
	MusicDataSummarizer musicDataSummarizer;

	float[] samples;
	int numSamples = 1024;
	float volume = 30f;
	float rms = 0;
	float outputPercentage = 0;
	float squareSum= 0;

	float pitch = 1f;

	float curlx = 0;
	float curly = 0;
	float curlyXGain = 0.03f;
	float curlyYGain = -0.03f;
	float baseCurl = 0.04f;
	float f = Mathf.Sqrt(2)/2f;
	float deley = 10;
	float growth =1;
	float growthTarget = 0;

	float nextTime = 0;
	float updateRate = 0.02f;


	void Start()
	{
		nextTime = Time.time + updateRate;
		//playedAudio = AudioPlayer.GetComponent<AudioSource>();
		musicDataSummarizer = AudioPlayer.GetComponent<MusicDataSummarizer>();
		//Debug.Log(musicDataSummarizer.name);
		// get a temporary RenderTexture //
		//RenderTexture renderTexture = RenderTexture.GetTemporary( width, height );

		// set the RenderTexture as global target (that means GL too)

	}

	void Update()
	{
		//updates teture to the trippy one
		//Texture2D trippyTexture = Resources.Load("trippy_dynamic") as Texture2D;
		if(Time.time > nextTime)
		{
			outputPercentage = musicDataSummarizer.output;
			Debug.Log(outputPercentage);
			pitch = musicDataSummarizer.pitch;
			curlx = 2.6f + outputPercentage * Mathf.PI/7;
			curly = - outputPercentage * Mathf.PI/7;
			RenderTexture.active = trippyTexture;
			GL.LoadPixelMatrix( 0, trippyTexture.width, trippyTexture.height, 0 );
			//growth += (growthTarget/10f-growth+1f)/deley; 
			//gameObject.GetComponent<Renderer>().material.mainTexture = RenderGLToTexture(trippyTexture.width,trippyTexture.height);
			RenderGLToTexture(trippyTexture.width,trippyTexture.height);
			nextTime += updateRate;
		}

	}

	void branch(float width, float height, float startx, float starty, float startCurl, float len,int num, int id) 
	{ 

		len *= f; 
		num -= 1; 
		if((len > 1) && (num > 0)) 
		{ 
			float endx, endy;


			startCurl = startCurl + curlx;
			endx = startx + Mathf.Cos(startCurl)*len;
			endy = starty + Mathf.Sin(startCurl)*len;
			GL.Vertex3( startx, starty, 0 );
			GL.Vertex3( endx, endy, 0 );
			//Debug.Log("just drew line from (" + startx + ", " + starty + "),  to (" + endx + ", " + endy + "), curvature " + startCurl + ", id is " + 1);

			branch(width, height, endx, endy, startCurl, len, num, 1); 



			len *= growth; 
			startCurl = startCurl + curlx - curly;
			endx = startx + Mathf.Cos(startCurl)*len;
			endy = starty + Mathf.Sin(startCurl)*len;
			GL.Vertex3( startx, starty, 0 );
			GL.Vertex3( endx, endy, 0 );
			//Debug.Log("Mathf.Cos(startCurl) = " + Mathf.Cos(startCurl));
			//Debug.Log("Mathf.Sin(startCurl) = " + Mathf.Sin(startCurl));

			//Debug.Log("just drew line from (" + startx + ", " + starty + "),  to (" + endx + ", " + endy + "), curvature " + startCurl + ", id is " + 2);

			branch(width, height, endx, endy, startCurl, len, num, 2); 

		} 

	}
	

		
	void RenderGLToTexture( int width, int height )
	{
		// get a temporary RenderTexture //
		//RenderTexture renderTexture = RenderTexture.GetTemporary( width, height );

		// set the RenderTexture as global target (that means GL too)


		// clear GL //
		//GL.Clear( false, true, Color.green );
		Color bgColor = new Color(outputPercentage*0.5f,1f - outputPercentage*0.5f,0.9f*pitch,1f);
		GL.Clear(false, true, bgColor);


		// render GL immediately to the active render texture //
		GL.Begin( GL.LINES );
		Color lineColor = new Color(1f-bgColor.r, 1f-bgColor.g, 1f-bgColor.b, 1f);
		GL.Color( lineColor );
		branch(width, height, width/2, height/2, curlx, 150f, 6, 0);
		//branch(width, height, 0, 0, 0, 150f, 6, 0);
		GL.End();

		// read the active RenderTexture into a new Texture2D //
		//newTexture.ReadPixels( new Rect( 0, 0, width, height ), 0, 0 );

		// apply pixels and compress //
		/*
		bool applyMipsmaps = false;
		newTexture.Apply( applyMipsmaps );
		bool highQuality = true;
		newTexture.Compress( highQuality );*/

		// clean up after the party //
		//RenderTexture.active = null;
		//RenderTexture.ReleaseTemporary( trippyTexture );

		// return the goods //
		//return newTexture;
	}
}
