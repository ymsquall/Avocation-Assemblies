using System;
using System.IO;
using System.Collections.Generic;
using Framework.Tools;

#if UNITY_3 || UNITY_4 || UNITY_5
#if NETFX_CORE && UNITY_METRO && !UNITY_EDITOR
        //using Threading_WP_8_1;
#else
#endif
#endif

namespace Scripting.UniLua
{
#if !UNITY_3 && !UNITY_4 && !UNITY_5
    public delegate string PathHook(string filename);
    public class LuaFile
    {
        private static PathHook pathhook = (s) => Path.Combine(Path.Combine(Environment.CurrentDirectory.Replace(@"\", "/"), "../"), s);
        public static void SetPathHook(PathHook hook)
        {
			pathhook = hook;
		}
		public static FileLoadInfo OpenFile( string filename )
		{
			//var path = System.IO.Path.Combine(LUA_ROOT, filename);
			var path = pathhook(filename);
			return new FileLoadInfo( File.Open( path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite ) );
		}

		public static bool Readable( string filename )
		{
			//var path = System.IO.Path.Combine(LUA_ROOT, filename);
			var path = pathhook(filename);
			try {
				using( var stream = File.Open( path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite ) ) {
					return true;
				}
			}
			catch( Exception ) {
				return false;
			}
		}
#else
    public class LuaFile
    {
        public static FileLoadInfo OpenFile(string filename)
        {
            FileLoadInfo ret = null;
    #if UNITY_ANDROID && !UNITY_EDITOR
            WWW file = StreamAssetHelper.LoadAsset(StreamAssetRoot.LUA_ROOT, filename);
            while (!file.isDone && (file.progress < 0.9f))
            {
                Thread.Sleep(100);
            }
            if (file.bytes != null)
            {
                MemoryStream stream = new MemoryStream(file.bytes);
                ret = new FileLoadInfo(stream);
            }
    #else
            Stream stream = StreamAssetHelper.LoadFile(StreamAssetRoot.LUA_ROOT, filename);
            ret = new FileLoadInfo(stream);
    #endif
            return ret;
        }

        public static bool Readable(string filename)
        {
    #if UNITY_ANDROID && !UNITY_EDITOR
            try
            {
                using(WWW file = StreamAssetHelper.LoadAsset(StreamAssetRoot.LUA_ROOT, filename))
                using (MemoryStream stream = new MemoryStream(file.bytes))
                {
                    return true;
                }
            }
    #else
            try
            {
                using (Stream stream = StreamAssetHelper.LoadFile(StreamAssetRoot.LUA_ROOT, filename))
                {
                    return true;
                }
            }
    #endif
            catch (Exception)
            {
                return false;
            }
        }
#endif
    }

    public class FileLoadInfo : ILoadInfo, IDisposable
    {
#if !UNITY_3 && !UNITY_4 && !UNITY_5
		public FileLoadInfo( FileStream stream )
        {
			Stream = stream;
            Reader = new StreamReader(Stream, System.Text.Encoding.UTF8);
            Buf = new Queue<char>();
		}
#else
        public FileLoadInfo(Stream stream)
        {
            Stream = stream;
            Buf = new Queue<byte>();
        }
#endif

        public int ReadByte()
		{
			if( Buf.Count > 0 )
				return (int)Buf.Dequeue();
            else
#if !UNITY_3 && !UNITY_4 && !UNITY_5
                return Reader.Read();
#else
				return Stream.ReadByte();
#endif
        }

		public int PeekByte()
		{
			if( Buf.Count > 0 )
				return (int)Buf.Peek();
			else
            {
#if !UNITY_3 && !UNITY_4 && !UNITY_5
				var c = Reader.Read();
				if( c == -1 )
					return c;
				Save( (char)c );
#else
                var c = Stream.ReadByte();
				if( c == -1 )
					return c;
				Save( (byte)c );
#endif
                return c;
			}
		}

		public void Dispose()
        {
#if !UNITY_3 && !UNITY_4 && !UNITY_5
            Reader.Dispose();
#endif
            Stream.Dispose();
		}

		private const string UTF8_BOM = "\u00EF\u00BB\u00BF";
#if !UNITY_3 && !UNITY_4 && !UNITY_5
        private FileStream Stream;
        private StreamReader Reader;
        private Queue<char>	Buf;
#else
        private Stream Stream;
        private Queue<byte> Buf;
#endif

#if !UNITY_3 && !UNITY_4 && !UNITY_5
        private void Save( char b )
#else
        private void Save( byte b )
#endif
        {
			Buf.Enqueue( b );
		}

		private void Clear()
		{
			Buf.Clear();
		}

#if !UNITY_3 && !UNITY_4 && !UNITY_5
    #if false
		private int SkipBOM()
		{
			for( var i=0; i<UTF8_BOM.Length; ++i )
			{
				var c = Stream.ReadByte();
				if(c == -1 || c != (byte)UTF8_BOM[i])
					return c;
				Save( (char)c );
			}
			// perfix matched; discard it
			Clear();
			return Stream.ReadByte();
		}
    #endif
		public void SkipComment()
		{
			var c = Reader.Read();//SkipBOM();
			// first line is a comment (Unix exec. file)?
			if( c == '#' )
			{
				do 
                {
					c = Reader.Read();
				} 
                while( c != -1 && c != '\n' );
				Save( (char)'\n' ); // fix line number
			}
			else if( c != -1 )
			{
				Save( (char)c );
			}
		}
#else
        private int SkipBOM()
		{
			for( var i=0; i<UTF8_BOM.Length; ++i )
			{
				var c = Stream.ReadByte();
				if(c == -1 || c != (byte)UTF8_BOM[i])
					return c;
				Save( (byte)c );
			}
			// perfix matched; discard it
			Clear();
			return Stream.ReadByte();
		}

		public void SkipComment()
		{
			var c = SkipBOM();

			// first line is a comment (Unix exec. file)?
			if( c == '#' )
			{
				do {
					c = Stream.ReadByte();
				} while( c != -1 && c != '\n' );
				Save( (byte)'\n' ); // fix line number
			}
			else if( c != -1 )
			{
				Save( (byte)c );
			}
        }
#endif
    }
}

