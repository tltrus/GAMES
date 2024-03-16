using System.Windows.Controls;
using System.Windows.Media;

namespace DefenseGame
{
    class DrawingVisualClass : Canvas
    {
        List<Visual> visuals = new List<Visual>();

        protected override int VisualChildrenCount
        {
            get
            {
                return visuals.Count;
            }
        }

        protected override Visual GetVisualChild(int index)
        {
            return visuals[index];
        }

        public void AddVisual(Visual visual)
        {
            visuals.Add(visual);
            AddVisualChild(visual);
            AddLogicalChild(visual);
        }

        public void RemoveVisual(Visual visual)
        {
            visuals.Remove(visual);
            RemoveVisualChild(visual);
            RemoveLogicalChild(visual);
        }
    }
}
