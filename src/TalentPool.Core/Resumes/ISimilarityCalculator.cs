using System.Collections.Generic;

namespace TalentPool.Resumes
{
    public interface ISimilarityCalculator
    {
        IList<SimilarityEntry> Calculate(string source, Dictionary<string, string> targets);
    }

    public class SimilarityEntry
    {
        // 相似度项的名称
        public string Name { get; set; }
        // 相似度 百分比
        public int Value { get; set; }
    }
}
