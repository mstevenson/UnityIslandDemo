using UnityEngine;
//using System.Collections;

public class FPCCursorManager : MonoBehaviour
{
	public bool ShowCursor = true;

    void Start()
    {
		UpdateCursor ();
    }

    /*void Update()
    {
		if(Cursor.visible != ShowCursor)
			UpdateCursor ();
    }*/


	private void UpdateCursor()
	{
		Cursor.visible = ShowCursor;
		Cursor.lockState = ShowCursor ? CursorLockMode.None : CursorLockMode.Locked;
	}
}