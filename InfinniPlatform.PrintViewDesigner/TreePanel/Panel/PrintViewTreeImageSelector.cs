using System.Windows.Media;

using DevExpress.Xpf.Grid;
using DevExpress.Xpf.Grid.TreeList;

using InfinniPlatform.PrintViewDesigner.Images;
using InfinniPlatform.PrintViewDesigner.TreePanel.Factory;

namespace InfinniPlatform.PrintViewDesigner.TreePanel.Panel
{
    internal sealed class PrintViewTreeImageSelector : TreeListNodeImageSelector
    {
        public override ImageSource Select(TreeListRowData rowData)
        {
            ImageSource image = null;

            var row = rowData.Row as PrintElementNode;

            if (row != null)
            {
                image = ImageRepository.GetImage(row.ElementType.ToString());
            }

            return image ?? ImageRepository.GetImage("UnknownTypeNode");
        }
    }
}