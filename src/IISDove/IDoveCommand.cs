using System;
using System.Collections.Generic;
using IISDove.Events;

namespace IISDove
{
    public interface IDoveCommand
    {
    }

    public interface ISenderCommand : IDoveCommand
    {
        void CheckAndSend();
    }

    public interface ICommanderCommand : IDoveCommand
    {
        event EventHandler<ExceuteIISRestartCommandEventArgs> ExceuteIISRestarted;

        void ReceiveAndProcess(MessageDto sendMessage);

        void SaveSenderInfo(DoveSenderDto doveSender);

        IList<DoveSenderDto> GetAllSenders();
    }
}
