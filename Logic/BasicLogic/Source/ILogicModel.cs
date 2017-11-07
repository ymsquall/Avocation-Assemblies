using Networking.TcpIPNetworking;

namespace Logic.BasicLogic
{
    public interface ILogicModel : ITcpNetLogicModel
    {
        bool HandleMessage(IS2C_Msg msg);
        bool LeaveScene { get; }
        void ActiveLogic();
        void DeActiveLogic();
    }
}
