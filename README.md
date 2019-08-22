# DGTools UI

Package of UI tools useful for any kind of Unity projects

## Installing

To install this package you'll need [DGTools-Core](https://github.com/Poulpinou/DGTools-Core), click [here](https://github.com/Poulpinou/DGTools-Core) to know how, then open "DGTools/Package Importer" window in the Unity Editor and click on "Install" next to "com.dgtools.ui".

Then open "DGTools/Package Importer" window in the Unity Editor and click on "Install" next to "com.dgtools.ui".

## How to use?

### Components

Every component have an example that can be instantiated in the editor from the creation menu at path **UI/DGTools/Components/{UIComponent name}**. You can start from this example to create your custom component. 

Obviously you can start from zero or override those components (See [Documentation API](https://poulpinou.github.io/DGTools-UI/annotated.html) for more details)

#### Components List
##### **Abstracts and interfaces**:
> Those components should be overrided

 - **UIComponent** : The main class for every component. It provides usefull properties, events and methods to manage display, animations and relations.

 - **IUITilable** : This interface allows your item to be used as a tile

##### **Basics** :
> Basic components are the components used by containers, but they can be used individually.

 - **UITile** : A generic tile that can contain any **IUITilable** item and display its properties. Feel free to customize it and override it at will.

 - **UISelectableTile** : A selectable variant of **UITile**

 - **UIPanel** : A simple Panel that implements **IUITilable**

##### **Containers** :
> Container components are the power of this tool, they are modular and can be used in thousand of cases. You can fill them from the inspector with the **items** field or by script with **AddItem()**.

 - **UIGrid** : A grid of **UITile** with auto focusing and events

 - **UISelectorSimple** : This components generates a list of **UISelectableTiles** from **IUITilable** items and allows you to select one. 

 - **UISelectorMultiple** : Same as **UISelectorSimple** with multiple choices

 - **UIPanelSwitcher** : This component creates **UISelectableTiles** from **UIPanels** and switches beetween panels when you select matching tile

### Modals
Modals can be displayed in front of all your UI with a blocking background. You can create you modals and easily open them from scripts. You just have to follow those steps :

**1. Create your Modal**
Create your modal's prefab (you can have an example with the Popup which is a Modal, you can instantiate it from create menu at path : **UI/DGTools/Modals/Popup**) and save it in a folder in a "Resources" folder (ex: "Assets/Resources/Modals/MyModal.prefab"). 

**2. Create Modal's Behaviour**
You have to override **Modal** to create a custom behaviour for your modal.
Example : 

```
using UnityEngine.UI;
using DGTools.UI;

public class MyModal : Modal
{
	public string title;
	[SerializeField] Text titleText;
	[SerializeField] Button validateButton;
    
	protected override void Build()
	{
		titleText.text = title;
		validateButton.onClick.AddListener(OnValidate);
	}

	public override void Clear()
	{
		titleText.text = "";
	}

	void OnValidate(){
		Debug.Log(title + " validated!");
	}
}
```

**3. Add a ModalManager to your scene**
Add an **Image** to your canvas (You can name it "Modals") and attach a **ModalManager** script to it. The image will become the background for your modals, so it should cover all the screen.

**4. Open your modal**
To open your modal, you just have to do this : 

```
ModalManager.OpenModal<MyModal>();
```

There is a lot of other methods that you can use, take a look at them in the [Documentation API](https://poulpinou.github.io/DGTools-UI/annotated.html)

#### Bonus : Popup
You can use a modular popup with many settings with this code : 
```
Popup.Open(new PopupSettings() {
		message = "What do you want?",
		callback = OnClickTrue,
		 etc...
	 });
```

### Menus

Menus can be used like modals (Modal => Menu, ModalManager => MenuManager). More details will come.

## Authors

 **Donovan Persent ([Poulpinou](https://github.com/Poulpinou))**

#### Contributors :
-  **Quentin Roussel ([Hermadeus](https://github.com/Hermadeus))**

## Licenses
See [Licence](https://github.com/Poulpinou/DGTools-Core/LICENCE.md)
