
/********************************************************************
created:    2015-10-26
author:     lixianmin

Copyright (C) - All Rights Reserved
*********************************************************************/

using System.IO;
using UnityEngine;

namespace Unique.IO
{
    public interface IOctetsWriter
    {
		void Write (Vector2 v);
		void Write (Vector3 v);
		void Write (Vector4 v);
		void Write (Color c);

		void Write (bool b);
		void Write (byte b);
//		void Write (char c);
		void Write (double d);
		void Write (short d);
		void Write (int d);
		void Write (long d);

		void Write (sbyte d);
		void Write (float d);
		void Write (string s);

		void Write (ushort d);
		void Write (uint d);
		void Write (ulong d);
    }
}