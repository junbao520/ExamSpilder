using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Helper
{
    public class httpHelper
    {

        public static string httpGet(string url)
        {
            try
            {
                HttpWebRequest MyRequest = (HttpWebRequest)WebRequest.Create(url);
                MyRequest.Method = "GET";
                MyRequest.Accept = "application/json";
                //返回类型可以为
                //1、application/json
                //2、text/json
                //3、application/xml
                //4、text/xml


                MyRequest.ContentType = "application/json";
                //上传类型是能为json



                string retData = null;//接收返回值
                HttpWebResponse MyResponse = (HttpWebResponse)MyRequest.GetResponse();
                if (MyResponse.StatusCode == HttpStatusCode.OK)
                {
                    Stream MyNewStream = MyResponse.GetResponseStream();
                    StreamReader MyStreamReader = new StreamReader(MyNewStream, Encoding.UTF8);
                    retData = MyStreamReader.ReadToEnd();
                    MyStreamReader.Close();
                }
                MyResponse.Close();
                return retData;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        /// <summary>
        /// post方式访问webapi
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postdata"></param>
        /// <returns></returns>
        public static string httpPost(string url, string postdata)
        {
            try
            {
                HttpWebRequest MyRequest = (HttpWebRequest)WebRequest.Create(url);
                MyRequest.Method = "POST";
                MyRequest.Accept = "application/json";
                //返回类型可以为
                //1、application/json
                //2、text/json
                //3、application/xml
                //4、text/xml

                MyRequest.ContentType = "application/json";
                //上传类型是能为json

                if (postdata != null)
                {
                    ASCIIEncoding MyEncoding = new ASCIIEncoding();
                    byte[] MyByte = MyEncoding.GetBytes(postdata);
                    Stream MyStream = MyRequest.GetRequestStream();
                    MyStream.Write(MyByte, 0, postdata.Length);
                    MyStream.Close();
                }

                string retData = null;//返回值
                HttpWebResponse MyResponse = (HttpWebResponse)MyRequest.GetResponse();
                if (MyResponse.StatusCode == HttpStatusCode.OK)
                {
                    Stream MyNewStream = MyResponse.GetResponseStream();
                    StreamReader MyStreamReader = new StreamReader(MyNewStream, Encoding.UTF8);
                    retData = MyStreamReader.ReadToEnd();
                    MyStreamReader.Close();
                }
                MyResponse.Close();
                return retData;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
