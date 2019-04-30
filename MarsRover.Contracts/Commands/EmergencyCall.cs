using NServiceBus;

namespace MarsRover.Contracts.Commands
{
    public class EmergencyCall : ICommand
    {
        /// <summary>
        /// Nasanın hangi araca komut gönderdiği bilgisi şifreli olmallıdır
        /// </summary>
        public string EncryptedRoverId { get; set; }
    }
}
