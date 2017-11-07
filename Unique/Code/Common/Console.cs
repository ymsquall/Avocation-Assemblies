
/********************************************************************
created:    2014-01-14
author:     lixianmin

purpose:    extended debug
Copyright (C) - All Rights Reserved
*********************************************************************/

using UnityEngine;
using System.IO;
using System.Threading;
using System.Text;
using Unique;

// this class should not be placed into Unique namespace, because it will cause compile error 
// when using 'System' and 'Unique' simutaniously.

public enum ConsoleFlags : ushort
{
	None			= 0x00,
	DetailedMessage	= 0x01,
	OpenStandardOutput	= 0x02,
	FlushOnWrite	= 0x04,
}

public static partial class Console
{
	// We use static ctor, because in editor mode there is no Way to call Init().
	static Console ()
	{
		_idMainThread = Thread.CurrentThread.ManagedThreadId;
		Flags = ConsoleFlags.DetailedMessage | ConsoleFlags.FlushOnWrite;
	}

	internal static void Tick ()
	{
		_time = Time.realtimeSinceStartup;

		if (null != _sbLogText && _time >= _nextFlushLogTime)
		{
			_FlushLogText();
			_nextFlushLogTime = _time + 3.0f;
		}
	}

    public static void WriteLine (string format, params object[] args)
    {
		_WriteLine(_lpfnLog, _FormatMessage(format, args));
    }

    public static void WriteLine (string message)
    {
		_WriteLine(_lpfnLog, message);
    }

    public static void WriteLine (object message)
    {
		_WriteLine(_lpfnLog, message);
    }
    
    public static class Warning
    {
        public static void WriteLine (string format, params object[] args)
        {
			_WriteLine(_lpfnLogWarning, _FormatMessage(format, args));
        }

        public static void WriteLine (string message)
        {
			_WriteLine(_lpfnLogWarning, message);
        }

        public static void WriteLine (object message)
        {
			_WriteLine(_lpfnLogWarning, message);
        }
    }
    
    public static class Error
    {
        public static void WriteLine (string format, params object[] args)
        {
			_WriteLine(_lpfnLogError, _FormatMessage(format, args));
        }

        public static void WriteLine (string message)
        {
			_WriteLine(_lpfnLogError, message);
        }

        public static void WriteLine (object message)
        {
			_WriteLine(_lpfnLogError, message);
        }
    }
	
    private static void _WriteLine (System.Action<object> output, object message)
    {
		var isMainThread = Thread.CurrentThread.ManagedThreadId == _idMainThread;

		if (_HasFlags(ConsoleFlags.DetailedMessage))
        {
			var frameCount = 0;
			var realtime   = string.Empty;

			if (isMainThread)
			{
				frameCount = Time.frameCount;
				realtime   = Time.realtimeSinceStartup.ToString("F3");
			}
			else
			{
				frameCount = os.frameCount;
				realtime   = _time.ToString("F3");
			}

			if (_lastFrameCount != frameCount)
			{
				_lastFrameCount = frameCount;
				_messageFormat[1] = frameCount.ToString();
			}

			_messageFormat [3] = realtime;
            _messageFormat [5] = null != message ? message.ToString() : "null mesage (-_-)";
            message = string.Concat(_messageFormat);
        }

        try
        {
			// main thread or in editor and isPlaying= false.
			if (isMainThread || _idMainThread == 0)
			{
				output (message);
			}
			else
			{
				if (os.isEditor)
				{
					var sbText = new StringBuilder(message.ToString());
					sbText.AppendLine();

					var text  = _AppendStackTrace (sbText);
					Loom.QueueOnMainThread(()=> output(text));
				}
				else
				{
					Loom.QueueOnMainThread(()=> output(message));
				}
			}
        }
        catch (System.MissingMethodException)
        {
            System.Console.WriteLine(message);
        }
    }

	private static string _AppendStackTrace (StringBuilder sbText)
	{
		var trace = new System.Diagnostics.StackTrace(2, true);
		var frames = trace.GetFrames();

		for (int i= 0; i< frames.Length; ++i)
		{
			var frame = frames[i];
			sbText.AppendFormat ("{0} (at {1}:{2})\n"
			                     , frame.GetMethod().ToString()
			                     , frame.GetFileName()
			                     , frame.GetFileLineNumber().ToString());
		}
		
		var result = sbText.ToString();
		return result;
	}

    private static string _FormatMessage (string format, params object[] args)
    {
        var message = null != format ? string.Format(null, format, args) : "null format (-__-)";

        return message;
    }

	private static void _Log (object message)
	{
		if (_HasFlags(ConsoleFlags.FlushOnWrite))
		{
			UnityEngine.Debug.Log(message);
		}
		else
		{
			_sbLogText.AppendLine(message.ToString());
		}

		if (_HasFlags(ConsoleFlags.OpenStandardOutput))
		{
			System.Console.WriteLine(message);
		}
	}

	private static void _LogWarning (object message)
	{
		_FlushLogText ();
		UnityEngine.Debug.LogWarning(message);

		if (_HasFlags(ConsoleFlags.OpenStandardOutput))
		{
			System.Console.WriteLine(message);
		}
	}

	private static void _LogError (object message)
	{
		_FlushLogText ();
		UnityEngine.Debug.LogError(message);

		if (_HasFlags(ConsoleFlags.OpenStandardOutput))
		{
			System.Console.Error.WriteLine(message);
		}
	}

	private static void _FlushLogText ()
	{
		if (null != _sbLogText && _sbLogText.Length > 0)
		{
			UnityEngine.Debug.Log (_sbLogText);
			_sbLogText.Length = 0;
		}
	}

	private static bool _HasFlags (ConsoleFlags flags)
	{
		return (_flags & flags) != 0;
	}

	[System.Obsolete("Deprecated, use Flags")]
	public static bool IsOpenStandardOutput
	{
		get 
		{
			return _HasFlags(ConsoleFlags.OpenStandardOutput);
		}

		set 
		{
			if (value)
			{
				Flags |= ConsoleFlags.OpenStandardOutput;
			}
			else
			{
				Flags &= ~ConsoleFlags.OpenStandardOutput;
			}
		}
	}

	public static ConsoleFlags Flags
	{
		get { return _flags; }
		set
		{
			if (_flags == value)
			{
				return;
			}

			_flags = value;

			if (_HasFlags(ConsoleFlags.FlushOnWrite))
			{
				_sbLogText = null;
			}
			else
			{
				_sbLogText = new StringBuilder(1024);
			}
		}
	}

	private static ConsoleFlags _flags;

	private static int	 _idMainThread;
	private static float _time;

	private static int   _lastFrameCount = 0;
	private static StringBuilder _sbLogText;
	private static float _nextFlushLogTime;

	private static System.Action<object> _lpfnLog			= _Log;
	private static System.Action<object> _lpfnLogWarning	= _LogWarning;
	private static System.Action<object> _lpfnLogError		= _LogError;

    private static readonly string[] _messageFormat = 
    {
        "[frame=",
        "(-_-)",
        ", time=",
        null,
        "] ",
        null
    };
}
