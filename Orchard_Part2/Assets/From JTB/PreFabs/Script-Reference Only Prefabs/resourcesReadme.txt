This folder is set up so that you still have your prefabs organized into the prefab folder but they're also accessable via the Resources.Load() method. I wanted to make it so that the calls to the method itself stay organized so you know what you're loading no matter which copy of the Resource folder you want to reference to.

Example:

//In our project there are two separate resource folders.
//  Assets/Prefabs/Script-Reference Only/Resources
//  Assets/Scripts/NPC Scripts and Texts/Resources

//Inside the first, there is a Prefabs folder. For this example, there is a prefab inside of it that we want to load called 'Prefab Name'
//Inside the second, there is a Text folder, and inside it are separate folders for each npc, then inside them separate folders for each quest. That is where the actual txt doccuments are.

//This is how you would reference these files in code:
class Example(){
	private GameObject myPrefab;
	private TextAsset myDialogueText;
...
	public void Start(){
		//Do not include the extension with the name of the file
		myPrefab = Resources.Load("Prefabs/Prefab Name") as GameObject;
		//Alternate but essentially identical way of writing it. In both cases you're casting the object to the object type you want
		myDialogueText = Resources.Load<TextAsset>("Text/Test NPC/Test Quest/Quest Dialogue");
	}
...
}	


TL;DR
There is a reason for the strange folder structure