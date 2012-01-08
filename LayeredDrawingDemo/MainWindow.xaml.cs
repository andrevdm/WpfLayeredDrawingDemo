using System.Globalization;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace LayeredDrawingDemo
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnLoaded( object sender, RoutedEventArgs e )
        {
            m_layers.AddLayer( 10, DrawBackground, ChangeType.Resize );
            m_layers.AddLayer( 11, DrawBackgroundBlock );

            m_layers.AddLayer( 20, DrawStaticForeground );
            m_layers.AddLayer( 21, DrawText, ChangeType.Scroll );

            m_layers.AddLayer( 30, DrawForeground );

            Draw( ChangeType.Redraw );
        }

        private void DrawText( DrawingContext ctx )
        {
            var pen = new Pen( Brushes.Black, 1 );
            var rect = new Rect( 20, m_scroll.Value, 15, 25 );
            ctx.DrawRectangle( Brushes.Teal, pen, rect );

            var txt = new FormattedText(
                "qazwsxedcrfvtgbyhnujmik,ol.p;/[']\r\nqwertyuiop\r\n\r\nasdfghjkl\r\nzxcvbnm\r\n0987654321", 
                CultureInfo.GetCultureInfo( "en-us" ),
                FlowDirection.LeftToRight,
                new Typeface( "Consolas" ),
                14, 
                Brushes.White );

            ctx.DrawText( txt, new Point( 20, m_scroll.Value ) );

            Log( "text" );
        }

        private void DrawBackground( DrawingContext ctx )
        {
            var pen = new Pen( Brushes.Black, 1 );
            var rect = new Rect( 0, 0, m_layers.ActualWidth, m_layers.ActualHeight );
            ctx.DrawRoundedRectangle( Brushes.Black, pen, rect, 50, 50 );

            Log( "background" );
        }

        private void DrawBackgroundBlock( DrawingContext ctx )
        {
            var pen = new Pen( Brushes.DarkOliveGreen, 1 );
            var rect = new Rect( 20, 60, 200, 50 );
            ctx.DrawRoundedRectangle( Brushes.DarkOliveGreen, pen, rect, 50, 50 );

            Log( "background block" );
        }


        private void DrawForeground( DrawingContext ctx )
        {
            var pen = new Pen( Brushes.Black, 1 );
            var rect = new Rect( 20, 20, 50, 55 );
            ctx.DrawRectangle( Brushes.Red, pen, rect );

            Log( "foreground" );
        }
        
        private void DrawStaticForeground( DrawingContext ctx )
        {
            var pen = new Pen( Brushes.Black, 1 );
            var rect = new Rect( 70, 20, 100, 100 );
            ctx.DrawRectangle( Brushes.Blue, pen, rect );

            Log( "foreground" );
        }

        protected override void OnRenderSizeChanged( SizeChangedInfo sizeInfo )
        {
            base.OnRenderSizeChanged( sizeInfo );

            m_scroll.Minimum = 0;
            m_scroll.Maximum = m_layers.ActualHeight - 70;
            Draw( ChangeType.Resize );
        }

        private void OnScroll( object sender, ScrollEventArgs e )
        {
            Draw( ChangeType.Scroll );
        }

        private void Draw( ChangeType change )
        {
            m_layers.Draw( change );
        }

        private void Log( string text )
        {
            m_log.Text = text + "\r\n" + m_log.Text;

            if( m_log.Text.Length > 1000 )
            {
                m_log.Text = m_log.Text.Substring( 0, 1000 );
            }
        }
    }
}
