using System;
using System.Linq;
using UnityEngine;

namespace YLib.GameCore
{
    public class EvaluateModule : BaseModule
    {

        public override void Initialize(params object[] param)
        {
            base.Initialize(param);
            Evaluate("AA&BB|(CC&DD)", null);
        }

        public bool Evaluate(string sourceString, Func<string, bool> checkFunction)
        {
            if (string.IsNullOrWhiteSpace(sourceString))
                return true;

            Debug.Log($"EvaluateModule::Evaluate. SourceString: {sourceString}");
            string logicString = sourceString;
            string[] conditionStrings = sourceString.Split(new string[] { "&", "&&", "|", "||" }, System.StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < conditionStrings.Length; i++)
            {
                var punctuation = conditionStrings[i].Where(Char.IsPunctuation).Distinct().ToArray();
                var conditionKey = conditionStrings[i].Trim(punctuation);
                bool result = true;
                if (checkFunction != null)
                {
                    result = checkFunction(conditionKey);
                }
                logicString = logicString.Replace(conditionKey, result.ToString());
            }

            logicString = logicString.Replace("&&", " AND ");
            logicString = logicString.Replace("&", " AND ");
            logicString = logicString.Replace("||", " OR ");
            logicString = logicString.Replace("|", " OR ");
            Debug.Log($"EvaluateModule::Evaluate. LogicString: {logicString}");
            return EvaluateLogic(logicString);
        }

        /// <summary>
        /// 回傳boolean邏輯
        /// </summary>
        /// <param name="logicString">範例: "true AND false OR true AND (false AND true)"</param>
        public bool EvaluateLogic(string logicString)
        {
            if (string.IsNullOrWhiteSpace(logicString))
                return false;
            System.Data.DataTable table = new System.Data.DataTable();
            table.Columns.Add("", typeof(bool));
            table.Columns[0].Expression = logicString;

            System.Data.DataRow r = table.NewRow();
            table.Rows.Add(r);
            bool result = (bool)r[0];
            Debug.Log($"EvaluateModule::EvaluateLogic. Result: {result}");
            return result;
        }
    }
}
