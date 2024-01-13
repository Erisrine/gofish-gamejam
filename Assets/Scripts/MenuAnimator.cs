using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnimator : MonoBehaviour
{
	[SerializeField] Animator mainMenuAnimator;
	[SerializeField] Animator optionsMenuAnimator;
	[SerializeField] Animator creditsAnimator;
	[SerializeField] Animator quitAnimator;
	[SerializeField] Animation creditScroll;
	[SerializeField] AnimationClip scrollUp;
	[SerializeField] AnimationClip rewind;


	public void GoToOptions()
	{
		mainMenuAnimator.SetTrigger("SlideOut");
		optionsMenuAnimator.SetTrigger("SlideIn");
	}
	public void OptionsToMain()
	{
		mainMenuAnimator.SetTrigger("SlideIn");
		optionsMenuAnimator.SetTrigger("SlideOut");
	}
	public void GoToQuit()
	{
		mainMenuAnimator.SetTrigger("SlideOut");
		quitAnimator.SetTrigger("SlideIn");
	}
	public void QuitToMain()
	{
		mainMenuAnimator.SetTrigger("SlideIn");
		quitAnimator.SetTrigger("SlideOut");
	}
	public void GoToCredits()
	{
		mainMenuAnimator.SetTrigger("SlideOut");
		creditsAnimator.SetTrigger("SlideIn");
		StartCoroutine(CreditsStart());
	}
	public void CreditsToMain()
	{
		mainMenuAnimator.SetTrigger("SlideIn");
		creditsAnimator.SetTrigger("SlideOut");
		StartCoroutine(CreditsStop());
	}
	
	IEnumerator CreditsStart()
	{
		//creditScroll.AddClip(scrollUp, "scroll");
		yield return new WaitForSeconds(2f);
		creditScroll.Play();
		
	}
	IEnumerator CreditsStop()
	{
		creditScroll.Stop();
		yield return new WaitForSeconds(1f);
		creditScroll.Play();
		creditScroll.Rewind("creditsScroll");
		yield return null;
		creditScroll.Stop();
		
		
	}	
}
