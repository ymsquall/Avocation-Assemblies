using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

namespace Networking.HTTPNetworking
{
    public class HttpPost : IDisposable
    {
        public IEnumerator DoPost(string url, WWWForm ps, Action<WWW> handler)
        {
            WWW post = null;
            if (null != ps)
            {
                if(ps.data.Length <= 0)
                {
                    ps.AddField("", "");
                }
                post = new WWW(url, ps);
            }
            else
            {
                ps = new WWWForm();
                ps.AddField("", "");
                post = new WWW(url, ps);
            }
            yield return post;
            if (post.error != null)
            {
                throw new Exception(post.error);
            }
            handler?.Invoke(post);
        }
        public IEnumerator DoGet(string url, Action<WWW> handler)
        {
            var get = new WWW(url);
            yield return get;
            if (get.error != null)
            {
                throw new Exception(get.error);
            }
            handler?.Invoke(get);
        }

        public void Dispose()
        {
        }
    }
}
