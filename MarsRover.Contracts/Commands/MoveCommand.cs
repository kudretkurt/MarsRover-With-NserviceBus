using NServiceBus;

namespace MarsRover.Contracts.Commands
{
    public class MoveCommand : ICommand
    {
        /// <summary>
        /// Nasanın göndermiş olduğu hareket komutu şifreli olmalıdır
        /// </summary>
        public string EncryptedMoveCommand { get; set; }

        /// <summary>
        /// Nasanın hangi araca komut gönderdiği bilgisi şifreli olmallıdır
        /// </summary>
        public string EncryptedRoverId { get; set; }
    }
}
