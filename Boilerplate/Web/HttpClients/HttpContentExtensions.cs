using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Web.HttpClients
{
    public static class HttpContentExtensions
    {
        public static async Task<TModel> ReadAndDeserializeFromJson<TModel>(this HttpContent content)
        {
            if (!(content is object))
            {
                throw new ArgumentException("Cannot deserialize to Json format from other type than object");
            }

            var stream = await content.ReadAsStreamAsync();

            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            if (!stream.CanRead)
            {
                throw new NotSupportedException("Cannot read from stream");
            }

            return await JsonSerializer.DeserializeAsync<TModel>(stream);
        }

        //private static void Test(PipeWriter pipeWriter)
        //{
        //    var options = new JsonWriterOptions { Indented = true };

        //    using var writer = new Utf8JsonWriter(pipeWriter, options);

        //    writer.WriteStartObject();
        //    writer.WriteString("status", "");

        //    try
        //    {

        //    }
        //    finally
        //    {
        //        writer.Dispose();
        //    }
        //    // etc
        //}
    }
}
