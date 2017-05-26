using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Collections.Generic;
using ITD.ETC.VETC.Synchonization.Controller.Nlog;

namespace ITD.ETC.VETC.Synchonization.Controller
{
    public class Utility
    {
        #region Xml
        public static string SerializeXml<T>(T obj)
        {
            string xmlString = null;
            DataContractSerializer serializer = new DataContractSerializer(obj.GetType());
            using (MemoryStream ms = new MemoryStream())
            {
                serializer.WriteObject(ms, obj);
                xmlString = Encoding.UTF8.GetString(ms.ToArray());
                return xmlString;
            }
        }

        public static T DeserialiseXmlLocal<T>(string xmlString)
        {

            if (xmlString.Length > 0)
            {
                xmlString = xmlString.Replace("NULL", "");
            }
            T obj = Activator.CreateInstance<T>();
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(xmlString)))
            {
                DataContractSerializer serializer = new DataContractSerializer(obj.GetType());
                obj = (T)serializer.ReadObject(ms);
                return obj;
            }
        }
        #endregion
        public static string[] ReadFile(string pathToRead)
        {
            try
            {
                using (StreamReader sr = new StreamReader(pathToRead))
                {
                    string line = sr.ReadToEnd();
                    char[] key = {',',';'};
                    string[] words = line.Split(key);
                    return words;
                }
            }
            catch (Exception ex)
            {
                NLogHelper.Error(ex);
                return null;
            }

        }

        /// <summary>
        /// Get Datetime from ImageID
        /// </summary>
        /// <param name="TranID"></param>
        /// <returns></returns>
        public static DateTime GetDateTimefromTranID(string TranID)
        {
            return new DateTime(Int32.Parse(TranID.Substring(0, 4)),
                Int32.Parse(TranID.Substring(4, 2)),
                Int32.Parse(TranID.Substring(6, 2)),
                Int32.Parse(TranID.Substring(8, 2)),
                Int32.Parse(TranID.Substring(10, 2)),
                Int32.Parse(TranID.Substring(12, 2))
                );
        }
    }
}
