using MarsRover.Shared.Utilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using System.IO;

namespace MarsRover.Shared.Configuration
{
    public class DecryptConfigurationProvider : JsonConfigurationProvider
    {
        public DecryptConfigurationProvider(JsonConfigurationSource source) : base(source)
        {

        }

        public override void Load(Stream stream)
        {
            base.Load(stream);
            Data = EncryptionUtils.Instance.DecryptConfigurationJson(Data);
        }
    }
    public class DecryptConfigurationSource : IConfigurationSource
    {
        public DecryptConfigurationSource() { }


        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new DecryptConfigurationProvider((JsonConfigurationSource)builder.Sources[0]);
        }

    }
}
