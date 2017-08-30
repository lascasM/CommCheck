using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CommCheck
{
    public class WebPageAsync
    {
        public static async Task<HttpResponseMessage> Post(Uri uri, byte[] bson)
        {
            using (var client = new HttpClient())
            {
                SetClient(client);

                try
                {
                    // Webページを取得するのは、事実上この1行だけ
                    return await client.PostAsync(uri, new ByteArrayContent(bson));
                }
                catch (HttpRequestException)
                {
                    //// 404エラーや、名前解決失敗など
                    //Console.WriteLine("\n例外発生!");
                    //// InnerExceptionも含めて、再帰的に例外メッセージを表示する
                    //Exception ex = e;
                    //while (ex != null)
                    //{
                    //    Console.WriteLine("例外メッセージ: {0} ", ex.Message);
                    //    ex = ex.InnerException;
                    //}
                }
                catch (TaskCanceledException e)
                {
                    // タスクがキャンセルされたとき（一般的にタイムアウト）
                    Console.WriteLine("\nタイムアウト!");
                    Console.WriteLine("例外メッセージ: {0} ", e.Message);
                }
                return null;
            }
        }

        private static void SetClient(HttpClient client)
        {
            // ユーザーエージェント文字列をセット（オプション）
            client.DefaultRequestHeaders.Add(
                "User-Agent",
                "Mozilla/5.0 (Windows NT 6.3; Trident/7.0; rv:11.0) like Gecko");

            // 受け入れ言語をセット（オプション）
            client.DefaultRequestHeaders.Add("Accept-Language", "ja-JP");

            // タイムアウトをセット（オプション）
            client.Timeout = TimeSpan.FromSeconds(10.0);
        }
    }
}