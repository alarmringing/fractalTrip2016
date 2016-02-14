/* OpenProcessing Tweak of *@*http://www.openprocessing.org/sketch/8941*@* */


using UnityEngine;
using System.Collections;

public class SkyboxUpdater : MonoBehaviour {

	public RenderTexture trippyTexture;

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
	float updateRate = 0.08f;


	void Start()
	{
		nextTime = Time.time + updateRate;

		// get a temporary RenderTexture //
		//RenderTexture renderTexture = RenderTexture.GetTemporary( width, height );

		// set the RenderTexture as global target (that means GL too)

	}

	void Update()
	{
		//curlx += ((Mathf.Deg2Rad * (360f/trippyTexture.height*Input.mousePosition.x)-curlx)/deley); 

		//curly += ((Mathf.Deg2Rad * (360f/trippyTexture.height*Input.mousePosition.y)-curly)/deley); 

		//updates teture to the trippy one
		//Texture2D trippyTexture = Resources.Load("trippy_dynamic") as Texture2D;
		if(Time.time > nextTime)
		{
			curlx += curlyXGain;
			curly += curlyYGain;
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
		GL.Clear(false, true, Color.yellow);


		// render GL immediately to the active render texture //
		GL.Begin( GL.LINES );
		GL.Color( new Color( 1, 0, 0, 0.5f ) );
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
