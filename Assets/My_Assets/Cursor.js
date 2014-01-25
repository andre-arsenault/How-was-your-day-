
var cursorTexture : Texture;
var cursorMode : CursorMode = CursorMode.Auto;
var hotSpot : Vector2 = Vector2.zero;
var over_me;
var dialogue;

function Awake () {

	Screen.showCursor = true;
	over_me = false;
	dialogue = GameObject.Find("Game_Logic").GetComponent(DialogueInstance);
}


function Update () {

	if(Input.GetMouseButtonDown(0) && over_me.Equals(true)){
	
			/* startOn is the node we want to initiate the dialogue it will be different for every object
			   It's propably to best to have different DialogueInstance.js for different toys instead of having one
			   huge dialogue Tree
			*/   
			     
			if ( this.name.Equals("Test_toy"))
				dialogue.startOn = 0;
				
			dialogue.enabled = true;
			
	}
}

function OnMouseEnter () {

	UnityEngine.Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
	over_me = true;
			
}

function OnMouseExit () {

	UnityEngine.Cursor.SetCursor(null, hotSpot, cursorMode);
	over_me = false;
}