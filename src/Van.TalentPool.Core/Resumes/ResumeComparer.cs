using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Van.TalentPool.Resumes
{
    public class ResumeComparer : IResumeComparer
    { 
        public ResumeComparer()
        { 
        }
        public async Task<List<ResumeCompare>> CompareAsync(ResumeManager manager, Resume resume)
        {
            if (resume == null)
                throw new ArgumentNullException(nameof(resume));

            if (resume.KeyMaps != null)
            {
                //Dictionary<string, string> targets = new Dictionary<string, string>();
                //foreach (var keyMap in resume.KeyMaps)
                //{
                //    var items = await manager.GetResumeKeyMapsAsync(keyMap.Keyword);
                //    foreach (var item in items)
                //    {
                //        string name = item.Name;
                //        if (string.IsNullOrEmpty(item.Name))
                //            name = "未知姓名";
                //        var dictionaryKey = $"{item.Id}:{name}";
                //        if (!targets.ContainsKey(dictionaryKey))
                //        { 
                //            targets.Add(dictionaryKey, item.Keyword);
                //        }
                //    }
                //}
             
                //var similarityEntries = _similarityCalculator.Calculate(sourceKeyword, targets);
            }

            return await Task.FromResult<List<ResumeCompare>>(null);
        }
    }
}
