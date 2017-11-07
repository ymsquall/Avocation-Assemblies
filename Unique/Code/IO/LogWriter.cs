
/********************************************************************
created:    2015-09-16
author:     lixianmin

Copyright (C) - All Rights Reserved
*********************************************************************/

using UnityEngine;
using System;
using System.IO;

namespace Unique.IO
{
    public class LogWriter: StreamWriter
    {
		public LogWriter (string path, bool append) : base(path, append)
		{
			_path	= path;
			NewLine = os.linesep;

			if (BaseStream.Position == 0)
			{
				_WriteFileHead();
			}
		}

        private void _WriteFileHead ()
        {
			var now = DateTime.Now.ToString("yyyy-M-d HH:mm ddd");
			WriteLine(now);
			base.WriteLine(os.linesep);
        }

        public override void WriteLine (string format, params object[] args)
        {
            var message = _FormatMessage(format, args);
            _WriteLine(message);
        }

        public override void WriteLine (string message)
        {
            _WriteLine(message);
        }

        public override void WriteLine (object message)
        {
            _WriteLine(message);
        }

        private void _WriteLine (object message)
        {
			base.Write("[frame= ");
			base.Write(Time.frameCount);
			base.Write(", time= ");
			base.Write(Time.realtimeSinceStartup.ToString("F3"));
			base.Write("] ");
			
			base.WriteLine(message);
			base.WriteLine();

			base.Flush();
        }

        private string _FormatMessage (string format, params object[] args)
        {
            var message = null != format ? string.Format(null, format, args) : "null format (-__-)";
            return message;
        }

        public string GetPath ()
        {
            return _path;
        }

        private string _path;
    }
}