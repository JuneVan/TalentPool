using System;
using System.Text;

namespace Van.TalentPool.Infrastructure.Extensions
{

    public static class PathExtensions
    {
        /// <summary>
        /// 比较两个路径的值是否相同
        /// </summary>
        /// <param name="str">左值</param>
        /// <param name="target">右值</param>
        /// <param name="hierarchy">需要比较的前几个层级，以/分层。</param>
        /// <returns></returns>
        public static bool ComparePath(this string str, string target, int hierarchy)
        {
            if (hierarchy <= 0)
                throw new ArgumentNullException(nameof(hierarchy));

            if (string.IsNullOrEmpty(str) || string.IsNullOrEmpty(target))
                return str == target;

            return GetHierarchyPath(str, hierarchy) == GetHierarchyPath(target, hierarchy);
        }
        private static string GetHierarchyPath(string path, int hierarchy)
        {
            var values = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            if (values.Length < hierarchy)
                hierarchy = values.Length;
            StringBuilder hierarchyStr = new StringBuilder();
            for (int i = 0; i < hierarchy; i++)
            {
                hierarchyStr.Append(values[i]);
            }
            return hierarchyStr.ToString();
        }

    }
}
