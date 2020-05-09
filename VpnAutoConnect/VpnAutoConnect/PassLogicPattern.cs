using AngleSharp.Html.Dom;
using AngleSharp.Html.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace VpnAutoConnect
{
    public class PassLogicPattern
    {
        /// <summary>
        /// PassLogicワンタイムパスワード接続先URI
        /// </summary>
        private Uri _uri { get; set; }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="uri"></param>
        public PassLogicPattern(string uri)
        {
            _uri = new Uri(uri);
        }

        /// <summary>
        /// ワンタイムパスワードテーブル取得
        /// </summary>
        /// <returns></returns>
        public async Task<int[]> GetTable()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(_uri);
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var source = await response.Content.ReadAsStringAsync();

                    // アカウントロック検出
                    if (source.Contains("This Account is locked."))
                        throw new Exception("アカウントがロックされています。");

                    // AngleSharpでパース
                    var doc = await new HtmlParser().ParseDocumentAsync(source);
                    var tds = doc.QuerySelector("#passlogic-matrix").QuerySelectorAll("td.cell").Select(m => Int32.Parse(m.TextContent)).ToArray();

                    var passTbl = new int[48];
                    for (int row = 0; row < 4; row++)
                        for (int grp = 0; grp < 3; grp++)
                            Array.Copy(tds, row * 12 + grp * 4, passTbl, grp * 16 + row * 4, 4);
                    return passTbl;
                }
                else
                {
                    throw new Exception($"ワンタイムパスワードが取得できませんでした。{response.StatusCode}");
                }
            }
        }
    }
}
