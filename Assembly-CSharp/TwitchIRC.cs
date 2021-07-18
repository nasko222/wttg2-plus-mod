using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class TwitchIRC
{
	public TwitchIRC()
	{
		this.commandQueue = new Queue<string>();
		this.server = "irc.chat.twitch.tv";
		this.port = 6667;
		this.buffer = string.Empty;
		this.recievedMsgs = new List<string>();
	}

	public void StartTwitch(string setOAuth, string setNickName)
	{
		this.oauth = setOAuth;
		this.nickName = setNickName;
		this.channelName = setNickName;
		UnityEngine.Debug.Log("[TwitchIRC] Starting IRC...");
		this.StartIRC();
	}

	private void StartIRC()
	{
		TcpClient tcpClient = new TcpClient();
		tcpClient.Connect(this.server, this.port);
		if (!tcpClient.Connected)
		{
			return;
		}
		this.isConnected = true;
		NetworkStream networkStream = tcpClient.GetStream();
		StreamReader input = new StreamReader(networkStream);
		StreamWriter output = new StreamWriter(networkStream);
		output.WriteLine("PASS " + this.oauth);
		output.WriteLine("NICK " + this.nickName.ToLower());
		output.Flush();
		this.outProc = new Thread(delegate()
		{
			this.IRCOutputProcedure(output);
		});
		this.outProc.Start();
		this.inProc = new Thread(delegate()
		{
			this.IRCInputProcedure(input, networkStream);
		});
		this.inProc.Start();
		this.stopThreads = false;
		UnityEngine.Debug.Log("[TwitchIRC] Initialized successfully.");
	}

	private void IRCInputProcedure(TextReader input, NetworkStream networkStream)
	{
		while (!this.stopThreads)
		{
			if (networkStream.DataAvailable)
			{
				this.buffer = input.ReadLine();
				if (this.buffer.Contains("PRIVMSG #"))
				{
					object obj = this.recievedMsgs;
					lock (obj)
					{
						this.recievedMsgs.Add(this.buffer);
					}
				}
				if (this.buffer.StartsWith("PING "))
				{
					this.SendCommand(this.buffer.Replace("PING", "PONG"));
				}
				if (this.buffer.Split(new char[]
				{
					' '
				})[1] == "001")
				{
					this.SendCommand("JOIN #" + this.channelName);
				}
			}
		}
	}

	private void IRCOutputProcedure(TextWriter output)
	{
		Stopwatch stopwatch = new Stopwatch();
		stopwatch.Start();
		while (!this.stopThreads)
		{
			object obj = this.commandQueue;
			lock (obj)
			{
				if (this.commandQueue.Count > 0 && stopwatch.ElapsedMilliseconds > 1750L)
				{
					output.WriteLine(this.commandQueue.Peek());
					output.Flush();
					this.commandQueue.Dequeue();
					stopwatch.Reset();
					stopwatch.Start();
				}
			}
		}
	}

	public void SendCommand(string cmd)
	{
		object obj = this.commandQueue;
		lock (obj)
		{
			this.commandQueue.Enqueue(cmd);
		}
	}

	public void SendMsg(string msg)
	{
		object obj = this.commandQueue;
		lock (obj)
		{
			this.commandQueue.Enqueue("PRIVMSG #" + this.channelName + " :" + msg);
		}
		UnityEngine.Debug.Log(msg);
	}

	public void SendMsg(string msg, float delay)
	{
		GameManager.TimeSlinger.FireTimer<string>(delay, new Action<string>(this.SendMsg), msg, 0);
	}

	private void Start()
	{
	}

	public void Update()
	{
		object obj = this.recievedMsgs;
		lock (obj)
		{
			if (this.recievedMsgs.Count > 0)
			{
				for (int i = 0; i < this.recievedMsgs.Count; i++)
				{
					GameManager.GetDOSTwitch().chatMessageRecv(this.recievedMsgs[i]);
				}
				this.recievedMsgs.Clear();
			}
		}
	}

	public string oauth;

	public string nickName;

	public string channelName;

	public bool stopThreads;

	private Queue<string> commandQueue;

	public bool isConnected;

	private string server;

	private int port;

	private Thread outProc;

	private Thread inProc;

	private string buffer;

	private List<string> recievedMsgs;
}
