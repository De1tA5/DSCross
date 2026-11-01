using System.Text.RegularExpressions;

namespace DemonShadeCrossMod.Core.Utils
{
    public static class TooltipHelper
    {
        /// <summary>
        /// 移除套装奖励中0属性的一整行文本，请在代码最后加入
        /// </summary>
        public static void RemoveZeroDate(ref string text)
        {
            //多行模式批量消除数值为0行数
            string pattern = @".*(?<!\d)0(?!\d).*\n?"; //数值为0的一行
            text = Regex.Replace(text, pattern, "", RegexOptions.Multiline);

            //为避免数值为0且在最后一行，导致的删除留有换行号，
            //则改为单行字符串模式，移除最后的换行符
            string pattern2 = @"\n$";
            text = Regex.Replace(text, pattern2, "");

        }

        /// <summary>
        /// 移除文本中的双重换行
        /// </summary>
        public static void RemoveDoubleLF(ref string text)
        {
            string pattern = @"\n\n";
            text = Regex.Replace(text, pattern, "\n");
        }

    }
}
