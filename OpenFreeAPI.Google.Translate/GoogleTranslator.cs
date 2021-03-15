/* 
 * Author   : GitHub@Guila767
 * Modifier : GitHub@KevinZonda
 * Source   : https://github.com/Guila767/GoogleTranslateApi/blob/GoogleTranslateApi_v2/GoogleTranslateApi/GoogleTranslator.cs
 * SRC LIC. : MIT license
 */

using System;
using System.Text;
using System.Net;
using System.Threading.Tasks;

using OpenFreeAPI.Google.Translate.Model;

namespace OpenFreeAPI.Google.Translate
{
    public class GoogleTranslator
    {
        private const string Url = "https://translate.googleapis.com/translate_a/single?client=gtx&sl=";
        private string Request { get; set; } = string.Empty;

        public GoogleTranslator(SupportedLanguageEnum source, SupportedLanguageEnum target)
        {
            if (target == SupportedLanguageEnum.auto)
                throw new ArgumentException("The target language can't be Auto");
            this.Request = Url + $"{source}&tl={target}&dt=t&q=";
        }

        private string Download(string text)
        {
            WebClient web = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            //web.Headers.Add(HttpRequestHeader.UserAgent, "Mozilla/5.0");
            //web.Headers.Add(HttpRequestHeader.AcceptCharset, "UTF-8");
            Uri uri = new Uri(this.Request + Uri.EscapeUriString(text));
            return web.DownloadString(uri);
        }

        private async Task<string> DownloadAsync(string text)
        {
            WebClient web = new WebClient
            {
                Encoding = Encoding.UTF8
            };
            Uri uri = new Uri(this.Request + Uri.EscapeDataString(text));
            return await web.DownloadStringTaskAsync(uri);
        }

        public string Text(string text)
        {
            string Dest = string.Empty;
            /* FIXED - Remove '\n' (Line feed/new line char) */
            text = (Download(text)).Replace("\n", "");
            /* FIXED - Gets the multiples blocks that can be received */
            Block Datablock = new Block(text);
            for (int n = 0; n < Datablock[0][0].Blocks; n++)
            {
                Block splitData = Datablock[0][0][n];
                Dest = string.Concat(Dest, splitData.Data[0]);
            }
            return Dest;
        }

        public async Task<string> GetTextAsync(string source)
        {
            string result = string.Empty;
            source = (await DownloadAsync(source)).Replace("\n", "");

            var dataBlock = new Block(source);
            for (int n = 0; n < dataBlock.Blocks; n++)
            {
                Block splitData = dataBlock[0][0][n];
                result = string.Concat(result, splitData.Data[0]);
            }
            return result;
        }
    }
}
