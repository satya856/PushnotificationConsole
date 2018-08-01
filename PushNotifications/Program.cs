using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PushSharp.Apple;
using PushSharp.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace PushNotifications
{
    class Program
    {
        static void Main(string[] args)
        {
            //SendNotificationToIOS();
            SendPushNotificationToANDROID();
        }

        public static void SendNotificationToIOS(string message = "Hi", int badgeCount = 1)
        {
            try
            {
                var config = new ApnsConfiguration(ApnsConfiguration.ApnsServerEnvironment.Sandbox, Settings.Instance.ApnsCertificateFile, Settings.Instance.ApnsCertificatePassword);
                var tokenList = new List<string>();
                tokenList.Add("818ced5aeccb086e872ea9871d7272843b98ac566c5ea82c6f81655d7a79e768");
                tokenList.Add("f2661a9faf61b7724a716175cd423c3a9e49f3978edfb29013948a4642ec96f7");

                var broker = new ApnsServiceBroker(config);
                broker.Start();

                foreach (var token in tokenList)
                {
                    broker.QueueNotification(new ApnsNotification
                    {
                        DeviceToken = token,
                        Payload = JObject.Parse("{ \"aps\" : { \"alert\" : \"" + message + "\", \"badge\":\"1\",\"sound\":\"sms-received1.mp3\"} }")
                    });
                }

                broker.Stop();
            }
            catch (Exception ex)
            {
                Log.Error("SendNotificationToiOS Error", ex);
            }
        }

        public static void SendPushNotificationToANDROID(string token = "", string message = "Hi")
        {
            try
            {
                var serverKey = "AAAAK_i5ges:APA91bFxfzDxjVRYkXbVqV3GkyhcJy6cBkGA3bo6GrHud6tLU0W54niZD4vyjWQQoLgI4VFjLLtbQQJnXw2qdJuUiGuW78ypM1Ep98c_51MWiqKu6fwmKu6R1RnDsmOpY2BE8FxwpyB2";
                var senderId = "188856500715";
                token = "c2eiYnexmOI:APA91bH8_DzOIfP9YD9TFLWiF9bXACJRCEcGtxuctuPAQ3AZ1hrEhc-kd_9PmgCfvUhp6RxzIhSZAYZjUqMjtTIkX-26mZif95MpkLWlykyNS_HfXHPhZvUVKyEU9BZ3NFX9E3pFIQrB";

                var tokens = new List<string>();
                tokens.Add("c2eiYnexmOI:APA91bH8_DzOIfP9YD9TFLWiF9bXACJRCEcGtxuctuPAQ3AZ1hrEhc-kd_9PmgCfvUhp6RxzIhSZAYZjUqMjtTIkX-26mZif95MpkLWlykyNS_HfXHPhZvUVKyEU9BZ3NFX9E3pFIQrB");
                tokens.Add("c2eiYnexmOI:APA91bH8_DzOIfP9YD9TFLWiF9bXACJRCEcGtxuctuPAQ3AZ1hrEhc-kd_9PmgCfvUhp6RxzIhSZAYZjUqMjtTIkX-26mZif95MpkLWlykyNS_HfXHPhZvUVKyEU9BZ3NFX9E3pFIQrB");
                var tokenArray = tokens.ToArray();

                WebRequest tRequest = WebRequest.Create("https://fcm.googleapis.com/fcm/send");
                tRequest.Method = "post";
                tRequest.ContentType = "application/json";

                //to = token,
                var data = new
                {
                    registration_ids = tokenArray,
                    data = new
                    {
                        body = message,
                        title = "A Smile Message",
                        sound = "Enabled"
                    }
                };

                var json = JsonConvert.SerializeObject(data);
                Byte[] byteArray = Encoding.UTF8.GetBytes(json);
                tRequest.Headers.Add(string.Format("Authorization: key={0}", serverKey));
                tRequest.Headers.Add(string.Format("Sender: id={0}", senderId));
                tRequest.ContentLength = byteArray.Length;

                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    //using (WebResponse tResponse = tRequest.GetResponse())
                    //{
                    //    using (Stream dataStreamResponse = tResponse.GetResponseStream())
                    //    {
                    //        using (StreamReader tReader = new StreamReader(dataStreamResponse))
                    //        {
                    //            String sResponseFromServer = tReader.ReadToEnd();
                    //            Log.Error("ANDROID notifiation for token:" + token + " message:" + message + " resposnse:" + sResponseFromServer);
                    //        }
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
            }
        }
    }
}