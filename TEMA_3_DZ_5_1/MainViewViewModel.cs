using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Plumbing;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

using Prism.Commands;

using TEMA_3_DZ_5_1_Library;

namespace TEMA_3_DZ_5_1
{
    public class MainViewViewModel
    {
        private ExternalCommandData _commandData;

        public DelegateCommand SelectPipeCommand { get; }
        public DelegateCommand SelectWallCommand { get; }
        public DelegateCommand SelectDoorCommand { get; }

        public MainViewViewModel(ExternalCommandData commandData)
        {
            _commandData = commandData;
            SelectPipeCommand = new DelegateCommand(OnSelectPipeCommand);
            SelectWallCommand = new DelegateCommand(OnSelectWallCommand);
            SelectDoorCommand = new DelegateCommand(OnSelectDoorCommand);
        }



        public event EventHandler HideRequest;
        private void RaiseHideRequest()
        {
            HideRequest?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler ShowRequest;
        private void RaiseShowRequest()
        {
            ShowRequest?.Invoke(this, EventArgs.Empty);
        }

        private void OnSelectPipeCommand()
        {
            RaiseHideRequest();

            List<Pipe> fInstances = SelectionUtils.PipeCount(_commandData);
            TaskDialog.Show("Количество труб в модели", fInstances.Count.ToString());

            RaiseShowRequest();

        }

        private void OnSelectWallCommand()
        {
            RaiseHideRequest();

            List<Wall> fInstances = SelectionUtils.WallVolume(_commandData);
            var wallVolumeList_SI = new List<Double>();

            foreach (var vol in fInstances)
            {
                wallVolumeList_SI.Add(UnitUtils.ConvertFromInternalUnits(vol.get_Parameter(BuiltInParameter.HOST_VOLUME_COMPUTED).AsDouble(), UnitTypeId.CubicMeters));
            }

            TaskDialog.Show("Объем стен в модели", $"{wallVolumeList_SI.Sum().ToString()}, м3");

            RaiseShowRequest();
        }

        private void OnSelectDoorCommand()
        {
            List<FamilyInstance> fInstances = SelectionUtils.DoorCount(_commandData);
            TaskDialog.Show("Количество дверей в модели", fInstances.Count.ToString());
        }

    }
}
