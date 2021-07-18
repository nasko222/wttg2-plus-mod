using System;

public class BehaviourManager
{
	public crossHairBehaviour CrossHairBehaviour
	{
		get
		{
			return this.crossHairBehaviour;
		}
		set
		{
			this.crossHairBehaviour = value;
		}
	}

	public NotesBehaviour NotesBehaviour
	{
		get
		{
			return this.notesBehaviour;
		}
		set
		{
			this.notesBehaviour = value;
		}
	}

	public AnnBehaviour AnnBehaviour
	{
		get
		{
			return this.annBehaviour;
		}
		set
		{
			this.annBehaviour = value;
		}
	}

	public PlayerAudioBehaviour PlayerAudioBehaviour
	{
		get
		{
			return this.playerAudioBehaviour;
		}
		set
		{
			this.playerAudioBehaviour = value;
		}
	}

	public DroneBehaviour DroneBehaviour
	{
		get
		{
			return this.droneBehaviour;
		}
		set
		{
			this.droneBehaviour = value;
		}
	}

	public SourceViewerBehaviour SourceViewerBehaviour
	{
		get
		{
			return this.sourceViewerBehaviour;
		}
		set
		{
			this.sourceViewerBehaviour = value;
		}
	}

	private crossHairBehaviour crossHairBehaviour;

	private NotesBehaviour notesBehaviour;

	private AnnBehaviour annBehaviour;

	private PlayerAudioBehaviour playerAudioBehaviour;

	private DroneBehaviour droneBehaviour;

	private SourceViewerBehaviour sourceViewerBehaviour;
}
