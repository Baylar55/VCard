using QRCoder;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using VCardQRCode.Models;

namespace VCardQRCode.Extensions
{
    public class Helper
    {
        public static byte[] GenerateQRCode(string plainText)
        {
            QRCodeGenerator generator = new();
            QRCodeData data = generator.CreateQrCode(plainText, QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode qRCode = new(data);
            byte[] byteGraphic = qRCode.GetGraphic(3, new byte[] { 84, 99, 71 }, new byte[] { 240, 240, 240 });
            return byteGraphic;
        }

        public static string CreateVCard(VCard card)
        {
            StringBuilder vCardBuilder = new StringBuilder();

            vCardBuilder.AppendLine("BEGIN:VCARD");
            vCardBuilder.AppendLine("VERSION:3.0");
            vCardBuilder.AppendLine($"N:{card.Surname} {card.FirstName}");
            vCardBuilder.AppendLine($"EMAIL:{card.Email}");
            vCardBuilder.AppendLine($"TEL:{card.Phone}");
            vCardBuilder.AppendLine($"ADR:{card.Country}, {card.City}");
            vCardBuilder.AppendLine("END:VCARD");

            return vCardBuilder.ToString();
        }

        public static byte[] QRCodeToCard(VCard card)
        {
            string plainText = JsonSerializer.Serialize(card);
            var byteImage = GenerateQRCode(plainText);
            return byteImage;
        }

        public static VCard MapJsonToVCard(JsonNode json)
        {
            return new VCard
            {
                Id = json["login"]["uuid"].ToString(),
                FirstName = json["name"]["first"].ToString(),
                Surname = json["name"]["last"].ToString(),
                Email = json["email"].ToString(),
                Phone = json["phone"].ToString(),
                Country = json["location"]["country"].ToString(),
                City = json["location"]["city"].ToString(),
            };
        }
    }
}
