using MarsRover.Shared.Utilities;
using Xunit;

namespace MarsRover.Tests
{
    public class EncryptionUtilsTests
    {
        [Fact]
        [Trait("EncryptionUtilsTests", "Encrypt")]
        public void Encrypt_And_Decrypt_Method_Must_Work_Correctly()
        {
            #region Arrange
            var val = "Nasa.Error";
            var encryptVal = EncryptionUtils.Instance.Encrypt(val);
            var decryptVal = EncryptionUtils.Instance.Decrypt(encryptVal); 
            #endregion

            Assert.Equal(val, decryptVal);

            
        }
    }
}
