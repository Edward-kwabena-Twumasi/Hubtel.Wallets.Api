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
            
            string accountNumber, owner, name;

            if (json.TryGetProperty("accountNumber", out var accountNumberProp) && accountNumberProp.ValueKind != JsonValueKind.Null)
            {
                accountNumber = accountNumberProp.GetString();
            }
            else
            {
                accountNumber = "";
            }

            if (json.TryGetProperty("ownerPhone", out var ownerProp) && ownerProp.ValueKind != JsonValueKind.Null)
            {
                owner = ownerProp.GetString();
            }
            else
            {
                owner = "";
            }

            if (json.TryGetProperty("name", out var nameProp) && nameProp.ValueKind != JsonValueKind.Null)
            {
                name = nameProp.GetString();
            }
            else
            {
                name = "";
            }

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
