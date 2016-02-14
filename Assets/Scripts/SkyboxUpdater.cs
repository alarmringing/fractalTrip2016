/* OpenProcessing Tweak of *@*http://www.openprocessing.org/sketch/8941*@* */


using UnityEngine;
using System.Collections;

public class SkyboxUpdater : MonoBehaviour {

	public RenderTexture trippyTexture;

	float curlx = 0;
	float curly = 0;
	float f = Mathf.Sqrt(2)/2f;
	float deley = 10;
	float growth =0;
	float growthTarget = 0;

	float nextTime = 0;
	float updateRate = 0.2f;


	void Start()
	{
		nextTime = Time.time + updateRate;

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
			RenderTexture.active = trippyTexture;
			GL.LoadPixelMatrix( 0, trippyTexture.width, trippyTexture.height, 0 );
			growth += (growthTarget/10f-growth+1f)/deley; 
			//gameObject.GetComponent<Renderer>().material.mainTexture = RenderGLToTexture(trippyTexture.width,trippyTexture.height);
			RenderGLToTexture(trippyTexture.width,trippyTexture.height);
			nextTime += updateRate;
		}
		//branch(1.0f, 1);

		//background(250); 
		//stroke(0); 

		//translate(width/2,height/3*2); 
		//line(0,0,0,height/2); 
		//branch(height/4.,17); 
		//growth += (growthTarget/10-growth+1.)/deley; 
	}

	void branch(int width, int height, float startx, float starty, float len,int num, int id) 
	{ 
		//RenderTexture.active = trippyTexture;

		//GL.Color (Color.green);

		//curlx += ((Mathf.Deg2Rad * (360f/height*Input.mousePosition.x)-curlx)/deley)*num; 
		curlx += 0.005f*num;
		curly += 0.005f*num;
		//curly += ((Mathf.Deg2Rad * (360f/height*Input.mousePosition.y)-curly)/deley)*num; 

		//Debug.Log("len is " + len + "num is " + num + " startx is " + startx + " starty is " + starty + " width is " + width + " curlx is " + curlx + " id is " + id);


		len *= f; 
		num -= 1; 
		if((len > 1) && (num > 0)) 
		{ 
			float endx, endy;
			endx = 0; endy = 0;

			Debug.Log("len is " + len + "num is " + num + " startx is " + startx + " starty is " + starty + " width is " + width + " curlx is " + curlx + " id is " + id);


			GL.PushMatrix(); 
			//..GL.LoadPixelMatrix( 0, width, height, 0 );
			endx = startx + Mathf.Cos(curlx)*len;
			endy = starty + Mathf.Sin(curlx)*len;

			GL.Begin( GL.LINES );
			GL.Color( new Color( 1, 0, 0, 0.5f ) );
			GL.Vertex3( startx, starty, 0 );
			GL.Vertex3( endx, endy, 0 );
			GL.End();

			//translate(0,-len); 
			branch(width, height, endx, endy, len, num, 1); 
			GL.PopMatrix(); 


			len *= growth; 
			GL.PushMatrix(); 
			endx = startx + Mathf.Cos(curlx-curly)*len;
			endy = starty + Mathf.Sin(curlx-curly)*len;
			endx = startx; endy = starty + len; //some end position modification

			//Debug.Log("We are here");

			GL.Begin( GL.LINES );
			GL.Color( new Color( 1, 0, 0, 0.5f ) );
			GL.Vertex3( startx, starty, 0 );
			GL.Vertex3( endx, endy, 0 );
			GL.End();

			//translate(0,-len); 
			branch(width, height, endx, endy, len, num, 2); 
			//Debug.Log("We are here");
			GL.PopMatrix(); 
		} 

		/*
		GL.PushMatrix();
		GL.LoadPixelMatrix( 0, width, height, 0 );
		GL.Begin( GL.LINES );
		GL.Color( new Color( 1, 0, 0, 0.5f ) );
		for( int i=0; i<10; i++ ) GL.Vertex3( Random.value * width, Random.value * height, 0 );
		GL.End();
		GL.PopMatrix();*/
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
		//GL.Begin( GL.LINES );
		branch(width, height, width/2, height/2, 150f, 4, 0);
		//GL.End();

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
