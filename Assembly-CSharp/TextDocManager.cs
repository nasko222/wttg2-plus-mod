using System;
using System.Collections.Generic;
using UnityEngine;

public class TextDocManager : MonoBehaviour
{
	public void CreateTextDoc(string Name, string Text)
	{
		int hashCode = Name.GetHashCode();
		if (!this.currentTextDocIcons.ContainsKey(hashCode))
		{
			float x = UnityEngine.Random.Range(15f, (float)Screen.width * 0.9f);
			float y = -UnityEngine.Random.Range(56f, (float)Screen.height - 40f - 15f);
			TextDocIconData textDocIconData = new TextDocIconData(hashCode, Name, Text, new Vect2(x, y));
			TextDocIconObject textDocIconObject = this.textDocIconPool.Pop();
			textDocIconObject.Build(textDocIconData);
			this.currentTextDocIcons.Add(hashCode, textDocIconObject);
			this.myData.CurrentDocs.Add(textDocIconData);
			DataManager.Save<TextDocManagerData>(this.myData);
			return;
		}
		string setName = Name;
		Name = Name + "(" + UnityEngine.Random.Range(0, 100000).ToString() + ")";
		int hashCode2 = Name.GetHashCode();
		if (!this.currentTextDocIcons.ContainsKey(hashCode2))
		{
			float x2 = UnityEngine.Random.Range(15f, (float)Screen.width * 0.9f);
			float y2 = -UnityEngine.Random.Range(56f, (float)Screen.height - 40f - 15f);
			TextDocIconData textDocIconData2 = new TextDocIconData(hashCode2, setName, Text, new Vect2(x2, y2));
			TextDocIconObject textDocIconObject2 = this.textDocIconPool.Pop();
			textDocIconObject2.Build(textDocIconData2);
			this.currentTextDocIcons.Add(hashCode2, textDocIconObject2);
			this.myData.CurrentDocs.Add(textDocIconData2);
			DataManager.Save<TextDocManagerData>(this.myData);
		}
	}

	public void OpenTextDoc(TextDocIconData DocData)
	{
		if (this.currentTextDocs.ContainsKey(DocData.ID))
		{
			this.currentTextDocs[DocData.ID].gameObject.SetActive(true);
			this.currentTextDocs[DocData.ID].BumpMe();
		}
		else
		{
			this.buildTextDoc(DocData);
		}
	}

	private void updateTextDocIconData()
	{
		DataManager.Save<TextDocManagerData>(this.myData);
	}

	private void buildTextDoc(TextDocIconData DocData)
	{
		TextDocObject textDocObject = this.textDocPool.Pop();
		textDocObject.gameObject.SetActive(true);
		textDocObject.Build(DocData.TextDocName, DocData.TextDocText);
		this.currentTextDocs.Add(DocData.ID, textDocObject);
	}

	private void clearTextDocs()
	{
		foreach (KeyValuePair<int, TextDocIconObject> keyValuePair in this.currentTextDocIcons)
		{
			this.textDocIconPool.Push(keyValuePair.Value);
		}
		foreach (TextDocIconObject textDocIconObject in this.textDocIconPool)
		{
			textDocIconObject.UpdateMyData.Event -= this.updateTextDocIconData;
			textDocIconObject.OpenEvents.Event -= this.OpenTextDoc;
		}
		this.currentTextDocIcons.Clear();
		this.textDocIconPool.Clear();
	}

	private void stageMe()
	{
		GameManager.StageManager.Stage -= this.stageMe;
		this.myData = DataManager.Load<TextDocManagerData>(this.myID);
		if (this.myData == null)
		{
			this.myData = new TextDocManagerData(this.myID);
			this.myData.CurrentDocs = new List<TextDocIconData>();
		}
		for (int i = 0; i < this.myData.CurrentDocs.Count; i++)
		{
			TextDocIconData textDocIconData = this.myData.CurrentDocs[i];
			TextDocIconObject textDocIconObject = this.textDocIconPool.Pop();
			int hashCode = textDocIconData.TextDocName.GetHashCode();
			textDocIconObject.Build(textDocIconData);
			this.currentTextDocIcons.Add(hashCode, textDocIconObject);
		}
		DataManager.Save<TextDocManagerData>(this.myData);
	}

	private void Awake()
	{
		this.myID = base.transform.position.GetHashCode();
		GameManager.ManagerSlinger.TextDocManager = this;
		GameManager.StageManager.Stage += this.stageMe;
		this.textDocIconPool = new PooledStack<TextDocIconObject>(delegate()
		{
			TextDocIconObject component = UnityEngine.Object.Instantiate<GameObject>(this.TextDocIconObject, LookUp.DesktopUI.TEXT_DOC_ICONS_PARENT).GetComponent<TextDocIconObject>();
			component.SoftBuild();
			component.UpdateMyData.Event += this.updateTextDocIconData;
			component.OpenEvents.Event += this.OpenTextDoc;
			return component;
		}, this.TEXT_DOC_POOL_COUNT);
		this.textDocPool = new PooledStack<TextDocObject>(delegate()
		{
			TextDocObject component = UnityEngine.Object.Instantiate<GameObject>(this.TextDocObject, LookUp.DesktopUI.WINDOW_HOLDER).GetComponent<TextDocObject>();
			component.SoftBuild();
			component.gameObject.SetActive(false);
			return component;
		}, this.TEXT_DOC_POOL_COUNT);
	}

	private void OnDestroy()
	{
		this.clearTextDocs();
	}

	[SerializeField]
	private int TEXT_DOC_POOL_COUNT = 10;

	[SerializeField]
	private GameObject TextDocIconObject;

	[SerializeField]
	private GameObject TextDocObject;

	private PooledStack<TextDocIconObject> textDocIconPool;

	private PooledStack<TextDocObject> textDocPool;

	private Dictionary<int, TextDocIconObject> currentTextDocIcons = new Dictionary<int, TextDocIconObject>(10);

	private Dictionary<int, TextDocObject> currentTextDocs = new Dictionary<int, TextDocObject>(10);

	private int myID;

	private TextDocManagerData myData;
}
