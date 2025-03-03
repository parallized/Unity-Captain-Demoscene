using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Editor
{
    internal class InvokeFormat
    {
        public static string MergeSchemaMessage(string schemaContent, string chatContent)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("我提供了一份 schenma 定义文件放在文件开头，最后提供了我的要求描述，请直接输出 json 或者按照我在最后要求输出的类型格式内容，如果提供的文件不能被视作一个 schema 内容，则返回 { data: 'not a schema file' }, 不需要添加口头话的语法表述，直接提供内容即可：");
            stringBuilder.AppendLine(schemaContent);
            stringBuilder.AppendLine(chatContent);
            return stringBuilder.ToString();
        }
    }
}
