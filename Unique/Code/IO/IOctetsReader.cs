
/********************************************************************
created:    2015-10-26
author:     lixianmin

Copyright (C) - All Rights Reserved
*********************************************************************/

using System.IO;
using UnityEngine;

namespace Unique.IO
{
    public interface IOctetsReader
    {
		Vector2 ReadVector2 ();
		Vector3 ReadVector3 ();
		Vector4 ReadVector4 ();
		Color	ReadColor ();

		bool	ReadBoolean ();
		byte	ReadByte ();
//		char	ReadChar ();
		double	ReadDouble ();
		short	ReadInt16 ();
		int		ReadInt32 ();
		long	ReadInt64 ();

		sbyte	ReadSByte ();
		float	ReadSingle ();
		string	ReadString ();

		ushort	ReadUInt16 ();
		uint	ReadUInt32 ();
		ulong	ReadUInt64 ();
    }
}