using System;
using System.Collections.Generic;

[Serializable]
public class EndingStepDefinition : Definition
{
	public bool HasAnimationTrigger;

	public string AnimationTriggerName;

	public bool LookingForPlayerResponse;

	public List<EndingResponseDefinition> ResponseOptions = new List<EndingResponseDefinition>();
}
