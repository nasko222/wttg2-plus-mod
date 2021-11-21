using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DancingLoader : MonoBehaviour
{
	public void Start()
	{
		DancingLoader.Ins = this;
		base.StartCoroutine(this.loadEndingWorld());
	}

	private IEnumerator loadEndingWorld()
	{
		AsyncOperation result = SceneManager.LoadSceneAsync(8, LoadSceneMode.Additive);
		Debug.Log("Loaded Scene 8");
		while (!result.isDone)
		{
			yield return new WaitForEndOfFrame();
		}
		UnityEngine.Object.Destroy(GameObject.Find("SecretManager").gameObject);
		UnityEngine.Object.Destroy(GameObject.Find("SecretCanvas").gameObject);
		Debug.Log("Destroyed Scene 8");
		GameObject original = GameObject.Find("CultMaleSecret");
		GameManager.TheCloud.dancingNoir = UnityEngine.Object.Instantiate<GameObject>(original, new Vector3(0f, 0f, 0f), Quaternion.identity);
		SceneManager.UnloadSceneAsync(8);
		Debug.Log("Unloaded Scene 8");
		yield break;
	}

	public static DancingLoader Ins;
}
