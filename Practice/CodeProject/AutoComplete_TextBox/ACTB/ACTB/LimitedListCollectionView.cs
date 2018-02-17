namespace Aviad.WPF.Controls
{
    using System;
    using System.Collections;
    using System.Windows.Data;

    public class LimitedListCollectionView : CollectionView, IEnumerable
    {
        #region Constructors
        public LimitedListCollectionView(IEnumerable list)
          : base(list)
        {
            this.Limit = int.MaxValue;
        }
        #endregion Constructors

        #region Properties
        public int Limit { get; set; }

        public override int Count
        {
            get { return Math.Min(base.Count, this.Limit); }
        }
        #endregion Properties

        #region Methods
        public override bool MoveCurrentToLast()
        {
            return base.MoveCurrentToPosition(this.Count - 1);
        }

        public override bool MoveCurrentToNext()
        {
            if (this.CurrentPosition == this.Count - 1)
                return base.MoveCurrentToPosition(base.Count);
            else
                return base.MoveCurrentToNext();
        }

        public override bool MoveCurrentToPrevious()
        {
            if (this.IsCurrentAfterLast)
                return base.MoveCurrentToPosition(this.Count - 1);
            else
                return base.MoveCurrentToPrevious();
        }

        public override bool MoveCurrentToPosition(int position)
        {
            if (position < this.Count)
                return base.MoveCurrentToPosition(position);
            else
                return base.MoveCurrentToPosition(base.Count);
        }
        #endregion Methods

        #region IEnumerable Members
        IEnumerator IEnumerable.GetEnumerator()
        {
            do
            {
                yield return CurrentItem;
            }
            while (this.MoveCurrentToNext());
        }
        #endregion
    }
}
