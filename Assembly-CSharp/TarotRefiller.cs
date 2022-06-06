using System;
using UnityEngine;

public static class TarotRefiller
{
	public static void RefillCards()
	{
		if (TarotCardsBehaviour.Owned)
		{
			UnityEngine.Object.Destroy(TarotCardsBehaviour.Ins.gameObject);
			UnityEngine.Object.Instantiate<GameObject>(CustomObjectLookUp.TarotCards).GetComponent<TarotCardsBehaviour>().SoftBuild();
			TarotCardsBehaviour.Ins.MoveMe(new Vector3(1.393f, 40.68f, 2.489f), new Vector3(0f, -20f, 180f), new Vector3(0.3f, 0.3f, 0.3f));
		}
	}
}
