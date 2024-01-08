using System.Text.Json.Nodes;
using VCardQRCode.Constants;
using VCardQRCode.Extensions;
using VCardQRCode.Models;

internal class Program
{
    private static async Task Main(string[] args)
    {
        using (HttpClient client = new HttpClient())
        {
            HttpResponseMessage response = await client.GetAsync(Constant.RandomUserApiUrl);
            string jsonData = await response.Content.ReadAsStringAsync();
            JsonNode json = JsonNode.Parse(jsonData);
            var firstResult = json["results"][1];

            VCard card = Helper.MapJsonToVCard(firstResult);

            string vCard = Helper.CreateVCard(card);
            string vCardFilePath = Path.Combine(Constant.Directory, card.FirstName + card.Surname + ".Contact.vcf");
            File.WriteAllText(vCardFilePath, vCard);

            string qrCodeFilePath = Path.Combine(Constant.Directory, card.FirstName + card.Surname + ".QRCode.png");
            byte[] qrCodeBytes = Helper.QRCodeToCard(card);
            File.WriteAllBytes(qrCodeFilePath, qrCodeBytes);
        }
    }
}