
/********************************************************************
created:    2015-12-28
author:     lixianmin

purpose:    extended debug
Copyright (C) - All Rights Reserved
*********************************************************************/

using Unique;

public static partial class Console
{
    public static class Editor
    {
        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void WriteLine (string format, params object[] args)
        {
			if (os.isEditorMode)
			{
				_WriteLine(_lpfnLog, _FormatMessage(format, args));
			}
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void WriteLine (string message)
        {
			if (os.isEditorMode)
			{
				_WriteLine(_lpfnLog, message);
			}
        }

        [System.Diagnostics.Conditional("UNITY_EDITOR")]
        public static void WriteLine (object message)
        {
			if (os.isEditorMode)
			{
				_WriteLine(_lpfnLog, message);
			}
        }

		public static class Warning
		{
			[System.Diagnostics.Conditional("UNITY_EDITOR")]
			public static void WriteLine (string format, params object[] args)
			{
				if (os.isEditorMode)
				{
					_WriteLine(_lpfnLogWarning, _FormatMessage(format, args));
				}
			}
			
			[System.Diagnostics.Conditional("UNITY_EDITOR")]
			public static void WriteLine (string message)
			{
				if (os.isEditorMode)
				{
					_WriteLine(_lpfnLogWarning, message);
				}
			}
			
			[System.Diagnostics.Conditional("UNITY_EDITOR")]
			public static void WriteLine (object message)
			{
				if (os.isEditorMode)
				{
					_WriteLine(_lpfnLogWarning, message);
				}
			}
		}
		
		public static class Error
		{
			[System.Diagnostics.Conditional("UNITY_EDITOR")]
			public static void WriteLine (string format, params object[] args)
			{
				if (os.isEditorMode)
				{
					_WriteLine(_lpfnLogError, _FormatMessage(format, args));
				}
			}
			
			[System.Diagnostics.Conditional("UNITY_EDITOR")]
			public static void WriteLine (string message)
			{
				if (os.isEditorMode)
				{
					_WriteLine(_lpfnLogError, message);
				}
			}
			
			[System.Diagnostics.Conditional("UNITY_EDITOR")]
			public static void WriteLine (object message)
			{
				if (os.isEditorMode)
				{
					_WriteLine(_lpfnLogError, message);
				}
			}
		}
    }
}
