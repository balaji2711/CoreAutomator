using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Net.Mail;
using System.Text;
using System.Xml.Linq;
using System.Drawing;
using NUnit.Framework;
using System.Xml.Serialization;
using static CoreAutomator.CommonUtils.LoggerUtil;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Dynamic;
using System.Net;
using System.Net.Sockets;
using CoreAutomator.ReportUtilities;

namespace CoreAutomator.CommonUtils
{
    public class Utils
    {
        public Utils()
        {
            ConfigManager.InitializeEnvConfig();
        }

        private static readonly Random getrandom = new Random();

        public static string GetXMLData(string fileName, string searchKey, string environmentType)
        {
            string searchValue = string.Empty;
            if (!string.IsNullOrWhiteSpace(fileName) && !string.IsNullOrWhiteSpace(searchKey) && !string.IsNullOrWhiteSpace(environmentType))
            {
                string xmlPath = Directory.GetCurrentDirectory() + "\\" + fileName;
                if (File.Exists(xmlPath))
                {
                    var doc = XDocument.Load(xmlPath);
                    var result = doc.Descendants("Environment")
                                    .Where(e => ((string)e.Attribute("type"))
                                    .Equals(environmentType))
                                    .Select(z => z.Elements("Key")
                                    .Where(x => ((string)x.Attribute("id"))
                                    .Equals(searchKey, StringComparison.CurrentCultureIgnoreCase))
                                    .Select(y => (string)y.Attribute("value"))
                                    .FirstOrDefault());
                    if (result != null)
                        searchValue = result.ToList().FirstOrDefault();
                }
            }
            return searchValue;
        }

        public static void SendEMail(string emailIDs, string suite)
        {
            try
            {
                string filePath = @"C:/" + Environment.MachineName + "/" + GenerateReport.CurrentRunFolderName;
                SmtpClient SmtpServer;
                List<MailAddress> to = new List<MailAddress>();
                string[] toList = emailIDs.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string emailId in toList)
                {
                    to.Add(new MailAddress(emailId));
                }
                MailMessage mail = new MailMessage();
                Attachment extentReport;
                string from = "balaji.november@gmail.com";
                mail.From = new MailAddress(from);
                mail.Body = "Dear Team, <br/> <br/> Our test suite has just finished its execution.<br/>Failure case's will be analyzed by us and will post out the observation. Please find the attachment for the detailed test run report and the results below. <br/><br/>";
                to.ForEach(entry => mail.To.Add(entry));
                mail.BodyEncoding = Encoding.UTF8;
                mail.Subject = "Automation Report : " + suite + " - " + DateTime.Now.ToShortDateString();
                mail.SubjectEncoding = Encoding.UTF8;
                StringBuilder builder = new StringBuilder();
                builder.AppendLine(startNode("table", "1"));
                builder.AppendLine(startNode("tr"));
                builder.AppendLine(getNode(TagType.th, "Result"));
                builder.AppendLine(getNode(TagType.th, "Count"));
                builder.AppendLine(closeNode("tr"));

                double rate = GenerateReport.passCount * 100 / GenerateReport.totalCount;

                builder.AppendLine(getRow("Passed", GenerateReport.passCount.ToString()));
                builder.AppendLine(getRow("Failed", GenerateReport.failCount.ToString()));
                builder.AppendLine(getRow("Total", GenerateReport.totalCount.ToString()));
                builder.AppendLine(getRow("Pass %", rate.ToString()));

                builder.AppendLine(closeNode("table"));
                mail.Body = mail.Body + builder.ToString();
                mail.IsBodyHtml = true;
                mail.Body = mail.Body + "<br/>This email was sent automatically. Please do not reply.<br/><br/>Regards,<br/>Automation Team.<br/> <br/>";

                if (File.Exists(filePath + "\\index.html"))
                {
                    extentReport = new Attachment(filePath + "\\index.html");
                    mail.Attachments.Add(extentReport);
                }
                else
                    Console.WriteLine("ERROR!!!!! Report not found in the specified path...!");
                SmtpServer = new SmtpClient("smtp.gmail.com");
                SmtpServer.Send(mail);
                SmtpServer.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void RunCmd(string cli)
        {
            try
            {
                Process cmd = new Process();
                cmd.StartInfo.FileName = "cmd.exe";
                cmd.StartInfo.RedirectStandardInput = true;
                cmd.StartInfo.RedirectStandardOutput = true;
                cmd.StartInfo.CreateNoWindow = true;
                cmd.StartInfo.UseShellExecute = false;
                cmd.StartInfo.WindowStyle = ProcessWindowStyle.Maximized;
                cmd.Start();

                cmd.StandardInput.WriteLine(cli);
                cmd.StandardInput.Flush();
                cmd.StandardInput.Close();
                cmd.WaitForExit();
                Console.WriteLine(cmd.StandardOutput.ReadToEnd());
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }

        public static void ExecuteBatchFile(string fileName)
        {
            Process process = null;
            try
            {
                string folderPath = @"C:/" + Environment.MachineName;
                process = new Process();
                process.StartInfo.WorkingDirectory = folderPath;
                process.StartInfo.FileName = fileName;
                process.StartInfo.CreateNoWindow = false;
                //https://github.com/dotnet/runtime/issues/28005
                process.StartInfo.UseShellExecute = true;
                process.Start();
                process.WaitForExit();
                process.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception - " + ex.Message.ToString());
            }
        }

        public static DataSet GetDataFromDB(string query, string dataSource, string userName, string password, string InitialCatalog)
        {
            DataSet dataSet = null;
            try
            {
                SqlConnection connection = new SqlConnection("Data Source=" + dataSource + ";database=" + InitialCatalog + ";uid=" + userName + ";pwd=" + password);
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.CommandText = query.ToString();
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                dataSet = new DataSet();
                adapter.Fill(dataSet);
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Exception - " + ex.Message.ToString());
            }
            return dataSet;
        }

        public static bool CompareImages(Bitmap bmpSource, Bitmap target, ref int count1, ref int count2)
        {
            bool flag = true;
            string img1_ref, img2_ref;
            Bitmap? image_1 = new Bitmap(bmpSource);
            Bitmap? image_2 = new Bitmap(target);
            if (image_1.Width == image_2.Width && image_1.Height == image_2.Height)
            {
                for (int i = 0; i < image_1.Width; i++)
                {
                    for (int j = 0; j < image_1.Height; j++)
                    {
                        img1_ref = image_1.GetPixel(i, j).ToString();
                        img2_ref = image_2.GetPixel(i, j).ToString();
                        if (img1_ref != img2_ref)
                        {
                            count2++;
                            flag = false;
                            break;
                        }
                        count1++;
                    }
                }
            }
            else
                Assert.Fail("Can not compare these images");
            return flag;
        }

        public static int GetRandomNumber(int min, int max)
        {
            lock (getrandom)
            {
                return getrandom.Next(min, max);
            }
        }

        public static T DeserializeXML<T>(string content)
        {
            using TextReader reader = new StringReader(content);
            return (T)new XmlSerializer(typeof(T)).Deserialize(reader);
        }

        public static string WebSocketUri()
        {
            string baseUri = null;
            string type = ConfigManager.config.appConfig.socketType;
            string url = ConfigManager.config.appConfig.serviceFabricURL;
            baseUri = type + url;
            return baseUri;
        }

        public static dynamic JsonParser(string filePath)
        {
            dynamic data = JObject.Parse(File.ReadAllText(filePath));
            return data;
        }

        public static string ReadXmlFromFile(string filepath)
        {
            string xmlString = File.ReadAllText(filepath);
            return xmlString;
        }

        public static dynamic XmlParser(string xml)
        {
            XDocument doc = XDocument.Parse(xml);
            string jsonText = JsonConvert.SerializeXNode(doc);
            dynamic data = JsonConvert.DeserializeObject<ExpandoObject>(jsonText);
            return data;
        }

        public static string GetZonedDateTime()
        {
            DateTime dateTime = DateTime.Now;
            string zonedDateTime = string.Format("{0:yyyy-MM-ddTHH:mm:ss.fffZ}", dateTime.ToLocalTime());
            return zonedDateTime;
        }

        public static string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public static string RestBaseUrl()
        {
            string url = ConfigManager.config.appConfig.baseURL;
            return url;
        }
    }
}