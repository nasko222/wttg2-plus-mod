using System;
using System.Collections.Generic;

namespace ZenFulcrum.EmbeddedBrowser
{
	public class JSONNode
	{
		public JSONNode(JSONNode.NodeType type = JSONNode.NodeType.Null)
		{
			this._type = type;
			if (type == JSONNode.NodeType.Object)
			{
				this._objectValue = new Dictionary<string, JSONNode>();
			}
			else if (type == JSONNode.NodeType.Array)
			{
				this._arrayValue = new List<JSONNode>();
			}
		}

		public JSONNode(string value)
		{
			this._type = JSONNode.NodeType.String;
			this._stringValue = value;
		}

		public JSONNode(double value)
		{
			this._type = JSONNode.NodeType.Number;
			this._numberValue = value;
		}

		public JSONNode(Dictionary<string, JSONNode> value)
		{
			this._type = JSONNode.NodeType.Object;
			this._objectValue = value;
		}

		public JSONNode(List<JSONNode> value)
		{
			this._type = JSONNode.NodeType.Array;
			this._arrayValue = value;
		}

		public JSONNode(bool value)
		{
			this._type = JSONNode.NodeType.Bool;
			this._boolValue = value;
		}

		public static JSONNode Parse(string json)
		{
			return JSONParser.Parse(json);
		}

		public JSONNode.NodeType Type
		{
			get
			{
				return this._type;
			}
		}

		public bool IsValid
		{
			get
			{
				return this._type != JSONNode.NodeType.Invalid;
			}
		}

		public JSONNode Check()
		{
			if (this._type == JSONNode.NodeType.Invalid)
			{
				throw new InvalidJSONNodeException();
			}
			return this;
		}

		public static implicit operator string(JSONNode n)
		{
			return (n._type != JSONNode.NodeType.String) ? null : n._stringValue;
		}

		public static implicit operator JSONNode(string v)
		{
			return new JSONNode(v);
		}

		public static implicit operator int(JSONNode n)
		{
			return (n._type != JSONNode.NodeType.Number) ? 0 : ((int)n._numberValue);
		}

		public static implicit operator JSONNode(int v)
		{
			return new JSONNode((double)v);
		}

		public static implicit operator float(JSONNode n)
		{
			return (n._type != JSONNode.NodeType.Number) ? 0f : ((float)n._numberValue);
		}

		public static implicit operator JSONNode(float v)
		{
			return new JSONNode((double)v);
		}

		public static implicit operator double(JSONNode n)
		{
			return (n._type != JSONNode.NodeType.Number) ? 0.0 : n._numberValue;
		}

		public static implicit operator JSONNode(double v)
		{
			return new JSONNode(v);
		}

		public JSONNode this[string k]
		{
			get
			{
				JSONNode result;
				if (this._type == JSONNode.NodeType.Object && this._objectValue.TryGetValue(k, out result))
				{
					return result;
				}
				return JSONNode.InvalidNode;
			}
			set
			{
				if (this._type != JSONNode.NodeType.Object)
				{
					throw new InvalidJSONNodeException();
				}
				if (value._type == JSONNode.NodeType.Invalid)
				{
					this._objectValue.Remove(k);
				}
				else
				{
					this._objectValue[k] = value;
				}
			}
		}

		public static implicit operator Dictionary<string, JSONNode>(JSONNode n)
		{
			return (n._type != JSONNode.NodeType.Object) ? null : n._objectValue;
		}

		public JSONNode this[int idx]
		{
			get
			{
				if (this._type == JSONNode.NodeType.Array && idx >= 0 && idx < this._arrayValue.Count)
				{
					return this._arrayValue[idx];
				}
				return JSONNode.InvalidNode;
			}
			set
			{
				if (this._type != JSONNode.NodeType.Array)
				{
					throw new InvalidJSONNodeException();
				}
				if (idx == -1)
				{
					if (value._type == JSONNode.NodeType.Invalid)
					{
						this._arrayValue.RemoveAt(this._arrayValue.Count - 1);
					}
					else
					{
						this._arrayValue.Add(value);
					}
				}
				else if (value._type == JSONNode.NodeType.Invalid)
				{
					this._arrayValue.RemoveAt(idx);
				}
				else
				{
					this._arrayValue[idx] = value;
				}
			}
		}

		public static implicit operator List<JSONNode>(JSONNode n)
		{
			return (n._type != JSONNode.NodeType.Array) ? null : n._arrayValue;
		}

		public void Add(JSONNode item)
		{
			if (this._type != JSONNode.NodeType.Array)
			{
				throw new InvalidJSONNodeException();
			}
			this._arrayValue.Add(item);
		}

		public int Count
		{
			get
			{
				JSONNode.NodeType type = this._type;
				if (type == JSONNode.NodeType.Array)
				{
					return this._arrayValue.Count;
				}
				if (type != JSONNode.NodeType.Object)
				{
					return 0;
				}
				return this._objectValue.Count;
			}
		}

		public bool IsNull
		{
			get
			{
				return this._type == JSONNode.NodeType.Null;
			}
		}

		public static implicit operator bool(JSONNode n)
		{
			return n._type == JSONNode.NodeType.Bool && n._boolValue;
		}

		public static implicit operator JSONNode(bool v)
		{
			return new JSONNode(v);
		}

		public object Value
		{
			get
			{
				switch (this._type)
				{
				case JSONNode.NodeType.Invalid:
					this.Check();
					return null;
				case JSONNode.NodeType.String:
					return this._stringValue;
				case JSONNode.NodeType.Number:
					return this._numberValue;
				case JSONNode.NodeType.Object:
					return this._objectValue;
				case JSONNode.NodeType.Array:
					return this._arrayValue;
				case JSONNode.NodeType.Bool:
					return this._boolValue;
				case JSONNode.NodeType.Null:
					return null;
				default:
					throw new ArgumentOutOfRangeException();
				}
			}
		}

		public string AsJSON
		{
			get
			{
				return JSONParser.Serialize(this);
			}
		}

		public static readonly JSONNode InvalidNode = new JSONNode(JSONNode.NodeType.Invalid);

		public static readonly JSONNode NullNode = new JSONNode(JSONNode.NodeType.Null);

		public JSONNode.NodeType _type;

		private string _stringValue;

		private double _numberValue;

		private Dictionary<string, JSONNode> _objectValue;

		private List<JSONNode> _arrayValue;

		private bool _boolValue;

		public enum NodeType
		{
			Invalid,
			String,
			Number,
			Object,
			Array,
			Bool,
			Null
		}
	}
}
