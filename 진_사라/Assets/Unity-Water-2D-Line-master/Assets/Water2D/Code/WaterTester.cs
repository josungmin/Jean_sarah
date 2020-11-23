using UnityEngine;
using System.Collections;

public class WaterTester : MonoBehaviour {


	public Water2D water;
	public float force;
	public int size = 0;

	private static uint screenshot1Count = 0;
	private static uint screenshot1Count1 = 0;
	private static uint screenshot1Count2 = 0;
	private static uint screenshot1Count3 = 0;
	
	//public float waterHeightChanger = 100;
	
	public GameObject objectToInstantiate;
	
	void Awake()
	{
		//Physics.gravity = new Vector3(0,-500,0);	
	}
	
	// Update is called once per frame
	void Update () {
	
		if (Input.GetMouseButtonDown(0))
		{
			Vector3 touchPosition =  Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, water.transform.position.z - transform.position.z));
			
			if (objectToInstantiate != null)
			{
				GameObject instatiatedObject =  Instantiate(objectToInstantiate, touchPosition+Vector3.forward*10 , Quaternion.identity) as GameObject;
				instatiatedObject.transform.eulerAngles = new Vector3(0,-180, Random.Range(0,360));
				instatiatedObject.transform.localScale = Vector3.one*Random.Range(0.5f,1.3f);
				instatiatedObject.GetComponent<Rigidbody>().mass = Mathf.Lerp(0.1f,1f,Mathf.InverseLerp(0.5f,1.3f, instatiatedObject.transform.localScale.x));
			}
			else
			{
				water.ObjectEnteredWater(touchPosition, force,size, true);
			}
			
		}
	//water.SetHeight(waterHeightChanger);
	
	}

	void OnGUI()
	{
		if (GUI.Button (new Rect (Screen.width/2f - 200f, 0f, 200, 100), "Capture1")) {
			Debug.Log ("Button capture1 with Application");
			Debug.Log ("Path:" + Application.persistentDataPath);
			Debug.Log ("Path:" + Application.dataPath);
			FileLog.Instance.Log ("Path:" + Application.persistentDataPath);
			CaptureScreenshot1();

		}

		if (GUILayout.Button ("Capture 1", GUILayout.Width(300), GUILayout.Height (100))) {
			string ImageName = "screenshot1-" + screenshot1Count1 + ".png";
			screenshot1Count1++;

			CaptureByUnity(ImageName);
		}

		if (GUILayout.Button ("Capture 2", GUILayout.Width(300), GUILayout.Height (100))) {
			string ImageName = "screenshot2-" + screenshot1Count2 + ".png";
			screenshot1Count2++;
			string ImagePath = Application.persistentDataPath + "/" + ImageName;
			Debug.Log ("Path:" + Application.dataPath);
			StartCoroutine(CaptureByRect(new Rect(0,0,Screen.width, Screen.height),ImagePath));
			
		}

		if (GUILayout.Button ("Capture 3", GUILayout.Width(300), GUILayout.Height (100))) {
			Debug.Log ("Path:" + Application.dataPath);
				//string ImageName = "screenshot3-" + screenshot1Count3 + ".png";
				//screenshot1Count3++;
				//string ImagePath = Application.persistentDataPath + "/" + ImageName;
				
			//Transform CameraTrans;
			//CameraTrans.camera.enabled = true;
			//	StartCoroutine(CaptureByCamera(new Rect(0,0,Screen.width, Screen.height),ImagePath));
		}
	}

	static void CaptureByUnity(string imageName){
		ScreenCapture.CaptureScreenshot (imageName, 0);
	}

	IEnumerator CaptureByRect(Rect rect, string imageName){
		yield return new WaitForEndOfFrame ();

		Texture2D texture = new Texture2D ((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
		texture.ReadPixels (rect, 0, 0);
		texture.Apply ();

		byte[] bytes = texture.EncodeToPNG ();

		System.IO.File.WriteAllBytes (imageName, bytes);
	}

	IEnumerator CaptureByCamera(Camera camera, Rect rect, string imageName){
		yield return new WaitForEndOfFrame ();

		RenderTexture texture = new RenderTexture ((int)rect.width, (int)rect.height, 0);

		camera.targetTexture = texture;

		camera.Render ();
		RenderTexture.active = texture;

		Texture2D texture2d = new Texture2D ((int)rect.width, (int)rect.height, TextureFormat.RGB24, false);
		texture2d.ReadPixels (rect, 0, 0);
		texture2d.Apply ();

		camera.targetTexture = null;
		RenderTexture.active = null;
		GameObject.Destroy (texture);
		
		byte[] bytes = texture2d.EncodeToPNG ();
		
		System.IO.File.WriteAllBytes (imageName, bytes);

	}

	static void CaptureScreenshot1(){
		string ImageName = "screenshot-" + screenshot1Count + ".png";
		// string ImagePath = Application.persistentDataPath + "/" + ImageName;
		screenshot1Count++;
		ScreenCapture.CaptureScreenshot(ImageName);
	}
}
