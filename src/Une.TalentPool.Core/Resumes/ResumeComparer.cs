using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Une.TalentPool.Resumes
{
    public class ResumeComparer : IResumeComparer
    {
        public ResumeComparer()
        {
        }
        public async Task CompareAsync(ResumeManager manager, Resume resume, decimal minSimilarityValue)
        {
            if (resume == null)
                throw new ArgumentNullException(nameof(resume));

            if (resume.KeyMaps != null)
            {
                // 查找所有keyword的简历
                Dictionary<Guid, string> _tempResumes = new Dictionary<Guid, string>();
                foreach (var keyMap in resume.KeyMaps)
                {
                    var keywordMaps = await manager.GetResumeKeyMapsAsync(keyMap.Keyword);
                    foreach (var item in keywordMaps)
                    {
                        if (!_tempResumes.ContainsKey(item.ResumeId) && item.ResumeId != resume.Id)
                            _tempResumes.Add(item.ResumeId, item.Name);
                    }
                }
                foreach (var temp in _tempResumes)
                {
                    var keywordMaps = await manager.GetResumeKeyMapsAsync(temp.Key);
                    // 相同关键词个数
                    int sameCount = 0;
                    // 循环最小的集合
                    if (keywordMaps.Count > resume.KeyMaps.Count)
                    {
                        foreach (var item in resume.KeyMaps)
                        {
                            if (keywordMaps.FirstOrDefault(f => f.Keyword == item.Keyword) != null)
                                sameCount += 1;
                        }
                    }
                    else
                    {
                        foreach (var item in keywordMaps)
                        {
                            if (resume.KeyMaps.FirstOrDefault(f => f.Keyword == item.Keyword) != null)
                                sameCount += 1;
                        }
                    }
                    int maxLength = Math.Max(resume.KeyMaps.Count, keywordMaps.Count);

                    decimal similarity = Math.Round((decimal)sameCount / maxLength, 4);
                    if (similarity > minSimilarityValue / 100)
                    {
                        resume.ResumeCompares.Add(new ResumeCompare()
                        {
                            RelationResumeId = temp.Key,
                            Similarity = similarity,
                            RelationResumeName = temp.Value
                        });
                    }
                }
            }
        }
    }
}
