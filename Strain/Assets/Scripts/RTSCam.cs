    using UnityEngine;
    using System.Collections;
	using System.Collections.Generic;
     
    public class RTSCam : MonoBehaviour {
     
        public float verticalScrollArea = 10f;
        public float horizontalScrollArea = 10f;
        public float verticalScrollSpeed = 10f;
        public float horizontalScrollSpeed = 10f;
       
        public void EnableControls(bool _enable)
        {
            if(_enable)
            {
                ZoomEnabled = true;
                MoveEnabled = true;
            }
            else
            {
                ZoomEnabled = false;
                MoveEnabled = false;
            }
        }
        public bool ZoomEnabled = true;
        public bool MoveEnabled = true;
	
		private static Vector3 moveToDestination = Vector3.zero;
		private static List<string> passables = new List<string>(){"floor"};
	
       
        private Vector2 _mousePos;
        private Vector3 _moveVector;
        private int _xMove;
        private int _yMove;
        private int _zMove;
     //Camera controls for selecting units
		public Texture2D selectionBox = null;
		public static Rect selection = new Rect(0,0,0,0);
		private Vector3 startClick = -Vector3.one;  
	
	
        // Update is called once per frame
        void Update () {
		checkCam();
		cleanUp();
            _mousePos = Input.mousePosition;
           
            //Move camera if mouse is at the edge of the screen
            if(MoveEnabled)
            {
                if(_mousePos.x < horizontalScrollArea) { _xMove = -1; }
                else if (_mousePos.x >= Screen.width - horizontalScrollArea) { _xMove = 1; }
                else { _xMove = 0; }
                   
               
                if(_mousePos.y < verticalScrollArea) { _zMove = -1; }
                else if(_mousePos.y >= Screen.height - verticalScrollArea) { _zMove = 1; }
                else { _zMove = 0; }
            }
            else
            {
                _xMove = 0;
                _yMove = 0;
            }
            // Zoom Camera in or out
            if(ZoomEnabled)
            {
                if (Input.GetAxis("Mouse ScrollWheel") < 0) {
                    _yMove = 3;
                }
                else if (Input.GetAxis("Mouse ScrollWheel") > 0) {
                    _yMove = -3;
                }
                else
                {
                    _yMove = 0;
                }
            }
            else
            {
                _zMove = 0;
            }
            //move the object
            MoveMe(_xMove, _yMove, _zMove);
        }
           
        private void MoveMe(int x, int y, int z)
        {
            _moveVector = (new Vector3(x * horizontalScrollSpeed,
                                             y * verticalScrollSpeed, z * horizontalScrollSpeed) * Time.deltaTime);
            transform.Translate(_moveVector, Space.World);
        }
		private void checkCam()
		{
			if (Input.GetMouseButtonDown(0))
				startClick = Input.mousePosition;
			else if (Input.GetMouseButtonUp(0))
			{
			
				startClick = -Vector3.one;
		
   			}
	
			if(Input.GetMouseButton(0))
			selection = new Rect(startClick.x,InvertMouseY(startClick.y),Input.mousePosition.x - startClick.x,InvertMouseY(Input.mousePosition.y)-InvertMouseY(startClick.y));
		
			if (selection.width < 0)
				{
					selection.x += selection.width;
					selection.width = -selection.width;
				}
				if (selection.height < 0)
					{
						selection.y += selection.height;
						selection.height = -selection.height;
					}
		}			
		public static float InvertMouseY(float y)
		{
			return Screen.height - y;
		}
		private void OnGUI()
		{
			if(startClick != -Vector3.one)
			{
				GUI.color = new Color(1,1,1,0.5f);
				GUI.DrawTexture(selection, selectionBox);
	
			}
		}
		private void cleanUp()
		{
			if(!Input.GetMouseButtonUp(1))
			moveToDestination = Vector3.zero;
		
		}
		public static Vector3 getDestination()
		{
			if(moveToDestination == Vector3.zero)
			{
				RaycastHit hit;
				Ray r = Camera.mainCamera.ScreenPointToRay(Input.mousePosition);
			
				if(Physics.Raycast(r, out hit))
				{
					while(!passables.Contains(hit.transform.gameObject.name))
					{
						if(!Physics.Raycast(hit.point + r.direction * 0.1f,r.direction, out hit))
							break;
					}
				}
				if(hit.transform != null)
				moveToDestination = hit.point;
			}
			return moveToDestination;
		}

}