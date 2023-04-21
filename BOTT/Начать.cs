//using MySql.Data.MySqlClient;
using System;
using System.Threading;
using VkNet;
using VkNet.Model;
using VkNet.Model.Keyboard;
using VkNet.Model.RequestParams;
namespace BOTT
{
    class Program
    {
        //public static MySqlConnection con = new MySqlConnection("Database = bottest;Data Source = 127.0.0.1;Username = root;Password = 0314530886;Port = 3306;CharSet = utf8");
        //public static MySqlCommand com = new MySqlCommand();
        //public static MySqlConnection con1 = new MySqlConnection("Database = bottest;Data Source = 127.0.0.1;Username = root;Password = 0314530886;Port = 3306;CharSet = utf8");
        //public static MySqlCommand com1 = new MySqlCommand();
        public static VkApi vk = new VkApi();
        static void Main(string[] args)
        {
            vk.Authorize(new ApiAuthParams
            {
                AccessToken = "vk1.a.TnRfKh4l-KAFD1MqdY3ec1pheGMMUSRWf19ySd1S0qpN0wvoxSNpTY9KG1PmtFNzypiIQauGAlyybi057eyz9FBaI4ZgfXodxJ7ifyC--wdAD9Ky_3hyn6os1qpSwWGGtbc1NnaXJIb5bwhQaBBH8l3bXfF90L4Ah6TQc1iktia3X2kyhUjV4YpnNI984NuwV1SwLpBbjepHxLqyU2EXkQ"
            });
            //con.Open();
            //com.Connection = con;
            while (true)
            {
                Thread.Sleep(50);
                Receive();
            }

        }
        //public static bool IsBanned(long? userid)
        //{
        //    com.CommandText = string.Format("SELECT profile_ban FROM Profiles WHERE profile_id = {0};",userid.ToString());
        //    int result = Convert.ToInt32(com.ExecuteScalar());
        //    if(result == 1)
        //        return true;

        //    return false;
        //}
        //public static bool IsRegistred (long? userid)
        //{
        //    com.CommandText = string.Format("SELECT COUNT(*) FROM Profiles WHERE profile_id = {0};",userid.ToString());
        //    int result = Convert.ToInt32(com.ExecuteScalar());
        //    if(result == 0)
        //        return false;
        //    return true;
        //}
        public static bool Receive()
        {
            object[] minfo = GetMessage();
            long? userid = Convert.ToUInt32(minfo[2]);
            if (minfo[0] == null)
                return false;
            KeyboardBuilder key = new KeyboardBuilder();
            key.AddButton("Ассортимент", "menu", VkNet.Enums.SafetyEnums.KeyboardButtonColor.Primary);
            string code = "";
            if (minfo[1].ToString() != "")
                code = minfo[1].ToString();
            else
                code = minfo[0].ToString();
            switch (code.ToLower())
            {
                case "Go":
                    key.AddButton("Ассортимент", "menu", VkNet.Enums.SafetyEnums.KeyboardButtonColor.Primary);
                    key.AddLine();
                    key.AddButton("Тупой бот", "bot", VkNet.Enums.SafetyEnums.KeyboardButtonColor.Negative);
                    SendMessage("Здравствуйте! Вас приветствует лучший брендовый магазин г.Волгограда, выберите необходимую кнопку:", userid, key.Build());
                    break;
                case "bot":
                    SendMessage("Сам ты бот сука, Пошел нахуй долбаеб, дай сука отдохнуть заебали", userid, null);
                    break;
                case "menu":

                    key.AddButton("Мужское", "m", VkNet.Enums.SafetyEnums.KeyboardButtonColor.Positive);
                    key.AddButton("Женское", "j", VkNet.Enums.SafetyEnums.KeyboardButtonColor.Negative);

                    SendMessage("Для выбора одежды выберите: m - мужская " +
                        " j - женская ", userid, null);
                    break;
                case "m":
                    SendMessage("Мужской раздел в разработке...", userid, null);

                    break;
                case "j":
                    SendMessage("Женский раздел в разработке...", userid, null);

                    break;
                default:
                    SendMessage("жду команду", userid, null);
                    break;
            }
            return true;
        }
        public static object[] GetProfile(long? userid)
        {
            //con1.Open();
            //com.Connection = con;
            object[] info = new object[3];
            //com.CommandText = string.Format("SELECT profile_id,profile_name,profile_coin FROM Profiles WHERE profile_id = {0};",userid.ToString());
            //MySqlDataReader reader = com.ExecuteReader();
            //while(reader.Read())
            //{
            //    info[0] = reader.GetUInt32(0);
            //    info[1] = reader.GetString(1);
            //    info[2] = reader.GetInt32(2);
            //}
            //reader.Close();
            //con1.Close();
            return info;
        }
        public static void SendMessage(string message, long? userid, MessageKeyboard keyboard)
        {
            vk.Messages.Send(new MessagesSendParams
            {
                Message = message,
                PeerId = userid,
                RandomId = new Random().Next(),
                Keyboard = keyboard
            });
        }
        public static object[] GetMessage()
        {
            string message = "";
            string keyname = "";
            long? userid = 0;
            var messages = vk.Messages.GetDialogs(new MessagesDialogsGetParams
            {
                Count = 10,
                Unread = true
            });
            if (messages.Messages.Count != 0)
            {
                if (messages.Messages[0].Body.ToString() != "" && messages.Messages[0].Body.ToString() != null)
                    message = messages.Messages[0].Body.ToString();
                else
                    message = "";

                if (messages.Messages[0].Payload != null)
                {
                    keyname = messages.Messages[0].Payload.ToString();
                    keyname = keyname.Split("{\"button\":\"")[1];
                    keyname = keyname.Split("\"}")[0];
                }
                else
                    keyname = "";
                userid = messages.Messages[0].UserId.Value;
                object[] keys = new object[3] { message, keyname, userid };
                vk.Messages.MarkAsRead(userid.ToString());
                Console.WriteLine("готово!");
                return keys;
                
            }
            else
                Console.WriteLine("готово! (ex2)");

            return new object[] { null, null, null };
            
        }
        
    }
}
