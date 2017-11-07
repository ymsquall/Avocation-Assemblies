namespace Networking.TcpIPNetworking
{
    public interface ITcpNetLogicModel
    {
        RecvMsgType[] CanRecvMessages { get; }

        bool HandleMessage(IS2C_Msg msg, ref bool handled, ref bool breaked);
    }

    public interface ITcpNetworkingModel
    {
        void TcpNetLockScreen();
        void TcpNetUnlockScreen();
    }
}
