using UnityEngine;
using UnityEngine.UI;


public class ScreenFader : MonoBehaviour 
{

	public float solidAlpha = 1f;
	public float clearAlpha = 0f;
	public float fadeTime = 1f;

	MaskableGraphic maskableGraphic;

	void Start () 
	{
		maskableGraphic = GetComponent<MaskableGraphic> ();
		
	}
	
	//End game
	public void FadeOn()
	{
		maskableGraphic.CrossFadeAlpha(solidAlpha, fadeTime, true);
	}

	//Start game
	public void FadeOff()
	{
		maskableGraphic.CrossFadeAlpha(clearAlpha, fadeTime, true);
	}
}
