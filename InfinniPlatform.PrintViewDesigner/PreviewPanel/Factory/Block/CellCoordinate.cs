using System;
using System.Diagnostics;

namespace InfinniPlatform.PrintViewDesigner.PreviewPanel.Factory.Block
{
    /// <summary>
    /// Координаты ячеки в таблице.
    /// </summary>
    [DebuggerDisplay("Row:{" + nameof(Row) + "}, Column:{" + nameof(Column) + "}")]
    public class CellCoordinate : IEquatable<CellCoordinate>
    {
        public CellCoordinate(int row, int column)
        {
            Row = row;
            Column = column;
        }

        public int Row { get; set; }

        public int Column { get; set; }

        public bool Equals(CellCoordinate other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }
            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return Column == other.Column && Row == other.Row;
        }
    }
}