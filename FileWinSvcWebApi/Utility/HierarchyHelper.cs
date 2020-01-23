using FileWinSvcWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileWinSvcWebApi.Utility
{
    static class HierarchyHelper
    {
        public static void HydrateHierarchyChildren(List<HierarchyBase> flatHierList)
        {
            Dictionary<long, HierarchyBase> dict = flatHierList.ToDictionary(h => h.Id);

            foreach(HierarchyBase currHier in flatHierList)
            {
                if (currHier.Id == currHier.ParentId || currHier.ParentId == null)
                {
                    // Sanity check to avoid circular reference
                    continue;
                }

                HierarchyBase parentObj = null;
                if (!dict.TryGetValue(currHier.ParentId.Value, out parentObj))
                {
                    continue;
                }

                parentObj.Children.Add(currHier);
            }
        }
    }
}
