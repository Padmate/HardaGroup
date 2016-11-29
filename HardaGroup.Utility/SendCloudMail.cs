using HardaGroup.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HardaGroup.Utility
{
    public class SendCloudMail
    {
        public static Message Send(M_Mail mail)
        {
            Message message = new Message();
            message.Success = true;

            String url = "http://api.sendcloud.net/apiv2/mail/send";

            String api_user = "AllenMa_test_PjhzQW";
            String api_key = "vcxHk2Jcquzs4W66";

            HttpClient client = null;
            HttpResponseMessage response = null;

            try
            {
                client = new HttpClient();

                List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>();

                paramList.Add(new KeyValuePair<string, string>("apiUser", api_user));
                paramList.Add(new KeyValuePair<string, string>("apiKey", api_key));
                paramList.Add(new KeyValuePair<string, string>("from", mail.From));
                paramList.Add(new KeyValuePair<string, string>("fromName", mail.Creator));
                //多个收件人使用‘;’号隔开
                paramList.Add(new KeyValuePair<string, string>("to", mail.To));
                paramList.Add(new KeyValuePair<string, string>("subject", mail.Subject));
                paramList.Add(new KeyValuePair<string, string>("html", mail.Body));

                response = client.PostAsync(url, new FormUrlEncodedContent(paramList)).Result;
                String result = response.Content.ReadAsStringAsync().Result;

            }
            catch (Exception e)
            {
                message.Success = false;
                message.Content = "邮件发送失败。异常：" + e.Message;

            }
            finally
            {
                if (null != client)
                {
                    client.Dispose();
                }
            }

            return message;
        }
    }
}
