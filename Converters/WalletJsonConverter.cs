using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Hubtel.Wallets.Api.Models;

namespace Hubtel.Wallets.Api.Converters
{

    public class WalletConverter : JsonConverter<Wallet>
    {
        public override Wallet Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var json = JsonSerializer.Deserialize<JsonElement>(ref reader);
            var accountNumber = json.GetProperty("AccountNumber").GetString();
            var owner = json.GetProperty("Owner").GetString();
            var name = json.GetProperty("Name").GetString();
            var wallet = new Wallet(accountNumber);
            wallet.Owner = owner;
            wallet.Name = name;
            return wallet;
        }

        public override void Write(Utf8JsonWriter writer, Wallet value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WriteString("Id", value.Id);
            writer.WriteString("AccountNumber", value.AccountNumber);
            writer.WriteString("Type", value.Type.ToString());
            writer.WriteString("Scheme", value.Scheme.ToString());
            writer.WriteString("CreatedAt", value.CreatedAt.ToString());
            writer.WriteString("Owner", value.Owner);
            writer.WriteString("Name", value.Name);
            writer.WriteEndObject();
        }
    }

}
