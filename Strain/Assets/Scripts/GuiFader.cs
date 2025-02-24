    using UnityEngine;  
    using System.Collections;  
      
    public class GuiFader : MonoBehaviour  
    {  
        //A 4x4 Matrix  
        private Matrix4x4 trsMatrix;  
        //A three dimension vector that will translate GUI coordinate system  
        private Vector3 positionVec;  
        //Two booleans to determine which of the GUI buttons have been pressed  
        private bool next = false;  
        private bool back = false;
		
	
		
		
		
	
		//Volume slider variables etc...
	
		public float MusicValue = 1.0F;
		public float SFXValue = 1.0F;
		
		
      
        // Use this for initialization  
        void Start()  
	{
            //Initialize the matrix  
            trsMatrix = Matrix4x4.identity;  
            //Initialize the Vector  
            positionVec = Vector3.zero;
		
 			
        }  
      
        // Update is called once per frame  
        void Update()  
        {  
            //If the 'next' boolean is true  
            if(next)  
            {  
                //Interpolate the current vector x component until it has the same as value the screen width  
                positionVec.x = Mathf.SmoothStep(positionVec.x, Screen.width,Time.deltaTime*10);  
                /*Make 'trsMatrix' a matrix that translates, rotates and scales the GUI. 
                The position is set to positionVec, the Quaternion is set to identity 
                and the scale is set to one.*/  
                trsMatrix.SetTRS(positionVec , Quaternion.identity, Vector3.one);  
            }  
            else if(back) //If 'back is true'  
            {  
                //Interpolate the current vector x component until it reaches zero  
                positionVec.x = Mathf.SmoothStep(positionVec.x, 0,Time.deltaTime*10);  
                //Make 'trsMatrix' a matrix that translates, rotates and scales the GUI.  
                trsMatrix.SetTRS(positionVec , Quaternion.identity, Vector3.one);  
            }  
      
        } 
		 IEnumerator playSoundThenLoad()
			{
			//stops the BGmusic loop
			AudioManager.Instance.stopSound(AudioManager.Instance.msource.audio);
			//initialize first two sounds in sfxArray and lock controls
			AudioManager.Instance.Play(AudioManager.Instance.sfxArray[0],Vector3.zero,1);
			Screen.lockCursor = true;
			yield return new WaitForSeconds(AudioManager.Instance.sfxArray[0].length);
			AudioManager.Instance.Play(AudioManager.Instance.sfxArray[1],Vector3.zero,1);
			
			yield return new WaitForSeconds(AudioManager.Instance.sfxArray[1].length);
 			// after sounds load and complete we load the next level
			Application.LoadLevel("Wasteland");
			Screen.lockCursor = false;
			}
		
	
	
      
        void OnGUI()  
        {  
            //The GUI matrix must changed to the trsMatrix  
            GUI.matrix = trsMatrix;  
      
         
 		if(GUI.Button(new Rect(Screen.width/2.225f,Screen.height/1.6f,Screen.width/8.2f,Screen.height/12.2f), "Start"))
		{
			
			StartCoroutine(playSoundThenLoad());
			
		} 

		    //If the button labeled 'Options' is pressed  
            if(GUI.Button(new Rect(Screen.width/2.225f,Screen.height/1.3f,Screen.width/8.2f,Screen.height/12.2f), "Options"))  
            {  
                next = true;  
                back = false;  
            }  
      
		
		
             //Init all settings GUI
      
            //If the button labeled 'Save' is pressed  
            if(GUI.Button(new Rect(-Screen.width/1.225f,Screen.height/1.8f,Screen.width/8.2f,Screen.height/12.2f), "Save"))  
            {  
                next = false;  
                back = true;  
            }  
      		MusicValue = GUI.HorizontalSlider(new Rect(-Screen.width/1.225f,Screen.height/2.8f,Screen.width/8.2f,Screen.height/12.2f), MusicValue, 0.0F, 10.0F);
            
            SFXValue = GUI.HorizontalSlider(new Rect(-Screen.width/1.225f,Screen.height/2.3f,Screen.width/8.2f,Screen.height/12.2f), SFXValue, 0.0F, 10.0F);
            
		
		
		//To reset to GUI matrix, just make it equal to a 4x4 identity matrix  
            GUI.matrix = Matrix4x4.identity;  
      
             
        }  
    }  