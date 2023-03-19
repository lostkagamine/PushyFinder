using System.Linq;
using Lumina.Excel.GeneratedSheets;

namespace PushyFinder.Util;

public static class LuminaDataUtil
{
    public static string GetJobAbbreviation(uint jobId)
    {
        var jobEnum = Service.DataManager.GetExcelSheet<ClassJob>()!
                             .Where(a => a.RowId == jobId);
        var job = jobEnum.DefaultIfEmpty(null).FirstOrDefault();
        return job == null ? "???" : job.Abbreviation.ToString();
    }
}
