using FFLTask.GLB.Global.Enum;
using FFLTask.SRV.ViewModel.Task;
using Global.Core.ExtensionMethod;

namespace FFLTask.SRV.ViewModelMap
{
    public static class ProjectConfigMap
    {
        public static void FilledBy(this StatusModel model, Status status)
        {
            model.Stage = (int)status;
            model.Name = status.GetEnumDescription();
        }
    }
}
