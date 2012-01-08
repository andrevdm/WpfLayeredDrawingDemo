using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace LayeredDrawingDemo
{
    public class WpfLayers : FrameworkElement
    {
        private readonly VisualCollection m_children;
        private readonly List<WpfLayerInfo> m_layers = new List<WpfLayerInfo>();

        public WpfLayers()
        {
            m_children = new VisualCollection( this );
        }

        public void AddLayer( int priority, Action<DrawingContext> draw, ChangeType notifyOnChange = ChangeType.Redraw )
        {
            var drawingVisual = new DrawingVisual();

            var layerInfo = new WpfLayerInfo( priority, draw, drawingVisual, notifyOnChange );
            m_layers.Add( layerInfo );

            //Sort the layers by priority
            m_layers.Sort( ( x, y ) => x.Priority.CompareTo( y.Priority ) );

            //Remove all the visual layers and add them in order
            m_children.Clear();
            m_layers.ForEach( l => m_children.Add( l.Visual ) );
        }

        public void Draw( ChangeType change )
        {
            var affected = from l in m_layers
                           where ((change & ChangeType.Redraw) != 0) || ((l.NotifyOnChange & change) != 0)
                           orderby l.Priority
                           select l;

            foreach( WpfLayerInfo layer in affected )
            {
                DrawingContext ctx = layer.Visual.RenderOpen();
                layer.Draw( ctx );
                ctx.Close();
            }
        }

        protected override int VisualChildrenCount
        {
            get { return m_children.Count; }
        }

        protected override Visual GetVisualChild( int index )
        {
            if( index < 0 || index >= m_children.Count )
            {
                throw new ArgumentOutOfRangeException( "index" );
            }

            return m_children[index];
        }
    }
}