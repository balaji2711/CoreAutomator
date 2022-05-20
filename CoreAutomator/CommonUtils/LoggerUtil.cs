using System.Text;

namespace CoreAutomator.CommonUtils
{
    public class LoggerUtil
    {
        public static string startNode(string tag)
        {
            return string.Format("<{0}>", tag);
        }

        public static string startNode(string tag, string border)
        {
            return string.Format("<{0} border=\"{1}\">", tag, border);
        }

        public static string closeNode(string tag)
        {
            return string.Format("</{0}>", tag);
        }

        public static string getRow(string td1, string td2)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine(startNode("tr"));
            builder.AppendLine(getNode(TagType.td, td1));
            builder.AppendLine(getNode(TagType.td, td2));
            builder.AppendLine(closeNode("tr"));
            return builder.ToString();
        }

        public static string getNode(TagType tag, string innerText, string? color = null)
        {
            string style = string.Format("style=\"background-color: {0};\"", color);
            if (color == null)
            {
                return string.Format("<{0}>{1}</{0}>", tag.ToString(), innerText);
            }
            else
            {
                return string.Format("<{0} {1}>{2}</{0}>", tag.ToString(), style, innerText);
            }
        }

        public enum TagType
        {
            canvas,
            P,
            h2,
            h3,
            h4,
            h1,
            td,
            tr,
            table,
            th,
            br,
        }
    }
}
