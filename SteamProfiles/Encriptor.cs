using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SteamProfiles
{
    public static class Encriptor
    {
        static string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz!.,<>{}[]\"№;%':?*()_+-=/\\0123456789";
        public static string Encypter(in string text)
        {
            string result = "";
            try
            {
                string key = Key();
                string tn = "";
                string kn = "";

                for (int i = 0; i < text.Length; i++)
                {
                    for (int j = 0; j < alphabet.Length; j++)
                    {
                        if (text[i] == ' ')
                        {
                            if (tn.Length > 0)
                            {
                                tn = tn.Remove(tn.Length - 1);
                            }
                            tn += " ";
                            break;
                        }
                        if (text[i] == alphabet[j])
                        {
                            tn += j + "-";
                        }
                    }
                }
                tn = tn.Remove(tn.Length - 1);
                for (int i = 0; i < key.Length; i++)
                {
                    for (int j = 0; j < alphabet.Length; j++)
                    {
                        if (key[i] == alphabet[j])
                        {
                            kn += j + "-";
                        }
                    }
                }
                kn = kn.Remove(kn.Length - 1);
                string[] textnumber = tn.Split(' ');
                string encn = "";
                string[] keysp = kn.Split('-');
                int k = 0;
                for (int i = 0; i < textnumber.Length; i++)
                {
                    string[] textsp = textnumber[i].Split('-');
                    for (int j = 0; j < textsp.Length; j++, k++)
                    {
                        if (k >= keysp.Length)
                        {
                            k = 0;
                        }
                        var tmp = Convert.ToInt32(textsp[j].ToString());
                        var tmp1 = Convert.ToInt32(keysp[k].ToString());
                        var add = tmp + tmp1;
                        if (add >= alphabet.Length)
                        {
                            add -= alphabet.Length;
                        }
                        encn += add + "-";
                    }
                    encn = encn.Remove(encn.Length - 1);
                    encn += " ";

                }
                encn = encn.Remove(encn.Length - 1);
                string[] entotext = encn.Split(' ');

                for (int i = 0; i < entotext.Length; i++)
                {
                    string[] sp = entotext[i].Split('-');
                    for (int j = 0; j < sp.Length; j++)
                    {
                        result += alphabet[Convert.ToInt32(sp[j])];
                    }
                    result += " ";
                }
                return ToHex(ToBase64(result));
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return "";
        }
        public static string Decypter(string text)
        {
            try
            {
                string key = Key();
                string tn = "";
                string kn = "";
                string result = "";
                text = FromBase64(FromHex(text));
                for (int i = 0; i < text.Length; i++)
                {
                    for (int j = 0; j < alphabet.Length; j++)
                    {
                        if (text[i] == ' ')
                        {
                            if (tn.Length > 0)
                            {
                                tn = tn.Remove(tn.Length - 1);
                            }
                            tn += " ";
                            break;
                        }
                        if (text[i] == alphabet[j])
                        {
                            tn += j + "-";
                        }
                    }
                }
                tn = tn.Remove(tn.Length - 1);
                for (int i = 0; i < key.Length; i++)
                {
                    for (int j = 0; j < alphabet.Length; j++)
                    {
                        if (key[i] == alphabet[j])
                        {
                            kn += j + "-";
                        }
                    }
                }
                kn = kn.Remove(kn.Length - 1);
                string[] textnumber = tn.Split(' ');
                string encn = "";
                string[] keysp = kn.Split('-');
                int k = 0;
                for (int i = 0; i < textnumber.Length; i++)
                {
                    string[] textsp = textnumber[i].Split('-');
                    for (int j = 0; j < textsp.Length; j++, k++)
                    {
                        if (k >= keysp.Length)
                        {
                            k = 0;
                        }
                        var tmp = Convert.ToInt32(textsp[j].ToString());
                        var tmp1 = Convert.ToInt32(keysp[k].ToString());
                        var add = tmp - tmp1 + alphabet.Length;
                        if (add >= alphabet.Length)
                        {
                            add -= alphabet.Length;
                        }
                        encn += add + "-";
                    }
                    encn = encn.Remove(encn.Length - 1);
                    encn += " ";

                }
                encn = encn.Remove(encn.Length - 1);
                string[] entotext = encn.Split(' ');

                for (int i = 0; i < entotext.Length; i++)
                {
                    string[] sp = entotext[i].Split('-');
                    for (int j = 0; j < sp.Length; j++)
                    {
                        result += alphabet[Convert.ToInt32(sp[j])];
                    }
                    result += " ";
                }
                return result;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            return "";
        }
        static string ToBase64(string value)
        {
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(value));
        }
        static string FromBase64(string value)
        {
            return Encoding.UTF8.GetString(Convert.FromBase64String(value));
        }
        static string ToHex(string value)
        {
            return BitConverter.ToString(Encoding.UTF8.GetBytes(value));//.Replace("-", "");
        }
        static string FromHex(string value)
        {
            value = value.Replace("-", "");
            byte[] raw = new byte[value.Length / 2];
            for (int i = 0; i < raw.Length; i++)
            {
                raw[i] = Convert.ToByte(value.Substring(i * 2, 2), 16);
            }
            return Encoding.UTF8.GetString(raw);
        }
        static string Key()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\SteamProfiles", true))
            {
                if (key != null)
                {
                    if (key.GetValue("Guid") != null)
                    {
                        return key.GetValue("Guid").ToString();
                    }
                    else
                    {
                        key.SetValue("Guid", Guid.NewGuid().ToString());
                        return key.GetValue("Guid").ToString();
                    }
                }
                else
                {
                    key.SetValue("Guid", Guid.NewGuid().ToString());
                    return key.GetValue("Guid").ToString();
                }
            }
        }
    }
}
