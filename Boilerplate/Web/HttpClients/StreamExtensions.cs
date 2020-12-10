using System;
using System.IO;
using Newtonsoft.Json;

namespace Web.HttpClients
{
    public static class StreamExtensions
    {
        public static TModel ReadAndDeserializeFromJson<TModel>(this Stream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (!stream.CanRead)
            {
                throw new NotSupportedException("Cannot read from stream");
            }

            using (var streamReader = new StreamReader(stream))
            {
                using (var jsonTextReader = new JsonTextReader(streamReader))
                {
                    var jsonSerializer = new JsonSerializer();
                    return jsonSerializer.Deserialize<TModel>(jsonTextReader);
                }
            }
        }
    }
}
