using System;

namespace Framework.Tools
{
    public interface ILooping
    {
        void OnMainLoop(int dt);
    }
    public interface ILogic : IDisposable
    {
        bool Init();
        bool Update(int deltaTime);
    }
}
