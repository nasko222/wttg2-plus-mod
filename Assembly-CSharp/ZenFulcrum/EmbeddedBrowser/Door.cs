using System;
using System.Diagnostics;
using UnityEngine;

namespace ZenFulcrum.EmbeddedBrowser
{
	public class Door : MonoBehaviour
	{
		[DebuggerBrowsable(DebuggerBrowsableState.Never)]
		public event Action<Door.OpenState> stateChange = delegate(Door.OpenState state)
		{
		};

		public Door.OpenState State
		{
			get
			{
				return this._state;
			}
			set
			{
				this._state = value;
				this.stateChange(this._state);
			}
		}

		public void Start()
		{
			this.closedPos = base.transform.position;
			this.openPos = base.transform.position + this.openOffset;
			this.State = Door.OpenState.Closed;
			Browser browser = base.GetComponentInChildren<Browser>();
			browser.CallFunction("setRequiredCoins", new JSONNode[]
			{
				this.numCoins
			});
			browser.RegisterFunction("toggleDoor", delegate(JSONNode args)
			{
				string text = args[0].Check();
				if (text != null)
				{
					if (!(text == "open"))
					{
						if (!(text == "close"))
						{
							if (text == "toggle")
							{
								this.Toggle();
							}
						}
						else
						{
							this.Close();
						}
					}
					else
					{
						this.Open();
					}
				}
			});
			PlayerInventory.Instance.coinCollected += delegate(int coinCount)
			{
				browser.CallFunction("setCoinCoint", new JSONNode[]
				{
					coinCount
				});
			};
		}

		public void Toggle()
		{
			if (this.State == Door.OpenState.Open || this.State == Door.OpenState.Opening)
			{
				this.Close();
			}
			else
			{
				this.Open();
			}
		}

		public void Open()
		{
			if (this.State == Door.OpenState.Open)
			{
				return;
			}
			this.State = Door.OpenState.Opening;
		}

		public void Close()
		{
			if (this.State == Door.OpenState.Closed)
			{
				return;
			}
			this.State = Door.OpenState.Closing;
		}

		public void Update()
		{
			if (this.State == Door.OpenState.Opening)
			{
				float num = Vector3.Distance(base.transform.position, this.closedPos) / this.openOffset.magnitude;
				num = Mathf.Min(1f, num + Time.deltaTime / this.openSpeed);
				base.transform.position = Vector3.Lerp(this.closedPos, this.openPos, num);
				if (num >= 1f)
				{
					this.State = Door.OpenState.Open;
				}
			}
			else if (this.State == Door.OpenState.Closing)
			{
				float num2 = Vector3.Distance(base.transform.position, this.openPos) / this.openOffset.magnitude;
				num2 = Mathf.Min(1f, num2 + Time.deltaTime / this.openSpeed);
				base.transform.position = Vector3.Lerp(this.openPos, this.closedPos, num2);
				if (num2 >= 1f)
				{
					this.State = Door.OpenState.Closed;
				}
			}
		}

		public Vector3 openOffset = new Vector3(0f, -6.1f, 0f);

		[Tooltip("Time to open or close, in seconds.")]
		public float openSpeed = 2f;

		[Tooltip("Number of coins needed to open the door.")]
		public int numCoins;

		private Vector3 closedPos;

		private Vector3 openPos;

		private Door.OpenState _state;

		public enum OpenState
		{
			Open,
			Closed,
			Opening,
			Closing
		}
	}
}
